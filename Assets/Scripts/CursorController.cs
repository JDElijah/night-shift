using UnityEngine;

// CursorController.cs handles the player's cursor state when the game starts. 


public class CursorController : MonoBehaviour
{
    // Locks and hides the cursor before gameplay begins. 
    public void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;       // Lock the cursor to the center of the game window.
        Cursor.visible = false;                         // Hide the cursor so it does not appear during first-person gamepla
    }
}
