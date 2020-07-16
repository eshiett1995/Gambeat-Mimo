using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoyalChild : MonoBehaviour
{
    public Text indexText, name, players, entryFee, prize, timeLeft;
    public Button select;
    private int index;
    Tournament tournament;

    void Start()
    {
        index = FindObjectOfType<GameCode>().tournamentItems.Count;
        tournament = RoyalRumbleScript.currentPage[index];
        int no = RoyalRumbleScript.startIndex + index + 1;
        indexText.text = no + ".";
        name.text = tournament.tournamentName;
        players.text = tournament.getPlayerCount() +"/"+ tournament.totalPlayers +" Players";
        if (tournament.totalPlayers==0)
            players.text = tournament.getPlayerCount() + "/00 Players";
        entryFee.text = "Entry Fee : " + UI.getNaira(tournament.entryFee);
        prize.text = UI.getNaira(tournament.getPrize());
        timeLeft.text = GetRemainingTime(tournament.startTime);
        FindObjectOfType<GameCode>().tournamentItems.Add(this.gameObject);

        select.onClick.AddListener(() => selectMatch(index));

    }

    public void selectMatch(int matchIndex) {
        FindObjectOfType<RoyalRumbleScript>().openTournament(matchIndex);
    }

    public string GetRemainingTime(long matchStartTimeStamp)
    {
        if (matchStartTimeStamp <= 0)
        {
            return "00:00:00";
        }
        Debug.Log("milisecond " + matchStartTimeStamp);
        matchStartTimeStamp = (long)TimeSpan.FromMilliseconds(matchStartTimeStamp).TotalSeconds;
        Debug.Log("second " + matchStartTimeStamp);

        var epochStart = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(matchStartTimeStamp).ToLocalTime();
        var endDate = epochStart.AddHours(1);
        
        long endDateTimeStamp = new DateTimeOffset(endDate).ToUnixTimeSeconds();
        long presentDateTimeStamp = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
        Debug.Log("ending second " + endDateTimeStamp);
        var secondsLeft = endDateTimeStamp - presentDateTimeStamp;
        Debug.Log("secondsLeft " + secondsLeft);
        var time = TimeSpan.FromSeconds(secondsLeft);


       
        if (secondsLeft < 86400)
        {
            Debug.Log($"0{ time.Hours} : 0{ time.Minutes} : {time.Seconds}");
            return $"0{ time.Hours} : 0{ time.Minutes} : {time.Seconds}";
        }
        else
        {
            Debug.Log($"{time.TotalHours} hours");
            return $"{time.TotalHours} hours";
        }
    }

    private void Update()
    {
        timeLeft.text = GetRemainingTime(tournament.startTime);
    }

}
