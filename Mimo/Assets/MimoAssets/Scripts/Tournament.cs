using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tournament
{
    public string id;
    public string tournamentName;
    public int totalPlayers;
    public int playerLimit;
    public bool hasStarted;
    public bool hasFinished;
    public long entryFee;
    public long startTime;
    public bool registered;
    public int hr, day;
    private List<string> players;


    public Tournament(string name, int maxPlayers, long entryFee)
    {
        this.tournamentName = name;
        this.totalPlayers = maxPlayers;
        this.entryFee = entryFee;
        hr = DateTime.Now.Hour;
        day = DateTime.Now.Day;

        updateDatabase();
    }

    public Tournament(string id,string name, int totalPlayers, int playerLimit, long entryFee, bool registered, long startTime, bool hasStarted, bool hasFinished)
    {
        this.id = id;
        this.tournamentName = name;
        this.totalPlayers = totalPlayers;
        this.playerLimit = playerLimit;
        this.entryFee = entryFee;
        this.registered = registered;
        this.startTime = startTime;
        this.hasStarted = hasStarted;
        this.hasFinished = hasFinished;
        hr = DateTime.Now.Hour;
        day = DateTime.Now.Day;
    }

    public Tournament(string id, string name, int totalPlayers, int playerLimit, long entryFee)
    {
        this.id = id;
        this.tournamentName = name;
        this.totalPlayers = totalPlayers;
        this.playerLimit = playerLimit;
        this.entryFee = entryFee;
        hr = DateTime.Now.Hour;
        day = DateTime.Now.Day;
    }

    public void addPlayer(string playerID)
    {
        if (players.Count < totalPlayers)
            players.Add(playerID);
        Debug.Log("Player " + playerID + " added to Tournament " + tournamentName);
        updateDatabase();
    }

    public string getTime()
    {
        int hrsLeft;

        if (DateTime.Now.Day.Equals(day))
            hrsLeft = 24 - (DateTime.Now.Hour - hr);
        else
        {
            hrsLeft = hr - DateTime.Now.Hour;
        }

        return hrsLeft + " Hrs Left";
    }

    public int getPlayerCount()
    {
        return totalPlayers;
    }

    public int getPrize()
    {
        return (int)(entryFee/100 * totalPlayers);
    }

    public void updateDatabase()
    {
        if (id == null || id.Trim().Equals(""))
        {
            //Save New Tounament to Database
        }
        else
        {
            //Update Tournament on Database
        }

        Debug.Log("Tournament updated to Database");
    }

}
