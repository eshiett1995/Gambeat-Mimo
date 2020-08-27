using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string firstName;
    public string lastName;
    public string photoUrl;
    public string email;
    public List<int> score;
    public long position;

    public Player(string firstName, string lastName, string photoUrl, long position, List<int> score){
        this.firstName =firstName;
        this.lastName =lastName;
        this.photoUrl = photoUrl;
        this.position = position;
        this.score = score;
    }

    public string getScore(){
        string sc = "";
        for (int i = 0; i < score.Count; i++)
        {
            sc += score[i];
            if(i < score.Count-1)
                sc += ",";
        }
        Debug.Log(firstName + " No of scores: " + score.Count);
        return sc;
    }
}
