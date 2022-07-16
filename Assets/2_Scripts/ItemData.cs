using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New ItemData", menuName = "Item Data", order = 51)]

public class ItemData : ScriptableObject
{
    [SerializeField] public string itemName; 
    [SerializeField] public Mesh itemMesh;
    [SerializeField] public float itemMass;
    [SerializeField] public Material itemMaterial;
    [SerializeField] public Vector3 itemColliderCenter; 
    [SerializeField] public Vector3 itemColliderSize;
    [SerializeField] public Vector3 itemGrabPosition;
    [SerializeField] public float itemRadius; 
    [SerializeField] public float itemHeight;

} 
