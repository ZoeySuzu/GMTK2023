using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy_Data_Holder : MonoBehaviour
{
    [SerializeField] public Enemy_Data data;
    [SerializeField] public Enemy_Object obj;
    [SerializeField] public TMP_Text healthText;

    private void Start()
    {
        obj = new Enemy_Object(data);
        obj.healthDisplay = healthText;
    } 
}

