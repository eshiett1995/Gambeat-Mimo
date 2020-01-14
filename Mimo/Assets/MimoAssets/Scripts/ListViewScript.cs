using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListViewScript : MonoBehaviour
{
    private int index;
    public Text name, score, indexText;

    void Start()
    {
        index = FindObjectOfType<GameCode>().leaderboardItems.Count;
        name.text = Multiplayer.lbNames[index];
        score.text = Multiplayer.lbScores[index] + "";
        int num = index + 1;
        indexText.text = ""+num;
        FindObjectOfType<GameCode>().leaderboardItems.Add(this.gameObject);
    }

}
