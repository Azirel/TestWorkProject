using UnityEngine;
using System.Collections;

public class MenuSwitcher : MonoBehaviour
{

    public GameObject game;
    public GameObject startMenu;
    public GameObject additionMenu;
    CursorManagerScript cms;
    GeneralManagment gm;
    public RectTransform gameDrawArea;
    public RectTransform additionDrawArea;
    public LineRenderer line;
    // Use this for initialization
    void Start()
    {
        cms = GetComponent<CursorManagerScript>();
        gm = GetComponent<GeneralManagment>();
        StartMenuEnable();
        //line = GetComponent<LineRenderer>();
        //line = cms.line;
        //Debug.Log("StartmenuEnabled");
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void StartMenuEnable()
    {
        startMenu.SetActive(true);
        game.SetActive(false);
        additionMenu.SetActive(false);
        cms.enabled = false;
        gm.isInStartMenu = true;
        gm.isInGame = false;
        gm.isInAddition = false;
        if (line==null)
        {
            Debug.Log("Лажа!");
        }
            line.SetVertexCount(0); 
    }
    public void AdditionMenuEnable()
    {
        startMenu.SetActive(false);
        game.SetActive(false);
        additionMenu.SetActive(true);
        cms.enabled = false;
        gm.isInStartMenu = false;
        gm.isInGame = false;
        gm.isInAddition = true;
        gm.drawArea = additionDrawArea;
        gm.GetComponent<LineRenderer>().enabled = true;
    }
    public void Game()
    {
        startMenu.SetActive(false);
        game.SetActive(true);
        additionMenu.SetActive(false);
        cms.enabled = true;
        gm.isInStartMenu = false;
        gm.isInGame = true;
        gm.isInAddition = false;
        gm.drawArea.gameObject.SetActive(true);
        gm.drawArea = gameDrawArea;
    }
}
