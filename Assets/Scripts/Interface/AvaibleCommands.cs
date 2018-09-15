using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvaibleCommands : MonoBehaviour {
    public List<UIComplexCommand> AvaibleCommandsSet = new List<UIComplexCommand>();
    int BasicCommandsNumber;
	// Use this for initialization
	void Start () {
        BasicCommandsNumber = this.transform.childCount;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void RewriteAllrefs(UIComplexCommand CommandToRewrite, UIComplexCommand NewCommand)
    {
        Debug.Log("in rewrite");
        int Index = -1 ;
        Debug.Log(AvaibleCommandsSet.Count);
        for(int i=AvaibleCommandsSet.IndexOf(CommandToRewrite)+1;i<AvaibleCommandsSet.Count-1; ++i)
        { 
            if (AvaibleCommandsSet[i].GetNumberofCommands() < CommandToRewrite.GetNumberofCommands())
               return;
            int deletedcommandsoffset = 0;
            for (int k = 0, l = 0; AvaibleCommandsSet[i].GetNumberofCommands() - k >= CommandToRewrite.GetNumberofCommands(); ++k)
            {

                Debug.Log("Fast check " +k + " " + l);
                if (Command.IsEQ(AvaibleCommandsSet[i].CommandsSet[k], CommandToRewrite.CommandsSet[l]))
                {
                    
                    Index = k;
                    ++k; ++l;
                    for (; l < CommandToRewrite.GetNumberofCommands();)
                    {
                        Debug.Log("now checking " + k + " " + l);
                        if (!Command.IsEQ(AvaibleCommandsSet[i].CommandsSet[k], CommandToRewrite.CommandsSet[l]))
                        {
                            l = 0;
                            Index = -1;
                            break;
                        }
                        ++k; ++l;
                    }
                    l = 0;
                    Debug.Log("K= " + k);
                    k-=1;
                    Debug.Log("Index= " + Index);
                    if (Index != -1)
                    {
                        Debug.Log("Deleted" + " " + (Index ) + " " + CommandToRewrite.GetNumberofCommands() + " of " + AvaibleCommandsSet[i].CommandsSet.Count);
                        AvaibleCommandsSet[i].CommandsSet.RemoveRange(Index, CommandToRewrite.GetNumberofCommands());
                        //-_-----------------------
                        int sum = 0;
                        for (int s = 0; s < AvaibleCommandsSet[i].UICommandElements.Count; ++s)
                        {
                            sum += AvaibleCommandsSet[i].UICommandElements[s].GetNumberofCommands();

                            if ((sum > Index) && (AvaibleCommandsSet[i].UICommandElements[s].GetNumberofCommands() == CommandToRewrite.GetNumberofCommands()))
                            {
                                AvaibleCommandsSet[i].UICommandElements[s] = NewCommand;
                            }
                        }
                        //-_-----------------------
                                AvaibleCommandsSet[i].CommandsSet.InsertRange(Index, NewCommand.CommandsSet);
                        deletedcommandsoffset +=  CommandToRewrite.GetNumberofCommands()- NewCommand.GetNumberofCommands();
                        Debug.Log("k--" + deletedcommandsoffset);
                        k -= CommandToRewrite.GetNumberofCommands() - NewCommand.GetNumberofCommands();

                   
                    }

                }
            }
            
        }
        NewCommand.transform.SetSiblingIndex(CommandToRewrite.transform.GetSiblingIndex());
        AvaibleCommandsSet[AvaibleCommandsSet.IndexOf(CommandToRewrite)] = AvaibleCommandsSet[AvaibleCommandsSet.Count - 1];
        AvaibleCommandsSet.RemoveAt(AvaibleCommandsSet.Count - 1);
        Destroy(CommandToRewrite.gameObject);
    }
    //public void RewriteRefs()
    //{
    //    int Index = -1;
    //    Debug.Log(AvaibleCommandsSet.Count);
    //    foreach(UIComplexCommand command in AvaibleCommandsSet)
    //        for (int i = AvaibleCommandsSet.IndexOf(command) + 1; i < AvaibleCommandsSet.Count; ++i)
    //        { 
    //            if (AvaibleCommandsSet[i].GetNumberofCommands() < command.GetNumberofCommands())
    //                return;
    //            for (int k = 0, l = 0; AvaibleCommandsSet[i].GetNumberofCommands() - k >= command.GetNumberofCommands(); ++k)
    //            {

    //                Debug.Log("Fast check " + k + " " + l);
    //                if (Command.IsEQ(AvaibleCommandsSet[i].CommandsSet[k], command.CommandsSet[l]))
    //                {

    //                    Index = k;
    //                    ++k; ++l;
    //                    for (; l < command.GetNumberofCommands();)
    //                    {
    //                        Debug.Log("now checking " + k + " " + l);
    //                        if (!Command.IsEQ(AvaibleCommandsSet[i].CommandsSet[k], command.CommandsSet[l]))
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

    //                        int sum = 0;
    //                        for (int s = 0; s < AvaibleCommandsSet[i].UICommandElements.Count; ++s)
    //                        {



    //                                AvaibleCommandsSet[i].UICommandElements[s] =command;

    //                        }
    //                    }
    //                }
    //            }
    //        }


    //}

    public void SetSavedOrderIndex(UIComplexCommand comp)
    {
        comp.transform.SetSiblingIndex(BasicCommandsNumber + comp.localSaveIndex);
    }
}
