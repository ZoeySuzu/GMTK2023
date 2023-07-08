using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArmorTier
{
    None = 0,
    Light = 1,
    Heavy = 2,
    Sneaky = 3, //Avoid Traps
    FireResist = 4,
    IceResist = 5,
    ElectricResist = 6
}

public enum WeaponType
{
    None = 0,
    Sword = 1, //Standard
    Bow = 2, //Good Vs Fly
    FireSword = 3,
    IceSword = 4,
    ElectricSword = 5,
    Hammer = 6, //Good Vs armor
}

public enum CharacterSprite
{
    Boy1 = 1,
    Girl1 = 2,
}

public enum LootTier
{
    Common = 5,
    Uncommon = 20,
    Rare = 100,
    Epic = 200,
    Legendary = 500,
    Ultimate = 999
}

public enum CharacterRole
{
    Fighter = 1,
    Mage = 2,
    Tank = 3,
    Explorer = 4,
}