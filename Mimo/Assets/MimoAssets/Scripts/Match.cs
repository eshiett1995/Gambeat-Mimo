using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class Match
{
    public string ID;
    public string gameID;
    public string date;
    public string player1ID;
    public string player2ID;
    public int p1Life=3;
    public int p2Life=3;
    public int p1Score;
    public int p2Score;
    public int stake;
    public float p1X, p1Y, p2X, p2Y;
    public string winnerID;
    public string p1Ready;
    public string p2Ready;
    public int bladeInterval;
    public int spawnIndex;
   
    public Match(string gameID, string player1ID, string player2ID, int stake)
    {
        this.gameID = gameID;
        this.player1ID = player1ID;
        this.player2ID = player2ID;
        this.stake = stake;
        winnerID = "null";
        date = DateTime.Now.ToString();

    }

    public void setWinner(string playerId)
    {
        winnerID = playerId;
        updateDatabase();
    }

    public void updateDatabase()
    {

    }

}
