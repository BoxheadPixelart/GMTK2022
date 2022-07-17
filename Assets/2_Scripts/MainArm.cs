using System.Collections;
using System; 
using System.Collections.Generic;
using KinematicCharacterController.Jam;
using UnityEngine;
using UnityEngine.Events; 

public class MainArm : ArmBase
{

    private static MainArm _arm;
    public static MainArm arm
    {
        get
        {
            if (_arm is null)
            {
                _arm = GameObject.FindObjectOfType<MainArm>(); 
            }

            return _arm; 
        }
    }
    
    public Stats stats; 
    
    // Start is called before the first frame update
    void Start()
    {
      
    }
    private void Update()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
    }

  

    public void PickupItem(ItemBase item)
    {
        heldItem = item.gameObject;
            item.Pickup(handJoint,this);
    }
    /*
    void DropHeld()
    {
        if (heldItem)
        {
            ItemBase item = heldItem.GetComponent<ItemBase>();
            item?.Drop();``
            Vector3 launchVector = item.velo + (Vector3.up * (4 + (item.velo.magnitude * .4f))); 
            item?.SetVelocity(Vector3.ClampMagnitude(launchVector,maxThrowSterngth)); 
            heldItem = null;
        }
    }
    */
    void SetStats(Stats importedStats)
    {
     
    }
    
}
