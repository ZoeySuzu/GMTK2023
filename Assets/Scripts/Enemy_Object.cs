using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Object
{
    public GameObject enemyObject;

    public int MaxHealth; 
    public int Health;
    public int power;

    public EnemyType enemyType;
    public bool dead = false;
    
    public Enemy_Object(Enemy_Data enemy_Data)
    {
        MaxHealth = enemy_Data.maxHealth;
        Health = MaxHealth;
        power = enemy_Data.power;
        enemyType = enemy_Data.enemyType;
    }

    public void TakeDamage(int value)
    {
        Health -= value;
        if (Health <= 0) { 
            Health = 0;
            dead = true;
            enemyObject.GetComponent<Image>().sprite = SpriteManager.Instance.GetEnemyDeathSprite((int)enemyType);
            enemyObject.GetComponent<Animator>().enabled = false;
        }
        else if (Health > MaxHealth) Health = MaxHealth;
    }
}
