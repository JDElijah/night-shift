using System;
using UnityEngine;

public class SequencePuzzleActivator : MonoBehaviour
{
    [SerializeField] private GameObject sequencePuzzleObject;

    private void Start()
    {
        if (sequencePuzzleObject != null)
        {
            sequencePuzzleObject.SetActive(false); 
        }
        
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnRequiredNotesCollected += ActivatePuzzle;
        }
    }

    private void OnDestroy()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnRequiredNotesCollected -= ActivatePuzzle;
        }
    }

    private void ActivatePuzzle()
    {
        if (sequencePuzzleObject == null)
        {
            Debug.LogWarning("No sequence puzzle object assigned.");
            return;
        }

        sequencePuzzleObject.SetActive(true);
        Debug.Log("Sequence puzzle activated."); 
    }
}
