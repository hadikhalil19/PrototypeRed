using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Runtime.CompilerServices;

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Item item;
    private ActiveInventory activeInventoryOld;
    private InventorySlot inventorySlotOld;

    [Header("UI")]
    public Image image;
    public int stackCount = 1;
    public TextMeshProUGUI countText;
    [HideInInspector] public Transform parentAfterDrag;

    private void Start() {
        InitialiizeItem(item);
    }

    public void InitialiizeItem(Item newitem) {
        if (newitem != null) {
            item = newitem;
            image.sprite = newitem.image;
            RefreshCount();
        }
        
    }

    public void RefreshCount() {
        countText.text = stackCount.ToString();
        bool textActive = stackCount > 1;
        countText.gameObject.SetActive(textActive);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("Start dragging " + this.name + "and current parent is" + transform.parent.name);
        
        inventorySlotOld = transform.parent.GetComponent<InventorySlot>();
        activeInventoryOld = transform.parent.GetComponentInParent<ActiveInventory>();
        
        parentAfterDrag = transform.parent;

        if (transform.root.gameObject.name == "UI_Canvas") {
            //Debug.Log("Root is UI_Canvas");
           transform.SetParent(transform.root);
        } else {
            Transform CanvasTransform = GetComponentInParent<Canvas>().transform; 
            transform.SetParent(CanvasTransform);
            //Debug.Log("Root is: " + transform.root.gameObject.name + " and setparent is: " +  CanvasTransform.gameObject.name);
            
        }

        
        transform.SetAsLastSibling();
        
        image.raycastTarget =false;
        countText.raycastTarget = false;

        
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Dragging");
        //transform.position = Input.mousePosition;
        var screenPoint = new Vector3 (Input.mousePosition.x,Input.mousePosition.y, 10.0f);
        transform.position = screenPoint;
        //transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
        //Debug.Log (Input.mousePosition.x + " " + Input.mousePosition.y +" "+ itemDragged.transform.localPosition);

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("Stopped dragging " + this.name + "and new parent is" + parentAfterDrag.name);
        
        transform.SetParent(parentAfterDrag);
        image.raycastTarget =true;
        countText.raycastTarget = true;
        
        parentAfterDrag.GetComponent<InventorySlot>().InfoUpdate();
        
        if(inventorySlotOld != null) {
            inventorySlotOld.InfoMakeEmpty();
        }
        ActiveInventory activeInventory = parentAfterDrag.GetComponentInParent<ActiveInventory>();
        if (activeInventory) {
            activeInventory.ChangeEquipedItem();
        }
        if (activeInventoryOld) {
            activeInventoryOld.ChangeEquipedItem();
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
