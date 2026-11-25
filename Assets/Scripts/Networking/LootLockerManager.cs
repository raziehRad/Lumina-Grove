using System;
using System.Threading.Tasks;
using UnityEngine;
using LootLocker.Requests;
using Nakama;

public class LootLockerManager : MonoBehaviour
    {
        public bool isLoggedIn;
        [SerializeField] private string leaderboardKey="38951";
        private void Start()
        {
            StartGuestSession();
        }
        void StartGuestSession()
        {
            LootLockerSDKManager.StartGuestSession((response) =>
            {
                if (!response.success)
                {
                    Debug.LogWarning("player authentication failed"+response.errorData.message);
                }
                else
                {
                    isLoggedIn = true;
                    Debug.Log("successfully authentication player");
                    GameEvents.RaiseShowNicknamePanel();
                }
            });
        }

        public void SubmitScore(int _score)
        {
            if (!isLoggedIn)
            {
                return;
            }
            LootLockerSDKManager.SubmitScore("joy3uqmr",_score,leaderboardKey, response =>
            {
                if (!response.success)
                {
                    Debug.Log("failed to submit ,error" +response.errorData.message);
                }
                else
                {
                    Debug.Log("successfully submited score"+ _score);
                    
                }
            });
        }

     
        public async Task<LootLockerGetScoreListResponse> GetLeaderboard()
        {
            var tcs = new TaskCompletionSource<LootLockerGetScoreListResponse>();

            if (!isLoggedIn)
            {
                tcs.TrySetResult(null);
                return await tcs.Task;
            }

            LootLockerSDKManager.GetScoreList(
                leaderboardKey,
                50,
                0,
                response =>
                {
                    if (!response.success)
                    {
                        Debug.LogError(
                            "Failed to get leaderboard: " +
                            response.errorData.message
                        );
                    }
                    else
                    {
                        Debug.Log("Leaderboard loaded successfully!");
                    }

                    tcs.TrySetResult(response);
                }
            );

            return await tcs.Task;
        }

        public async Task<bool> HasChosenNickname()
        {
            var tcs = new TaskCompletionSource<bool>();

            LootLockerSDKManager.GetPlayerName(response =>
            {
                bool hasName = response.success &&
                               !string.IsNullOrWhiteSpace(response.name);

                tcs.TrySetResult(hasName);
            });

            return await tcs.Task;
        }

        public async Task<bool> SetNickname(string nickname)
        {
            var tcs = new TaskCompletionSource<bool>();

            LootLockerSDKManager.SetPlayerName(nickname, response =>
            {
                tcs.TrySetResult(response.success);
            });

            return await tcs.Task;
        }
    }
