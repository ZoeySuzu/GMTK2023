using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team_Data
{
    public List<NPC_Data> NpcList;

    public Team_Data(int _level = 1)
    {
        NpcList = new List<NPC_Data>();
        int teamSize = Random.Range(2, 5);
        for(int i = 1; i <= teamSize; i++)
        {
            NPC_Data npc = new NPC_Data();
            NpcList.Add(npc);
        }
    }

    public void TeamLevelUp()
    {
        foreach(NPC_Data npc in NpcList)
        {
            npc.IncreaseLevel();
        }
    }

    public void ConsoleLog()
    {
        Debug.Log("Team:");
        foreach (NPC_Data npc in NpcList)
        {
            Debug.Log(npc.ToString());
        }
    }
}
