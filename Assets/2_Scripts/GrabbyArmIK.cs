using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Storm.Utils;
using UnityEngine;

public class GrabbyArmIK : MonoBehaviour
{
    public Transform[] joints;
    [ReadOnly]
    public Vector3 aimVector;


    public Vector3[] goalPositions = new Vector3[2]; 
    public Transform ikTarget; 
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public Numeric_Springing.Vector3Spring elbowSpring; 
    public Numeric_Springing.Vector3Spring handSpring;
    public Vector3 rotOffset;

    public Vector3 elbowRotOffset; 
    // Update is called once per frame
    void Update()
    {
    
            var position = ikTarget.position;
        aimVector = position - joints[0].position;
        goalPositions[0] = joints[0].position; 
        goalPositions[2] = position;
        goalPositions[1] =  joints[0].position + aimVector/2;
        
        

      
        Quaternion shoulderRot = Quaternion.LookRotation(aimVector,-ikTarget.right);
        joints[0].rotation = shoulderRot * Quaternion.Euler(90,0,0);
        //
        
        Quaternion elbow =  Quaternion.LookRotation(position - joints[1].position, -ikTarget.right); 
        joints[1].rotation = elbow * Quaternion.Euler(90,0,0);
        
        //
        //joints[0].rotation = 
        
        joints[0].position = goalPositions[0];
        joints[1].position = elbowSpring.Spring(goalPositions[1]).output; 
        joints[2].position = handSpring.Spring(goalPositions[2]).output; 
        
        //



    }
}
