using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

using System.Collections.Generic;
using UnityEngine.EventSystems; 

/*
    InventoryUI.cs controls the inventory UI for collected notes. 

    Responsibilities: 
        Opens and closes the inventory screen
        Displays collected notes as buttons
        Shows note details whn a note is selected
        Enables and disables player controls while the inventory is open.
 */


public class InventoryUI : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference inventoryAction;

    [Header("UI References")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Transform noteButtonContainer;
    [SerializeField] private Button noteButtonPrefab;

    [Header("Inspection References")]
    [SerializeField] private GameObject inspectionPanel;
    [SerializeField] private TMP_Text noteTitleText;
    [SerializeField] private TMP_Text noteBodyText;

    [Header("Player References")]
    [SerializeField] private MonoBehaviour playerMovement;
    [SerializeField] private Interactor interactor;
    [SerializeField] private MonoBehaviour cameraInputController;

    // Tracks if inventory is currently open.
    private bool isInventoryOpen;

    private void Start()
    {
        // Ensure inventory and note inspection UI start hidden.
        inventoryPanel.SetActive(false);
        inspectionPanel.SetActive(false);
    }

    private void OnEnable()
    { 
        inventoryAction.action.performed += ToggleInventory;                // Subscribe to inventory input event when this object becomes active.
        inventoryAction.action.Enable();                                    // Enable the input action so it can listen for player input. 
    }

    private void OnDisable()
    {
        inventoryAction.action.performed -= ToggleInventory;                // Unsubscribe from teh input event to avoid duplicate callbacks. 
        inventoryAction.action.Disable();                                   // Disable the input action when object is no longer active. 
        
    }

    // Opens or closes inventory UI when the inventory input is pressed
    private void ToggleInventory(InputAction.CallbackContext context)
    {
        if (!context.performed) return;                                     // Ignore this callback unless the input was performed.

        isInventoryOpen = !isInventoryOpen;                                 // Toggle the inventory state. 
        inventoryPanel.SetActive(isInventoryOpen);                          // Show/Hide main inventory panel.

        if (isInventoryOpen)
        {
            
            RefreshInventory();                                             // Rebuild the note button list using the currently collected notes. 
            Cursor.lockState = CursorLockMode.None;                         // Unlock and show the cursor so the player can click UI buttons. 
            Cursor.visible = true;

            playerMovement.enabled = false;                                 // Disable gameplay controls while inventory is in use.
            interactor.enabled = false;
            cameraInputController.enabled = false;

            Debug.Log("Inventory open: " + isInventoryOpen);
            Debug.Log("Cursor visible: " + Cursor.visible);
            Debug.Log("Cursor lock state: " + Cursor.lockState);
        }
        else
        {
            inspectionPanel.SetActive(false);                               // Hide the note inspection panel when closing inventory.
            Cursor.lockState = CursorLockMode.Locked;                       // Lock and hide cursor again for fps gameplay.
            Cursor.visible = false;

            playerMovement.enabled = true;                                  // Re-enable gameplay controls after leaving the inventory. 
            interactor.enabled = true;
            cameraInputController.enabled = true;
        }
    }

    // Rebuild the inventory note list by creating one button for each collected note. 
    private void RefreshInventory()
    {
        foreach (Transform child in noteButtonContainer)                    // Clear old note buttons so UI doesn't create duplicates
        {
            Destroy(child.gameObject);
        }

        foreach (NoteData note in InventoryManager.Instance.CollectedNotes) // Create a new button for each collected note. 
        {
            Button button = Instantiate(noteButtonPrefab, noteButtonContainer);

            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>(); // Set button label to the note's  title.
            if (buttonText != null)
            {
                buttonText.text = note.title;
            }

            Debug.Log("Created button for note: " + note.title);

            button.onClick.RemoveAllListeners();                             // Ensure the button only has this note's click behavior.
            button.onClick.AddListener(() =>                                 // When clicked, show the selecetd note's details
            {
                Debug.Log("Button Clicked for note: " + note.title);
                InspectNote(note);
            });
        }
    }

    // Displays the selected note's title and body text in the inspection panel.
    private void InspectNote(NoteData note)
    {
        Debug.Log("Clicked note: " + note.title); 

        inspectionPanel.SetActive(true);                                        // Show the note inspection UI.
        noteTitleText.text = note.title;                                        // Fill the UI text fields with selected note's data. 
        noteBodyText.text = note.bodyText;
    }

    private void Update()
    {
        if (!isInventoryOpen) return;                                           // Only Check UI clicks while inventory is open.

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);   // Create pointer data using the current mouse position.

            pointerData.position = Mouse.current.position.ReadValue();

            List<RaycastResult> results = new List<RaycastResult>();                    // Store all UI objects currently under mouse
            EventSystem.current.RaycastAll(pointerData, results);                       // Raycast through the UI to find what the mouse is pointing at. 

            Debug.Log("UI objects under mouse: ");

            foreach (RaycastResult result in results)
            {
                Debug.Log(result.gameObject.name); 
            }
        }
    }

}
