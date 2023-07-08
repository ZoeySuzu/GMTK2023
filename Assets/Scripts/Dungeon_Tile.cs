using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dungeon_Tile: MonoBehaviour
{
    [SerializeField] public Dungeon_TileData tileData;

    public Direction TileRealExits;

    public (int x, int y) position; 

    private void Start()
    {
        if (tileData != null)
        {
            UpdateTile(tileData);
            GetComponent<Button>().onClick.AddListener(() => UI_Shop.Instance.SetShopTile(this));
        }
    }

    public void UpdateTilePosition(int x, int y)
    {
        position = (x, y);
    }

    public void UpdateTile(Dungeon_TileData data)
    {
        tileData = data;
        gameObject.name = tileData.TileName;
        GetComponent<Button>().interactable = tileData.CanModify;
        GetComponent<Image>().sprite = tileData.TileSprite;
    }

        
}
