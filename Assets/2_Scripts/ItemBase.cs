using System;
using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController.Jam;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;


[ExecuteInEditMode]
    public class ItemBase : MonoBehaviour // this class is shared by ALL ITEMS and must include features for EVERY ITEM
{
    public static List<ItemBase> allItems = new List<ItemBase>();
        [SerializeField] public ItemData itemData;
        // private ItemData cachedItemData; 
        [ReadOnly]
        bool isPickedUp;
        [ReadOnly]
        public Transform pickupTarget;
        [ReadOnly]
        public Vector3 velo;

        private ArmBase holder;
        private Vector3 gItemDir;
        private Vector3 itemDirVelo;
        public ItemBin itembBin; 
        public bool isSuspended;
        public int binIndex; 
        // Item Data Stuff that defines indivdual items; 
        public Rigidbody rb;
        

        void Start()
        {
            if (!allItems.Contains(this))
            {
                            allItems.Add(this);
                        }
            rb = GetComponent<Rigidbody>();
            gameObject.layer = LayerMask.NameToLayer("Item");
            //UpdateItemData(itemData);
            
        }
     
        
        private void OnDestroy()
        {
            if (allItems.Contains(this))
            {
                allItems.Remove(this); 
            }
        }

   

        public static void PhysicsUpdate(ItemBase item)
        {
            if (item.isPickedUp) //this looks fucking gross and you should feel bad
            {
                MoveToHand(item); 
            }
            else
            {
                if (item.itembBin)
                {
                    Vector3 dist = Vector3.ClampMagnitude(item.itembBin.itemPositions[item.binIndex].position - item.rb.position, 2); 
                    item.rb.velocity += (dist - item.rb.velocity) * .9f;
                }
                else
                {
                    item.rb.useGravity = true; 
                }
            }
        }

        public static void SetBinIndex(int index)
        {
            
        }

        public static void MoveToHand(ItemBase item)
        {
            item.transform.parent = item.pickupTarget; 
            
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }

        void SpringRotateToHand()
        {
            Vector3 look = (pickupTarget.position - holder.transform.position);
            Quaternion rot = Quaternion.LookRotation(look, Vector3.up);
            rb.rotation = rot; 
        }
        public void Pickup(Transform pT, ArmBase hold)
        {
            gameObject.layer = LayerMask.NameToLayer("ItemHeld");
            rb.isKinematic = true;
            pickupTarget = pT;
            isPickedUp = true;
            holder = hold;
        }

        public void Drop()
        {
            rb.isKinematic = false; 
            gameObject.layer = LayerMask.NameToLayer("Item");
            transform.parent = null; 
            isPickedUp = false;
            pickupTarget = null;
            holder = null;
        }
        public void SetVelocity(Vector3 desiredVelocity)
        {
            rb.velocity = desiredVelocity;
        }
        public void AddVelocity(Vector3 desiredVelocity)
        {
            rb.velocity += desiredVelocity;
        }
        public void ResetAngularVelocity()
        {
            rb.angularVelocity = Vector3.zero; 
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (isPickedUp)
            {
               //
            }
        print("Item has collided"); 
        CollisionAction(collision); 
        }

        public void CollisionAction(Collision collision)
        {
            
        }

    }

