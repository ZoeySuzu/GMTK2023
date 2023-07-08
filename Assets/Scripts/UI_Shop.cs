using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Shop : MonoBehaviour
{
    public static UI_Shop Instance;


    [SerializeField] public Dungeon_Tile shopTile;
    private Dungeon_Tile targetTile;

    private void Awake()
    {
        if (Instance == null) Instance = this; else Destroy(gameObject);
    }

    public void SetShopTile(Dungeon_Tile tile)
    {
        Dungeon_Tile newShopTile;
        newShopTile = Instantiate(tile,shopTile.transform.position,Quaternion.identity,shopTile.transform.parent);
        Destroy(newShopTile.GetComponent<Button>());
        //newShopTile.GetComponent<RectTransform>().p
        Destroy(shopTile.gameObject);
        shopTile = newShopTile;
        targetTile = tile;
    }

    internal void UpdateTile(Dungeon_TileData tileData)
    {
        targetTile.UpdateTile(tileData);
        Dungeon_Controller.Instance.UpdateTileExits(targetTile.position.x, targetTile.position.y, tileData);
        SetShopTile(targetTile);
    }
}
