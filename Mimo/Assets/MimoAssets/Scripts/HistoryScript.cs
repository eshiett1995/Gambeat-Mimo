using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HistoryScript : MonoBehaviour
{
    public Text txt;

    void Start()
    {
        int index = GameCode.historyItems.Count;
        txt.text = Multiplayer.transactions[index];

        GameCode.historyItems.Add(this.gameObject);
    }

}
