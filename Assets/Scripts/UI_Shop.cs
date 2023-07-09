using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Shop : MonoBehaviour
{
    public static UI_Shop Instance;

    [SerializeField] public TMP_Text PlayerGoldText;
    [SerializeField] public TMP_Text RoomCostGoldText;

    [SerializeField] public List<Button> shopButtons;

    public int PlayerGold = 250;
    public int RoomCost = 100;


    [SerializeField] public Dungeon_Tile shopTile;
    [SerializeField] public TMP_Text shopTileEnemyCount;
    private Dungeon_Tile targetTile;

    private void Awake()
    {
        if (Instance == null) Instance = this; else Destroy(gameObject);
        PlayerGoldText.text = "Gold: " + PlayerGold;
        RoomCostGoldText.text = "New Room cost: " + RoomCost;
    }

    public void SetShopTile(Dungeon_Tile tile)
    {
        shopButtons.ForEach(x => x.interactable = true);

        Dungeon_Tile newShopTile;
        newShopTile = Instantiate(tile,shopTile.transform.parent.position,Quaternion.identity,shopTile.transform.parent);
        RectTransform rt = newShopTile.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.sizeDelta = Vector2.zero;
        rt.anchoredPosition = Vector2.zero;
        Destroy(newShopTile.GetComponent<Button>());
        //newShopTile.GetComponent<RectTransform>().p
        Destroy(shopTile.gameObject);
        shopTile = newShopTile;
        targetTile = tile;
        shopTileEnemyCount.text = "Enemies: " +targetTile.enemies.Count+"/4";
    }

    public void PurchaseTile(Dungeon_TileData tileData)
    {
        if (Dungeon_Controller.Instance.IsRaidCurrentlyHapening) return;
        if (targetTile.tileData.TileType == TileType.Empty)
        {
            if (PlayerGold >= RoomCost)
            {
                PlayerGold -= RoomCost;
                RoomCost += 100;
                PlayerGoldText.text = "Gold: " + PlayerGold;
                RoomCostGoldText.text = "New Room cost: " + RoomCost;
                targetTile.UpdateTile(tileData);
                Dungeon_Controller.Instance.UpdateTileExits(targetTile.position.x, targetTile.position.y, tileData);
                SetShopTile(targetTile);
            }
            else
            {
                return;
            }
        }
        else if (PlayerGold >= 250)
        {
            PlayerGold -= 250;
            PlayerGoldText.text = "Gold: " + PlayerGold;
            targetTile.UpdateTile(tileData);
            Dungeon_Controller.Instance.UpdateTileExits(targetTile.position.x, targetTile.position.y, tileData);
            SetShopTile(targetTile);
        }

    }

    public void PurchaseEnemy(Enemy_Data enemyData)
    {
        if (Dungeon_Controller.Instance.IsRaidCurrentlyHapening) return;
        if (targetTile.tileData.TileType != TileType.Empty && targetTile.enemies.Count < 4)
        {
            if (PlayerGold >= enemyData.goldValue)
            {
                PlayerGold -= enemyData.goldValue;
                PlayerGoldText.text = "Gold: " + PlayerGold;
                targetTile.AddEnemy(enemyData);
                SetShopTile(targetTile);
            }
        }
        SetShopTile(targetTile);
    }

    public void UpdateGoldDisplay()
    {
        PlayerGoldText.text = "Gold: " + PlayerGold;
    }

    public void SetStartLocation()
    {
        if (Dungeon_Controller.Instance.IsRaidCurrentlyHapening) return;

        if (targetTile != null && (int)targetTile.tileData.TileType > 1)
        {
            SetShopTile(targetTile);
            Dungeon_Controller.Instance.SetStartPosition(targetTile.position);
        }
    }
}
