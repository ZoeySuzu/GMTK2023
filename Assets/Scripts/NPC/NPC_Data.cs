using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Data
{
    public string CharacterName;
    public CharacterSprite Sprite;
    public CharacterRole Role;
    public int Level; //Times in Dungeon
    public int MaxHealth; public int Health;
    public int MaxMana; public int Mana;
    public int Potions;
    public int GoldWorth;
    public int Inteligence; //Will consistently make smart choices to pick right options
    public int Luck;        //Will ocasionaly boost int;

    public int WeaponAtk;
    public int ArmorDef;


    public bool dead;
    public bool frozen;
    //Spells


    public NPC_Data(int _level = 1)
    {
        CharacterName = RandomizeName();
        Role = ChooseRandomRole();
        SetLevel(_level);
        Inteligence = (Role == CharacterRole.Explorer) ?
            Mathf.Max(Random.Range(1, _level*2 + 1), (int)(_level / 5f)):
            Mathf.Max(Random.Range(1, _level + 1), (int)(_level/10f));
        Luck = Random.Range(1, 100);
        WeaponAtk = Random.Range(6, 14);
        ArmorDef = Random.Range(4, 10);
        Sprite = ChooseRandomSprite();
        GoldWorth = CalculateGoldWorth();
    }

    public void TakeDamage(int value)
    {
        if (value  > 0 && Role == CharacterRole.Tank && Random.Range(0,3) == 0) value/=2;//HalfDamageSometimesOnTanks
        if (value > 0) { value -= ArmorDef/10;  if (value <= 1) value = 1; }
        Health -= value;
        if (Health <= 0) { Health = 0; dead = true; }
        else if (Health > MaxHealth) Health = MaxHealth;
    }
    public void TakeManaDamage(int value)
    {
            Mana -= value;
        if (Mana <= 0) { Mana = 0;}
        else if (Mana > MaxMana) Mana = MaxMana;
    }

    public void SetLevel(int _level)
    {
        Level = _level;
        MaxHealth = (Role == CharacterRole.Tank) ?
            Random.Range(15, 26) + _level * Random.Range(7, 10) + Random.Range(2, 8) * _level:
            Random.Range(7, 13) + _level * Random.Range(4, 7) + Random.Range(0, 6) * _level;
        MaxMana = (Role == CharacterRole.Mage) ?
              Random.Range(_level / 2 + 1, _level*2 + 1):
              Random.Range(0, _level + 1);
        Potions = Random.Range(0, ((_level+1)/2));

        Health = MaxHealth;
        Mana = MaxMana;
    }

    private CharacterSprite ChooseRandomSprite()
    {
        System.Array values = System.Enum.GetValues(typeof(CharacterSprite));
        CharacterSprite sprite = (CharacterSprite)values.GetValue(Random.Range(0, values.Length));
        return sprite;
    }

    private CharacterRole ChooseRandomRole()
    {
        System.Array values = System.Enum.GetValues(typeof(CharacterRole));
        CharacterRole role = (CharacterRole)values.GetValue(Random.Range(0, values.Length));
        return role;
    }

    public void IncreaseLevel()
    {
        Level++;
        MaxHealth += (Role == CharacterRole.Tank) ?
             Random.Range(9, 16):
             Random.Range(6, 13);
        MaxMana += (Role == CharacterRole.Mage) ?
            Random.Range(1, 3):
            Random.Range(0, 3)/2;
        bool increaseInt = (Role == CharacterRole.Explorer) ?
             (Random.Range(0, 5 - (Level - (Inteligence / 2))) == 0):
             (Random.Range(0,10-(Level-(Inteligence/2))) == 0);

        WeaponAtk += (Role == CharacterRole.Fighter) ?
             Random.Range(1, 5):
             Random.Range(0, 4);
        if (WeaponAtk > 500) WeaponAtk = 500;

        ArmorDef += Random.Range(0, 3);
        if(ArmorDef > 500) ArmorDef = 500;

        if (increaseInt) Inteligence++;
        Potions = Random.Range(0, ((Level + 1) / 2));
        GoldWorth = CalculateGoldWorth();
        Health = MaxHealth;
        Mana = MaxMana;
        dead = false;
    }

    private int CalculateGoldWorth()
    {
        float value = (MaxHealth * 1.2f) + (MaxMana * 1.5f) + (Potions * 15) + (Inteligence * 2) + WeaponAtk + ArmorDef;
        float ratio = 1 + Luck / 400f;
        float finalValue = value * ratio;
        return (int)finalValue;
    }

    private string[] namepool = { "Wren", "Turnip", "Luna", "Gooseman", "AAAA", "Link", "Dan", "Phoebe","Tess","Homer", "Puggle", "Reimu", "Cirno", "Jerna", "Ben", "Alex", "Alek", "jekhkl", "Nutmeg", "Jerry", "PigPig", "Peppy", "Ronald", "Mandy", "Eggbean", "Tess","Mike","Dumpstin","Sammule","Legend","Jane","Chris","Melon","Fridge","George","Kramer","Elaine","Newman","Paul","Carl","Leah","Sarah"};
    private string RandomizeName()
    {
        int index = Random.Range(0, namepool.Length);
        return namepool[index];
    }

    public override string ToString()
    {
        return Sprite +" "+ CharacterName + " " + Role +  " level:" + Level + " HP:" + MaxHealth + " MP:" + MaxMana + " Potions:" + Potions + " Inteligence:" + Inteligence + " Luck:" + Luck; 
    }
}



