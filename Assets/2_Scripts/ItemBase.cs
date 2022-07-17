using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
    public class ItemBase : MonoBehaviour // this class is shared by ALL ITEMS and must include features for EVERY ITEM
    {
        [SerializeField] public ItemData _PublicitemData;
        private ItemData _itemData;
        // private ItemData cachedItemData; 
        bool isPickedUp;
        public Transform pickupTarget;
        public Vector3 velo;
        public Transform rotTarget;
        private ArmBase holder;
        private Vector3 itemDir;
        private Vector3 gItemDir;
        private Vector3 itemDirVelo;
        public Vector3 debugRot;
        public Stats stats; 
        // Item Data Stuff that defines indivdual items; 
        private Rigidbody rb;

        public ItemData LocalItemData
        {
            get
            {
                return _itemData;
            }
            set
            {
                print("Item Data has been updated");
                if (_itemData == value)
                {
                    print("Item Data Is the same");
                    return;
                }
                
                _itemData = value;
                if (OnItemDataChange != null)
                {
                    OnItemDataChange(_itemData);
                }
            }
        }
        public delegate void OnDataChangeDelegate(ItemData newItemData);
        public event OnDataChangeDelegate OnItemDataChange;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            gameObject.layer = LayerMask.NameToLayer("Item");
            //UpdateItemData(itemData);
            OnItemDataChange += UpdateItemData;
        }
        public void UpdateItemData(ItemData newItemData)
        {
            print("Update Item Method has been called");
            print(gameObject.name + " is being updated to have " + newItemData.name + "'s Item Data");
            gameObject.name = newItemData.name;
            rb.mass = newItemData.itemMass;
            print("Incoming Data has a different Collider");

        }
        
        private void FixedUpdate()
        {
            velo = rb.velocity;
            if (isPickedUp) //this looks fucking gross and you should feel bad
            {
                MoveToHand(); 
            }
            else
            {
                itemDir = rb.rotation.eulerAngles; 
            }
        }

        private void OnValidate()
        {
            LocalItemData = _PublicitemData; 
            print("Variable has been changed");
        }
        

        public void MoveToHand()
        {
            transform.parent = pickupTarget; 
            
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
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
                if (velo.magnitude > stats.maxThrowStrength)
                {
                    holder.ForceDropHeld();
                }
            }
        print("Item has collided"); 
        CollisionAction(collision); 
        }

        public void CollisionAction(Collision collision)
        {
            
        }
        public void SaveItemDataToScriptableObject()
        {
            if (_itemData is null)
            {
                print("There is no data on this item. You cannot save the item's data to nothing... what are you doing?");
            }
            else
            {
                _itemData.name = gameObject.name;
            //    _itemData.itemMesh = meshFilter.mesh;
            //    _itemData.itemMaterial = renderer.material;
               // _itemData.itemColliderSize = collider.size;
              //  _itemData.itemColliderCenter = collider.center;
                _itemData.itemMass = rb.mass;
                print(gameObject.name + "'s Data has been saved to the " + _itemData.name + " Data");
            }
        }

}

