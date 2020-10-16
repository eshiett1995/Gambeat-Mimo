using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System.Net;
using static MatchSearchResponse;

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
        int pageNumber = 0;
        MatchSearchResponse matchSearchResponse = new MatchSearchResponse();
        StartCoroutine(HttpUtil.Get($"{HttpUtil.matchHistory}/{pageNumber}", (response) =>
        {
            Debug.Log("the response text " + response.downloadHandler.text);
            matchSearchResponse = JsonUtility.FromJson<MatchSearchResponse>(response.downloadHandler.text);
            if (matchSearchResponse.isSuccessful || matchSearchResponse.successful)
            {
                for (var index = 0; index < matchSearchResponse.content.Count(); index++)
                {
                    String tournamentStat = $"Date: {FromUnixTime(matchSearchResponse.content[index].startTime)}\n" +
                                        $"Match details: {matchSearchResponse.content[index].name}\n" +
                                        $"Entry fee: N{matchSearchResponse.content[index].entryFee/1000}\n" +
                                        $"Price: N{(matchSearchResponse.content[index].entryFee/1000) * matchSearchResponse.content[index].numberOfCompetitors}\n" +
                                        $"Status:{GetStatus(matchSearchResponse.content[index])}";
                    transactions.Add(tournamentStat);

                    if(index == matchSearchResponse.content.Count()-1)
                        UI.doneLoading = true;
                }
            }
            else
            {
                UI.doneLoading = true;
                Debug.Log("this is the message: " + matchSearchResponse.message);
            }
        }));

        Debug.Log("Uploading Highscore to database");

    }

    public String GetStatus(FormattedMatch formattedMatch) {
        if (formattedMatch.winner)
        {
            return "Winner";
        }
        else if (!formattedMatch.matchEnded)
        {
            return "Result pending";
        }
        else {
            return "Losser";
        }
    }
    public String FromUnixTime(long unixTime)
    {
        var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return epoch.AddMilliseconds(unixTime).ToString("dd-MM-yyyy");
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
