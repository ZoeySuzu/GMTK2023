using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DialogController : MonoBehaviour
{

    public static DialogController Instance;
    int scriptLength;
    int count = 0;
    DialogAsset[] currentConvo;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(this); }
    }

    public bool resolved;

    [SerializeField] GameObject LilimDialog;
    [SerializeField] GameObject AzuraDialog;
    [SerializeField] GameObject RydenDialog;

    private void StartDialog(Characters speaker, string text)
    {
        int character = (int)speaker;
        switch (character)
        {
            case 0:
                AzuraDialog.SetActive(false);
                RydenDialog.SetActive(false);
                LilimDialog.SetActive(true);
                LilimDialog.GetComponentInChildren<TextMeshProUGUI>().text = text;
                break;
            case 1:
                AzuraDialog.SetActive(true);
                RydenDialog.SetActive(false);
                LilimDialog.SetActive(false);
                AzuraDialog.GetComponentInChildren<TextMeshProUGUI>().text = text;
                break;
            case 2:
                AzuraDialog.SetActive(false);
                RydenDialog.SetActive(true);
                LilimDialog.SetActive(false);
                RydenDialog.GetComponentInChildren<TextMeshProUGUI>().text = text;
                break;
            case 3:
                AzuraDialog.SetActive(false);
                RydenDialog.SetActive(false);
                LilimDialog.SetActive(false);
                break;
        }
        count++;
        if (count == scriptLength) Resolve();

    }

    public void ParseScript(DialogAsset[] script)
    {
        count = 0;
        resolved = false;
        scriptLength = script.Length;
        currentConvo = script;
        StartDialog(currentConvo[count].character, currentConvo[count].text);
    }

    public void Update()
    {
        if (!resolved && Input.GetKeyDown(KeyCode.Space))
        {
            StartDialog(currentConvo[count].character, currentConvo[count].text);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Resolve();
        }
    }

    private void Resolve()
    {
        AzuraDialog.SetActive(false);
        RydenDialog.SetActive(false);
        LilimDialog.SetActive(false);
        resolved = true;
    }

}
