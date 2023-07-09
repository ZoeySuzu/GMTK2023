using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dungeon_Tile: MonoBehaviour
{
    [SerializeField] public Dungeon_TileData tileData;

    [SerializeField] public Transform[] EnemyPositions;

    //[SerializeField] public List<>//Enemy OBJ from Anemy Data

    public Direction TileRealExits;

    public (int x, int y) position;

    public List<Enemy_Object> enemies = new List<Enemy_Object>(); 

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

    public void AddEnemy(Enemy_Data enemy_Data)
    {
        Enemy_Object enemy = new Enemy_Object(enemy_Data);
        enemy.enemyObject = Instantiate(enemy_Data.prefab, transform);
        enemies.Add(enemy);
        DisplayEnemies();
    }

    public void DisplayEnemies()
    {
        for(int i = 0; i < enemies.Count; i++)
        {
            enemies[i].enemyObject.transform.position = EnemyPositions[i].position;
        }
    }
}
