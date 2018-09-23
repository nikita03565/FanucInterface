using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//rename

public class CommandBuilder : MonoBehaviour,IDropHandler,IPointerExitHandler, IPointerEnterHandler{
    public List<UICommand> UICommandElements = new List<UICommand>();
    //[SerializeField]
    public List<Command> CommandsSet = new List<Command>();
    public GameObject ComplexCommandPrefab;
    public GameObject CommandPrefab;
    Vector3 startCoord;
   
    float UICommandHeight;
   
    public InputField CommandName;

    void Start ()
    {
        UICommandHeight = UICommand.UISize;
	}
   
    public void AddUIElementToGroup(UICommand UIelement,  bool withCommand = true)
    {
        UICommandElements.Add(UIelement);
        //think about it
        if (withCommand)
        {
            if (UIelement.isComplex)
            {
                for (int i = 0; i < UIelement.GetNumberofCommands(); ++i)
                {
                    CommandsSet.Add(UIelement.GetComponent<UIComplexCommand>().CommandsSet[i]);
                }
            }
            else CommandsSet.Add(UIelement.command);
        }
    }

    public void AddUIElementToGroup(UICommand UIelement, int index, bool withCommand = true)
    {  
        UICommandElements.Insert(index, UIelement);
        int CommandIndex = GetCommandIndexbyUIElement(UIelement);
        if (withCommand)
            if (UIelement.isComplex)
            {
               
                for (int i = 0;  i < UIelement.GetNumberofCommands(); ++i)
                {
                    CommandsSet.Insert(CommandIndex + i, UIelement.GetComponent<UIComplexCommand>().CommandsSet[i]);
                }
            }
        else CommandsSet.Insert(CommandIndex, UIelement.command);
       
    }

    public void AddCommandToSet(Command command)
    {
        CommandsSet.Add(command);
    }
    
    public void UIElementRemoveFromGroup(UICommand UIelement, bool withCommand = true)
    {
        if (withCommand)
        {
            if (UIelement.isComplex)
            {
                CommandsSet.RemoveRange(GetCommandIndexbyUIElement(UIelement), UIelement.GetComponent<UIComplexCommand>().GetNumberofCommands());
            }
            else CommandsSet.Remove(UIelement.command);
        }
        UICommandElements.Remove(UIelement);
    }
    void Update()
    {
        if (DragDrop.itemBeingDragged)
        {
            for (int i = 0; i < UICommandElements.Count; ++i)
            {
                if (UICommandElements[i].transform.position.y < DragDrop.itemBeingDragged.transform.position.y)
                { 
                    DragDrop.itemBeingDragged.transform.SetSiblingIndex(i);
                    break;
                }
                else if (i == UICommandElements.Count - 1)
                {
                    DragDrop.itemBeingDragged.transform.SetSiblingIndex(i + 1);
                    //startCoord = DragDrop.itemBeingDragged.transform.position;
                }
            }
        }
    } 

    public int UIElementGetIndex(UICommand Element)
    {
        return UICommandElements.IndexOf(Element);
    }

    public int CommandGetindex(Command command)
    {
        return CommandsSet.IndexOf(command);
    }

    public int GetCommandIndexbyUIElement(UICommand Element)
    {
        int index = 0;
        for (int i = 0; i < UIElementGetIndex(Element); ++i)
            index+=UICommandElements[i].GetNumberofCommands();
        
        return index;
    }

    public void OnDrop(PointerEventData eventData)
    {   
        if (DragDrop.itemBeingDragged && DragDrop.itemBeingDragged.GetComponent<DragDrop>().сommandBuilder == this)
        { 
            AddUIElementToGroup(DragDrop.itemBeingDragged.GetComponent<UICommand>(), DragDrop.itemBeingDragged.transform.GetSiblingIndex());
            DragDrop.itemBeingDragged.transform.localPosition = new Vector3(DragDrop.itemBeingDragged.transform.localPosition.x,- (DragDrop.itemBeingDragged.transform.GetSiblingIndex() - (float)(UICommandElements.Count-1) / 2)  * UICommandHeight,0);

            DragDrop.itemBeingDragged = null;
        }
    }

    public void ResetBuilder()
    {
        CommandsSet.Clear();
        CommandName.GetComponentInChildren<Text>().text = "Enter the name";
        UICommandElements.Clear();
        CommandName.text = null;
        foreach (UICommand child in GetComponentsInChildren<UICommand>())
            Destroy(child.gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (DragDrop.itemBeingDragged)
            DragDrop.inSlot = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (DragDrop.itemBeingDragged)
            DragDrop.inSlot = false;
    }
}
