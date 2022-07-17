using System;
using System.Collections;
using System.Collections.Generic;
using Storm.CharacterController;
using UnityEngine;
using Storm.Utils;

public class ArmBase : MonoBehaviour
{
    public float maxThrowStrength;
    public GameObject heldItem;
    public Transform handJoint;
    public Numeric_Springing.Vector3Spring posSpring = new Numeric_Springing.Vector3Spring(1, 1, 1);
    public Vector3 goalPos;

    public Numeric_Springing.RotationSpring rotSpring = new Numeric_Springing.RotationSpring(1, 1, 1);
    public Vector3 goalRot;

    public bool isIdle; 
    public float lerpScaler = 0.1f;
    public MoverBase mover; 
    // Start is called before the first frame update

     public virtual void FixedUpdate()
     {
      
         

     }
    public void DropHeld()
    {
        if (heldItem)
        {
            ItemBase item = heldItem.GetComponent<ItemBase>();
            if (item != null)
            {
                item.Drop();
                Vector3 launchVector = item.velo + (Vector3.up * (4 + (item.velo.magnitude * .4f)));
                item.SetVelocity(Vector3.ClampMagnitude(launchVector, maxThrowStrength));
                item.ResetAngularVelocity();
                heldItem = null;
            }
        }
    }

    IEnumerator LerpAcrossInterval( float interval)
    {
        float fraction = 0.0f;
        float age = 0.0f;
        do
        {
            fraction = age / interval;
            age += Time.deltaTime;
 
            // Apply Lerp here using 'fraction' as controller alpha, as
            // fraction goes from 0.0 to 1.0 over time "interval"
 
            // for example:
        //    Vector3 position = Vector3.Lerp( startPosition, endPosition, fraction);
 
            // or alternately:
        //    float size = Mathf.Lerp( minSize, maxSize, fraction);
 
            // or for color:
       //     Color color = Color.Lerp( Color.red, Color.green, fraction);
 
            yield return null;
        } while( fraction < 1.0f);
    }
    public void ForceDropHeld()
    {
        if (heldItem)
        {
            ItemBase item = heldItem.GetComponent<ItemBase>();
            if (item != null)
            {
                item.Drop();
                Vector3 launchVector = item.velo + (Vector3.up * (4 + (item.velo.magnitude * .4f)));
                item.SetVelocity(Vector3.ClampMagnitude(launchVector, maxThrowStrength / 3));
                item.ResetAngularVelocity();
                heldItem = null;
            }
        }
    }
    
    public void SetGoalPosition(Vector3 pos)
    {
        goalPos = pos;
    }
    public void SetGoalRotation(Vector3 euler)
    {
        goalRot = euler; 
    }

}

  
