
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChristmasTree : MonoBehaviour, IInteractable
{
    [Header("Quest Reference")]
    [SerializeField] private QuestManager questManager;

    [Header("Decoration Settings")]
    [SerializeField] private List<GameObject> ornaments;

    [Header("Ending Sequence")]
    [SerializeField] private EndingCameraController endingCameraController;
    [SerializeField] private CelebrationController celebrationController;
    [SerializeField] private CameraMovement cameraMovement;

    private bool completionEffectPlayed;

    public string InteractionMessage => "Decorate Tree";

    private void Awake()
    {
        if (questManager == null)
            Debug.LogError("QuestManager not assigned!", this);

        foreach (GameObject ornament in ornaments)
        {
            if (ornament != null)
                ornament.SetActive(false);
        }
    }

    public void Interact()
    {
        if (questManager == null || completionEffectPlayed)
            return;

        int previousCount = questManager.PlacedOrnaments;

        if (questManager.CollectedOrnaments <= previousCount)
        {
            Debug.Log("No ornaments available!");
            return;
        }

        questManager.PlaceOrnament();

        int currentCount = questManager.PlacedOrnaments;

        UpdateDecoration(previousCount, currentCount);

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(SoundType.DepositItem);

        if (questManager.IsQuestCompleted)
            CompleteDecoration();
    }

    private void UpdateDecoration(int previousCount, int currentCount)
    {
        for (int i = previousCount; i < currentCount; i++)
        {
            if (i >= ornaments.Count)
                break;

            if (ornaments[i] != null)
                ornaments[i].SetActive(true);
        }
    }

    private void CompleteDecoration()
    {
        if (completionEffectPlayed)
            return;

        cameraMovement.enabled = false;
        completionEffectPlayed = true;
        StartCoroutine(EndingSequence());
    }

    private IEnumerator EndingSequence()
    {
        if (endingCameraController != null)
            yield return StartCoroutine(
                endingCameraController.MoveToEndingPosition()
            );
        else
            Debug.LogError("EndingCameraController is not assigned!", this);

        if (celebrationController != null)
            celebrationController.PlayCelebration();
        else
            Debug.LogError("CelebrationController is not assigned!", this);
    }
}
