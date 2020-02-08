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
        Tournament tournament = RoyalRumbleScript.tournaments[index];
        int no = index + 1;
        indexText.text = no + ".";
        name.text = tournament.tournamentName;
        players.text = tournament.currentPlayers+"/"+ tournament.totalPlayers +" Players";
        if (tournament.totalPlayers==0)
            players.text = tournament.currentPlayers + "/00 Players";
        entryFee.text = "Entry Fee : " + tournament.entryFee;
        prize.text = "N" + tournament.getPrize();
        timeLeft.text = tournament.getTime();
        FindObjectOfType<GameCode>().tournamentItems.Add(this.gameObject);
    }

}
