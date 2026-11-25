
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LootLocker.Requests;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LeaderboardUIController : MonoBehaviour
{
    [Header("Rank Medals")]
    [SerializeField] private Sprite goldMedal;
    [SerializeField] private Sprite silverMedal;
    [SerializeField] private Sprite bronzeMedal;
    [Header("References")]
    [SerializeField] private LootLockerManager lootLockerManager;
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private Transform content;
    [SerializeField] private LeaderboardRowUI recordPrefab;
    [SerializeField] private Button closeButton;
    [SerializeField] private TMP_Text statusText;
    private GameObject previousPanel;
    private void Awake()
    {
        leaderboardPanel.SetActive(false);
    }

    private void OnEnable()
    {
        closeButton.onClick.AddListener(CloseLeaderboard);
        GameEvents.OnOpenLeaderboard += OpenLeaderboard;
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(CloseLeaderboard);
        GameEvents.OnOpenLeaderboard -= OpenLeaderboard;
    }

    public async void OpenLeaderboard(GameObject returnPanel)
    {
        previousPanel = returnPanel;

        if (previousPanel != null)
            previousPanel.SetActive(false);

        leaderboardPanel.SetActive(true);
        statusText.text = "Loading...";
        ClearRecords();

        LootLockerGetScoreListResponse result =
            await lootLockerManager.GetLeaderboard();

        if (!leaderboardPanel.activeInHierarchy)
            return;

        if (result == null)
        {
            statusText.text = "Failed to load leaderboard.";
            return;
        }

        statusText.text = "";
        DisplayRecords(result);
    }
   
    private void DisplayRecords(LootLockerGetScoreListResponse result)
    {
        if (result.items == null || result.items.Length == 0)
        {
            statusText.text = "No records yet.";
            return;
        }

        foreach (var record in result.items)
        {
            LeaderboardRowUI row = Instantiate(recordPrefab, content);

            if (row == null)
            {
                Debug.LogError("LeaderboardRowUI is missing!");
                continue;
            }

            string playerName = string.IsNullOrEmpty(record.player?.name)
                ? "Gardener"
                : record.player.name;

            row.Setup(
                record.rank.ToString(),
                playerName,
                record.score
            );
        }
    }

    private void ClearRecords()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }

    public void CloseLeaderboard()
    {
        leaderboardPanel.SetActive(false);

        if (previousPanel != null)
            previousPanel.SetActive(true);

        previousPanel = null;
    }
}
