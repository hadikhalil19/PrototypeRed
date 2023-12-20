using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveMiscItem : Singleton<ActiveMiscItem>
{
    public MonoBehaviour CurrentActiveMiscItem {get; private set;}
    private PlayerControls playerControls;
    public bool MiscItemChanged = false;

    protected override void Awake() {
        base.Awake();
        playerControls = new PlayerControls();
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void Start()
    {
        playerControls.Inventory.UseItem.started += _ => UseActiveItem();

    }

    public void NewItem(MonoBehaviour newMiscItem) {
        CurrentActiveMiscItem = newMiscItem;
        MiscItemChanged = true;
    }


    private void UseActiveItem()
    {
        if (CurrentActiveMiscItem) {
            (CurrentActiveMiscItem as IMiscItem).UseActiveItem();
        }
    }

    public void MiscItemNull() {
        CurrentActiveMiscItem = null;
    }

    public void MiscItemReset() {
        (CurrentActiveMiscItem as IMiscItem).ItemReset();
    }



}
