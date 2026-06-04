using System;
using UnityEngine;

// SequenceSwitch.s Represents one interactable swithc in a sequence puzzle. 
public class SequenceSwitch : MonoBehaviour, IInteractable
{
    [SerializeField] private SequencePuzzleManager puzzleManager;
    [SerializeField] private string switchId;

    [Header("Visuals")]
    [SerializeField] private Light switchLight;
    [SerializeField] private Color inactiveColor = Color.white;
    [SerializeField] private Color activeColor = Color.yellow;
    [SerializeField] private Color successColor = Color.green;
    [SerializeField] private Color failureColor = Color.red;

    private bool _hasBeenPressed; 

    // Public ID used by the puzzle manager to identify this switch. 
    public string SwitchId => switchId;

    private void Start()
    {
        SetInactive(); 
    }

    public void Interact()
    {
        if (_hasBeenPressed)                                                                    // Ignore repeated interaction unil the puzzle resets this switch.
        {
            return; 
        }

        _hasBeenPressed = true;
        SetActive(); 

        if (puzzleManager != null )
        {
            puzzleManager.RegisterSwitchPress(this);                                            // Notify the puzzle manager that this witch was pressed. 
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} has no SequencePuzzleManager assigned.");
        }
    }

    // Allow this switch to be pressed again. 
    public void ResetSwitch()
    {
        _hasBeenPressed = false;
        SetInactive();
    }

    public void SetSuccess()
    {
        if (switchLight != null)
        {
            switchLight.enabled = true;
            switchLight.color = successColor;
        }
    }

    public void SetFailure()
    {
        if (switchLight != null)
        {
            switchLight.enabled = true;
            switchLight.color = failureColor;
        }
    }

    private void SetActive()
    {
        if (switchLight != null)
        {
            switchLight.enabled = true;
            switchLight.color = activeColor; 
        }

    }

    private void SetInactive()
    {
        if (switchLight != null )
        {
            switchLight.enabled = true;
            switchLight.color = inactiveColor; 
        }
    }
}
