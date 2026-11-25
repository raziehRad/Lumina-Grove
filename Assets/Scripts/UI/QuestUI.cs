using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private TMP_Text statusText;

    [SerializeField] private QuestManager questManager;

    private void OnEnable()
    {
        if (questManager == null)
        {
            Debug.LogError("QuestManager not found!");
            return;
        }

        questManager.OnQuestProgressChanged += UpdateProgress;
        questManager.OnQuestCompleted += ShowQuestCompleted;

        UpdateProgress();
    }

    private void OnDisable()
    {
        if (questManager == null)
            return;

        questManager.OnQuestProgressChanged -= UpdateProgress;
        questManager.OnQuestCompleted -= ShowQuestCompleted;
    }

    private void UpdateProgress()
    {
        progressText.text =
            $"{questManager.PlacedOrnaments}/{questManager.TotalOrnaments}";

        statusText.text =
            $"{questManager.CollectedOrnaments}/{questManager.TotalOrnaments}";
    }

    private void ShowQuestCompleted()
    {
        statusText.text =$"{questManager.CollectedOrnaments}/{questManager.TotalOrnaments}";
    }
}