using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ElevatorExit : MonoBehaviour
{
    [Header("State")]
    [SerializeField] private bool isUnlocked;

    [Header("Scene")]
    [SerializeField] private string endingSceneName;

    [Header("Doors")]
    [SerializeField] private Transform leftDoor;
    [SerializeField] private Transform rightDoor;
    [SerializeField] private Vector3 leftDoorOpenOffset= new Vector3(1.5f, 0f, 0f);
    [SerializeField] private Vector3 rightDoorOpenOffset = new Vector3(-1.5f, 0f, 0f);
    [SerializeField] private float doorOpenSpeed = 2f; 


    [Header("Optional Visuals")]
    [SerializeField] private Light elevatorLight;
    [SerializeField] private Color lockedColor = Color.red;
    [SerializeField] private Color unlockedColor = Color.green;

    [Header("Optional Fade")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 1.5f;

    private Vector3 _leftDoorClosedPosition;
    private Vector3 _rightDoorClosedPosition;
    private Vector3 _leftDoorOpenPosition;
    private Vector3 _rightDoorOpenPosition;

    private bool _doorsOpened;
    private bool _isEnding;

    public bool IsUnlocked => isUnlocked;

    private void Awake()
    {
        if (leftDoor != null)
        {
            _leftDoorClosedPosition = leftDoor.localPosition;
            _leftDoorOpenPosition = _leftDoorClosedPosition + leftDoorOpenOffset;
        }

        if (rightDoor != null)
        {
            _rightDoorClosedPosition = rightDoor.localPosition;
            _rightDoorOpenPosition = _rightDoorClosedPosition + rightDoorOpenOffset;
        }
    }

    private void Start()
    {
        UpdateVisuals(); 
    }

    public void Unlock()
    {
        if (isUnlocked)
        {
            return;
        }

        isUnlocked = true;
        UpdateVisuals();

        Debug.Log("Elevator unlocked.");

        if (!_doorsOpened)
        {
            StartCoroutine(OpenDoors()); 
        }
    }
    
    public void TriggerEnding()
    {
        if (!isUnlocked)
        {
            Debug.Log("Elevator ending trigger entered, but elevator is locked.");
            return;
        }

        if (_isEnding)
        {
            return;
        }
        
        StartCoroutine(EndGame());
    }
    //public void Interact()
    //{
    //    if (!isUnlocked)
    //    {
    //        Debug.Log("Elevator is locked.");
    //        return;
    //    }

    //    if (_isEnding)
    //    {
    //        return;
    //    }

    //    StartCoroutine(EndGame());
    //}

    private IEnumerator OpenDoors()
    {
        _doorsOpened = true; 

        while (leftDoor != null && rightDoor != null &&
            (Vector3.Distance(leftDoor.localPosition, _leftDoorOpenPosition) > 0.01f || Vector3.Distance(rightDoor.localPosition, _rightDoorOpenPosition) > 0.01f))
        {
            leftDoor.localPosition = Vector3.MoveTowards(leftDoor.localPosition, _leftDoorOpenPosition, doorOpenSpeed * Time.deltaTime);
            rightDoor.localPosition = Vector3.MoveTowards(rightDoor.localPosition, _rightDoorOpenPosition, doorOpenSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator EndGame()
    {
        _isEnding = true;

        Debug.Log("Elevator entered. Ending game.");

        if (fadeCanvasGroup != null)
        {
            float timer = 0f; 
            
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                fadeCanvasGroup.alpha = Mathf.Clamp01(timer / fadeDuration);
                yield return null;
            }
        }

        if (!string.IsNullOrEmpty(endingSceneName))
        {
            SceneManager.LoadScene(endingSceneName);
        }
        else
        {
            Debug.Log("Game complete.No ending scene assigned.");
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
