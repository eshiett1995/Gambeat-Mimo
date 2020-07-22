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


    void start()
    {
        Button profile = profilePanel.GetComponent<Button>();
        profile.onClick.AddListener(() => openProfile());
        profile_name.text = firstName;
        avatar.texture = profilePic;
        Debug.Log("Run FBHolder");
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

        Debug.Log("FB init done");

        if (FB.IsLoggedIn)
        {
            profilePanel.SetActive(true);
        }
        else
        {
            //profilePanel.SetActive(false);
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

        Debug.Log("login in result");

        Debug.Log(result);

        //StartCoroutine(Post("https://webhook.site/11470d78-0e31-4dc1-8e06-ea94a6ee5086", JsonUtility.ToJson(result)));

        if (FB.IsLoggedIn)
        {
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            Debug.Log("User Logged in");
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
        profilePanel.SetActive(true);

        firstName = result.ResultDictionary["first_name"].ToString();
        lastName = result.ResultDictionary["last_name"].ToString();
        email = result.ResultDictionary["email"].ToString();
        id = result.ResultDictionary["id"].ToString();

        profile_name.text = firstName;
        avatar.texture = result.Texture;
        profilePic = result.Texture;

        PlayerPrefs.SetString(LocalStorageUtil.Keys.firstName.ToString(), result.ResultDictionary["first_name"].ToString());
        PlayerPrefs.SetString(LocalStorageUtil.Keys.lastName.ToString(), result.ResultDictionary["last_name"].ToString());

        FacebookLoginRequest facebookLoginRequest = new FacebookLoginRequest();
        facebookLoginRequest.firstName = result.ResultDictionary["first_name"].ToString();
        facebookLoginRequest.lastName = result.ResultDictionary["last_name"].ToString();
        facebookLoginRequest.email = result.ResultDictionary["email"].ToString();
        facebookLoginRequest.id = result.ResultDictionary["id"].ToString();

        StartCoroutine(HttpUtil.Post(HttpUtil.facebookAuthUrl, JsonUtility.ToJson(facebookLoginRequest), (response) =>
        {
            ResponseModel responseModel = new ResponseModel();
            responseModel = JsonUtility.FromJson<ResponseModel>(response.downloadHandler.text);
            Debug.Log(response.downloadHandler.text);
            if (responseModel.isSuccessful || responseModel.successful)
            {
                Debug.Log("this is the message: " + responseModel.message);
                Debug.Log("this is the auth: " + responseModel.jtwToken);
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
}
