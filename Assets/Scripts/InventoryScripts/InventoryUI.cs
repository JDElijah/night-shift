using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

using System.Collections.Generic;
using UnityEngine.EventSystems; 


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

    private bool isInventoryOpen;

    private void Start()
    {
        inventoryPanel.SetActive(false);
        inspectionPanel.SetActive(false);
    }

    private void OnEnable()
    {
        inventoryAction.action.performed += ToggleInventory;
        inventoryAction.action.Enable();
    }

    private void OnDisable()
    {
        inventoryAction.action.performed -= ToggleInventory;
        inventoryAction.action.Disable();
        
    }

    private void ToggleInventory(InputAction.CallbackContext context)
    {
        if (!context.performed) return; 

        isInventoryOpen = !isInventoryOpen;
        inventoryPanel.SetActive(isInventoryOpen); 

        if (isInventoryOpen)
        {
            RefreshInventory();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            playerMovement.enabled = false;
            interactor.enabled = false;
            cameraInputController.enabled = false;

            Debug.Log("Inventory open: " + isInventoryOpen);
            Debug.Log("Cursor visible: " + Cursor.visible);
            Debug.Log("Cursor lock state: " + Cursor.lockState);
        }
        else
        {
            inspectionPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            playerMovement.enabled = true; 
            interactor.enabled = true;
            cameraInputController.enabled = true;
        }
    }

    private void RefreshInventory()
    {
        foreach (Transform child in noteButtonContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (NoteData note in InventoryManager.Instance.CollectedNotes)
        {
            Button button = Instantiate(noteButtonPrefab, noteButtonContainer);

            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.text = note.title;
            }

            Debug.Log("Created button for note: " + note.title);

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                Debug.Log("Button Clicked for note: " + note.title);
                InspectNote(note);
            });
        }
    }

    private void InspectNote(NoteData note)
    {
        Debug.Log("Clicked note: " + note.title); 

        inspectionPanel.SetActive(true);
        noteTitleText.text = note.title; 
        noteBodyText.text = note.bodyText;
    }

    private void Update()
    {
        if (!isInventoryOpen) return; 

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);

            pointerData.position = Mouse.current.position.ReadValue();

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            Debug.Log("UI objects under mouse: ");

            foreach (RaycastResult result in results)
            {
                Debug.Log(result.gameObject.name); 
            }
        }
    }

}
