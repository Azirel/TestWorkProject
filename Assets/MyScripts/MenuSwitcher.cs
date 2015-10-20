using UnityEngine;
using System.Collections;

public class MenuSwitcher : MonoBehaviour
{

    public GameObject game;
    public GameObject startMenu;
    public GameObject additionMenu;
    public GameObject drawArea;
    CursorManagerScript cms;
    GeneralManagment gm;

    // Use this for initialization
    void Start()
    {
        cms = GetComponent<CursorManagerScript>();
        gm = GetComponent<GeneralManagment>();
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
        drawArea.SetActive(false);
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
        drawArea.SetActive(true);
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
        drawArea.SetActive(true);
    }
}
