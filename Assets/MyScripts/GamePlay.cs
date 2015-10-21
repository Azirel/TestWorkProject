using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using GestureRecognizer;
using UnityEngine.UI;

public class GamePlay : MonoBehaviour
{
    public GeneralManagment gm;
    bool isShow;
    List<string> gestureNames;
    System.Random rand;
    void Awake()
    {
        gestureNames = new List<string>();
        rand = new System.Random();
    }
    void OnEnable()
    {
        //Debug.Log("WokeUp!");
        LibraryForming();
    }
    void LibraryForming()
    {
        foreach (var gesture in gm.gl.Library)
        {
            if(!gestureNames.Contains(gesture.Name))
            {
                gestureNames.Add(gesture.Name);
            }
        }
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(isShow==true)
        {
            
        }
    }
}
