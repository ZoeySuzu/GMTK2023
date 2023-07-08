using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dungeon_Tile: MonoBehaviour
{
    [SerializeField] public Dungeon_TileData tileData;

    [SerializeField] Direction tileRotation;

    private void Start()
    {
        UpdateTile(tileData);
    }

    public void UpdateTile(Dungeon_TileData data)
    {
        tileData = data;
        gameObject.name = tileData.TileName;
        GetComponent<Button>().interactable = tileData.CanModify;
        GetComponent<Image>().sprite = tileData.TileSprite;
    }
}
