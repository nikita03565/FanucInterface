using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop: MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler{
    public static GameObject itemBeingDragged;
    public static bool inSlot=true;
    public SlotScript CommandBuilder;

public void OnBeginDrag(PointerEventData eventData)
{
        
    inSlot = true;
  
    itemBeingDragged = gameObject;
    itemBeingDragged.transform.SetParent(CommandBuilder.transform);
    CommandBuilder.UIElementRemoveFromGroup(this.GetComponent<UICommand>());
            
        
    GetComponent<CanvasGroup>().blocksRaycasts = false;

}

public void OnDrag(PointerEventData eventData)
{

        if(inSlot)
            itemBeingDragged.transform.position = new Vector3(itemBeingDragged.transform.position.x, eventData.position.y);

        
}



public void OnEndDrag(PointerEventData eventData)
{

    GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (!inSlot)
            CommandBuilder.OnDrop(eventData);
      
    }

// Use this for initialization
void Start()
{

       CommandBuilder= GameObject.Find("CommandBuilder").GetComponent<SlotScript>();
}

// Update is called once per frame
void Update()
{

}
}
