using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Gameplay Controllers")]
    [SerializeField] private Behaviour[] gameplayControllers;

    [Header("Gameplay UI")]
    [SerializeField] private GameObject questPanel;
    [SerializeField] private GameObject mobileControls;

    private bool isQuestActive;
    private float elapsedTime;
    private int lastReportedSecond;

    private void Awake()
    {
        SetGameplayEnabled(false);

        questPanel.SetActive(false);
        mobileControls.SetActive(false);
    }

    private void Update()
    {
        if (!isQuestActive)
            return;

        elapsedTime += Time.deltaTime;

        int currentSecond = Mathf.FloorToInt(elapsedTime);

        if (currentSecond != lastReportedSecond)
        {
            lastReportedSecond = currentSecond;
            GameEvents.OnTimerUpdated?.Invoke(currentSecond);
        }
    }

    public void StartQuest()
    {
        elapsedTime = 0f;
        lastReportedSecond = 0;
        isQuestActive = true;

        questPanel.SetActive(true);
        mobileControls.SetActive(true);

        SetGameplayEnabled(true);

        GameEvents.OnTimerUpdated?.Invoke(0);
        GameEvents.OnQuestStarted?.Invoke();
    }

    public void CompleteQuest()
    {
        if (!isQuestActive)
            return;

        isQuestActive = false;

        SetGameplayEnabled(false);

        questPanel.SetActive(false);
        mobileControls.SetActive(false);

        GameEvents.OnQuestCompleted?.Invoke();
    }

    private void SetGameplayEnabled(bool value)
    {
        foreach (Behaviour controller in gameplayControllers)
        {
            if (controller != null)
                controller.enabled = value;
        }
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }
}