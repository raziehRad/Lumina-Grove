using UnityEngine;
using System;
using UnityEngine.Serialization;

public class QuestManager : MonoBehaviour
{
    [Header("Quest Settings")]
    [SerializeField] private int totalOrnaments = 10;

    private int collectedOrnaments;
    private int placedOrnaments;
    private bool isQuestCompleted;

    public int TotalOrnaments => totalOrnaments;
    public int CollectedOrnaments => collectedOrnaments;
    public int PlacedOrnaments => placedOrnaments;
    public int RemainingOrnaments => totalOrnaments - placedOrnaments;
    public bool IsQuestCompleted => isQuestCompleted;

    public event Action OnQuestProgressChanged;
    public event Action OnQuestCompleted;

    public void CollectOrnament()
    {
        if (isQuestCompleted || collectedOrnaments >= totalOrnaments)
            return;

        collectedOrnaments++;

        OnQuestProgressChanged?.Invoke();
    }

    public void PlaceOrnament()
    {
        if (isQuestCompleted)
            return;

        int amountToPlace = collectedOrnaments - placedOrnaments;

        if (amountToPlace <= 0)
            return;

        placedOrnaments += amountToPlace;

        OnQuestProgressChanged?.Invoke();

        if (placedOrnaments >= totalOrnaments)
        {
            isQuestCompleted = true;
            OnQuestCompleted?.Invoke();
        }
    }
}