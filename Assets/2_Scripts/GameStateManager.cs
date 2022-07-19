using System;
using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController.Jam;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GameStateManager : MonoBehaviour
{
    private static GameStateManager _instance;
    public static GameStateManager instance
    {
        get
        {
            if (_instance is null)
            {
                _instance = GameObject.FindObjectOfType<GameStateManager>(); 
            }
            return _instance; 
        }   
    }
    [SerializeField] public bool DebugMode;
    [SerializeField] public JamPlayer Player;
    [SerializeField] public CursorManager cursorManager;
    [SerializeField] public MainArm MainArm; 
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (DebugMode)
            {
                SetDebug(false); 
            }
            else
            {
                SetDebug(true); 
            }
        }
    }

    private void FixedUpdate()
    {
        foreach (ItemBase item in ItemBase.allItems)
        {
           ItemBase.PhysicsUpdate(item);
        }
    }
    void SetDebug(bool active)
    {
        DebugMode = active; 
        cursorManager.debug = active;
        //MainArm.debug = active; 
        print("Debug Mode: " + active);
    }
}
