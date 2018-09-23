using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop: MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public static GameObject itemBeingDragged;
    public static bool inSlot=true;
    public CommandBuilder сommandBuilder;

    public void OnBeginDrag(PointerEventData eventData)
    {     
        inSlot = true;
      
        itemBeingDragged = gameObject;
        itemBeingDragged.transform.SetParent(сommandBuilder.transform);
        сommandBuilder.UIElementRemoveFromGroup(this.GetComponent<UICommand>());
                   
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
            if (inSlot)
            {
                itemBeingDragged.transform.position = new Vector3(itemBeingDragged.transform.position.x, eventData.position.y);
            }
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
    
        GetComponent<CanvasGroup>().blocksRaycasts = true;
            if (!inSlot)
            {
                сommandBuilder.OnDrop(eventData);
            }
    }
    
    // Use this for initialization
    void Start()
    {
        сommandBuilder= GameObject.Find("CommandBuilder").GetComponent<CommandBuilder>();
    }
}
