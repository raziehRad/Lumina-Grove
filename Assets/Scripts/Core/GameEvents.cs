using System;
using UnityEngine;

public static class GameEvents
{
    // Quest notifications
    public static event Action OnQuestStarted;
    public static event Action OnQuestCompleted;
    public static event Action<int> OnTimerUpdated;

    // Requests
    public static event Action OnStartQuest;
    public static event Action OnCompleteQuest;

    // UI events
    public static event Action OnBackToMainMenu;
    public static event Action<bool> OnOpenPauseMenu;
    public static event Action OnShowNicknamePanel;
    public static event Action<GameObject> OnOpenLeaderboard;

    // Camera events
    public static event Action OnStopCameraControl;

    // Publishers
    public static void RaiseQuestStarted() => OnQuestStarted?.Invoke();
    public static void RaiseQuestCompleted() => OnQuestCompleted?.Invoke();
    public static void RaiseTimerUpdated(int seconds) => OnTimerUpdated?.Invoke(seconds);

    public static void RaiseStartQuest() => OnStartQuest?.Invoke();
    public static void RaiseCompleteQuest() => OnCompleteQuest?.Invoke();

    public static void RaiseBackToMainMenu() => OnBackToMainMenu?.Invoke();
    public static void RaiseShowNicknamePanel() => OnShowNicknamePanel?.Invoke();
    public static void RaiseOpenLeaderboard(GameObject returnPanel) => OnOpenLeaderboard?.Invoke(returnPanel);

    public static void RaiseStopCameraControl() => OnStopCameraControl?.Invoke();

    public static void RaiseOpenPauseMenu(bool value) => OnOpenPauseMenu?.Invoke(value);
}