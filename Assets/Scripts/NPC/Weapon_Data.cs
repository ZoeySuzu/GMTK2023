using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Itens/WeaponData", order = 1)]
public class Weapon_Data : ScriptableObject
{
    public string WeaponName;
    public int Strength;
    public int BonusStrength; //Elemental or advantage bonus
    public WeaponType Type;
    public LootTier Tier;
}
