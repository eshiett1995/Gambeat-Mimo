﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HttpUtil
{
    static public string baseUrl = "https://gambeat.com.ng"; //"https://gambeat.com.ng"; //http://15.236.202.44
    static public string leaderBoardUrl = baseUrl + "/leader-board";
    static public string userProfileUrl = baseUrl + "/user";
    static public string facebookAuthUrl = baseUrl + "/auth/facebook";
    static public string royalRumbleSearch = baseUrl + "/match/royal-rumble/search";
    static public string matchHistory = baseUrl + "/match/entered";
    static public string royalRumbleCreate = baseUrl + "/match/royal-rumble/create";
    static public string royalRumbleInit = baseUrl + "/match/royal-rumble/init";
    static public string royalRumbleJoin = baseUrl + "/match/royal-rumble/join";
    static public string submitRoyalRumbleScore = baseUrl + "/match/royal-rumble/submit";
    static public string getPlayersInAMatch = baseUrl + "/match/royal-rumble/players";



    static public IEnumerator Post(string url, string bodyJsonString, System.Action<UnityWebRequest> callback)
    {
        var request = new UnityWebRequest(url, "POST");
        request.timeout = 60;
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        //request.SetRequestHeader("Authorization", "eyJhbGciOiJIUzI1NiJ9.eyJwcm92aWRlcl9jcmVkZW50aWFsIjp7ImZpcnN0TmFtZSI6Im90byIsImxhc3ROYW1lIjoiZXNoaWV0dCIsImVtYWlsIjoiZXNoaWV0dDE5OTVAZ21haWwuY29tIiwiaWQiOiIxMjM0NTYifSwicHJvdmlkZXIiOiJmYWNlYm9vayIsImVtYWlsIjoiZXNoaWV0dDE5OTVAZ21haWwuY29tIiwiaXNzIjoiR2FtYmVhdCIsInN1YiI6IkF1dGgifQ.CwspXgmggnt4Eujn0bCYOFmLu9V6KDzU41qLcPKIsyg");
        request.SetRequestHeader("Authorization", LocalStorageUtil.getAuthKey());
        yield return request.SendWebRequest();
        callback(request);
    }

    static public IEnumerator Get(string url, System.Action<UnityWebRequest> callback)
    {
        var request = new UnityWebRequest(url, "GET");
        request.timeout = 60;
        //byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(bodyJsonString);
        //request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("bears", LocalStorageUtil.getAuthKey());
        //request.SetRequestHeader("Authorization", "eyJhbGciOiJIUzI1NiJ9.eyJwcm92aWRlcl9jcmVkZW50aWFsIjp7ImZpcnN0TmFtZSI6Im90byIsImxhc3ROYW1lIjoiZXNoaWV0dCIsImVtYWlsIjoiZXNoaWV0dDE5OTVAZ21haWwuY29tIiwiaWQiOiIxMjM0NTYifSwicHJvdmlkZXIiOiJmYWNlYm9vayIsImVtYWlsIjoiZXNoaWV0dDE5OTVAZ21haWwuY29tIiwiaXNzIjoiR2FtYmVhdCIsInN1YiI6IkF1dGgifQ.CwspXgmggnt4Eujn0bCYOFmLu9V6KDzU41qLcPKIsyg");
        request.SetRequestHeader("Authorization", LocalStorageUtil.getAuthKey());
        yield return request.SendWebRequest();
        callback(request);
    }
}
