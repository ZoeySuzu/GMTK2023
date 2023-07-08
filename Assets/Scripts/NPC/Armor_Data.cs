using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Armor/NewArmorData", menuName = "Itens/ArmorData", order = 2)]
public class Armor_Data : ScriptableObject
{
    public string ArmorName;
    public int Strength;
    public ArmorTier Type;
    public LootTier Tier;
}

