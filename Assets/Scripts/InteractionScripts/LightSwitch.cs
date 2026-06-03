using UnityEngine;

/*
    LightSwitch.cs allows a light to be toggled on and off through the interaction system. 

    This class implements IInteractable, which means the Interactor script can call
    Interact() without needing to know this object is specifically a light switch. 
 */

public class LightSwitch : MonoBehaviour, IInteractable
{
    [SerializeField] private Light targetLight;

    // Toggles the assigned light on or off when the player interacts with this object. 
    public void Interact()
    {
        if (targetLight == null)
        {
            Debug.LogWarning("No light was assigned to LightSwitch");
            return;
        }

        targetLight.enabled = !targetLight.enabled;
    }
}
