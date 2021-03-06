﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class GameCode : MonoBehaviour
{
    private float lastUpdate;
    public static int score, highScore;
    public GameObject platform, spikes, breakPlatform, health, blades, gameOverPanel, restartButt;
    public static int life = 3, spikeInterval, healthInterval;
    public Text scoreText, highscoreText;
    const string highscoreString = "highscore";
    public static bool running, setHighscore;
    public Button restart;
    public static int difficulty, bladeInterval, bladeStart;
    private static float defScrollSpeed, defPlatformSpeed;
    public static Multiplayer mp;
    public static GameCode game;
    public List<GameObject> leaderboardItems = new List<GameObject>();
    public static List<GameObject> tournamentItems = new List<GameObject>();
    public static List<GameObject> historyItems = new List<GameObject>();
    public AudioSource audio, audio2, music;
    public AudioClip bounceClip, dieClip, reviveClip, breakClip, buttonClip, lifeClip, m1,m2,m3,m4;
    private bool isSound = true, isMusic = true;

    public enum Sound
    {
        Bounce,
        Break,
        Die,
        Life,
        Revive,
        Button,
    }

    void Start()
    {
        game = this;
        defPlatformSpeed = Screen.height / 4.26f;
        PlatformPhysics.speed = defPlatformSpeed;
        defScrollSpeed = Screen.height / 256f;
        UI.scrollSpeed = defScrollSpeed;
        difficulty = 0;
        bladeInterval = Random.Range(25, 40);
        bladeStart = bladeInterval * 4;

        // Debug.Log("BladeNumber: " + bladeInterval);

        restart.onClick.AddListener(() => restartListener());
        Debug.Log("Game Start");

        if(isMusic){

            music.volume = 0.5f;
            
            int x = Random.Range(0,4);
            if(x==0)
                music.clip = m1;
            else if(x==1)
                music.clip = m2;
            else if(x==2)
                music.clip = m3;
            else if(x==3)
                music.clip = m4;
                
            
            music.Play();

        }
    }

     public void playSound(Sound sound)
    {
        audio.volume = 1.0f;
        audio2.volume = 1.0f;
        audio2.loop = false;
        int aud=1;

        switch (sound)
        {
            case Sound.Bounce:
                audio.clip = bounceClip;
                break;
            case Sound.Die:
                audio.clip = dieClip;
                break;
            case Sound.Life:
                audio.clip = lifeClip;
                break;    
            case Sound.Revive:
                audio2.clip = reviveClip;
                aud = 2;
                break;
            case Sound.Button:
                audio.clip = buttonClip;
                break;   
            case Sound.Break:
                audio2.clip = breakClip;
                aud = 2;
                break;       
            
        }

        if(isSound){
            if(aud==1)
                audio.Play();
           else if(aud==2)
                audio2.Play();
        }

        Debug.Log("Playing Sound " + sound.ToString());
        
    }

    private void restartListener()
    {
        // Debug.Log("Ad Interval:");
        playSound(Sound.Button);
        Debug.Log("Restart Game");

        running = true;
        resetElements();

        FindObjectOfType<PlayerPhysics>().respawn();

    }


    void Update()
    {

        if (Time.time - lastUpdate >= 1f)
        {

            lastUpdate = Time.time;
            UI.matchTimer--;
            PlayerPhysics.invincible--;
            Multiplayer.CheckForInternetConnection();
            Multiplayer.ui.setLeaderBoardData();
            if(FBHolder.isLoggedIn){
                FindObjectOfType<UI>().cashPanel.SetActive(true);
              
            }else
                FindObjectOfType<UI>().cashPanel.SetActive(false);

            if (life <= 0 && Multiplayer.oppLife <= 0)
            {
                running = false;
            }


            if (UI.matchTimer == 0)
            {
                Multiplayer.ui.startMultiPlayer();
            }

            if (Multiplayer.state == Multiplayer.State.Finished)
                Multiplayer.ui.showWinner();

            if (Multiplayer.connection != Multiplayer.Connection.Offline && (Multiplayer.state == Multiplayer.State.Pairing
                || Multiplayer.state == Multiplayer.State.Waiting))
            {
                //  mp.checkPairStatus();
            }


            if (running)
            {
                if (life > 0)
                    score++;

                if (score % 20 == 0 && difficulty < 12)
                {
                    PlatformPhysics.speed *= 1.05f;
                    UI.scrollSpeed *= 1.05f;
                    difficulty++;

                    Debug.Log("Difficulty:" + difficulty + " UI Speed:" + UI.scrollSpeed + " Platform Speed:" + PlatformPhysics.speed);
                }
                if (score % bladeInterval == 0 && score >= bladeStart)
                {
                    Instantiate(blades, new Vector3(0, 0, 0), Quaternion.identity, GameObject.FindGameObjectWithTag("Panel").transform);
                }
            }
        }
    }
    public void spawnPlatforms()
    {
        int rand = UnityEngine.Random.Range(0, 5);
        GameObject instance;
        spikeInterval++;
        // Debug.Log("Rand: " + rand + "Interval: " + spikeInterval);

        if (rand == 1 && spikeInterval > 1)
        {
            instance = spikes;
            spikeInterval = 0;

        }
        else if (rand == 2)
        {
            instance = breakPlatform;
        }
        else
        {
            instance = platform;
            Debug.Log("Spawn Me");
        }


        float randPos = UnityEngine.Random.Range((float)(Screen.width / 8.5), (float)(Screen.width / 1.18));

        if (PlatformPhysics.lastPlatform.transform.position.y >= Screen.height / 6)
        {

            Instantiate(instance, new Vector3(randPos, 0, 0), Quaternion.identity, GameObject.FindGameObjectWithTag("Panel").transform);

            int rand2 = Random.Range(0, 30);

            healthInterval++;

            if (rand2 == 1 && rand != 1 && healthInterval > 30)
            {
                Instantiate(health, new Vector3(randPos, health.gameObject.transform.localScale.y, -97f), Quaternion.identity, GameObject.FindGameObjectWithTag("Panel").transform);
                healthInterval = 0;
            }

        }
        else
            Debug.Log("Overdo");

    }

    public void spawnMultiplayerPlatforms(int item)
    {
        if (Multiplayer.spawnIndex >= Multiplayer.objSpawns.Count)
            Multiplayer.spawnIndex = 0;

        Spawn spawn = Multiplayer.objSpawns[Multiplayer.spawnIndex];
        GameObject instance = platform;
        float x = spawn.X / 720 * Screen.width;

        if (item == 1)
            instance = platform;
        else if (spawn.ID.Equals("platform"))
            instance = platform;
        else if (spawn.ID.Equals("breakPlatform"))
            instance = breakPlatform;
        else if (spawn.ID.Equals("spikes"))
            instance = spikes;

        Instantiate(instance, new Vector3(x, 0, 0), Quaternion.identity, GameObject.FindGameObjectWithTag("Panel").transform);

        Multiplayer.spawnIndex++;
    }


    public static void setMultiplayerSpawns()
    {
        Debug.Log("Creating spawns");

        Multiplayer.objSpawns.Clear();
        Multiplayer.spawns = "{";
        int count = Multiplayer.spawnAmount;

        for (int i = 1; i <= count; i++)
        {
            int rand = Random.Range(0, 5);
            string instance;
            spikeInterval++;

            if (rand == 1 && spikeInterval > 1)
            {
                instance = "spikes";
                spikeInterval = 0;
            }
            else if (rand == 2)
            {
                instance = "breakPlatform";
            }
            else
            {
                instance = "platform";
            }


            float randPos = UnityEngine.Random.Range((float)(720 / 8.5), (float)(720 / 1.18));

            Spawn spawn = new Spawn(instance, randPos, false);
            string newSpawn = JsonUtility.ToJson(spawn);

            if (i == count)
                Multiplayer.spawns += newSpawn.Replace("ID", "ID" + i).Replace("X", "X" + i).Replace("Y", "Y" + i).Replace("{", "").Replace("}", "");
            else
            {
                Multiplayer.spawns += newSpawn.Replace("ID", "ID" + i).Replace("X", "X" + i).Replace("Y", "Y" + i).Replace("{", "").Replace("}", "") + ",";
            }

            Multiplayer.objSpawns.Add(spawn);

        }

        Multiplayer.spawns += "}";
        Debug.Log("Created " + Multiplayer.objSpawns.Count + " spawns");
        // mp.uploadSpawns();

    }

    public void gameOver()
    {
        /**
        if (Multiplayer.connection != Multiplayer.Connection.Offline)
        {
        
            Multiplayer.ready = false;
            UI.matchTimer = -1;
            Multiplayer.state = Multiplayer.State.Finished;

            Multiplayer.ui.multiPairPanel.SetActive(true);
            Multiplayer.ui.showWinner();
            
            
        }
        else
        {
            gameOverPanel.SetActive(true);
            running = false;
        }
        **/
        gameOverPanel.SetActive(true);
        running = false;
        restartButt.SetActive(false);

        switch(Multiplayer.type){
            case  Multiplayer.GameType.Single:
                restartButt.SetActive(true);
                break;
            case  Multiplayer.GameType.OnevOne:
                break;
            case  Multiplayer.GameType.Royal:
                Debug.Log("Total Tourn: " + RoyalRumbleScript.tournaments.Count);
                Debug.Log("Royal Rumble ID: " + RoyalRumbleScript.selectedTournament.id);
                Debug.Log(" User ID "+FBHolder.id);
                PostScore(score, RoyalRumbleScript.selectedTournament.id, FBHolder.id);
                running = false;
                break;
            case  Multiplayer.GameType.League:
                break;    
                
        }

        scoreText.text = score + "";

        highScore = PlayerPrefs.GetInt(highscoreString);
        
        if(score > highScore){
            highScore = score;
            PlayerPrefs.SetInt(highscoreString, score);
        }

        highscoreText.text = highScore + "";

        if (!setHighscore){
            mp.uploadHighScore();
            setHighscore = true;
        }

    }

    public void resetLeaderboard()
    {
        for (int i = 0; i < leaderboardItems.Count; i++)
        {
            Destroy(leaderboardItems[i]);
        }
        leaderboardItems.Clear();
    }

     public void resetHistory()
    {
        for (int i = 0; i < historyItems.Count; i++)
        {
            Destroy(historyItems[i]);
        }
        historyItems.Clear();
        Debug.Log("Clearing History");
    }

    public void resetTournmentData()
    {
        for (int i = 0; i < tournamentItems.Count; i++)
        {
            Destroy(tournamentItems[i]);
        }
        tournamentItems.Clear();
    }

    public void resetElements()
    {
        life = 3;
        score = 0;
        difficulty = 0;
        PlatformPhysics.speed = defPlatformSpeed;
        UI.scrollSpeed = defScrollSpeed;

        Multiplayer.restarting = false;
        Multiplayer.winnerSet = false;
        Multiplayer.spawnIndex = 1;
        Multiplayer.pending = false;
        Multiplayer.ui.isDraw = false;
        setHighscore = false;

        gameOverPanel.SetActive(false);
        FindObjectOfType<UI>().updateLife();

    }

    public void PostScore(int score, string matchID, string playerID)
    {
        var matchPlayedRequest = new MatchPlayedRequest
        {
            matchID = matchID,
            userID = playerID,
            scores = new List<int>() {score},
            matchType = "RoyalRumble"
        };
        StartCoroutine(HttpUtil.Post(HttpUtil.submitRoyalRumbleScore, JsonUtility.ToJson(matchPlayedRequest), RoyalRumbleScoreSubmitedCallback));
    }

    private void RoyalRumbleScoreSubmitedCallback(UnityWebRequest response)
    {
        MatchEntryResponse matchEntryResponse = new MatchEntryResponse();
        matchEntryResponse = JsonUtility.FromJson<MatchEntryResponse>(response.downloadHandler.text);
        if (matchEntryResponse.isSuccessful || matchEntryResponse.successful)
        {
            Debug.Log("this is the successful message: " + matchEntryResponse.message);
        }
        else
        {
            Debug.Log("this is the error message: " + matchEntryResponse.message);
        }
    }
}
