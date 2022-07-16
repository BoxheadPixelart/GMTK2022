using System.Collections;
using System; 
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public class MainArm : ArmBase
{
    public CursorManager cursorManager;
    public Transform elbowJoint; 
    float handX;
    float handZ;
    public float handDist;
    public float handDir;
    float gHandDir;
    float gHandDist;
    float handDistVelo = 0;
    float handDamp = 1f;
    float handDiff;
    float handAngFreq = 1f;
    float handTime = 1;
    float handDirVelo;
    float handSize = 1;
    public bool handOpen;
    public float maxHandDist = 18f;
    public LineRenderer armRender;
    public Animator handAnim;
    public GameObject handModel;
    public Collider handCollider;
    public List<GameObject> itemsOverHand = new List<GameObject>();
    float maxThrowSterngth;
    public int itemSelection; 
    public bool debug;
    public UnityEvent HandOpenedEvent; 
    public UnityEvent HandClosedEvent;

    public Stats stats; 
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject manager = GameObject.FindGameObjectWithTag("GameManager"); 
        //
        cursorManager = manager.GetComponent<CursorManager>();
        stats = manager.GetComponent<Stats>(); 
        armRender = GetComponent<LineRenderer>();

        Cursor.lockState = CursorLockMode.Confined; 

        Cursor.visible = false;
        SetStats(stats); 
    }
    private void Update()
    {
  

        if (debug)
        {
            
        }
        else
        {
            if (Input.GetMouseButtonDown(0)) // closing hand
            {
                HandClosedEvent.Invoke(); 
                handOpen = false;
                handAnim.Play("Close Hand");
                //  handCollider.isTrigger = false;
                PickupMostRecent();

            }
            if (Input.GetMouseButtonUp(0)) // opening hand
            {
                HandOpenedEvent.Invoke(); 
                handOpen = true;
                handAnim.Play("Open Hand");
                //   handCollider.isTrigger = true;
                DropHeld();
            }
        }

        if (handDist < .4f)
        {
            if (handOpen)
            {
            
            }
            else
            {
                if (heldItem is null)
                {
                    print("Sneak is not holding anything");
                }
                else
                {
                
                }
             
            } 
        }
    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            if (!itemsOverHand.Contains(other.gameObject))
            {
                itemsOverHand.Add(other.gameObject);
            }
        }
    }
    //
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            itemsOverHand.Remove(other.gameObject);
        }
    }

    void PickupMostRecent()
    {
        if (itemsOverHand.Count > 0)
        {
            heldItem = itemsOverHand[itemsOverHand.Count - 1];
            ItemBase item = heldItem.GetComponent<ItemBase>();
            item?.Pickup(handJoint, this);
        }
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
        maxThrowSterngth = importedStats.maxThrowStrength; 
    }
    
}
