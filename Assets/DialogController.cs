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

    public bool resolved= true;

    [SerializeField] GameObject LilimDialog;
    [SerializeField] GameObject AzuraDialog;
    [SerializeField] GameObject RydenDialog;
    [SerializeField] GameObject DescriptionDialogue;

    bool justOpened;


    private void StartDialog(Characters speaker, string text)
    {
        int character = (int)speaker;
        switch (character)
        {
            case 0:
                AzuraDialog.SetActive(false);
                RydenDialog.SetActive(false);
                LilimDialog.SetActive(true);
                DescriptionDialogue.SetActive(false);
                LilimDialog.GetComponentInChildren<TextMeshProUGUI>().text = text;
                break;
            case 1:
                AzuraDialog.SetActive(true);
                RydenDialog.SetActive(false);
                LilimDialog.SetActive(false);
                DescriptionDialogue.SetActive(false);
                AzuraDialog.GetComponentInChildren<TextMeshProUGUI>().text = text;
                break;
            case 2:
                AzuraDialog.SetActive(false);
                RydenDialog.SetActive(true);
                LilimDialog.SetActive(false);
                DescriptionDialogue.SetActive(false);
                RydenDialog.GetComponentInChildren<TextMeshProUGUI>().text = text;
                break;
            case 3:
                AzuraDialog.SetActive(false);
                RydenDialog.SetActive(false);
                LilimDialog.SetActive(false);
                DescriptionDialogue.GetComponentInChildren<TextMeshProUGUI>().text = text;
                DescriptionDialogue.SetActive(true);
                break;
            default:
                AzuraDialog.SetActive(false);
                RydenDialog.SetActive(false);
                LilimDialog.SetActive(false);
                DescriptionDialogue.SetActive(false);
                break;

        }
        count++;
        if (count == scriptLength) Resolve();

    }

    public void ParseScript(DialogAsset[] script)
    {
        justOpened = true;
        count = 0;
        resolved = false;
        scriptLength = script.Length;
        currentConvo = script;
        StartDialog(currentConvo[count].character, currentConvo[count].text);
    }

    public void Update()
    {
        if (currentConvo == null) return;
        if (!resolved && !justOpened && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            StartDialog(currentConvo[count].character, currentConvo[count].text);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Resolve();
        }
        if (justOpened) justOpened = false;
    }

    private void Resolve()
    {
        AzuraDialog.SetActive(false);
        RydenDialog.SetActive(false);
        LilimDialog.SetActive(false);
        DescriptionDialogue.SetActive(false);
        resolved = true;
    }

}
