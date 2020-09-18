using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System.Net;

public class Multiplayer : MonoBehaviour
{
    public string matchID;
    public static string userName, oppName;
    public static string gameID;
    public static int stake = 100, oppLife = 3, oppScore;
    public static Connection connection = Connection.Offline;
    public static State state = State.Searching;

    public static GameType type = GameType.Single;

    public static UI ui;
    public static bool winner, pending, ready, restarting, retreived, winnerSet;
    public static string oppReady = "";
    public static string spawns;
    public static int spawnIndex, spawnAmount = 400;
    public static List<Spawn> objSpawns = new List<Spawn>();
    public static List<string> lbNames = new List<string>();
    public static List<int> lbScores = new List<int>();
    public static List<string> transactions = new List<string>();
    public static LeaderBoardResponse leaderBoardData = new LeaderBoardResponse();

    public enum Connection
    {
        Offline,
        Host,
        Client
    }

    public enum State
    {
        Searching,
        Pairing,
        Waiting,
        Playing,
        Finished
    }
    public enum GameType{
        Single,
        OnevOne,
        Royal,
        League
    }

    void Start()
    {
        GameCode.mp = this;
        Debug.Log("Initializing real-time multiplayer");

    }

    void Update()
    {
        if (state == State.Playing)
        {
            //  sendData();
        }
    }

    public static bool CheckForInternetConnection()
    {
        try
        {
            using (var client = new WebClient())
            using (client.OpenRead("http://google.com/generate_204"))
                ui.internetText.text = "";
            return true;
        }
        catch
        {
            if (ui.isLoading)
                ui.internetText.text = "Check Your Internet Connection";
            return false;
        }
    }

    public void retreiveLeaderboardData()
    {
        LeaderBoardResponse leaderBoardResponse = new LeaderBoardResponse();
        StartCoroutine(HttpUtil.Get(HttpUtil.leaderBoardUrl, (response) =>
        {
            if (response.isNetworkError || response.isHttpError)
            {
                UI.doneLoading = true;
                if (Application.platform == RuntimePlatform.Android)
                {
                    UI.CustomAndroidToast("One vs One is coming soon");
                }
                return;
            }

            leaderBoardResponse = JsonUtility.FromJson<LeaderBoardResponse>(response.downloadHandler.text);
            leaderBoardData = leaderBoardResponse;
            if (leaderBoardResponse.isSuccessful || leaderBoardResponse.successful)
            {
                clearList();

                for (var index = 0; index < leaderBoardResponse.ranks.Count; index++)
                {
                    var rank = leaderBoardResponse.ranks[index];
                    lbNames.Add(rank.email);
                    lbScores.Add((int)rank.score);
                }
             
            }
            else
            {
                UI.doneLoading = true;
            }
            UI.doneLoading = true;
        }));
    }

    public static void clearList(){
        lbNames.Clear();
        lbScores.Clear();
    }

    public void retreiveHistory(){
        Debug.Log("Retreiving History");
        //Save list to Multiplayer.transactions
       
        UI.doneLoading = true;
    }

    public void uploadHighScore()
    {
        
        int newHighScore = GameCode.highScore;

        HighScoreRequest highscoreRequest = new HighScoreRequest
        {
            score = GameCode.highScore
        };

        ResponseModel responseModel = new ResponseModel();
        StartCoroutine(HttpUtil.Post(HttpUtil.leaderBoardUrl, JsonUtility.ToJson(highscoreRequest), (response) =>
        {
            responseModel = JsonUtility.FromJson<ResponseModel>(response.downloadHandler.text);
            if (responseModel.isSuccessful || responseModel.successful)
            {
              Debug.Log("HighScore updated to server");
            }
            else
            {
                Debug.Log("this is the message: " + responseModel.message);
            }
        }));

        Debug.Log("Uploading Highscore to database");
    }

}
