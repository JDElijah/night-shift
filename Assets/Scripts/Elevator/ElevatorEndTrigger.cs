using UnityEngine;

public class ElevatorEndTrigger : MonoBehaviour
{
    [SerializeField] private ElevatorExit elevatorExit; 

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (elevatorExit == null)
        {
            Debug.LogWarning("No ElevatorExit assigned to ElevatorEndTrigger.");
            return; 
        }

        elevatorExit.TriggerEnding();
    }
}
