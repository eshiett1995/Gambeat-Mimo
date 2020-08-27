using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static LeaderBoardResponse;

public class ListViewScript : MonoBehaviour
{
    private int index;
    public Text name, score, indexText;
    public RawImage avatar;
    public RawImage avatarMask;
    public Sprite coloredMask;
    private Mascots mascots;
    private string photoUrl;

    void Start()

    {
        mascots = FindObjectOfType<Mascots>();

        index = FindObjectOfType<GameCode>().leaderboardItems.Count;
        int num = index + 1;
        indexText.text = ""+num;

        if(UI.isRoyal){
            name.color = Color.black;
            score.color = Color.black;
            indexText.color = Color.black;

            Player player = RoyalRumbleScript.players[index];

            name.text = $"{player.firstName} .{player.lastName.Substring(0, 1)}";
            score.text = player.getScore();
            photoUrl = player.photoUrl;
        }
        else{

            FormattedRank userRank = Multiplayer.leaderBoardData.ranks[index];

            name.text = $"{userRank.firstName} .{userRank.lastName.Substring(0, 1)}";
            score.text = userRank.score.ToString();
            photoUrl = userRank.photoUrl;
        
        }

        if ( photoUrl == null || photoUrl == "")
        {
            SetMascotMask(avatarMask, coloredMask);
            SetRandomMascot(avatar, mascots.images);
        }
        else {
            StartCoroutine(FetchProfilePic(avatar, photoUrl));
        }

        FindObjectOfType<GameCode>().leaderboardItems.Add(this.gameObject);
            
    }

    private IEnumerator FetchProfilePic(RawImage avatar, string url)
    {
        WWW www = new WWW(url);
        yield return www;
        avatar.texture = www.texture;
    }

    private void SetRandomMascot(RawImage avatar, List<Sprite> images)
    {
        var size = images.Count;
        Debug.Log("this is the size " + size);
        avatar.texture = images[Random.Range(0, size)].texture;
    }

    private void SetMascotMask(RawImage avatarMask, Sprite coloredMask)
    {
        avatarMask.texture = coloredMask.texture;
    }
}
