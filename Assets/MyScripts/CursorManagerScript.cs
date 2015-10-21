using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using GestureRecognizer;

public class CursorManagerScript : MonoBehaviour
{
    Vector3 pos;
    public LineRenderer line;
    public int delays = 0;
    //public UnityEngine.UI.Text text;
    public float delayTime;
    List<TracePoint> pointsList;
    TracePoint tempPoint;
    //public Text pointsCountViewer;
    //public Text vertexesCountViewer;
    int timer = 0;
    public float startthinckness;
    public float endthinckness;
    class TracePoint
    {
        public Vector3 position;
        public int delaysCount;
    }
    // Use this for initialization
    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.SetWidth(startthinckness, endthinckness);
        pointsList = new List<TracePoint>();
        //line.SetVertexCount(0);
    }

    // Update is called once per frame
    void Update()
    {
    }
    void FixedUpdate()
    {
        //pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        //gameObject.transform.position = pos;
        if (Input.GetMouseButton(0) || Utility.IsTouchDevice())
        {
            AddPoint(pos);
            line.enabled = true;
            try
            {
                line.SetPosition(0, pos);
            }
            catch (System.Exception e)
            {
                line.SetVertexCount(1);
            }
        }
        ModifyLine();
        if (pointsList.Count > 0)
        {
            //pointsCountViewer.text = pointsList.Count.ToString();
            LastPointsEliminator();
        }
        else line.enabled = false;
        
    }
    void TextUpdate()
    {
    }
    void AddPoint(Vector3 pointPos)
    {
        tempPoint = new TracePoint();

        tempPoint.position = pointPos;
        tempPoint.delaysCount = delays;

        if (!pointsList.Contains(tempPoint))
        {
            pointsList.Insert(0, tempPoint);
        }
    }
    void ModifyLine()
    {
        line.SetVertexCount(pointsList.Count);
        //Debug.Log(pointsList.Count);
        for (int i = 1; i < pointsList.Count; i++)
        {
            line.SetPosition(i, pointsList[i].position);
        }
    }
    void LastPointsEliminator()
    {
        foreach (var item in pointsList)
        {
            item.delaysCount--;
            if (item.delaysCount < 1)
                pointsList.Remove(item);
        }
    }
}
