using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Gameplay Controllers")]
    [SerializeField] private Behaviour[] gameplayControllers;

    [Header("Gameplay UI")]
    [SerializeField] private GameObject questPanel;
    [SerializeField] private GameObject mobileControls;
    [SerializeField] private GameObject interactPanel;

    private bool isQuestActive;
    private float elapsedTime;
    private int lastReportedSecond;

    private void Awake()
    {
        SetGameplayEnabled(false);

        questPanel.SetActive(false);
        mobileControls.SetActive(false);
    }

    private void OnEnable()
    {
        GameEvents.OnStartQuest += StartQuest;
        GameEvents.OnCompleteQuest += CompleteQuest;
        GameEvents.OnBackToMainMenu += HandleBackToMainMenu;
        GameEvents.OnOpenPauseMenu += HandleOpenPauseMenu;
    }

    private void OnDisable()
    {
        GameEvents.OnStartQuest -= StartQuest;
        GameEvents.OnCompleteQuest -= CompleteQuest;
        GameEvents.OnBackToMainMenu -= HandleBackToMainMenu;
        GameEvents.OnOpenPauseMenu -= HandleOpenPauseMenu;
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
            GameEvents.RaiseTimerUpdated(currentSecond);
        }
    }
    private void HandleBackToMainMenu()
    {
        isQuestActive = false;

        SetGameplayEnabled(false);

        questPanel.SetActive(false);
        mobileControls.SetActive(false);
        interactPanel.SetActive(false);
    }
    private void HandleOpenPauseMenu(bool value)
    {
        SetGameplayEnabled(value);

        questPanel.SetActive(value);
        mobileControls.SetActive(value);
        interactPanel.SetActive(false);
    }
    public void StartQuest()
    {
        Time.timeScale = 1f;
        elapsedTime = 0f;
        lastReportedSecond = 0;
        isQuestActive = true;

        questPanel.SetActive(true);
        mobileControls.SetActive(true);

        SetGameplayEnabled(true);

        GameEvents.RaiseTimerUpdated(0);
        GameEvents.RaiseQuestStarted();
    }

    public void CompleteQuest()
    {
        if (!isQuestActive)
            return;

        isQuestActive = false;

        SetGameplayEnabled(false);

        questPanel.SetActive(false);
        mobileControls.SetActive(false);

        GameEvents.RaiseQuestCompleted();
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