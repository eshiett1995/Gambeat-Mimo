using Facebook.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FBHolder : MonoBehaviour
{

    public GameObject profilePanel;
    public static string userName, firstName = "", lastName, email, id;
    public static int gamesPlayed, gamesWon, gamesDrawn;
    public Text profile_name;
    public RawImage avatar;
    public static Texture2D profilePic;


    private void Start()
    {
        if (FB.IsLoggedIn)
        {
            profile_name.text = LocalStorageUtil.get("firstName");
            avatar.texture = profilePic;
            profilePanel.SetActive(true);
        }
        else {
            profilePanel.SetActive(false);
        }
    }

    public void openProfile()
    {
        if(firstName.Equals("")){
            Debug.Log("You need to log in first");
        }else{
            Debug.Log("Opening Profile");
            SceneManager.LoadScene("ProfileScene");
        }
    }

    private void Awake()
    {
        FB.Init(SetInit, OnHideUnity);
    }

    private void SetInit()
    {
        if (FB.IsLoggedIn)
        {
            profilePanel.SetActive(true);
        }
        else
        {
            profilePanel.SetActive(false);
        }
    }

    private void OnHideUnity(bool isGameShown)
    {

    }

    public void FBLogin()
    {
        var perms = new List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(perms, AuthCallback);
    }



    private void AuthCallback(ILoginResult result)
    {

        if (FB.IsLoggedIn)
        {
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            FetchFBProfile();
        }
        else{}
    }

    private void FetchFBProfile()
    {
        FB.API("/me?fields=first_name,last_name,email", HttpMethod.GET, FetchProfileCallback, new Dictionary<string, string>() { });
    }


    private void FetchProfileCallback(IGraphResult result)
    {
        firstName = result.ResultDictionary["first_name"].ToString();
        lastName = result.ResultDictionary["last_name"].ToString();
        email = result.ResultDictionary["email"].ToString();
        id = result.ResultDictionary["id"].ToString();

        profile_name.text = firstName;
        avatar.texture = result.Texture;
        profilePic = result.Texture;

        PlayerPrefs.SetString(LocalStorageUtil.Keys.firstName.ToString(), result.ResultDictionary["first_name"].ToString());
        PlayerPrefs.SetString(LocalStorageUtil.Keys.lastName.ToString(), result.ResultDictionary["last_name"].ToString());
        PlayerPrefs.SetString(LocalStorageUtil.Keys.email.ToString(), result.ResultDictionary["email"].ToString());

        DisplayProfilePanel();

        FacebookLoginRequest facebookLoginRequest = new FacebookLoginRequest();
        facebookLoginRequest.firstName = result.ResultDictionary["first_name"].ToString();
        facebookLoginRequest.lastName = result.ResultDictionary["last_name"].ToString();
        facebookLoginRequest.email = result.ResultDictionary["email"].ToString();
        facebookLoginRequest.id = result.ResultDictionary["id"].ToString();

        StartCoroutine(HttpUtil.Post(HttpUtil.facebookAuthUrl, JsonUtility.ToJson(facebookLoginRequest), (response) =>
        {
            ResponseModel responseModel = new ResponseModel();
            responseModel = JsonUtility.FromJson<ResponseModel>(response.downloadHandler.text);
            if (responseModel.isSuccessful || responseModel.successful)
            {
                LocalStorageUtil.saveAuthKey(responseModel.jtwToken);
            }
            else
            {
                Debug.Log("this is the bad message: " + responseModel.message);
            }
        }));
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

    private void DisplayProfilePanel() {
       if (FB.IsLoggedIn)
       {
           profile_name.text = LocalStorageUtil.get("firstName");
           avatar.texture = profilePic;
           profilePanel.SetActive(true);
       }
       else
       {
           profilePanel.SetActive(false);
       }
    }
}
