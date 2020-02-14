using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoyalRumbleScript : MonoBehaviour
{
    public GameObject newTournamentPanel, filterPanel, TournamentChild;
    public GameObject listView, tournamentPanel, newTournamentDialog, filterDialog;
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
        listView.GetComponent<VerticalLayoutGroup>().padding.left = (int)(Screen.width / 2.12f);
        if (FindObjectOfType<UI>().RoyalPanel.activeSelf)
        {
            RectTransform rectTransform = tournamentPanel.GetComponent<RectTransform>();
            float newTop = ((Screen.height - 1280f) / 2) + 240;
            float newBottom = ((Screen.height - 1280f) / 2) + 160;
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, newBottom);
            rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, -newTop);
        }

        Debug.Log("Displaying Royal Rumble Tournaments");
        //tournaments.Clear();
        FindObjectOfType<GameCode>().resetTournmentData();

        retreiveTournamentData();

        if (isFilter)
        {
            int filtered = 0;
            if (Players[minPIndex] == 0)
                Players[minPIndex] = 1000;
            if (Players[maxPIndex] == 0)
                Players[maxPIndex] = 1000;

            List<Tournament> invalid = new List<Tournament>();

            foreach (Tournament t in tournaments)
            {
                if (t.totalPlayers == 0)
                    t.totalPlayers = 1000;

                if ((t.totalPlayers < Players[minPIndex]) ||
                    (t.totalPlayers > Players[maxPIndex]) ||
                    (t.entryFee < entryFees[minEFIndex]) ||
                    (t.entryFee > entryFees[maxEFIndex])) 
                {
                    invalid.Add(t);
                    filtered++;
                }
                else
                {
                    if (t.totalPlayers == 1000)
                        t.totalPlayers = 0;
                   
                }
            }
            for(int i = 0; i < filtered; i++)
            {
                tournaments.Remove(invalid[i]);
            }

            if (Players[minPIndex] == 1000)
                Players[minPIndex] = 0;
            if (Players[maxPIndex] == 1000)
                Players[maxPIndex] = 0;
            Debug.Log("Filtered "+filtered+" Results");
        }

        for(int i=0; i < tournaments.Count; i++)
        {
            Instantiate(TournamentChild, GameObject.FindGameObjectWithTag("RoyalView").transform);
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
        entryFeeText.text = UI.getNaira(entryFees[entryFeeIndex]);
        maxPlayersText.text = "00";

        resizeDialog(newTournamentDialog);

        newTournamentPanel.SetActive(true);
    }
    private void resizeDialog(GameObject dialog)
    {
        RectTransform rectTransform = dialog.GetComponent<RectTransform>();
        float newTop = ((Screen.height - 1280f) / 2) + 353;
        float newBottom = ((Screen.height - 1280f) / 2) + 467;
        float newLeft = ((Screen.width - 720f) / 2) + 61;
        float newRight = ((Screen.width - 720f) / 2) + 54;
        rectTransform.offsetMin = new Vector2(newLeft, newBottom);
        rectTransform.offsetMax = new Vector2(-newRight, -newTop);
    }
    public void increaseEntryFee()
    {
        if(entryFeeIndex < entryFees.Length-1)
            entryFeeIndex++;
        entryFeeText.text = UI.getNaira(entryFees[entryFeeIndex]);
    }
    public void decreaseEntryFee()
    {
        if (entryFeeIndex > 0)
            entryFeeIndex--;
        entryFeeText.text = UI.getNaira(entryFees[entryFeeIndex]);
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
        minEFText.text = UI.getNaira(entryFees[minEFIndex]);
    }
    public void decreaseMinEF()
    {
        if (minEFIndex > 0)
            minEFIndex--;
        minEFText.text = UI.getNaira(entryFees[minEFIndex]);
    }
    public void increaseMaxEF()
    {
        if (maxEFIndex < entryFees.Length - 1)
            maxEFIndex++;
        maxEFText.text = UI.getNaira(entryFees[maxEFIndex]);
    }
    public void decreaseMaxEF()
    {
        if (maxEFIndex > 0)
            maxEFIndex--;
        maxEFText.text = UI.getNaira(entryFees[maxEFIndex]);
    }

    public void increaseMinP()
    {
        if (minPIndex < Players.Length - 1)
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
        if (maxPIndex < Players.Length - 1)
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
      
        Tournament newTournament = new Tournament(text, Players[maxPlayersIndex], entryFees[entryFeeIndex]);
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
        minEFText.text = UI.getNaira(entryFees[minEFIndex]);
        maxEFText.text = UI.getNaira(entryFees[maxEFIndex]);
        minPText.text = "" + Players[minPIndex];
        maxPText.text = "00";

        resizeDialog(filterDialog);

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
