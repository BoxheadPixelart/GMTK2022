﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using KinematicCharacterController.Examples;
using Unity.VisualScripting;

namespace KinematicCharacterController.Jam
{
    public class JamPlayer : MonoBehaviour
    {
        public JamCharacterController Character;
        public JamCharacterCamera CharacterCamera;

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
            CharacterCamera.SetFollowTransform(Character.CameraFollowPoint);

            // Ignore the character's collider(s) for camera obstruction checks
            CharacterCamera.IgnoredColliders.Clear();
            CharacterCamera.IgnoredColliders.AddRange(Character.GetComponentsInChildren<Collider>());
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

        private void LateUpdate()
        {
            // Handle rotating the camera along with physics movers
            if (CharacterCamera.RotateWithPhysicsMover && Character.Motor.AttachedRigidbody != null)
            {
                CharacterCamera.PlanarDirection = Character.Motor.AttachedRigidbody.GetComponent<PhysicsMover>().RotationDeltaFromInterpolation * CharacterCamera.PlanarDirection;
                CharacterCamera.PlanarDirection = Vector3.ProjectOnPlane(CharacterCamera.PlanarDirection, Character.Motor.CharacterUp).normalized;
            }

            HandleCameraInput();
        }

        private void HandleCameraInput()
        {
            // Create the look input vector for the camera
            float mouseLookAxisUp = Input.GetAxisRaw(MouseYInput);
            float mouseLookAxisRight = Input.GetAxisRaw(MouseXInput);
            Vector3 lookInputVector = new Vector3(mouseLookAxisRight, mouseLookAxisUp, 0f);

            // Prevent moving the camera while the cursor isn't locked
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                lookInputVector = Vector3.zero;
            }

            // Input for zooming the camera (disabled in WebGL because it can cause problems)
            float scrollInput = -Input.GetAxis(MouseScrollInput);
#if UNITY_WEBGL
        scrollInput = 0f;
#endif

            // Apply inputs to the camera
            CharacterCamera.UpdateWithInput(Time.deltaTime, 0, lookInputVector);
        }

        private void HandleCharacterInput()
        {
            PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();
            // Build the CharacterInputs struct
            characterInputs.MoveAxisForward = Input.GetAxisRaw(VerticalInput);
            characterInputs.MoveAxisRight = Input.GetAxisRaw(HorizontalInput);
            characterInputs.CameraRotation = CharacterCamera.Transform.rotation;
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