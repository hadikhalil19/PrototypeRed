// Scriptable Object for Item Info

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newItemInfo", menuName = "Prototype/ItemInfo")]
public class ItemInfo : ScriptableObject
{
   public GameObject itemPrefab;
    public float itemCooldown;
    public int itemGoldCost;
    public int itemManaCost;
    public int itemHealthCost;
    public int itemHealAmount;
}
