using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes; 

public class ItemBin : MonoBehaviour
{
    public enum BinStatus
    {
        Empty,
        InProgress,
        Invalid,
        Broken,
        Complete
    }
    public List<ItemBase> items;
    public UnityEvent<ItemBin> BinChanged;
    public BinStatus status;
    public ItemBinData goalData;

    public bool doesBinMeetGoal
    {
        get => goalData.CheckBin(this); 
    }
    // Start is called before the first frame update
 

    // Update is called once per frame

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Item"))
            return;
        ItemBase item = other.GetComponent<ItemBase>();
        if (item)
        {
            if (!items.Contains(item))
            {
                BinChanged.Invoke(this);
                items.Add(item);
            }
        }
    }
 
    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Item")) 
            return;
        ItemBase item = other.GetComponent<ItemBase>();
        if (!item) 
            return;
        if (items.Contains(item))
        {
            items.Remove(item);
            BinChanged.Invoke(this);
        }
    }   
    
    [Button()]
    public void CalculateBinStatus()
    {
        print( doesBinMeetGoal);
    }
}
