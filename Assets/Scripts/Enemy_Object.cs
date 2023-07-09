using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Object
{
    public GameObject enemyObject;

    int maxHealth; 
    int health;
    int power;

    public EnemyType enemyType;
    public bool dead = false;
    
    public Enemy_Object(Enemy_Data enemy_Data)
    {
        maxHealth = enemy_Data.maxHealth;
        health = maxHealth;
        power = enemy_Data.power;
        enemyType = enemy_Data.enemyType;
    }
}
