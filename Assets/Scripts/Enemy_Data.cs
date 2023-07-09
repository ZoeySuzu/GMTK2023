using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy/NewEnemyData", menuName = "Dungeon/EnemyData", order = 2)]
public class Enemy_Data : ScriptableObject
{
    public string enemyName;
    public EnemyType enemyType;

    public int goldValue;
    public int power;
    public int maxHealth;
    public GameObject prefab;
}
