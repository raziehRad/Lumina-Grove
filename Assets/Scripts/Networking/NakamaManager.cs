
using System;
using System.Linq;
using System.Threading.Tasks;
using Nakama;
using UnityEngine;

public class NakamaManager : MonoBehaviour
{
    private IClient client;
    private ISession session;

    private const string Scheme = "http";
    private const string Host = "127.0.0.1";
    private const int Port = 7350;
    private const string ServerKey = "defaultkey";

    private async void Start()
    {
        await Connect();
        var leaderboard = await GetLeaderboard();

        if (leaderboard != null)
        {
            foreach (var record in leaderboard.Records)
            {
                Debug.Log(
                    $"Rank: {record.Rank} | Name: {record.Username} | Time: {record.Score}"
                );
            }
        }
    }

    private async Task Connect()
    {
        try
        {
            client = new Client(
                Scheme,
                Host,
                Port,
                ServerKey,
                UnityWebRequestAdapter.Instance
            );

            string deviceId = PlayerPrefs.GetString("nakama.deviceId");

            if (string.IsNullOrEmpty(deviceId))
            {
                deviceId = Guid.NewGuid().ToString();
                PlayerPrefs.SetString("nakama.deviceId", deviceId);
                PlayerPrefs.Save();
            }

            session = await client.AuthenticateDeviceAsync(deviceId);

            Debug.Log("Connected to Nakama!");
            Debug.Log("Player ID: " + session.UserId);
          
            //nicknameUIController.ShowNicknamePanel();
        }
        catch (Exception e)
        {
            Debug.LogError("Nakama connection failed: " + e.Message);
        }
    }
    public async Task<bool> SetNickname(string nickname)
    {
        if (session == null)
        {
            Debug.LogError("Nakama session is not ready!");
            return false;
        }

        try
        {
            await client.UpdateAccountAsync(session, username: nickname);
            await client.WriteStorageObjectsAsync(
                session,
                new[]
                {
                    new WriteStorageObject
                    {
                        Collection = "player_settings",
                        Key = "nickname_selected",
                        Value = "{\"selected\":true}",
                        PermissionRead = 1,
                        PermissionWrite = 1
                    }
                }
            );
            Debug.Log("Nickname saved: " + nickname);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError("Nickname update failed: " + e.Message);
            return false;
        }
    }
    public async Task<string> GetNickname()
    {
        if (session == null)
            return null;

        try
        {
            var account = await client.GetAccountAsync(session);
            return account.User.Username;
        }
        catch (Exception e)
        {
            Debug.LogError("Get nickname failed: " + e.Message);
            return null;
        }
    }
    public async Task<bool> HasChosenNickname()
    {
        if (session == null)
            return false;

        try
        {
            var ids = new[]
            {
                new StorageObjectId
                {
                    Collection = "player_settings",
                    Key = "nickname_selected",
                    UserId = session.UserId
                }
            };

            var result = await client.ReadStorageObjectsAsync(session, ids);

            return result.Objects != null && result.Objects.Any();
        }
        catch (Exception e)
        {
            Debug.LogError("Nickname status check failed: " + e.Message);
            return false;
        }
    }
    public async Task SubmitScore(int score)
    {
        if (session == null)
        {
            Debug.LogError("Nakama session is not ready!");
            return;
        }

        try
        {
            string payload = JsonUtility.ToJson(
                new ScorePayload { score = score }
            );

            var result = await client.RpcAsync(
                session,
                "submit_score",
                payload
            );

            Debug.Log("Score submitted: " + result.Payload);
        }
        catch (Exception e)
        {
            Debug.LogError("Score submission failed: " + e.Message);
        }
    }
    public async Task<IApiLeaderboardRecordList> GetLeaderboard()
    {
        if (session == null)
        {
            Debug.LogError("Nakama session is not ready!");
            return null;
        }

        try
        {
            var result = await client.ListLeaderboardRecordsAsync(
                session,
                "global_leaderboard",
                limit: 10
            );

            Debug.Log("Leaderboard loaded! Records: " + result.Records.Count());

            return result;
        }
        catch (Exception e)
        {
            Debug.LogError("Leaderboard loading failed: " + e.Message);
            return null;
        }
    }
    [System.Serializable]
    private class ScorePayload
    {
        public int score;
    }
}
