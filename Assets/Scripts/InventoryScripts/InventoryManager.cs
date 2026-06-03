using System;
using System.Collections.Generic;
using UnityEngine;

/*
    InventoryManager.cs manages the player's collected notes during gameplay.

    This classes uses the Singleton pattern so other scripts can access
    the same inventory instance without needing a direct reference.

    Singleton pattern is a creational pattern that restricts a 
    class to a single instance while providing a global point of access 
    to that instance across an entire application. 
 */

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance {  get; private set; }          // Global access point for the active InventoryManager.

    private readonly List<NoteData> collectedNotes = new();                 // Stores the notes the player has collected.
    public IReadOnlyList<NoteData> CollectedNotes => collectedNotes;        // Read-only access to the collected notes list. Other scripts can view the notes, but cannot directly modify the list. 

    [SerializeField] private int requiredNoteCount = 5;

    private bool hasCollectedRequiredNotes;

    public event Action OnRequiredNotesCollected; 

    private void Awake()
    {
        if (Instance != null && Instance != this)                           // If another InventoryManager already exists, destroy this duplicate. 
        {
            Destroy(gameObject);
            return; 
        }

        Instance = this;                                                    // Set this object as the active InventoryManager instance.
    }

    // Adds a note to the inventory if it is valid and has not already been collected. 
    public void AddNote(NoteData note)
    {
        if (note == null)
        {
            return;                                                         // Ignore empty note references.
        }

        if (collectedNotes.Contains(note))                                  // Prevent the same note from being added more than once.
        {
            return;
        }

        collectedNotes.Add(note);
        Debug.Log("Added note: " + note.title);

        CheckRequiredNotesCollected(); 
    }

    private void CheckRequiredNotesCollected()
    {
        // Once true once, don't continue. Just return. 
        if (hasCollectedRequiredNotes)
        {
            return;
        }

        // First time reaching the required amount.
        if (collectedNotes.Count >= requiredNoteCount)
        {
            hasCollectedRequiredNotes = true;
            Debug.Log("All required notes collected.");
            OnRequiredNotesCollected?.Invoke(); 
        }
    }
}
