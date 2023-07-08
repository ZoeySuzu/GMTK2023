using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Character : MonoBehaviour
{
    [SerializeField] TMP_Text charName;
    [SerializeField] TMP_Text charLevel;
    [SerializeField] TMP_Text characterHealth;
    [SerializeField] TMP_Text characterMana;
    [SerializeField] TMP_Text characterPotions;
    [SerializeField] TMP_Text characterGold;
    [SerializeField] TMP_Text characterArmor;
    [SerializeField] TMP_Text characterWeapon;

    [SerializeField] Image characterSprite;
    [SerializeField] Image characterClassSprite;
    [SerializeField] Image characterWeaponSprite;
    [SerializeField] Image characterArmorSprite;

    public void SetCharacterData(NPC_Data data)
    {
        charName.text = data.CharacterName;
        charLevel.text = "LVL: " + data.Level;

        characterClassSprite.sprite = SpriteManager.Instance.GetRoleSprite((int)data.Role);
        characterSprite.sprite = SpriteManager.Instance.GetHeroSprite((int)data.Sprite);

        UpdateCharacterData(data);
    }

    public void UpdateCharacterData(NPC_Data data)
    {
        characterHealth.text = "HP: " + data.Health + "/" + data.MaxHealth;
        characterMana.text = "MP: " + data.Mana + "/" + data.MaxMana;
        characterGold.text = "Gold: " + data.GoldWorth;
        characterPotions.text = "x " + data.Potions;
    }

}
