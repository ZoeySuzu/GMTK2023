using System;
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
    Fighter = 0,
    Mage = 1,
    Tank = 2,
    Explorer = 3,
}

[Flags]
public enum Direction
{
    North = 1,
    East = 2,
    South = 4,
    West = 8
}

public enum TileType
{
    Locked = 0,
    Empty = 1,
    Room = 2
}
public enum EnemyType
{
    Slimeow = 1,
    Fairy = 2,
    Puglist = 3,
    CinamonWiz = 4,
}