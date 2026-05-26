using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance {  get; private set; }

    private readonly List<NoteData> collectedNotes = new(); 

    public IReadOnlyList<NoteData> CollectedNotes => collectedNotes;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return; 
        }

        Instance = this; 
    }

    public void AddNote(NoteData note)
    {
        if (note == null) return; 

        if (!collectedNotes.Contains(note))
        {
            collectedNotes.Add(note);
            Debug.Log("Added note: " + note.title);
        }
    }
}
