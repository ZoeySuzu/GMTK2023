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

    int day = 0;
    public Team_Data team;

    [SerializeField] public UI_Team teamUI;

    [SerializeField] DialogAsset[] start;
    [SerializeField] DialogAsset[] scene1;
    [SerializeField] DialogAsset[] scene2;
    [SerializeField] DialogAsset[] scene3;
    [SerializeField] DialogAsset[] end1;
    [SerializeField] DialogAsset[] end2;

    // Start is called before the first frame update
    void Start()
    {
        team = new Team_Data();
        teamUI.SetTeam(team);
        DayTracker.text = "Day: " + day;
        if (day == 0) DialogController.Instance.ParseScript(start);

    }


    public void DayEnd() {

        int goldGained = 0;
        team.NpcList.ForEach(x => goldGained += x.GoldWorth);
        UI_Shop.Instance.AddPlayerGold(goldGained);
        UI_Shop.Instance.UpdateGoldDisplay();

        if (day == 5) DialogController.Instance.ParseScript(scene1);
        if (day == 10) DialogController.Instance.ParseScript(scene2);
        if (day == 20) DialogController.Instance.ParseScript(scene3);
    }

    public void DayStart()
    {
        day++;
        DayTracker.text = "Day: " + day;
        StartDay.interactable = true;


        if (day != 1) team.TeamLevelUp();
        teamUI.SetTeam(team);
    }

}
