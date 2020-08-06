using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class RoyalRumbleScript : MonoBehaviour
{
    public GameObject newTournamentPanel, filterPanel, TournamentChild, paginationPanel;
    public GameObject listView, tournamentPanel, newTournamentDialog, filterDialog;
    private List<Tournament> tournaments = new List<Tournament>();
    public static List<Tournament> currentPage = new List<Tournament>();
    public Text nameText, maxPlayersText, entryFeeText;
    public Text minEFText, maxEFText, minPText, maxPText;
    private int minEFIndex, maxEFIndex, minPIndex, maxPIndex;
    private int maxPlayersIndex, entryFeeIndex;
    private int[] entryFees = { 50,100,200,300,500,1000,2000,3000,5000,10000,20000,50000};
    private int[] Players = { 0,3,5,7,10,15,20,25,30,50,100,0};
    private bool isFilter;
    public Tournament selectedTournament;
    public static int pageMax = 10, totalPages, startIndex;
    public StageObjectsModel stageObjectsModel;

    private void Start()
    {
       
    }

    public void yesClicked(){

        FindObjectOfType<UI>().loaderPanel.SetActive(true);
        UI.doneLoading = false;

         if (selectedTournament.registered) {
                Debug.Log("it has started initing");
                StartCoroutine(HttpUtil.Get(HttpUtil.royalRumbleInit + "/" + selectedTournament.id, royalRumbleMatchInitCallback));
            }
            else {

                Debug.Log("it has started joining");
                MatchJoinRequest matchJoinRequest = new MatchJoinRequest
                {
                    matchID = selectedTournament.id,
                    matchType = "RoyalRumble"
                };
                Debug.Log("what it is sending");
                Debug.Log(JsonUtility.ToJson(matchJoinRequest));
                StartCoroutine(HttpUtil.Post(HttpUtil.royalRumbleJoin, JsonUtility.ToJson(matchJoinRequest), royalRumbleMatchJoinedCallback));
            }

    }

    public void noClicked(){
        FindObjectOfType<UI>().confirmPanel.SetActive(false);
    }

    public void Initialize()
    {
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
            float newBottom = ((Screen.height - 1280f) / 2) + 210;
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, newBottom);
            rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, -newTop);
        }
        
        tournaments.Clear();
        FindObjectOfType<GameCode>().resetTournmentData();
        retreiveTournamentData(0);
    }

    public void retreiveTournamentData(int page)
    {
        FindObjectOfType<UI>().loaderPanel.SetActive(true);
        UI.doneLoading = false;
        
        RoyalRumbleSearchRequest royalRumbleSearch = new RoyalRumbleSearchRequest();
        if (isFilter)
        {
            royalRumbleSearch.competitionName = "";
            royalRumbleSearch.minimumAmount = entryFees[minEFIndex]; 
            royalRumbleSearch.maximumAmount = entryFees[maxEFIndex];
            royalRumbleSearch.minimumPlayers = Players[minPIndex];
            royalRumbleSearch.maximumPlayers = Players[maxPIndex];
            royalRumbleSearch.sortField = "";
            royalRumbleSearch.filter = true;
            royalRumbleSearch.ascending = true;
        }
        StartCoroutine(HttpUtil.Post(HttpUtil.royalRumbleSearch +"/"+ page, JsonUtility.ToJson(royalRumbleSearch), getRoyalRumbleMatchesCallback));
    }

    private void getRoyalRumbleMatchesCallback(UnityWebRequest response)
    {
        RoyalRumbleSearchResponse royalRumbleSearchResponse = new RoyalRumbleSearchResponse();
        royalRumbleSearchResponse = JsonUtility.FromJson<RoyalRumbleSearchResponse>(response.downloadHandler.text);
        if (royalRumbleSearchResponse.successful || royalRumbleSearchResponse.isSuccessful)
        {
            royalRumbleSearchResponse.content.ForEach(tournament => {
                tournaments.Add(new Tournament(tournament.id, tournament.name, tournament.numberOfCompetitors, tournament.competitorLimit, tournament.entryFee, tournament.registered, tournament.startTime));
            });

            sortPages(tournaments.Count);

            Debug.Log("getRoyalRumbleMatchesCallback: the successful message: " + royalRumbleSearchResponse.message);
        }
        else
        {
            Debug.Log("getRoyalRumbleMatchesCallback : the error message: " + royalRumbleSearchResponse.message);
        }
        UI.doneLoading = true;
    }

    void sortPages(int tournamentCount)
    {
       
        if (tournamentCount > pageMax)
        {
            totalPages = tournamentCount / pageMax;
            if (tournamentCount % pageMax > 0)
                totalPages++;
        }
        else
            totalPages = 1;


        paginationPanel.SetActive(true);
        FindObjectOfType<PaginationScript>().sort( 1, totalPages);

    }

    public void goToPage(int page)
    {
        FindObjectOfType<GameCode>().resetTournmentData();
        currentPage.Clear();
        startIndex = (page -1) * pageMax;

        for(int i=startIndex; i<tournaments.Count; i++){
            currentPage.Add(tournaments[i]);
            if(i==(startIndex + pageMax - 1) || i== tournaments.Count - 1)
            {
                i = tournaments.Count;
            }
        }

        showPage();
    }

    void showPage()
    {
        if (isFilter)
        {
            int filtered = 0;
            if (Players[minPIndex] == 0)
                Players[minPIndex] = 1000;
            if (Players[maxPIndex] == 0)
                Players[maxPIndex] = 1000;

            List<Tournament> invalid = new List<Tournament>();

            foreach (Tournament t in currentPage)
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
            for (int i = 0; i < filtered; i++)
            {
                currentPage.Remove(invalid[i]);
            }

            if (Players[minPIndex] == 1000)
                Players[minPIndex] = 0;
            if (Players[maxPIndex] == 1000)
                Players[maxPIndex] = 0;
            //Debug.Log("Filtered " + filtered + " Results");
        }

        for (int i = 0; i < currentPage.Count; i++)
        {
            Instantiate(TournamentChild, GameObject.FindGameObjectWithTag("RoyalView").transform);
        }

        //Debug.Log(currentPage.Count + " Tournaments displayed");
        UI.doneLoading = true;
        
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
        MatchCreationRequest matchCreationRequest = new MatchCreationRequest
        {
            matchName = text,
            // Gambeat server reads money in kobo (N1 == 100kobo)
            entryFee = entryFees[entryFeeIndex] * 100,
            matchType = "RoyalRumble",
            maxPlayers = Players[maxPlayersIndex] > 1 ? Players[maxPlayersIndex] : 100
        };
        StartCoroutine(HttpUtil.Post(HttpUtil.royalRumbleCreate, JsonUtility.ToJson(matchCreationRequest), createRoyalRumbleMatchCallback));
     
        
        isFilter = false;
        displayTournaments();
        closeNewTournamentDialog();
    }

    private void createRoyalRumbleMatchCallback(UnityWebRequest response)
    {
        MatchEntryResponse matchEntryResponse = new MatchEntryResponse();
        matchEntryResponse = JsonUtility.FromJson<MatchEntryResponse>(response.downloadHandler.text);
        if (matchEntryResponse.isSuccessful || matchEntryResponse.successful)
        {
            //tournaments.Add(newTournament);
            Debug.Log("this is the successful message: " + matchEntryResponse.message);
        }
        else
        {
            Debug.Log("this is the error message: " + matchEntryResponse.message);
        }
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

    public void OnTournamentClicked(int tournamentIndex) {
        selectedTournament = tournaments[tournamentIndex];
        if (selectedTournament.registered) {
            FindObjectOfType<UI>().confirmText.text = "Do you want to \n start this game?";
        }
        else
        {
            FindObjectOfType<UI>().confirmText.text = "Do you want to \n join this game?";
        }
        FindObjectOfType<UI>().confirmPanel.SetActive(true);
    }

    private void royalRumbleMatchJoinedCallback(UnityWebRequest response)
    {
  
        ResponseModel responseModel = new ResponseModel();
        responseModel = JsonUtility.FromJson<ResponseModel>(response.downloadHandler.text);
        if (responseModel.isSuccessful || responseModel.successful)
        {
            Debug.Log("royalRumbleMatchJoinedCallback : successful message: " + responseModel.message);
        }
        else
        {
            Debug.Log("royalRumbleMatchJoinedCallback : error message: " + responseModel.message);
        }
        UI.doneLoading = true;
    }

    private void royalRumbleMatchInitCallback(UnityWebRequest response)
    {
        GameStageResponse gameStageResponse = new GameStageResponse();
        gameStageResponse = JsonUtility.FromJson<GameStageResponse>(response.downloadHandler.text);
        if (gameStageResponse.isSuccessful || gameStageResponse.successful)
        {

            string JSONToParse = "{\"stageObjects\":" + gameStageResponse.data + "}";
            stageObjectsModel = JsonUtility.FromJson<StageObjectsModel>(JSONToParse);
            Debug.Log("royalRumbleMatchInitCallback : successful message: " + stageObjectsModel.stageObjects.Count);
            Multiplayer.objSpawns.Clear();
            for(int i=0; i <stageObjectsModel.stageObjects.Count; i++){
                Multiplayer.objSpawns.Add(new Spawn(stageObjectsModel.stageObjects[i].item, stageObjectsModel.stageObjects[i].coordinate, stageObjectsModel.stageObjects[i].hasLife));
            }

            FindObjectOfType<UI>().startRoyalRumbleMatch();
        }
        else
        {
            Debug.Log("royalRumbleMatchInitCallback : error message: " + gameStageResponse.message);
        }
        UI.doneLoading = true;
    }
}
