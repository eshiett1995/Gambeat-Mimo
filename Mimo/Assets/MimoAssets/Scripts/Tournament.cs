using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tournament
{
    public string tournamentName;
    public int totalPlayers;
    public int currentPlayers;
    public int entryFee;
    public int hr, day;

    
    public Tournament(string name, int maxPlayers, int entryFee)
    {
        this.tournamentName = name;
        this.totalPlayers = maxPlayers;
        this.entryFee = entryFee;
        hr = DateTime.Now.Hour;
        day = DateTime.Now.Day;

        updateDatabase();
        Debug.Log("Tournament " + tournamentName + " Created on Hour:"+hr+" of Day "+day);
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

    public int getPrize()
    {
        return entryFee * currentPlayers;
    }

    public void updateDatabase()
    {
        //Save Tournament to Database
    }

}
