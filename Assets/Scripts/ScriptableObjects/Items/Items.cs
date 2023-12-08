using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Items : ScriptableObject
{
    [Header("Only gameplay")]
    
    public ItemType itemType;
    public ActionType actionType;
    public Vector2Int range = new Vector2Int(5,4);

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
    EquipWeapon,
    UseTool,
    UseMiscItem
}
