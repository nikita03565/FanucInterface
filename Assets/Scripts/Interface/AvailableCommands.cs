﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvailableCommands : MonoBehaviour
{
    public List<UIComplexCommand> AvailableCommandsSet = new List<UIComplexCommand>();
    int BasicCommandsNumber;

	// Use this for initialization
	void Start ()
    {
        BasicCommandsNumber = this.transform.childCount;
	}

    //public void RewriteAllrefs(UIComplexCommand RWCommand, UIComplexCommand NewCommand)
    //{
    //    List<UIComplexCommand> ComToRW=new List<UIComplexCommand>();
    //    ComToRW.Add(RWCommand);
    //    Debug.Log("in rewrite");
    //    int Index = -1;
    //    Debug.Log(AvaibleCommandsSet.Count);
    //    foreach(UIComplexCommand CommandToRewrite in ComToRW )
    //    {
    //        for (int i = AvaibleCommandsSet.IndexOf(CommandToRewrite) + 1; i < AvaibleCommandsSet.Count - 1; ++i)
    //        {
    //            if (AvaibleCommandsSet[i].GetNumberofCommands() < CommandToRewrite.GetNumberofCommands())
    //                return;
    //            int deletedcommandsoffset = 0;
    //            for (int k = 0, l = 0; AvaibleCommandsSet[i].GetNumberofCommands() - k >= CommandToRewrite.GetNumberofCommands(); ++k)
    //            {

    //                Debug.Log("Fast check " + k + " " + l);
    //                if (Command.IsEQ(AvaibleCommandsSet[i].CommandsSet[k], CommandToRewrite.CommandsSet[l]))
    //                {

    //                    Index = k;
    //                    ++k; ++l;
    //                    for (; l < CommandToRewrite.GetNumberofCommands();)
    //                    {
    //                        Debug.Log("now checking " + k + " " + l);
    //                        if (!Command.IsEQ(AvaibleCommandsSet[i].CommandsSet[k], CommandToRewrite.CommandsSet[l]))
    //                        {
    //                            l = 0;
    //                            Index = -1;
    //                            break;
    //                        }
    //                        ++k; ++l;
    //                    }
    //                    l = 0;
    //                    Debug.Log("K= " + k);
    //                    k -= 1;
    //                    Debug.Log("Index= " + Index);
    //                    if (Index != -1)
    //                    {
    //                        //-_-----------------------
    //                        int sum = 0;
    //                        for (int s = 0; s < AvaibleCommandsSet[i].UICommandElements.Count; ++s)
    //                        {
    //                            sum += AvaibleCommandsSet[i].UICommandElements[s].GetNumberofCommands();

    //                            if ((sum > Index) && (AvaibleCommandsSet[i].UICommandElements[s].GetNumberofCommands() == CommandToRewrite.GetNumberofCommands()))
    //                            {
    //                                AvaibleCommandsSet[i].UICommandElements[s] = NewCommand;
    //                                Debug.Log("Deleted" + " " + (Index) + " " + CommandToRewrite.GetNumberofCommands() + " of " + AvaibleCommandsSet[i].CommandsSet.Count);
    //                                AvaibleCommandsSet[i].CommandsSet.RemoveRange(Index, CommandToRewrite.GetNumberofCommands());
    //                                AvaibleCommandsSet[i].CommandsSet.InsertRange(Index, NewCommand.CommandsSet);
    //                                break;
    //                            }
    //                        }
    //                        //-_-----------------------

    //                        deletedcommandsoffset += CommandToRewrite.GetNumberofCommands() - NewCommand.GetNumberofCommands();
    //                        Debug.Log("k--" + deletedcommandsoffset);
    //                        k -= CommandToRewrite.GetNumberofCommands() - NewCommand.GetNumberofCommands();
    //                    }
    //                }
    //            }
    //        }
    //    }
        // NewCommand.transform.SetSiblingIndex(CommandToRewrite.transform.GetSiblingIndex());
        // AvaibleCommandsSet[AvaibleCommandsSet.IndexOf(CommandToRewrite)] = AvaibleCommandsSet[AvaibleCommandsSet.Count - 1];
        //AvaibleCommandsSet.RemoveAt(AvaibleCommandsSet.Count - 1);
        // Destroy(CommandToRewrite.gameObject);
 //   }

    public void RewriteAllrefs(UIComplexCommand RWCommand, UIComplexCommand NewCom)
    {
        List<UIComplexCommand> ComToRW = new List<UIComplexCommand>();
        UIComplexCommand[] comsArray = new UIComplexCommand[AvailableCommandsSet.Count+1];
       
        int IndComNew = 0;
        //ComToRW.Add(RWCommand);
        comsArray[0]=NewCom;
       
        
         //AvaibleCommandsSet[AvaibleCommandsSet.IndexOf(RWCommand)] = AvaibleCommandsSet[AvaibleCommandsSet.Count - 1];
        //AvaibleCommandsSet.RemoveAt(AvaibleCommandsSet.Count - 1);        
        NewCom.transform.SetSiblingIndex(RWCommand.transform.GetSiblingIndex());
        Debug.Log(AvailableCommandsSet.Count);
        for(int i=0;i<AvailableCommandsSet.Count-1;++i )
        {
            ComToRW.Add(Instantiate<UIComplexCommand>(AvailableCommandsSet[i]));
        }
        Debug.Log(ComToRW.IndexOf(RWCommand));
        for(int i=AvailableCommandsSet.IndexOf(RWCommand), j=0;i<ComToRW.Count-1;++i,++j )
        {
            
            UIComplexCommand CommandToRewrite = ComToRW[i];
            UIComplexCommand NewCommand = comsArray[j];
            ++IndComNew;
            Debug.Log(CommandToRewrite.CommandName + " in rewrite");
            for (int Rw = i + 1; Rw < AvailableCommandsSet.Count-1; ++Rw)
            {
                Debug.Log("Cheking " + (Rw + 1) + " of " + AvailableCommandsSet.Count);
                for (int subRW = 0; subRW < AvailableCommandsSet[Rw].UICommandElements.Count; ++subRW)
                {
                    Debug.Log(AvailableCommandsSet[Rw].UICommandElements[subRW].CommandName + " == " + CommandToRewrite.CommandName);
                    if (AvailableCommandsSet[Rw].UICommandElements[subRW].CommandName == CommandToRewrite.CommandName)
                    {
                        Debug.Log("isEQ");
                        int sum = 0;
                        for (int index = 0; index < subRW; ++index)
                        {
                            sum += AvailableCommandsSet[Rw].UICommandElements[index].GetNumberofCommands();
                        }
                       // if (ComToRW.Count == i+1)                        
                        Debug.Log("deleted " + sum + " to " + (sum + CommandToRewrite.GetNumberofCommands()-1) + " of " + AvailableCommandsSet[Rw].GetNumberofCommands());
                        AvailableCommandsSet[Rw].CommandsSet.RemoveRange(sum, CommandToRewrite.GetNumberofCommands());

                         AvailableCommandsSet[Rw].UICommandElements[subRW].Copy(NewCommand);
                        Debug.Log("inserted " + sum + " to " + (sum + AvailableCommandsSet[Rw].UICommandElements[subRW].GetComponent<UIComplexCommand>().GetNumberofCommands()-1) + " of " + AvailableCommandsSet[Rw].GetNumberofCommands());

                        AvailableCommandsSet[Rw].CommandsSet.InsertRange(sum, AvailableCommandsSet[Rw].UICommandElements[subRW].GetComponent<UIComplexCommand>().CommandsSet);
                        comsArray[Rw]=AvailableCommandsSet[Rw];
                    }
                }


            }
        }
        AvailableCommandsSet[AvailableCommandsSet.IndexOf(RWCommand)] = AvailableCommandsSet[AvailableCommandsSet.Count - 1];
        RWCommand.Destroy();
        AvailableCommandsSet.RemoveAt(AvailableCommandsSet.Count - 1);
        for (int i = 0; i < ComToRW.Count; ++i)
            Destroy(ComToRW[i].gameObject);
    }



    public void SetSavedOrderIndex(UIComplexCommand comp)
    {
        comp.transform.SetSiblingIndex(BasicCommandsNumber + comp.localSaveIndex);
    }
}
