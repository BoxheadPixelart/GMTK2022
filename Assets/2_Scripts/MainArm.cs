using System.Collections;
using System; 
using System.Collections.Generic;
using KinematicCharacterController.Jam;
using Storm.Utils;
using UnityEngine;
using UnityEngine.Events; 

public class MainArm : ArmBase
{
    public Transform idlePose; 
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
    private void Awake()
    {
        SetIdle();
    }
    // Start is called before the first frame update
    public override void FixedUpdate()
    {
        SetIdle(); 
            base.FixedUpdate();
    }

    // Update is called once per frame



    public void PickupItem(ItemBase item)
    {
        heldItem = item.gameObject;
            item.Pickup(handJoint,this);
    }
    
    void DropHeld()
    {
        if (heldItem)
        {
            ItemBase item = heldItem.GetComponent<ItemBase>();
            item?.Drop();
            Vector3 launchVector = item.velo + (Vector3.up * (4 + (item.velo.magnitude * .4f))); 
            item?.SetVelocity(Vector3.ClampMagnitude(launchVector,10)); 
            heldItem = null;
        }
    }
    
    void SetStats(Stats importedStats)
    {
     
    }

    
    public void SetIdle()
    {
        SetGoalPosition(idlePose.position);
        SetGoalRotation(idlePose.eulerAngles);
    }
    
    
}
