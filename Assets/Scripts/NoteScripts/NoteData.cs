using UnityEngine;

/*
    NoteData.cs stores the data for a collectible note. 

    This is a ScriptableObject, meaning each note can be created as its own
    asset in the Unity Project window
 */

[CreateAssetMenu(fileName = "New Note", menuName = "Night Shift/Note")]
public class NoteData : ScriptableObject
{
    [TextArea(3, 10)]
    public string title;

    [TextArea(5, 20)]
    public string bodyText;
}

