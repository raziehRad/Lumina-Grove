using System;

public static class GameEvents
{
    public static Action OnQuestStarted;
    public static Action OnQuestCompleted;
    public static Action<int> OnTimerUpdated;
}