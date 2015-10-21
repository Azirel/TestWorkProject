using UnityEngine;
using System.Collections;
using System.Timers;
using System.Collections.Generic;
using GestureRecognizer;
using UnityEngine.UI;

public class GeneralManagment : MonoBehaviour
{

    [Tooltip("Disable or enable gesture recognition")]
    public bool isEnabled = true;

    [Tooltip("Overwrite the XML file in persistent data path")]
    public bool forceCopy = false;

    [Tooltip("Use the faster algorithm, however default (slower) algorithm has a better scoring system")]
    public bool UseProtractor = false;

    [Tooltip("The name of the gesture library to load. Do NOT include '.xml'")]
    string libraryToLoad = "shapes";

    [Tooltip("A new point will be placed if it is this further than the last point.")]
    public float distanceBetweenPoints = 10f;

    [Tooltip("Minimum amount of points required to recognize a gesture.")]
    public int minimumPointsToRecognize = 10;

    //[Tooltip("Material for the line renderer.")]
    //public Material lineMaterial;

    [Tooltip("Start thickness of the gesture.")]
    public float startThickness = 0.25f;

    [Tooltip("End thickness of the gesture.")]
    public float endThickness = 0.05f;

    [Tooltip("Start color of the gesture.")]
    public Color startColor = new Color(0, 0.67f, 1f);

    [Tooltip("End color of the gesture.")]
    public Color endColor = new Color(0.48f, 0.83f, 1f);

    [Tooltip("The RectTransform that limits the gesture")]
    public RectTransform drawArea;

    [Tooltip("The InputField that will hold the new gesture name")]
    public InputField newGestureName;

    [Tooltip("Messages will show up here")]
    public Text messageArea;

    [Tooltip("Messages will show up here")]
    public float attemptDelayTime;

    public Text additionResult;

    public Text textlScore;

    public GameObject startOverButton;

    public LineRenderer lineForShowWhatToDraw;

    public Text forTimer;

    // Current platform.
    RuntimePlatform platform;

    // Line renderer component.
    LineRenderer gestureRenderer;

    // The position of the point on the screen.
    Vector3 virtualKeyPosition = Vector2.zero;

    // A new point.
    Vector2 point;

    // List of points that form the gesture.
    List<Vector2> points = new List<Vector2>();

    // Vertex count of the line renderer.
    int vertexCount = 0;

    // Loaded gesture library.
    public GestureLibrary gl;

    // Recognized gesture.
    Gesture gesture;

    // Result.
    Result result;

    System.Random rand;

    public bool isInGame = false;

    public bool isInAddition = false;

    public bool isInStartMenu = true;

    public bool inInShowingState = false;

    public bool inAttemptState = false;

    bool inGameOverState = false;

    List<Gesture> uniqueGestureList;

    public float wholeTime;

    float currentTime;

    int score = 0;

    string rightAnswer = "";

    public float divider = 1.1f;

    // Get the platform and apply attributes to line renderer.
    void Awake()
    {
        platform = Application.platform;
        QualitySettings.antiAliasing = 8;
        //gestureRenderer = gameObject.AddComponent<LineRenderer>();
        gestureRenderer = GetComponent<LineRenderer>();
        gestureRenderer.SetVertexCount(0);
        //gestureRenderer.material = lineMaterial;
        gestureRenderer.SetColors(startColor, endColor);
        gestureRenderer.SetWidth(startThickness, endThickness);
        uniqueGestureList = new List<Gesture>();
        startOverButton.SetActive(false);
        inInShowingState = false;
    }


    // Load the library.
    void Start()
    {
        gl = new GestureLibrary(libraryToLoad, forceCopy);
        //lineForShowWhatToDraw.enabled = false;
        rand = new System.Random();
        //Debug.Log("Я стартанул");
        inInShowingState = false;
        //StartCoroutine("ShowFiguresFromLibrary");
    }


    void Update()
    {
        if (inInShowingState == true)
        {
            StartCoroutine(ShowFigureFromLibrary());
            inAttemptState = false;
        }
        else
        {
            if (currentTime <= 0f && inAttemptState == true)
            {

                isEnabled = false;
                inInShowingState = false;
                isInGame = false;
                startOverButton.SetActive(true);

                //gestureRenderer.enabled = false;
                messageArea.text = "";
                Debug.Log("Ой а вот таймер");
            }
            // Track user input if GestureRecognition is enabled.
            else
            {
                if ((isEnabled && inAttemptState) || isInAddition)
                {
                    if (isEnabled && inAttemptState)
                    {
                        currentTime -= Time.deltaTime;
                        forTimer.text = currentTime.ToString();
                    }
                    // If it is a touch device, get the touch position
                    // if it is not, get the mouse position
                    if (Utility.IsTouchDevice())
                    {
                        if (Input.touchCount > 0)
                        {
                            virtualKeyPosition = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
                        }
                    }
                    else
                    {
                        if (Input.GetMouseButton(0))
                        {
                            virtualKeyPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
                        }
                    }

                    if (RectTransformUtility.RectangleContainsScreenPoint(drawArea, virtualKeyPosition, Camera.main))
                    {

                        if (Input.GetMouseButtonDown(0))
                        {
                            ClearGesture();
                        }

                        // It is not necessary to track the touch from this point on,
                        // because it is already registered, and GetMouseButton event 
                        // also fires on touch devices
                        if (Input.GetMouseButton(0))
                        {

                            point = new Vector2(virtualKeyPosition.x, -virtualKeyPosition.y);

                            // Register this point only if the point list is empty or current point
                            // is far enough than the last point. This ensures that the gesture looks
                            // good on the screen. Moreover, it is good to not overpopulate the screen
                            // with so much points.
                            if (points.Count == 0 ||
                                (points.Count > 0 && Vector2.Distance(point, points[points.Count - 1]) > distanceBetweenPoints))
                            {
                                points.Add(point);

                                if (isInAddition == true)
                                {
                                    gestureRenderer.SetVertexCount(++vertexCount);
                                    gestureRenderer.SetPosition(vertexCount - 1, Utility.WorldCoordinateForGesturePoint(virtualKeyPosition));
                                }
                            }

                        }

                        // Capture the gesture, recognize it, fire the recognition event,
                        // and clear the gesture from the screen.
                        if (Input.GetMouseButtonUp(0) && inAttemptState == true)
                        {

                            if (points.Count > minimumPointsToRecognize)
                            {
                                gesture = new Gesture(points);
                                result = gesture.Recognize(gl, UseProtractor);
                                //SetMessage("Gesture is recognized as <color=#ff0000>'" + result.Name + "'</color> with a score of " + result.Score);
                                if (result.Name == rightAnswer && result.Score >= 0.8f)
                                {
                                    messageArea.color = Color.green;
                                    messageArea.text = "Currect";
                                    score++;
                                    textlScore.text = score.ToString();
                                    StartCoroutine(ShowFigureFromLibrary());
                                }
                                else
                                {
                                    messageArea.color = Color.red;
                                    messageArea.text = "Wrong";
                                }
                            }


                        }
                        //if (isInGame == true)
                        //{
                        //    messageArea.text = "Currect";
                        //    Debug.Log("Я в игре");
                        //}
                    }

                }
            }
        }
    }
    public void StartOver()
    {
        startOverButton.SetActive(false);
        currentTime = wholeTime;
        inInShowingState = true;
        isEnabled = false;
        inAttemptState = false;
        score = 0;
        textlScore.text = "0";
    }
    public void GestureListForming()
    {
        bool contains = false;
        foreach (var gestureFromLibrary in gl.Library)
        {

            foreach (var gestureFromList in uniqueGestureList)
            {

                if (gestureFromLibrary.Name == gestureFromList.Name)
                {
                    contains = true;
                    break;
                }
            }
            if (contains == false)
            {
                uniqueGestureList.Add(gestureFromLibrary);
                Debug.Log(gestureFromLibrary.Name + "Added");
            }
            contains = false;
        }
    }

    IEnumerator ShowFigureFromLibrary()
    {
        //Debug.Log("Коротин стартанул");
        //List<Gesture> gestureList = gl.Library;
        inInShowingState = false;
        isEnabled = false;
        isInGame = false;
        inAttemptState = false;
        messageArea.color = Color.yellow;
        messageArea.text = "Draw it!";
        float leftBound = -3.7f;
        float rightBound = 3.4f;
        float upperBound = -1.94f;
        float bottomBound = 4f;
        float xMin;
        float xMax;
        float yMin;
        float yMax;
        float[] xs = new float[64];
        float[] ys = new float[64];
        int i = 0;


        lineForShowWhatToDraw.enabled = false;
        Gesture gestureToDraw = uniqueGestureList[rand.Next(0, uniqueGestureList.Count)];
        for (int j = 0; j < 64; j++)
        {
            xs[j] = gestureToDraw.Points[j].x;
            ys[j] = gestureToDraw.Points[j].y;
        }
        xMin = xs[0];
        xMax = xs[0];
        yMin = ys[0];
        yMax = ys[0];
        for (int j = 1; j < 64; j++)
        {
            if (xs[j] < xMin)
                xMin = xs[j];

            if (xs[j] > xMax)
                xMax = xs[j];

            if (ys[j] < yMin)
                yMin = ys[j];

            if (ys[j] > yMax)
                yMax = ys[j];
        }
        float xRealDifference = xMin - xMax;
        float yRealDifference = yMin - yMax;
        float xLocalDifference = leftBound - rightBound;
        float yLocalDifference = bottomBound - upperBound;
        //Debug.Log("xMax=" + xMax.ToString() + ";" + "xMin=" + xMin.ToString() + ";" + "yMax=" + yMax.ToString() + ";" + "yMin=" + yMin.ToString() + ";" + gestureToDraw.Name);
        lineForShowWhatToDraw.SetVertexCount(gestureToDraw.Points.Count);
        foreach (var point in gestureToDraw.Points)
        {

            lineForShowWhatToDraw.SetPosition(i, new Vector3((((point.x - xMax) / xRealDifference) * xLocalDifference) + rightBound, (((point.y - yMax) / yRealDifference) * yLocalDifference) + upperBound));
            i++;
        }
        i = 0;
        lineForShowWhatToDraw.enabled = true;
        ///to do attemptDelayTime need to be divided on Something
        yield return new WaitForSeconds(1);
        currentTime = wholeTime / Mathf.Pow(divider, (float)(score + 1));
        rightAnswer = gestureToDraw.Name;
        lineForShowWhatToDraw.enabled = false;
        isEnabled = true;
        inAttemptState = true;
        inInShowingState = false;
        isInGame = true;
    }

    /// <summary>
    /// Adds a gesture to the library
    /// </summary>
    public void AddGesture()
    {
        try
        {
            Gesture newGesture = new Gesture(points, newGestureName.text);
            gl.AddGesture(newGesture);
            SetMessage(newGestureName.text + " has been added to the library");
            additionResult.text = newGestureName.text + " has been added to the library";

        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }


    /// <summary>
    /// Shows a message at the bottom of the screen
    /// </summary>
    /// <param name="text"></param>
    public void SetMessage(string text)
    {
        messageArea.text = text;
    }

    /// <summary>
    /// Remove the gesture from the screen.
    /// </summary>
    void ClearGesture()
    {
        points.Clear();

        if (isInAddition == true)
        {
            gestureRenderer.SetVertexCount(0);
        }
        vertexCount = 0;
    }
}
