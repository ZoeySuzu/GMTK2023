using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] UI_Team teamUI;
    // Start is called before the first frame update
    void Start()
    {
        Team_Data team = new Team_Data();

        team.TeamLevelUp();
        team.TeamLevelUp();
        team.TeamLevelUp();

        teamUI.SetTeam(team);

        team.TeamLevelUp();
        team.TeamLevelUp();
        team.TeamLevelUp();

        teamUI.SetTeam(team);
    }


}
