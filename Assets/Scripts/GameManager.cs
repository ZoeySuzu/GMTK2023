using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Team_Data team = new Team_Data();
        team.ConsoleLog();

        team.TeamLevelUp();
        team.TeamLevelUp();
        team.TeamLevelUp();

        team.ConsoleLog();

        team.TeamLevelUp();
        team.TeamLevelUp();
        team.TeamLevelUp();

        team.ConsoleLog();
    }
}
