using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newItem", menuName = "Prototype/Item")]
public class Item : ScriptableObject
{
    [Header("Only gameplay")]
    
    public ItemType itemType;
    public ActionType actionType;
    public Vector2Int range = new Vector2Int(5,4);
    public WeaponInfo weaponInfo;
    public ItemInfo itemInfo;

    [Header ("Only UI")]
    public bool stackable = true;

    [Header ("Both")]
    public Sprite image;
}

public enum ItemType {
    Tool,
    Weapon,
    Misc
}

public enum ActionType {
    UseTool,
    EquipWeapon,
    UseMiscItem
}
