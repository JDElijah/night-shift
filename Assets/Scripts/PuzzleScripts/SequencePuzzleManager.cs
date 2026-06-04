using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequencePuzzleManager : MonoBehaviour
{
    [Header("Puzzle Setup")]
    [SerializeField] private List<SequenceSwitch> switches = new();
    [SerializeField] private List<string> correctSequence = new();

    [Header("Timing")]
    [SerializeField] private float resetDelay = 1.5f;

    [Header("Completion")]
    [SerializeField] private ElevatorExit elevatorExit; 

    private readonly List<string> _playerSequence = new();
    private bool _isCheckingSequence;
    private bool _puzzleSolved; 

    public void RegisterSwitchPress(SequenceSwitch sequenceSwitch)
    {
        if (_isCheckingSequence || _puzzleSolved)
        {
            return;
        }

        _playerSequence.Add(sequenceSwitch.SwitchId);
        Debug.Log("Pressed switch: " + sequenceSwitch.SwitchId);

        if (_playerSequence.Count >= correctSequence.Count)
        {
            StartCoroutine(CheckSequence());                            // StartCoroutine is a Unity method that starts a coroutine. Lets yourun code over mulitple frames instead of all at once.
        }
    }

    private IEnumerator CheckSequence()
    {
        _isCheckingSequence = true;

        bool isCorrect = true; 

        for (int i =0; i < correctSequence.Count; i++)
        {
            if (_playerSequence[i] != correctSequence[i])
            {
                isCorrect = false;
                break;
            }
        }

        if (isCorrect)
        {
            HandleSuccess();
        }
        else
        {
            yield return StartCoroutine(HandleFailure());
        }
        _isCheckingSequence = false; 
    }
    private void HandleSuccess()
    {
        _puzzleSolved = true;
        
        foreach (SequenceSwitch sequenceSwitch in switches)
        {
            sequenceSwitch.SetSuccess();
        }
        Debug.Log("Correct sequence! Puzzle Solved.");

        if (elevatorExit != null)
        {
            elevatorExit.Unlock();
        }
        else
        {
            Debug.LogWarning("No ElevatorExit assigned to SequencePuzzleManager.");
        }

        Debug.Log("Correct sequence! Puzzle solved."); 
    }

    private IEnumerator HandleFailure()
    {
        foreach (SequenceSwitch sequenceSwitch in switches)
        {
            sequenceSwitch.SetFailure();
        }

        Debug.Log("Wrong sequence. Resettting puzzle.");

        yield return new WaitForSeconds(resetDelay);

        _playerSequence.Clear();

        foreach (SequenceSwitch sequenceSwitch in switches)
        {
            sequenceSwitch.ResetSwitch(); 
        }
    }
} 
