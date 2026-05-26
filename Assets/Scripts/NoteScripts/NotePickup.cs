using UnityEngine;

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
