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
    private string hostID;
    private float posX = -10, posY = 0;
    public static UI ui;
    public static bool winner, pending, ready, restarting, retreived, winnerSet;
    public static string oppReady = "";
    public static string spawns;
    public static int spawnIndex, spawnAmount = 400;
    public static List<Spawn> objSpawns = new List<Spawn>();
    int roomIndex = 0, leaderBoardMax = 15;
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
        // retreiveLeaderboardData();
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
    /**
    public void connectToGame()
    {
        CheckForInternetConnection();
        bool isMatch = false;
        string hostID = "";
        object room = null;
        int roomCount = 0;

        reference.Child("game_lobby")
        .RunTransaction(mutableData => {
            List<object> gameRooms = mutableData.Value as List<object>;

            if (gameRooms == null)
            {
                gameRooms = new List<object>();
            }
            else if (mutableData.ChildrenCount > 0)
            {

                foreach (var child in gameRooms)
                {
                    if (!(child is Dictionary<string, object>)) continue;

                    string stake = (string)
                                ((Dictionary<string, object>)child)["stake"];
                    hostID = (string)
                                ((Dictionary<string, object>)child)["hostID"];

                    if (Convert.ToInt32(stake) == Multiplayer.stake)
                    {
                        isMatch = true;
                        this.hostID = hostID;
                        room = child;
                        roomCount++;
                    }
                }

                gameRooms.Remove(room);
            }

            // Add the new room
            Dictionary<string, object> newRoom =
                             new Dictionary<string, object>();

            if (isMatch)
            {
                newRoom["hostID"] = this.hostID;
                newRoom["stake"] = "911";

                Debug.Log("We retreived a total of " + roomCount + " hosts");
                Debug.Log("You joined Room " + this.hostID);
                state = State.Pairing;

                matchID = "match" + this.hostID;
                connection = Connection.Client;
            }
            else
            {
                this.hostID = "- " + DateTime.Now.ToString().Replace("/", "-") + " " + reference.Child("game_lobby").Push().Key;
                newRoom["hostID"] = this.hostID;
                newRoom["stake"] = Multiplayer.stake + "";

                Debug.Log("New Room " + this.hostID + " added to lobby. Awaiting connection");
                connection = Connection.Host;
                state = State.Pairing;
            }

            gameRooms.Add(newRoom);
            mutableData.Value = gameRooms;

            roomIndex = gameRooms.IndexOf(newRoom);

            return TransactionResult.Success(mutableData);

        });
        
    }

    public void createMatch()
    { 
        matchID = "match" + hostID;
        Match match = new Match(gameID, userName, "null", stake);
        string json = JsonUtility.ToJson(match);

        reference.Child("matches").Child(matchID).SetRawJsonValueAsync(json);
        Debug.Log("New " + matchID + " created");
    }

    public void uploadSpawns()
    { 
        reference.Child("match spawns").Child(matchID).SetRawJsonValueAsync(spawns);
        Debug.Log("JSON: " + spawns + " uploaded");
    
    }

    public void checkPairStatus()
    {
        CheckForInternetConnection();

        if (connection == Connection.Host && state == State.Pairing)
        {

            reference.Child("game_lobby")
           .Child(roomIndex + "")
           .GetValueAsync().ContinueWith(task => {
               if (task.IsFaulted)
               {
                   Debug.Log("Error retreiving pair status");
               }
               else if (task.IsCompleted)
               {
                   DataSnapshot snapshot = task.Result;
                   string info = snapshot.Child("stake").GetValue(false).ToString();
                   if (info.Equals("911"))
                   {
                       createMatch();
                       reference.Child("game_lobby").Child(roomIndex + "").RemoveValueAsync();
                       state = State.Waiting;
                   }
               }
           });

        }
        if (connection == Connection.Host && state == State.Waiting)
        {
            reference.Child("matches")
          .Child(matchID)
          .GetValueAsync().ContinueWith(task =>
          {
              if (task.IsFaulted)
              {
                  Debug.Log("Error retreiving pair status");
              }
              else if (task.IsCompleted)
              {
                  DataSnapshot snapshot = task.Result;
                  string info = snapshot.Child("player2ID").GetValue(false).ToString();

                  if (!info.Equals("null"))
                  {
                      Debug.Log("Player " + info + " has connected");
                      state = State.Playing;
                      reference.Child("matches").Child(matchID).ValueChanged += playerPositionChanged;

                      oppName = info;
                      ui.showMatchUp();

                  }
                  else
                      Debug.Log("Player Not Connected!. Player2ID is " + info);
              }
          });

        }
        if (connection == Connection.Client)
        {
            reference.Child("matches")
           .Child(matchID)
           .GetValueAsync().ContinueWith(task =>
           {
               if (task.IsFaulted)
               {
                   Debug.Log("Error retreiving pair status");
               }
               else if (task.IsCompleted)
               {
                   DataSnapshot snapshot = task.Result;
                   string info = snapshot.Child("stake").GetValue(false).ToString();

                   if (info.Equals(stake.ToString()))
                   {
                       reference.Child("matches").Child(matchID).Child("player2ID").SetValueAsync(userName);
                       Debug.Log("Connected to " + matchID);
                       state = State.Playing;
                       oppName = snapshot.Child("player1ID").GetValue(false).ToString();
                       ui.showMatchUp();

                       reference.Child("matches").Child(matchID).ValueChanged += playerPositionChanged;
                   }
                   else
                       Debug.Log("Match Not Found!. Stake value is " + info);
               }
           });

        }
    
    }

    public void cancelPair()
    {
        if (state != State.Finished)
        {
            if (matchID != null)
            {
                reference.Child("matches").Child(matchID).RemoveValueAsync();
                reference.Child("match spawns").Child(matchID).RemoveValueAsync();
            }
            if (hostID != null)
                reference.Child("game_lobby").Child(roomIndex + "").RemoveValueAsync();
            connection = Connection.Offline;

            Debug.Log("Deleted match, state = " + state.ToString());
        }
    
    }
    
    private void playerPositionChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        DataSnapshot snapshot = args.Snapshot;

        string x;
        string y;
        string life;
        string oppPlayer;
        string score;


        if (connection == Connection.Host)
            oppPlayer = "p2";
        else
            oppPlayer = "p1";


        x = snapshot.Child(oppPlayer + "X").GetValue(false).ToString();
        y = snapshot.Child(oppPlayer + "Y").GetValue(false).ToString();
        string bladeInterval = snapshot.Child("bladeInterval").GetValue(false).ToString();
        string spIndex = snapshot.Child("spawnIndex").GetValue(false).ToString();
        life = snapshot.Child(oppPlayer + "Life").GetValue(false).ToString();
        score = snapshot.Child(oppPlayer + "Score").GetValue(false).ToString();
        oppReady = snapshot.Child(oppPlayer + "Ready").GetValue(false).ToString();

        float xx = (float)Convert.ToDouble(x);
        float yy = (float)Convert.ToDouble(y);

        oppScore = Convert.ToInt32(score);
        oppLife = Convert.ToInt32(life);

        ui.updateLife();

        if (connection == Connection.Client)
        {
            GameCode.bladeInterval = Convert.ToInt32(bladeInterval);
            GameCode.bladeStart = GameCode.bladeInterval * 4;
            if (oppLife > 0)
                spawnIndex = Convert.ToInt32(spIndex);

            if (!ready && oppReady.Equals("True") && !retreived)
            {
                Debug.Log("Retreiving Spawns from Host");
                retreiveSpawns();

                retreived = true;
            }
        }

        if (ready && oppReady.Equals("True") && !restarting)
        {
            UI.matchTimer = 6;
            restarting = true;
            Debug.Log("Match Timer set to 6");
        }

        posX = Screen.width / xx;
        posY = Screen.height / yy;

        if (oppLife > 0)
        {
            Player2Physics.X = posX;
            Player2Physics.Y = posY + Screen.height/14.4f;
        }

        // Debug.Log(oppName + " is at X: " + posX + " Y: " + posY);

    }
    
    public void sendData()
    {

        float xx = Screen.width / PlayerPhysics.X;
        float yy = Screen.height / PlayerPhysics.Y;

        string player;

        if (connection == Connection.Host)
        {
            player = "p1";

            reference.Child("matches").Child(matchID).Child("spawnIndex").SetValueAsync(spawnIndex);
            reference.Child("matches").Child(matchID).Child("bladeInterval").SetValueAsync(GameCode.bladeInterval);
        }
        else
            player = "p2";

        reference.Child("matches").Child(matchID).Child(player + "X").SetValueAsync(xx);
        reference.Child("matches").Child(matchID).Child(player + "Y").SetValueAsync(yy);
        reference.Child("matches").Child(matchID).Child(player + "Life").SetValueAsync(GameCode.life);
        reference.Child("matches").Child(matchID).Child(player + "Score").SetValueAsync(GameCode.score);
        reference.Child("matches").Child(matchID).Child(player + "Ready").SetValueAsync(ready.ToString());
    
    }

    public void retreiveSpawns()
    { 
        objSpawns.Clear();

        reference.Child("match spawns")
          .Child(matchID)
          .GetValueAsync().ContinueWith(task => {
              if (task.IsFaulted)
              {
                  Debug.Log("Error retreiving Spawn objects");
              }
              else if (task.IsCompleted)
              {
                  DataSnapshot snapshot = task.Result;

                  for (int i = 1; i <= spawnAmount; i++)
                  {
                      string id = snapshot.Child("ID" + i).GetValue(false).ToString();
                      float x = (float)Convert.ToDouble(snapshot.Child("X" + i).GetValue(false).ToString());
                      objSpawns.Add(new Spawn(id, x));
                  }


                  if (objSpawns.Count > 100)
                  {
                      Debug.Log("Retreived " + objSpawns.Count + "spawns");
                      ready = true;
                      reference.Child("match spawns").Child(matchID).ValueChanged += spawnChanged;
                  }
                  else
                  {
                      Debug.Log("Retreive only " + objSpawns.Count + " spawns");
                  }

              }
          });
    
    }

    void spawnChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        DataSnapshot snapshot = args.Snapshot;

        for (int i = 1; i <= spawnAmount; i++)
        {
            string id = snapshot.Child("ID" + i).GetValue(false).ToString();
            float x = (float)Convert.ToDouble(snapshot.Child("X" + i).GetValue(false).ToString());
            objSpawns[i - 1].ID = id;
            objSpawns[i - 1].X = x;
        }

        Debug.Log("Updated " + objSpawns.Count + "spawns");
    }

    public void setWinner(string winnerId)
    {
        reference.Child("matches").Child(matchID).Child("winnerID").SetValueAsync(winnerId);
        winnerSet = true;
        Debug.Log("Winner set to " + winnerId);
    }

    public void retreiveLeaderboardData()
    {
        lbNames.Clear();
        lbScores.Clear();

        reference.Child("leaderboard")
           .GetValueAsync().ContinueWith(task => {
              if (task.IsFaulted)
              {
                  Debug.Log("Error retreiving Spawn objects");
              }
              else if (task.IsCompleted)
              {
                  DataSnapshot snapshot = task.Result;
                  int count = (int)snapshot.ChildrenCount;
                  Debug.Log("There are " + count + " Users on the leaderboard");
                  
                   for (int i = 0; i< count; i++)
                   {
                       lbNames.Add(snapshot.Child(i+"").Child("userName").GetValue(false).ToString());
                       lbScores.Add(Convert.ToInt32(snapshot.Child(i+"").Child("score").GetValue(false).ToString()));
                       Debug.Log("Index-" + i + " Name:" + lbNames[i] + " Score:" + lbScores[i]);
                       
                   }

                   
               }
          });

        
    }

    public void uploadHighScore()
    {
        object room = null;
        int minScore = 5000;
        bool exists = false;
        
        string name = Multiplayer.userName;
       
        Debug.Log("Checking Leaderboard");

        reference.Child("leaderboard")
        .RunTransaction(mutableData => {
            List<object> highScores = mutableData.Value as List<object>;


            if (highScores == null)
            {
                highScores = new List<object>();
            }
            else if (mutableData.ChildrenCount > 0)
            {
                Debug.Log("There are "+highScores.Count+" names on the list");

                foreach (var child in highScores)
                {
                    if (!(child is Dictionary<string, object>)) continue;

                    string score = (string)
                                ((Dictionary<string, object>)child)["score"];
                    string childName = (string)
                                ((Dictionary<string, object>)child)["userName"];

                    if (name.Equals(childName))
                    {
                        exists = true;
                        room = null;

                        if (GameCode.highScore > Convert.ToInt32(score))
                        {
                            room = child;
                            Debug.Log("New HighScore");
                        }
                        else
                        {
                            Debug.Log("Name: "+name+" ChildName: "+childName + ". Your HighScore is already on Leaderboard");
                        }
                    }

                    if (Convert.ToInt32(score) < minScore && !exists)
                    {
                        minScore = Convert.ToInt32(score);
                        room = child;
                    }
                }

                if (GameCode.highScore > minScore && !exists)
                {
                    if(highScores.Count >= leaderBoardMax)
                       highScores.Remove(room);
                    Debug.Log("New User added to leaderboard");
                }
                else if(exists && room!=null)
                    highScores.Remove(room);

            }

            // Add the new room
            Dictionary<string, object> newHighScore =
                             new Dictionary<string, object>();

            newHighScore["userName"] = name;
            newHighScore["score"] = ""+GameCode.highScore;

        if (!exists) { 
            if (highScores.Count < leaderBoardMax)
            {
                highScores.Add(newHighScore);
                Debug.Log(name + " your Highscore has been added to leaderboard");
            }
            else
            {
                Debug.Log(name + " your Highscore not up to minimum");
            }
        }
        else if ( exists && room != null)
            {
                highScores.Add(newHighScore);
                Debug.Log(name + " your Highscore has been updated on leaderboard");
            }

            mutableData.Value = highScores;

            return TransactionResult.Success(mutableData);

        });

    }
    **/
}
