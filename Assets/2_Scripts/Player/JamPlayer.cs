using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using KinematicCharacterController;
using KinematicCharacterController.Examples;
using Unity.VisualScripting;

namespace KinematicCharacterController.Jam
{
    public class JamPlayer : MonoBehaviour
    {
        public JamCharacterController Character;
        public CinemachineVirtualCamera CharacterCamera;

        private const string MouseXInput = "Mouse X";
        private const string MouseYInput = "Mouse Y";
        private const string MouseScrollInput = "Mouse ScrollWheel";
        private const string HorizontalInput = "Horizontal";
        private const string VerticalInput = "Vertical";
        public PlayerInputs inputs = new PlayerInputs();

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

        void PlayerArmBehaviour(PlayerInputs inputs,CursorManager.PlayerView view) // THIS IS FUCKING GROSS
        {  
            if (inputs.mainHandInteract)
            {
                if (view.viewItem)
                {
                    if (view.viewItem._PublicitemData.canBeGrabbed)
                    {
                        MainArm.arm.PickupItem(view.viewItem);
                    }
                } 
            }

            if (inputs.mainHandInteractReleased)
            {
                if (MainArm.arm.heldItem)
                {
                    MainArm.arm.DropHeld();
                }
            }
            
            ///
        }
    }
    
}