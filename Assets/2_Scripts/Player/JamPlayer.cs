using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using KinematicCharacterController;
using KinematicCharacterController.Examples;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;

namespace KinematicCharacterController.Jam
{
    public class JamPlayer : MonoBehaviour
    {
        public JamCharacterController Character;

        private const string MouseXInput = "Mouse X";
        private const string MouseYInput = "Mouse Y";
        private const string MouseScrollInput = "Mouse ScrollWheel";
        private const string HorizontalInput = "Horizontal";
        private const string VerticalInput = "Vertical";
        public PlayerInputs inputs = new PlayerInputs();
        public Stats stats; 
        private static JamPlayer _player;
        public static JamPlayer player
        {
            get
            {
                if (_player is null)
                {
                    _player = GameObject.FindObjectOfType<JamPlayer>(); 
                }

                return _player; 
            }
        }


        public class PlayerInputs
        {
            public bool mainHandInteract;
            public bool mainHandInteractHeld;
            public bool mainHandInteractReleased;

            public bool mainHandThrowReleased; 
            public bool mainHandThrowHeld;
            public bool mainHandThrowDown; 
            
            public bool offHandInteract;
            public bool offHandInteractReleased; 
            public bool offHandInteractHeld;

            public bool petHand;
            public bool badHand;

            public bool robotCommandMod; 

            public void UpdateInputs()
            {
                robotCommandMod = Input.GetKeyDown(KeyCode.LeftShift);

                if (robotCommandMod)
                {
                    offHandInteract = Input.GetMouseButtonDown(0);
                    offHandInteractHeld = Input.GetMouseButton(0);
                    offHandInteractReleased = Input.GetMouseButtonUp(0);
                    mainHandInteract = false;
                    mainHandInteractHeld = false;
                    mainHandInteractReleased = false; 
                }
                else
                {
                    mainHandInteract = Input.GetMouseButtonDown(0);
                    mainHandInteractHeld = Input.GetMouseButton(0);
                    mainHandInteractReleased = Input.GetMouseButtonUp(0);
                    mainHandThrowReleased = Input.GetMouseButtonUp(1);
                     mainHandThrowHeld = Input.GetMouseButton(1);
                     mainHandThrowDown = Input.GetMouseButtonDown(1); 
                }

            }
            



        }
        

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;

            // Tell camera to follow transform
          

            // Ignore the character's collider(s) for camera obstruction checks
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            
            inputs.UpdateInputs();

            PlayerArmBehaviour(inputs,CursorManager.manager.playerView);
            
            HandleCharacterInput();
            
        }
        private void HandleCharacterInput()
        {
            PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();
            // Build the CharacterInputs struct
            characterInputs.MoveAxisForward = Input.GetAxisRaw(VerticalInput);
            characterInputs.MoveAxisRight = Input.GetAxisRaw(HorizontalInput);
            characterInputs.CameraRotation = Camera.main.transform.rotation;
            characterInputs.JumpDown = Input.GetKeyDown(KeyCode.Space);
            characterInputs.CrouchDown = Input.GetKeyDown(KeyCode.C);
            characterInputs.CrouchUp = Input.GetKeyUp(KeyCode.C);
            // Apply inputs to character
            Character.SetInputs(ref characterInputs);
        }

        public float chargeTime; 
        void PlayerArmBehaviour(PlayerInputs inputs,CursorManager.PlayerView view) // THIS IS FUCKING GROSS
        {  
            if (inputs.mainHandInteract)
            {  
                if (MainArm.arm.heldItem && !MainArm.arm.isPicking)
                {
                    MainArm.arm.DropHeld();
                    return;
                }
                print("Picked Up");
                if (view.viewItem)
                {
                    print("Picked Up See");
                    if (view.viewItem.itemData.canBeGrabbed)
                    {
                        print("Picked Up Grab");
                        MainArm.arm.PickupItem(view.viewItem);
                    }
                } 
            }

            if (inputs.mainHandInteractReleased)
            {
                if (MainArm.arm.heldItem && !MainArm.arm.isPicking)
                {
                    MainArm.arm.DropHeld();
                }
            }
            if (inputs.mainHandThrowDown)
            {
                chargeTime = Time.unscaledTime; 
            }
            
                if (inputs.mainHandThrowReleased)
                {
                    if (MainArm.arm.heldItem && !MainArm.arm.isPicking)
                    {
                        float scaler = (Time.unscaledTime - chargeTime); 
                        print(scaler);
                        MainArm.arm.ThrowItem( (5 * Mathf.Clamp(scaler * 2, 0, 2)));
                    }
                    chargeTime = 0; 
                }
                
            
            ///
        }
    }
    
}