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
    public static UI ui;
    public static bool winner, pending, ready, restarting, retreived, winnerSet;
    public static string oppReady = "";
    public static string spawns;
    public static int spawnIndex, spawnAmount = 400;
    public static List<Spawn> objSpawns = new List<Spawn>();
    public static List<string> lbNames = new List<string>();
    public static List<int> lbScores = new List<int>();

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

    void Start()
    {
        GameCode.mp = this;
        Debug.Log("Initializing real-time multiplayer");


        // setupFireBase();
        retreiveLeaderboardData();
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
            leaderBoardResponse = JsonUtility.FromJson<LeaderBoardResponse>(response.downloadHandler.text);
            if (leaderBoardResponse.isSuccessful)
            {
                Debug.Log("Successfully gotten");
            }
            else
            {
                Debug.Log("this is the message: " + leaderBoardResponse.message);
            }
        }));
    }

    public void uploadHighScore()
    {
        //Upload Highscore to database
    }

}
