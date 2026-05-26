using UnityEngine;

public class LightSwitch : MonoBehaviour, IInteractable
{
    [SerializeField] private Light targetLight;
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
