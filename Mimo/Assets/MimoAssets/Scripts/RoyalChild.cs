using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoyalChild : MonoBehaviour
{
    public Text indexText, name, players, entryFee, prize, timeLeft;
    public Button select;
    private int index;

    void Start()
    {
        index = FindObjectOfType<GameCode>().tournamentItems.Count;
        Tournament tournament = RoyalRumbleScript.currentPage[index];
        int no = RoyalRumbleScript.startIndex + index + 1;
        indexText.text = no + ".";
        name.text = tournament.tournamentName;
        players.text = tournament.getPlayerCount() +"/"+ tournament.totalPlayers +" Players";
        if (tournament.totalPlayers==0)
            players.text = tournament.getPlayerCount() + "/00 Players";
        entryFee.text = "Entry Fee : " + UI.getNaira(tournament.entryFee);
        prize.text = UI.getNaira(tournament.getPrize());
        timeLeft.text = tournament.getTime();
        FindObjectOfType<GameCode>().tournamentItems.Add(this.gameObject);

        select.onClick.AddListener(() => selectMatch(index));

    }

    public void selectMatch(int matchIndex) {
        FindObjectOfType<RoyalRumbleScript>().openTournament(matchIndex);
    }

}
