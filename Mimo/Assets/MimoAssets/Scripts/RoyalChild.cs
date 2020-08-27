using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoyalChild : MonoBehaviour
{
    public Text indexText, name, players, entryFee, prize, timeLeft;
    public Button select;
    public RawImage star;
    private int index;
    Tournament tournament;

    void Start()
    {
        index = FindObjectOfType<GameCode>().tournamentItems.Count;
        tournament = RoyalRumbleScript.currentPage[index];
        int no = RoyalRumbleScript.startIndex + index + 1;
        indexText.text = no + ".";
        name.text = tournament.tournamentName;
        players.text = $"{tournament.getPlayerCount()}/{tournament.playerLimit} Players";

        //dividing the entry fee by 100 to take it back to Naira.
        entryFee.text = "Entry Fee : " + UI.getNaira(tournament.entryFee/100);
        prize.text = UI.getNaira(tournament.getPrize());
        timeLeft.text = GetRemainingTime(tournament.startTime);
        FindObjectOfType<GameCode>().tournamentItems.Add(this.gameObject);    
        DetermineBackgroundPanelColor(tournament);
        
        select.onClick.AddListener(() => selectMatch(index));

    }

    public void selectMatch(int matchIndex) {

        FindObjectOfType<GameCode>().playSound(GameCode.Sound.Button); 
        RoyalRumbleScript.selectedTournament = tournament;
            
        if (tournament.hasFinished)
        {
            Debug.Log("Tournament has finished");
            FindObjectOfType<UI>().BottomPanelText.text = "Match has Ended";
            FindObjectOfType<UI>().BottomPanelButton.text = "View Results";
            FindObjectOfType<UI>().BottomPanel.SetActive(true);
        }else if(tournament.hasStarted){
            Debug.Log("Tournament has started");
            FindObjectOfType<UI>().BottomPanelText.text = "Awaiting Results in " + GetRemainingTime(tournament.startTime);
            FindObjectOfType<UI>().BottomPanelButton.text = "OK";
            FindObjectOfType<UI>().BottomPanel.SetActive(true);
        }else
        {
            FindObjectOfType<RoyalRumbleScript>().OnTournamentClicked(matchIndex);
        }

    }

    public string GetRemainingTime(long matchStartTimeStamp)
    {
        if (matchStartTimeStamp <= 0)
        {
            return "00:00:00";
        }
        matchStartTimeStamp = (long)TimeSpan.FromMilliseconds(matchStartTimeStamp).TotalSeconds;
  

        var epochStart = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(matchStartTimeStamp).ToLocalTime();
        var endDate = epochStart.AddHours(48);
        
        long endDateTimeStamp = new DateTimeOffset(endDate).ToUnixTimeSeconds();
        long presentDateTimeStamp = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
        var secondsLeft = endDateTimeStamp - presentDateTimeStamp;
        var time = TimeSpan.FromSeconds(secondsLeft);


        if (secondsLeft < 0) {
            return "00:00:00";
        }
        else if (secondsLeft < 86400)
        {
            var hour = time.Hours > 9 ? time.Hours.ToString() : $"0{time.Hours}";
            var minute = time.Minutes > 9 ? time.Minutes.ToString() : $"0{time.Minutes}";
            var second = time.Seconds > 9 ? time.Seconds.ToString() : $"0{time.Seconds}";
            return $"{hour}:{minute}:{second}";
        }
        else
        {
            return $"{(int)time.TotalHours} hours";
        }
    }

    private void Update()
    {
        timeLeft.text = GetRemainingTime(tournament.startTime);
        DetermineBackgroundPanelColor(tournament);
    }

    public void DetermineBackgroundPanelColor(Tournament tournament) {

        //RawImage backgroundImage = this.transform.Find("back").GetComponent<RawImage>();
        //Debug.Log("-----------------------------------------");
        //Debug.Log("tournament.name " + tournament.tournamentName);
        //Debug.Log("tournament.hasStarted " + tournament.hasStarted);
        //Debug.Log("tournament.hasFinished " + tournament.hasFinished);
        //Debug.Log("tournament.registered " + tournament.registered);


        if (tournament.hasStarted || tournament.hasFinished)
        {
            //Debug.Log("color red");
            star.texture = (Texture2D)Resources.Load("star2");
        }
        else if (tournament.registered)
        {
            //Debug.Log("color blue");
            star.texture = (Texture2D)Resources.Load("star");
        }
        else {
            //Debug.Log("color black");
            star.texture = (Texture2D)Resources.Load("blank");
        }
    }

}
