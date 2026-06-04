using UnityEngine;
using UnityEngine.SceneManagement;

public class ElevatorExit : MonoBehaviour, IInteractable
{
    [SerializeField] private bool isUnlocked;
    [SerializeField] private string endingSceneName;

    [Header("Optional Visuals")]
    [SerializeField] private Light elevatorLight;
    [SerializeField] private Color lockedColor = Color.red;
    [SerializeField] private Color unlockedColor = Color.green;

    private void Start()
    {
        UpdateVisuals(); 
    }

    public void Unlock()
    {
        isUnlocked = true;
        UpdateVisuals();
        Debug.Log("Elevator unlocked.");
    }

    public void Interact()
    {
        if (!isUnlocked)
        {
            Debug.Log("Elevator is locked.");
            return;
        }

        Debug.Log("Elevator entered. Game ending."); 

        if (!string.IsNullOrEmpty(endingSceneName))
        {
            SceneManager.LoadScene(endingSceneName);
        }

        else
        {
            Debug.Log("Game complete. No eding scene assigned."); 
        }
    }

    private void UpdateVisuals()
    {
        if (elevatorLight == null)
        {
            return; 
        }

        elevatorLight.color = isUnlocked ? unlockedColor : lockedColor;
    }
}
