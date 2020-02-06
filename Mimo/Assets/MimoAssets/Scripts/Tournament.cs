using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tournament
{
    public string tournamentName;
    public int maxPlayers;
    public int curPlayers;
    public int entryFee;
    public int prize;
    public string time;

    
    public Tournament(string name, int maxPlayers, int entryFee, int prize, string time)
    {
        this.tournamentName = name;
        this.maxPlayers = maxPlayers;
        this.entryFee = entryFee;
        this.prize = prize;
        this.time = time;

        updateDatabase();
        Debug.Log("Tournament " + tournamentName + " Created");
    }

    public void updateDatabase()
    {
        //Save Tournament to Database
    }

}
