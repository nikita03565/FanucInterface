using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvailableCommands : MonoBehaviour
{
    public List<UIComplexCommand> AvailableCommandsSet = new List<UIComplexCommand>();
    public List<string> Names = new List<string>();
    int BasicCommandsNumber;

	// Use this for initialization
	void Start ()
    {
        BasicCommandsNumber = this.transform.childCount;
        foreach (var com in this.transform.GetComponentsInChildren<UICommand>())
        {
            Names.Add(com.CommandName);
        }
       
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
        NewCom.localSaveIndex = RWCommand.localSaveIndex;
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
            if (comsArray[j]!=null)
                for (int Rw = i + 1; Rw < AvailableCommandsSet.Count-1; ++Rw)
            {
                    bool needToRewrite = false;
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
                            Debug.Log("Number of commands " + index + "subcommand of  command " + Rw + " is " + AvailableCommandsSet[Rw].UICommandElements[index].GetNumberofCommands());
                         }
                            // if (ComToRW.Count == i+1)                        
                        Debug.Log("deleted " + sum + " to " + (sum + CommandToRewrite.GetNumberofCommands()-1) + " of " + AvailableCommandsSet[Rw].GetNumberofCommands());
                        //trouble is here
                        AvailableCommandsSet[Rw].CommandsSet.RemoveRange(sum, CommandToRewrite.GetNumberofCommands());

                         AvailableCommandsSet[Rw].UICommandElements[subRW].Copy(NewCommand);
                        Debug.Log("inserted " + sum + " to " + (sum + AvailableCommandsSet[Rw].UICommandElements[subRW].GetComponent<UIComplexCommand>().GetNumberofCommands()-1) + " of " + AvailableCommandsSet[Rw].GetNumberofCommands());
                        AvailableCommandsSet[Rw].CommandsSet.InsertRange(sum, AvailableCommandsSet[Rw].UICommandElements[subRW].GetComponent<UIComplexCommand>().CommandsSet);
                        comsArray[Rw]=AvailableCommandsSet[Rw];
                            needToRewrite = true;
                        
                    }

                }

                    if (needToRewrite) SceneManager.builderInterface.RewritetoJson(AvailableCommandsSet[Rw]);
            }
        }
        AvailableCommandsSet[AvailableCommandsSet.IndexOf(RWCommand)] = AvailableCommandsSet[AvailableCommandsSet.Count - 1];
        Names[Names.IndexOf(RWCommand.CommandName)] = NewCom.CommandName;

        if (RWCommand.CommandName != NewCom.CommandName)
        {
            RWCommand.Destroy();
        }

        else
        {
            Destroy(RWCommand.gameObject);
            AvailableCommandsSet.RemoveAt(AvailableCommandsSet.Count - 1);
            Names.RemoveAt(Names.Count - 1);

        }


       // Names.RemoveAt(Names.Count - 1);
        for (int i = 0; i < ComToRW.Count; ++i)
            Destroy(ComToRW[i].gameObject);
    }


    public void Remove(UIComplexCommand com)
    {
        AvailableCommandsSet.Remove(com);
        Names.Remove(com.CommandName);
    }
    public void SetSavedOrderIndex(UIComplexCommand comp)
    {
        Debug.Log("added to "+(BasicCommandsNumber + AvailableCommandsSet.IndexOf(comp)) +" " + AvailableCommandsSet.IndexOf(comp));
        comp.transform.SetSiblingIndex(BasicCommandsNumber + AvailableCommandsSet.IndexOf(comp));
        
    }

}
