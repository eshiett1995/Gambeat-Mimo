using Facebook.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FBHolder : MonoBehaviour
{

    public GameObject profilePanel;

    public Text nameText;

    private void Awake()
    {
        FB.Init(SetInit, OnHideUnity);
    }

    private void SetInit() {

        Debug.Log("FB init done");

        if (FB.IsLoggedIn)
        {
            //profilePanel.SetActive(true);
        }
        else {
            //profilePanel.SetActive(false);
        }
    }

    private void OnHideUnity(bool isGameShown) {

    }

    public void FBLogin() {
        var perms = new List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(perms, AuthCallback);
    }

   

    private void AuthCallback(ILoginResult result)
    {

        Debug.Log("login in result");

        Debug.Log(result);

        //StartCoroutine(Post("https://webhook.site/11470d78-0e31-4dc1-8e06-ea94a6ee5086", JsonUtility.ToJson(result)));

        if (FB.IsLoggedIn)
        {
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            FetchFBProfile();
        }
        else
        {
            StartCoroutine(Post("https://webhook.site/11470d78-0e31-4dc1-8e06-ea94a6ee5086", "{'msg':'error'}"));
            Debug.Log("User cancelled login");
        }
    }

    private void FetchFBProfile()
    {
        FB.API("/me?fields=first_name,last_name,email", HttpMethod.GET, FetchProfileCallback, new Dictionary<string, string>() { });
    }

    private void FetchProfileCallback(IGraphResult result)
    {

        Debug.Log(result.RawResult);
        StartCoroutine(Post("https://webhook.site/11470d78-0e31-4dc1-8e06-ea94a6ee5086", result.ResultDictionary.ToJson()));

        profilePanel.SetActive(true);
        nameText.text = "";
        nameText.text = result.ResultDictionary["first_name"].ToString() +" "+ result.ResultDictionary["last_name"].ToString();

    }

    IEnumerator Post(string url, string bodyJsonString)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);
    }

    IEnumerator Upload(string url, string bodyJsonString)
    {
        WWWForm form = new WWWForm();
        form.AddField("myField", "myData");

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }
}
