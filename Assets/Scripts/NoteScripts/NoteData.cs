using UnityEngine;

[CreateAssetMenu(fileName = "New Note", menuName = "Night Shift/Note")]
public class NoteData : ScriptableObject
{
    [TextArea(3, 10)]
    public string title;

    [TextArea(5, 20)]
    public string bodyText;
}

