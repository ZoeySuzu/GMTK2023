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
    [SerializeField] DialogAsset[] scene4;
    [SerializeField] DialogAsset[] scene5;
    [SerializeField] DialogAsset[] end1;
    [SerializeField] DialogAsset[] end2;


    [SerializeField] GameObject TitlePanel;
    [SerializeField] GameObject TitleScroll;
    [SerializeField] GameObject CreditPanel;

    [SerializeField] GameObject AzuraPanel;

    private GameState gameState;

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameState.Title;
        TitlePanel.SetActive(true);

    }

    public void Update()
    {
        if (gameState == GameState.Title)
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                TitlePanel.SetActive(false);
                TitleScroll.SetActive(true);
                gameState = GameState.Scroll;
            }
        }
        else if (gameState == GameState.Scroll)
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                StartGame();
                Invoke("StartDialogue", 0.1f);
            }
        }

    }

    private void StartDialogue()
    {
        TitleScroll.SetActive(false);
        gameState = GameState.Game;
        DialogController.Instance.ParseScript(start);

    }


    public void StartGame()
    {
        team = new Team_Data();
        teamUI.SetTeam(team);
        day = 0;
        DayTracker.text = "Day: " + day;
    }

    public void DayEnd() {

        int goldGained = 0;
        team.NpcList.ForEach(x => goldGained += x.GoldWorth);
        UI_Shop.Instance.AddPlayerGold(goldGained);
        UI_Shop.Instance.UpdateGoldDisplay();

        if (day == 5) DialogController.Instance.ParseScript(scene1);
        if (day == 10) DialogController.Instance.ParseScript(scene2);
        if (day == 15) DialogController.Instance.ParseScript(scene3);
        if (day == 25) DialogController.Instance.ParseScript(scene4);
        if (day == 35) DialogController.Instance.ParseScript(scene5);
        if (day == 50) GameOver(1);
    }

    public void DayStart()
    {
        day++;
        DayTracker.text = "Day: " + day;
        StartDay.interactable = true;

        if (day != 1) team.TeamLevelUp();
        teamUI.SetTeam(team);
    }

    public void RevealAzuraPanel()
    {
        AzuraPanel.SetActive(true);
    }

    public void GameOver(int i)
    {
        Debug.Log("Gameover:" + i);
        if(i == 0)
            DialogController.Instance.ParseScript(end1);
        else if (i == 1)
            CreditPanel.SetActive(true);
        else
            DialogController.Instance.ParseScript(end2);
    }
}
