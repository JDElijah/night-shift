using UnityEngine;

public class CursorController : MonoBehaviour
{
    public void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
    }
}
