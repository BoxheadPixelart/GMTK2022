using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmBase : MonoBehaviour
{
    public float maxThrowStrength; 
    public GameObject heldItem;
    public Transform handJoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
    
    public void ForceDropHeld()
    {
        if (heldItem)
        {
            ItemBase item = heldItem.GetComponent<ItemBase>();
            if (item != null)
            {
                item.Drop();
                Vector3 launchVector = item.velo + (Vector3.up * (4 + (item.velo.magnitude * .4f)));
                item.SetVelocity(Vector3.ClampMagnitude(launchVector, maxThrowStrength/3));
                item.ResetAngularVelocity();
                heldItem = null;
            }
        }
    }
}
