using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HttpUtil
{
    static public string baseUrl = "http://56bc1e6e.ngrok.io";
    static public string leaderBoardUrl = baseUrl + "/leader-board";
    static public string userProfileUrl = baseUrl + "/user";
    static public string facebookAuthUrl = baseUrl + "/auth/facebook";

    
    static public IEnumerator Post(string url, string bodyJsonString, System.Action<UnityWebRequest> callback)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", LocalStorageUtil.getAuthKey());
        yield return request.SendWebRequest();
        callback(request);
    }

    static public IEnumerator Get(string url, System.Action<UnityWebRequest> callback)
    {
        var request = new UnityWebRequest(url, "GET");
        //byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(bodyJsonString);
        //request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", LocalStorageUtil.getAuthKey());
        yield return request.SendWebRequest();
        callback(request);
    }
}
