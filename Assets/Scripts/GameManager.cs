using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    int day = 1;
    Team_Data team;

    [SerializeField] UI_Team teamUI;

    [SerializeField] DialogAsset[] dialog;
    // Start is called before the first frame update
    void Start()
    {
        team = new Team_Data();
        teamUI.SetTeam(team);
    }


    public void DayEnd() {

        int goldGained = 0;
        team.NpcList.ForEach(x => goldGained += x.GoldWorth);
        UI_Shop.Instance.PlayerGold += goldGained;
        UI_Shop.Instance.UpdateGoldDisplay();

        day++;

        team.TeamLevelUp();
        teamUI.SetTeam(team);

    }

}
