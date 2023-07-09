using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


[SerializeField]
public enum Characters
{
    Lilim,
    Azura,
    Ryden,
    ENDCONVERSATION
}

[System.Serializable]
public struct DialogAsset
{
    public Characters character;
    public string text;

}

[SerializeField]
[CreateAssetMenu(menuName = "Script/Conversation Data")]
public class DialogData : ScriptableObject
{
    public DialogAsset[] script;
}