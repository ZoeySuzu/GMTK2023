using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleController 
{
    List<Enemy_Object> enemies;
    List<NPC_Data> heroes;

    bool enemiesFirst;
    bool firstTurn;

    public bool heroesDead;

    public BattleController(List<Enemy_Object> _enemies, Team_Data _team)
    {
        enemies = _enemies;
        heroes = _team.NpcList;
        enemiesFirst = Random.Range(0, 3) <= 1 ? true : false;
        firstTurn = true;
        heroesDead = false;
    }

    private Enemy_Object activeEnemy;
    private NPC_Data activeNpc;

    public bool Tick()
    {
        if(!firstTurn || !enemiesFirst)
        {
            foreach (NPC_Data hero in heroes.FindAll(x => x.dead == false))
            {
                if (hero.frozen) { hero.frozen = false; continue; }
                activeNpc = hero;
                if (!Potion())
                {
                    if (!Spell())
                    {
                        Attack();
                    }
                }
                if (enemies.Find(x => x.dead == false) == null)
                {
                    return true;
                }
            }
        }


        //Resolve enemies
        foreach (Enemy_Object enemy in enemies.FindAll(x => x.dead == false))
        {
            activeEnemy = enemy;
            switch (enemy.enemyType)
            {
                case EnemyType.Slimeow:
                    SlimeowAttack();
                    break;
                case EnemyType.CinamonWiz:
                    PigzardAttack();
                    break;
                case EnemyType.Puglist:
                    PugleistAttack();
                    break;
                case EnemyType.Fairy:
                    CirinaAttack();
                    break;
            }
            if (heroes.Find(x => x.dead == false) == null)
            {
                heroesDead = true;
                return true;
            }
        }
        enemiesFirst = false;
        return false;
    }

    private bool Spell()
    {
        if (activeNpc.Mana > 0 && Random.Range(0,3) == 0)
        {
            int attackIndex = Random.Range(0, 10);
            if (attackIndex < 7)
            {
                int dmg = activeNpc.WeaponAtk + activeNpc.WeaponAtk/2;

                List<Enemy_Object> targets = enemies.FindAll(x => x.dead == false);
                int targetIndex = Random.Range(0, targets.Count);
                targets[targetIndex].TakeDamage(dmg);
            }
            else if(activeNpc.Mana > 1 && activeNpc.Role == CharacterRole.Mage)
            {
                int dmg = (activeNpc.WeaponAtk * 4) / enemies.Where(x => x.dead == false).Count();
                foreach (Enemy_Object enemy in enemies.Where(x => x.dead == false))
                {
                    enemy.TakeDamage(dmg);
                }
                activeNpc.Mana--;
            }
            else
            {
                return false;
            }
            activeNpc.Mana--;
            return true;
        }
        return false;
    }

    private void Attack()
    {
        int attackIndex = Random.Range(0, 10);
        if(attackIndex < 8)
        {
            int dmg = activeNpc.WeaponAtk;
            if (activeNpc.Role == CharacterRole.Fighter && Random.Range(0, 3) == 0) dmg *= 2; 

            List<Enemy_Object> targets = enemies.FindAll(x => x.dead == false);
            int targetIndex = Random.Range(0, targets.Count);
            targets[targetIndex].TakeDamage(dmg);
        }
        else
        {
            int dmg = (activeNpc.WeaponAtk * 2) / enemies.Where(x => x.dead == false).Count();
            foreach (Enemy_Object enemy in enemies.Where(x => x.dead == false))
            {
                enemy.TakeDamage(dmg);
            }
        }
    }

    private bool Potion()
    {
        if (activeNpc.Potions <= 0) return false;
        List<NPC_Data> targets = heroes.FindAll(x => x.dead == false).FindAll(y => y.MaxHealth - y.Health > 50);
        if (targets.Count > 0 && Random.Range(0,3) == 0)
        {
            int targetIndex = Random.Range(0, targets.Count);
            targets[targetIndex].TakeDamage(-50);
            activeNpc.Potions--;
            return true;
        }
        return false;
    }

    private void CirinaAttack()
    {

        int attackIndex = Random.Range(0, 5);
        if (attackIndex < 3)
        {
            int dmg = activeEnemy.power / heroes.Where(x => x.dead == false).Count();
            foreach (NPC_Data hero in heroes.Where(x => x.dead == false))
            {
                hero.TakeDamage(dmg);
                bool freeze = (Random.Range(0, 2) == 1) ? true : false;
                hero.frozen |= freeze;
            }
        }
        else
        {
            int dmg = activeEnemy.power;

            List<NPC_Data> targets = heroes.FindAll(x => x.dead == false);
            int targetIndex = Random.Range(0, targets.Count);
            targets[targetIndex].TakeDamage(dmg*4);
        }
    }

    private void SlimeowAttack()
    {
        int dmg = activeEnemy.power;


        List<NPC_Data> targets = heroes.FindAll(x => x.dead == false);
        int targetIndex = Random.Range(0, targets.Count);

        int attackIndex = Random.Range(0, 5);
        if (attackIndex < 3)
        {
            targets[targetIndex].TakeDamage(dmg);
        }
        else
        {
            targets[targetIndex].TakeDamage(dmg*2);
        }
    }

    private void PugleistAttack()
    {
        int dmgBonus = 0;
        if (enemies.FindAll(x => x.dead == false).Find(y => y != activeEnemy) == null)
        {
            dmgBonus = 30;
        }
        int attackIndex = Random.Range(0, 5);
        if (attackIndex < 3)
        {
            int dmg = activeEnemy.power + dmgBonus;

            List<NPC_Data> targets = heroes.FindAll(x => x.dead == false);
            int targetIndex = Random.Range(0, targets.Count);
            targets[targetIndex].TakeDamage(dmg);
        }
        else
        {
            int dmg = activeEnemy.power / heroes.Where(x => x.dead == false).Count() + dmgBonus; 
            foreach (NPC_Data hero in heroes.Where(x => x.dead == false))
            {
                hero.TakeDamage(dmg);
            }
        }
    }

    private void PigzardAttack()
    {
        int dmg = activeEnemy.power;

        List<NPC_Data> targets = heroes.FindAll(x => x.dead == false);
        int targetIndex = Random.Range(0, targets.Count);

        int attackIndex = Random.Range(0, 5);
        if (attackIndex < 2)
        {
            targets[targetIndex].TakeDamage(dmg);
            targets[targetIndex].TakeManaDamage(2);
        }
        else if (attackIndex < 4)
        {
            targets[targetIndex].TakeDamage(dmg);
            targets[targetIndex].frozen = true;
        }
        else
        {
            targets[targetIndex].TakeDamage(dmg*2);
        }
    }

}
