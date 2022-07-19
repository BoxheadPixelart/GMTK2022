using System.Collections;
using System; 
using System.Collections.Generic;
using KinematicCharacterController.Jam;
using NaughtyAttributes;
using Storm.Utils;
using UnityEngine;
using UnityEngine.Events; 

public class MainArm : ArmBase
{
    public Transform ikTarget; 
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
        
        if (!isPicking)
        {
            SetIdle();   
        }
 
            base.FixedUpdate();
    }

    // Update is called once per frame

    public void Update()
    {
        ikTarget.position = transform.position;
        ikTarget.rotation = transform.rotation; 
    }

    public void PickupItem(ItemBase item)
    {
        print("PICK UP");
        StartCoroutine(PickupRoutine(item)); 

    }
    [ReadOnly]
    public bool isPicking; 
    public IEnumerator PickupRoutine(ItemBase item)
    {
        isPicking = true; 
        SetGoalPosition(Camera.main.transform.InverseTransformPoint(item.transform.position));
        yield return new WaitForSeconds(.25f); 
        GrabItem(item);  
        yield return new WaitForSeconds(.1f); 
        SetGoalPosition(idlePose.localPosition);
        isPicking = false; 
        Invoke(nameof(cacheAction),0.1f);
        yield return null;
    }
    public Action cacheAction;

    public void QueueAction(Action a)
    {
        cacheAction = a; 
    }

    public void RunQueueAction()
    {
        
        if (cacheAction is null){ return;}
        cacheAction.Invoke();
        cacheAction = null; 
        
    }

    public void QueueDrop()
    {
        QueueAction(DropHeld);
    }

    void GrabItem(ItemBase item)
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
        SetGoalPosition(idlePose.localPosition);
        SetGoalRotation(idlePose.eulerAngles);
    }
    
    
}
