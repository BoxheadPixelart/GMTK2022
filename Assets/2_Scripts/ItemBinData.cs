using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New ItemBinData", menuName = "ItemBin Data", order = 51)]
public class ItemBinData : ScriptableObject
{
    [System.Serializable]
    public class BinGoal
    {
        public ItemData data;
        public int amount;
    }

    public List<BinGoal> Goals;


    public bool CheckBin(ItemBin bin)
    {
        List<bool> success = new List<bool>(Goals.Count);
        foreach (var goal in Goals)
        {
            success.Add(false);
        }
        for (int i = 0; i < Goals.Count; i++)
        {
            int total = 0; 
            foreach (var item in bin.items)
            {
                if (Goals[i].data == item.itemData)
                {
                    total += 1; 
                    
                }
            }
            if (total >= Goals[i].amount)
            {
                success[i] = true; 
            }
        }
        
        return !success.Contains(false);
    }


}
