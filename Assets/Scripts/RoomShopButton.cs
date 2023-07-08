using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomShopButton : MonoBehaviour
{
    [SerializeField] Dungeon_TileData tileData;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(PurchaseItem);
        GetComponent<Image>().sprite = tileData.TileSprite;
    }

    private void PurchaseItem()
    {

    }
}
