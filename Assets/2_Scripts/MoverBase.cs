using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Storm.Utils;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering; 
using UnityEditor;
using UnityEngine.UIElements.Experimental;

namespace Storm.CharacterController
{


    public class MoverBase : MonoBehaviour
    {
        // Start is called before the first frame update
        public Transform head; 
        [SerializeField] Rigidbody rb;
        [SerializeField] float liftForce;
       // [SerializeField] LayerMask layerMask;
       // [SerializeField] float maxLiftDist;
       // [SerializeField] float goalHeight;
       // [SerializeField] float springStrength;
       // [SerializeField] float springDamp;
       // [SerializeField] float uprightSpringStrength; 
       // [SerializeField] float uprightSpringDamp; 
        [SerializeField] Quaternion uprightTargetRot;
        [SerializeField] Vector3 raycastDir;
        private Vector3 goalVelo;
        private Vector3 moveUnit;
        private Vector3  m_maxAccel;
        public LayerMask layerMask;
        public float speedScaler;

        public float moveAngle;
        public float turnPower = 1; 

        public bool isMove;
        public bool addLeft;
        public bool addRight;





        [SerializeField] MoverData data;
        
        Vector3 floorPos;
        
        public Vector3 moveDir; 
        
        
        private void Awake()
        {
            rb = GetComponent<Rigidbody>(); 
        }
        void Start()
        {
            
            uprightTargetRot = Quaternion.Euler(transform.up);
            
            
            raycastDir = Vector3.down; 
               
        }
        private void FixedUpdate()
        {
            ForceMoveToPos(); 
            UpdateUprightForce();
        }

         void SpringFloatToPos()
        {
            if (rb && Physics.Raycast(transform.position, raycastDir, out RaycastHit hit, data.maxLiftDist, data.layerMask))
            {
                Vector3 otherVel = Vector3.zero;
                Vector3 rayDir = raycastDir;
                floorPos = hit.point; 
                float dist = hit.distance - data.goalHeight;
                float rayDirVel = Vector3.Dot(raycastDir, rb.velocity);
                float otherDirVel = Vector3.Dot(rayDir,otherVel); 
                float relVel = (rayDirVel - otherDirVel); 
                float springForce = (dist * (data.springStrength * rb.mass)) - (relVel * data.springDamp); 
                rb.AddForce(rayDir * (springForce), ForceMode.Force);
            }
        }
         private Vector3 goalVel;
         public Transform goalTransform;
         public Vector3 goalPoint; 
         protected Vector3 targetPos;
        void ForceMoveToPos()
        {
            if (rb){
                Vector3 pos = goalPos.position;
                Vector3 dist = targetPos -  rb.position;
                if (goalTransform != null)
                {
                    dist = goalTransform.position - rb.position; 
                    print("TRANSFORM GOING");
                }
                else
                {
                    print("Point GOING");

                    dist = goalPoint - rb.position; 
                    print("TRANSFORM Went");

                }
         
                goalVel = (dist.normalized) * ((data.maxAccelForce * Mathf.InverseLerp(0,2,Mathf.Clamp(dist.magnitude,0,2))) );
                Vector3 mGoalVel = goalVel;
                Vector3 neededAccel = (mGoalVel - rb.velocity) / (Time.fixedDeltaTime);
                rb.AddForce(Vector3.Scale((neededAccel * (rb.mass)),data.ForceScale));
            }
            else
            {
                rb = GetComponentInChildren<Rigidbody>();
            }
        }
        public Transform goalPos; 
        public void FloatForce()
        {
            Vector3 vel = rb.velocity;
            Vector3 rayDir = transform.TransformDirection(Vector3.down); 

            Vector3 otherVel = Vector3.zero; 
                
            float rayDirVel = Vector3.Dot(raycastDir, vel);
            float otherDirVel = Vector3.Dot(rayDir,otherVel);
                
            float relVel = (rayDirVel - otherDirVel);

            float x = (transform.position - goalPos.position).magnitude; 
                
            float springForce = (x * (data.springStrength)) - (relVel * data.springDamp); 
                
            rb.AddForce(rayDir * springForce);  
        }

        
        public Numeric_Springing.Vector3Spring moveDirSpring = new Numeric_Springing.Vector3Spring(.05f,.9f,1);
        private Vector3 goalMove;
        void SetMoveDir(float hMove, float vMove, float uMove)
        {
            Vector3 inputDir = new Vector3(hMove, uMove, vMove);
            Vector3 fOffset = transform.forward * inputDir.z;
            Vector3 hOffset = transform.right * inputDir.x;
            Vector3 uOffset = transform.up * inputDir.y; 
            Vector3 offsetPos = fOffset + hOffset + uOffset;

            moveDir = moveDirSpring.Spring(offsetPos.normalized).output; 
            
            // moveDir = (transform.forward + inputDir).normalized;//((transform.position + inputDir) * (isMove ? 1f : 0f)) * speedScaler;  
            Debug.DrawRay(transform.position,moveDir * 3.5f,Color.green);
            Debug.DrawRay(transform.position,transform.forward * 3,Color.cyan);
            Debug.DrawRay(transform.position,offsetPos * 4,Color.red);
            //

            
        }

      

        void Move()
        {
            //moveDir = moveDir.normalized; 
            
            Vector3 move = moveDir;
            
            if (move.magnitude > 1.0f)
            {
                move.Normalize();
            }
            
            moveUnit = move; 

            Vector3 unitVel = goalVelo.normalized;
            float velDot = Vector3.Dot(moveUnit,unitVel);
            float accel = data.Accel * data.AccelerationFactorFromDot.Evaluate(velDot); 
            
            Vector3 goalVel = (moveUnit * data.maxSpeed) * data.speedScale;
            
            goalVelo = Vector3.MoveTowards(goalVelo, (goalVel + Vector3.zero),
                accel * Time.deltaTime);
            
            Vector3 neededAccel = (goalVelo - rb.velocity) / Time.deltaTime; 
            float maxAccel = data.maxAccelForce * data.MaxAccelerationForceFactorFromDot.Evaluate(velDot) * data.maxAccelForce;
            neededAccel = Vector3.ClampMagnitude(neededAccel,maxAccel);
            rb.AddForce(Vector3.Scale(neededAccel * rb.mass,data.ForceScale));
                    
    
            //Vector3 unitVel = m_GoalVel.normalized; 

            
        }
        void UpdateUprightForce()
        {
            Quaternion CharacterCurrent = transform.rotation;
           Quaternion toGoal = MathUtils.ShortestRotation(uprightTargetRot, CharacterCurrent);
            Vector3 rotAxis;
            float rotDegrees;
            toGoal.ToAngleAxis(out rotDegrees, out rotAxis);
            rotAxis.Normalize();
           float rotRadians = rotDegrees * Mathf.Deg2Rad;
           print(data);
           print(data.uprightSpringStrength);
           print(data.uprightSpringDamp);
            rb.AddTorque((rotAxis * (rotRadians * data.uprightSpringStrength)) - (rb.angularVelocity * data.uprightSpringDamp)); 
        }

        public Vector3 lookDir;
        public Quaternion lookRot; 
        void Steer()
        {
            
         //   lookDir = PlayerController.Input
            uprightTargetRot = head.rotation;   //quaternion.LookRotation( lookDir ,Vector3.up);
            head.position = transform.position; 
      

        }
        [Range(0.1f, 9f)][SerializeField] float sensitivity = 2f;
        [Tooltip("Limits vertical camera rotation. Prevents the flipping that happens when rotation goes above 90.")]
        [Range(0f, 90f)][SerializeField] float yRotationLimit = 88f;
        void SetViewDir(float hLook,float vLook, float tiltLook)
        {
           Vector3 inputDir = new Vector3(hLook,vLook ,0);
           // //
           inputDir.y = Mathf.Clamp(inputDir.y, -yRotationLimit, yRotationLimit);
           var xQuat = Quaternion.AngleAxis(inputDir.x, Vector3.up);
           var yQuat = Quaternion.AngleAxis(inputDir.y, Vector3.left);

           Quaternion rotChange = xQuat * yQuat; //Quaternions seem to rotate more consistently than EulerAngles. Sensitivity seemed to change slightly at certain degrees using Euler. transform.localEulerAngles = new Vector3(-rotation.y, rotation.x, 0);
//
           head.rotation *= rotChange;
           head.transform.position = transform.position;
            
            //
        }
        
        public void AddMoveAngle( int sign)
        {
            moveAngle += turnPower * Mathf.Sign(sign);
            
            float result = moveAngle - Mathf.CeilToInt(moveAngle / 360f) * 360f;
            if (result < 0)
            {
                result += 360f;
            }
            moveAngle = result; 
        }
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {

            Vector3 endPos = transform.position + (raycastDir * data.maxLiftDist);

            Handles.color = Color.red;
            Vector3 drawPos = new Vector3(floorPos.x, floorPos.y + data.goalHeight, floorPos.z); 
            Handles.DrawSolidDisc(drawPos, Vector3.up,.5f);
            Handles.color = Color.green;
            Handles.DrawSolidDisc(floorPos, Vector3.up, .25f);
            Handles.color = Color.gray;
            Handles.DrawLine(transform.position, endPos);
            Handles.color = Color.white;
        }
#endif
    }


}
