using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop: MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public static GameObject itemBeingDragged;
    public static bool inSlot=true;
<<<<<<< HEAD
    public SlotScript CommandBuilder;
=======
    public CommandBuilder сommandBuilder;
>>>>>>> temporary-artem

    public void OnBeginDrag(PointerEventData eventData)
    {     
        inSlot = true;
      
        itemBeingDragged = gameObject;
<<<<<<< HEAD
        itemBeingDragged.transform.SetParent(CommandBuilder.transform);
        CommandBuilder.UIElementRemoveFromGroup(this.GetComponent<UICommand>());
=======
        itemBeingDragged.transform.SetParent(сommandBuilder.transform);
        сommandBuilder.UIElementRemoveFromGroup(this.GetComponent<UICommand>());
>>>>>>> temporary-artem
                   
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
<<<<<<< HEAD
                CommandBuilder.OnDrop(eventData);
=======
                сommandBuilder.OnDrop(eventData);
>>>>>>> temporary-artem
            }
    }
    
    // Use this for initialization
    void Start()
    {
<<<<<<< HEAD
        CommandBuilder= GameObject.Find("CommandBuilder").GetComponent<SlotScript>();
=======
        сommandBuilder= GameObject.Find("CommandBuilder").GetComponent<CommandBuilder>();
>>>>>>> temporary-artem
    }
}
