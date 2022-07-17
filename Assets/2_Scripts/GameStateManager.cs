using System;
using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController.Jam;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GameStateManager : MonoBehaviour
{
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
    void SetDebug(bool active)
    {
        DebugMode = active; 
        cursorManager.debug = active;
        //MainArm.debug = active; 
        print("Debug Mode: " + active);
    }
}
