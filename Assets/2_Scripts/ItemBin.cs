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
    
    public List<Transform> itemPositions;
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
            if (items.Count >= itemPositions.Count)
            {
                return;
            }
            
            if (!items.Contains(item))
            {
                item.itembBin = this;
                item.binIndex = items.Count; 
                item.isSuspended = true; 
                item.rb.useGravity = false; 
                items.Add(item);
                BinChanged.Invoke(this);
                
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
            item.itembBin = null; 
            item.isSuspended = false;
            item.rb.useGravity = true;
            item.binIndex = -1; 
            items.Remove(item);
            BinChanged.Invoke(this);
        }
        ResetAllItemBinIndex(); 
    }

    public void ResetAllItemBinIndex()
    {
        for (int i = 0; i <= items.Count - 1; i++)
        {
            items[i].binIndex = i; 
        }
    }

  //  public int FirstOpenIndex()
    //{
       
    //}
    
    [Button()]
    public void CalculateBinStatus()
    {
        print( doesBinMeetGoal);
    }
}
