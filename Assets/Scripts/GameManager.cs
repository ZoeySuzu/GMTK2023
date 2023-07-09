using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    public TMP_Text DayTracker;
    public Button StartDay;

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
        DayTracker.text = "Day: " + day;
    }


    public void DayEnd() {

        int goldGained = 0;
        team.NpcList.ForEach(x => goldGained += x.GoldWorth);
        UI_Shop.Instance.PlayerGold += goldGained;
        UI_Shop.Instance.UpdateGoldDisplay();

        day++;
        DayTracker.text = "Day: " + day;
        StartDay.interactable = true;

        team.TeamLevelUp();
        teamUI.SetTeam(team);
    }

}
