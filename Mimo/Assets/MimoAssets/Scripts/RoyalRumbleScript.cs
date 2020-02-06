using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoyalRumbleScript : MonoBehaviour
{
    public GameObject newTournamentPanel, filterPanel, TournamentChild;
    public static List<Tournament> tournaments = new List<Tournament>();
    public Text nameText, maxPlayersText, entryFeeText;
    public Text minEFText, maxEFText, minPText, maxPText;
    private int minEFIndex, maxEFIndex, minPIndex, maxPIndex;
    private int maxPlayersIndex, entryFeeIndex;
    private int[] entryFees = { 50,100,200,300,500,1000,2000,3000,5000,10000,20000,50000};
    private int[] Players = { 0,3,5,7,10,15,20,25,30,50,100,0};
    private bool isFilter;


    void Start()
    {
        Debug.Log("Royal Rumble Selected");
        displayTournaments();
        isFilter = false;
    }

    private void displayTournaments()
    {
        Debug.Log("Displaying Royal Rumble Tournaments");
        //tournaments.Clear();
        FindObjectOfType<GameCode>().resetTournmentData();

        retreiveTournamentData();

        if (isFilter)
        {
            int filtered = 0;
            for (int i = 0; i < tournaments.Count; i++)
            {
                if ((tournaments[i].maxPlayers < Players[minPIndex]) ||
                    (tournaments[i].maxPlayers > Players[maxPIndex]) ||
                    (tournaments[i].entryFee < entryFees[minEFIndex]) ||
                    (tournaments[i].entryFee > entryFees[maxEFIndex])) 
                {
                    tournaments.Remove(tournaments[i]);
                    filtered++;
                }
            }

            Debug.Log("Filtered "+filtered+" Results");
        }

        for(int i=0; i < tournaments.Count; i++)
        {
            Instantiate(TournamentChild.gameObject, GameObject.FindGameObjectWithTag("RoyalView").transform);
        }
        Debug.Log(tournaments.Count + " Tournaments displayed");
    }
    public void retreiveTournamentData()
    {
        //Pull Data From API save to tournaments List
    }
    public void openNewTournamentDialog()
    {
        entryFeeIndex = 0;
        maxPlayersIndex = 0;
        entryFeeText.text = "N" + entryFees[entryFeeIndex];
        maxPlayersText.text = "00";
        newTournamentPanel.SetActive(true);
    }
    public void increaseEntryFee()
    {
        if(entryFeeIndex < entryFees.Length-1)
            entryFeeIndex++;
        entryFeeText.text = "N" + entryFees[entryFeeIndex];
    }
    public void decreaseEntryFee()
    {
        if (entryFeeIndex > 0)
            entryFeeIndex--;
        entryFeeText.text = "N" + entryFees[entryFeeIndex];
    }
    public void increaseMaxPlayers()
    {
        if (maxPlayersIndex < Players.Length - 1)
            maxPlayersIndex++;
        maxPlayersText.text = Players[maxPlayersIndex] + "";
        if (maxPlayersText.text.Equals("0"))
            maxPlayersText.text = "00";
    }
    public void decreaseMaxPlayers()
    {
        if (maxPlayersIndex > 0)
            maxPlayersIndex--;
        maxPlayersText.text = Players[maxPlayersIndex] + "";
        if (maxPlayersText.text.Equals("0"))
            maxPlayersText.text = "00";
    }
    public void increaseMinEF()
    {
        if (minEFIndex < entryFees.Length - 1)
            minEFIndex++;
        minEFText.text = "N" + entryFees[minEFIndex];
    }
    public void decreaseMinEF()
    {
        if (minEFIndex > 0)
            minEFIndex--;
        minEFText.text = "N" + entryFees[minEFIndex];
    }
    public void increaseMaxEF()
    {
        if (maxEFIndex < entryFees.Length - 1)
            maxEFIndex++;
        maxEFText.text = "N" + entryFees[maxEFIndex];
    }
    public void decreaseMaxEF()
    {
        if (maxEFIndex > 0)
            maxEFIndex--;
        maxEFText.text = "N" + entryFees[maxEFIndex];
    }

    public void increaseMinP()
    {
        if (minPIndex < entryFees.Length - 1)
            minPIndex++;
        minPText.text = "" + Players[minPIndex];
        if (minPText.text.Equals("0"))
            minPText.text = "00";
    }
    public void decreaseMinP()
    {
        if (minPIndex > 0)
            minPIndex--;
        minPText.text = "" + Players[minPIndex];
        if (minPText.text.Equals("0"))
            minPText.text = "00";
    }
    public void increaseMaxP()
    {
        if (maxPIndex < entryFees.Length - 1)
            maxPIndex++;
        maxPText.text = "" + Players[maxPIndex];
        if (maxPText.text.Equals("0"))
            maxPText.text = "00";
    }
    public void decreaseMaxP()
    {
        if (maxPIndex > 0)
            maxPIndex--;
        maxPText.text = "" + Players[maxPIndex];
        if (maxPText.text.Equals("0"))
            maxPText.text = "00";
    }

    public void closeNewTournamentDialog()
    {
        newTournamentPanel.SetActive(false);
    }
    public void createTournament()
    {
        string text = nameText.text;
        if (nameText.text.Trim().Equals(""))
            text = "New Tournament";
      
        Tournament newTournament = new Tournament(text, Players[maxPlayersIndex], entryFees[entryFeeIndex], 0, Time.time.ToString());
        tournaments.Add(newTournament);
        isFilter = false;
        displayTournaments();
        closeNewTournamentDialog();
    }
    public void previewTournament()
    {

    }
    public void selectTournament()
    {

    }
    public void openFilterDialog()
    {
        minEFIndex = 0;
        maxEFIndex = 8;
        minPIndex = 1;
        maxPIndex = 0;
        minEFText.text = "N" + entryFees[minEFIndex];
        maxEFText.text = "N" + entryFees[maxEFIndex];
        minPText.text = "" + Players[minPIndex];
        maxPText.text = "00";
        filterPanel.SetActive(true);
    }
    public void filter()
    {
        isFilter = true;
        displayTournaments();

        closeFilterDialog();
    }
    public void closeFilterDialog()
    {
        filterPanel.SetActive(false);
    }

    void Update()
    {
        
    }
}
