using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyShopButton : MonoBehaviour
{
    [SerializeField] Enemy_Data enemyData;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(PurchaseEnemy);
    }

    private void PurchaseEnemy()
    {
        UI_Shop.Instance.PurchaseEnemy(enemyData);
    }
}
