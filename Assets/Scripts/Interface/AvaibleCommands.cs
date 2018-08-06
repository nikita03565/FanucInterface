using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvaibleCommands : MonoBehaviour {
    public List<UIComplexCommand> AvaibleCommandsSet = new List<UIComplexCommand>();
	// Use this for initialization
	void Start () {
	 
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

                        //int sum = 0;
                        //for (int s = 0; s < AvaibleCommandsSet[i].UICommandElements.Count; ++s)
                        //{
                        //    sum += AvaibleCommandsSet[i].UICommandElements[s].GetNumberofCommands();

                        //    if ((sum > Index) && (AvaibleCommandsSet[i].UICommandElements[s].GetNumberofCommands() == CommandToRewrite.GetNumberofCommands()))
                        //    {
                        //        Debug.Log(sum + " " + Index + " it ends");
                        //        // AvaibleCommandsSet[i].UICommandElements[s] = Instantiate(NewCommand, AvaibleCommandsSet[i].transform);
                        //        {
                        //            AvaibleCommandsSet[i].UICommandElements[s].GetComponent<UIComplexCommand>().UICommandElements = NewCommand.GetComponent<UIComplexCommand>().UICommandElements;
                        //            AvaibleCommandsSet[i].UICommandElements[s].GetComponent<UIComplexCommand>().CommandsSet = NewCommand.GetComponent<UIComplexCommand>().CommandsSet;
                        //            //  for (int d = Index, t = 0; d < Index + NewCommand.GetNumberofCommands(); ++d, ++t)
                        //            // {
                        //            //     Command.Copy(AvaibleCommandsSet[i].CommandsSet[d], NewCommand.CommandsSet[t]);
                        //            // }
                        //            Debug.Log("Deleted" + " " + Index + " " + CommandToRewrite.GetNumberofCommands() + " of " + AvaibleCommandsSet[i].CommandsSet.Count);

                        //            AvaibleCommandsSet[i].CommandsSet.RemoveRange(Index, CommandToRewrite.GetNumberofCommands());
                        //            AvaibleCommandsSet[i].CommandsSet.InsertRange(Index, NewCommand.CommandsSet);
                        //        }
                        //        AvaibleCommandsSet[i].UICommandElements[s].gameObject.SetActive(false);
                        //        break;
                        //    }   
                        //    else if (sum > Index)
                        //        break;



                    }

                }
            }
            //     {
            //for (int j = i - 1; j >= 0; --j)
            // {
            //     //if (AvaibleCommandsSet[i].GetNumberofCommands() < AvaibleCommandsSet[j].GetNumberofCommands())
            //     //    return;
            //     for (int k = 0,  l = 0 ; AvaibleCommandsSet[i].GetNumberofCommands() - k >= AvaibleCommandsSet[j].GetNumberofCommands(); ++k)
            //     {
            //         Debug.Log("now im trying " + i + " " + k + "       " + j + " " + l);
            //         Debug.Log(AvaibleCommandsSet[i].GetNumberofCommands() + " - " + k + " > " + AvaibleCommandsSet[j].GetNumberofCommands());
            //         if (Command.IsEQ(AvaibleCommandsSet[i].CommandsSet[k],AvaibleCommandsSet[j].CommandsSet[l]))
            //         {
            //             Debug.Log("AAAAAA");
            //             Index = k;
            //             for (; l < AvaibleCommandsSet[j].GetNumberofCommands();)
            //             {


            //                 Debug.Log("AvaibleCommandsSet[j].GetNumberofCommands()"+ AvaibleCommandsSet[j].GetNumberofCommands()+"    " + k+" "+l);
            //                 if (!Command.IsEQ(AvaibleCommandsSet[i].CommandsSet[k], AvaibleCommandsSet[j].CommandsSet[l]))
            //                 {
            //                     l = 0;
            //                     Index = -1;
            //                     return;
            //                 }
            //                 ++k; ++l;
            //             }
            //             l = 0;
            //             --k;
            //             if(Index!=-1)
            //             {
            //                 Debug.Log("Index!=-1 "+Index);

            //                 int sum = 0;
            //                 for (int s = 0; s < AvaibleCommandsSet[i].UICommandElements.Count; ++s)
            //                 {
            //                     sum += AvaibleCommandsSet[i].UICommandElements[s].GetNumberofCommands();
            //                     Debug.Log("SUM+= " + Index + " " + sum);
            //                     if (sum > Index)
            //                     {

            //                         AvaibleCommandsSet[i].UICommandElements[s] = Instantiate(AvaibleCommandsSet[j], AvaibleCommandsSet[i].transform);
            //                         {
            //                             AvaibleCommandsSet[i].UICommandElements[s].GetComponent<UIComplexCommand>().UICommandElements = AvaibleCommandsSet[j].GetComponent<UIComplexCommand>().UICommandElements;
            //                             AvaibleCommandsSet[i].UICommandElements[s].GetComponent<UIComplexCommand>().CommandsSet = AvaibleCommandsSet[j].GetComponent<UIComplexCommand>().CommandsSet;

            //                             AvaibleCommandsSet[i].CommandsSet.RemoveRange(Index, AvaibleCommandsSet[j].GetNumberofCommands());
            //                             AvaibleCommandsSet[i].CommandsSet.InsertRange(Index, AvaibleCommandsSet[j].CommandsSet);
            //                         }
            //                         //AvaibleCommandsSet[i].UICommandElements[s].gameObject.SetActive(false);
            //                         break;
            //                     }


            //                 }



            //             }



            //         }

            //     }
            // }
        }
        NewCommand.transform.SetSiblingIndex(CommandToRewrite.transform.GetSiblingIndex());
        AvaibleCommandsSet[AvaibleCommandsSet.IndexOf(CommandToRewrite)] = AvaibleCommandsSet[AvaibleCommandsSet.Count - 1];
        AvaibleCommandsSet.RemoveAt(AvaibleCommandsSet.Count - 1);
        Destroy(CommandToRewrite.gameObject);
    }
}
