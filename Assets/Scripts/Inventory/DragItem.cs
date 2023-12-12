using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Item item;
    private ActiveInventory activeInventoryOld;
    private InventorySlot inventorySlotOld;

    [Header("UI")]
    public Image image;
    [HideInInspector] public Transform parentAfterDrag;

    private void Start() {
        InitialiizeItem(item);
    }

    public void InitialiizeItem(Item newitem) {
        if (newitem != null) {
            item = newitem;
            image.sprite = newitem.image;
        }
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Start dragging " + this.name + "and current parent is" + transform.parent.name);
        inventorySlotOld = transform.parent.GetComponent<InventorySlot>();
        activeInventoryOld = transform.parent.GetComponentInParent<ActiveInventory>();
        
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget =false;

        
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Dragging");
        transform.position = Input.mousePosition;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("Stopped dragging " + this.name + "and new parent is" + parentAfterDrag.name);
        transform.SetParent(parentAfterDrag);
        image.raycastTarget =true;
        
        parentAfterDrag.GetComponent<InventorySlot>().InfoUpdate();
        
        if(inventorySlotOld != null) {
            inventorySlotOld.InfoMakeEmpty();
        }
        ActiveInventory activeInventory = parentAfterDrag.GetComponentInParent<ActiveInventory>();
        if (activeInventory) {
            activeInventory.ChangeActiveWeapon();
        }
        if (activeInventoryOld) {
            activeInventoryOld.ChangeActiveWeapon();
        }

        Debug.Log("Stopped dragging " + this.name + "and new parent is" + parentAfterDrag.name);
        
    }

//     void Update()
//     {
//         if (EventSystem.current.IsPointerOverGameObject())
//         {
//             Debug.Log("Its over UI elements");
//         }
//         else
//         {
//             Debug.Log("Its NOT over UI elements");
//         }
// }

}
