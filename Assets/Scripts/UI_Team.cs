using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Team : MonoBehaviour
{
    [SerializeField] private GameObject[] Panels;
    public void SetTeam(Team_Data teamdata)
    {
        for(int i = 0; i < 4; i++)
        {
          
            if(i >= teamdata.NpcList.Count) 
            {  
                Panels[i].SetActive(false);
            }
            else
            {
                Panels[i].SetActive(true);
                Panels[i].GetComponent<UI_Character>().SetCharacterData(teamdata.NpcList[i]);
            }
        }
    }

    public void UpdateTeam(Team_Data teamdata)
    {
        for(int i = 0; i < teamdata.NpcList.Count; i++)
        {
            Panels[i].GetComponent<UI_Character>().UpdateCharacterData(teamdata.NpcList[i]);
        }
    }
}
