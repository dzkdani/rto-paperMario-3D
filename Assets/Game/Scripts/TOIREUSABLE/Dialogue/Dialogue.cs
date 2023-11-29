using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueObj")]
public class Dialogue : ScriptableObject
{
    public PotraitData[] potraitDatas;
    public Conversation[] Dialogues;
}

[System.Serializable]
public class Conversation
{
    public string characterId;
    [TextArea] public string Dialogue;
}




