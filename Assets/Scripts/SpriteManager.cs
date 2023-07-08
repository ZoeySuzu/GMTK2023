using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public static SpriteManager Instance;

    [SerializeField] Sprite[] ClassSprites;
    [SerializeField] Sprite[] HeroSprites;

    private void Awake()
    {
        if (Instance == null) Instance = this; else Destroy(gameObject);
    }

    public Sprite GetRoleSprite(int index)
    {
        return ClassSprites[index];
    }
    public Sprite GetHeroSprite(int index)
    {
        return HeroSprites[index];
    }
}
