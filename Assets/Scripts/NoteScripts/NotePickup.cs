using UnityEngine;

/*
    NotePickup.cs allows a note object in the scene to be collected through the interaction system

    This class implements IInteractiable, so the player's Interactor script
    can call Interact() when the player looks at the note and presses the interact button.
 */

public class NotePickup : MonoBehaviour, IInteractable
{
    [SerializeField] private NoteData noteData; 

    public void Interact()
    {
        if (noteData == null)
        {
            Debug.LogWarning("No NoteData assigned to NotePickup.");
            return;
        }

        InventoryManager.Instance.AddNote(noteData);
        gameObject.SetActive(false); 
    }
}
