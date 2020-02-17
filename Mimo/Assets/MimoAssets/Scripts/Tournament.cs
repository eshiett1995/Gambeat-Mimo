using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tournament
{
    public string tournamentName;
    public int totalPlayers;
    public int entryFee;
    public int hr, day;
    private List<string> players;

    
    public Tournament(string name, int maxPlayers, int entryFee)
    {
        this.tournamentName = name;
        this.totalPlayers = maxPlayers;
        this.entryFee = entryFee;
        hr = DateTime.Now.Hour;
        day = DateTime.Now.Day;
        players = new List<string>();

        updateDatabase();
        Debug.Log("Tournament '" + tournamentName + "' Created");
    }

    public Tournament(string name, int maxPlayers, int entryFee, int hr, int day, List<string> playersIDs)
    {
        this.tournamentName = name;
        this.totalPlayers = maxPlayers;
        this.entryFee = entryFee;
        this.hr = hr;
        this.day = day;
        players = playersIDs;

        Debug.Log("Tournament '" + tournamentName + "' Retreived");
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
        return players.Count;
    }

    public int getPrize()
    {
        return entryFee * players.Count;
    }

    public void updateDatabase()
    {
        //Save Tournament to Database

        Debug.Log("Tournament updated to Database");
    }

}
