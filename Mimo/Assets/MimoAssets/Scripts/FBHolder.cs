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
    public static string userName, firstName = "", lastName, email, id, tempPhotoURL;
    public static int gamesPlayed, gamesWon, gamesDrawn;
    public Text profile_name;
    public RawImage avatar;
    public RawImage facebookButtonImage;
    public Sprite faceBookLoginImage;
    public Sprite faceBookLogoutImage;
    public static Texture2D profilePic;


    private void Start()
    {
        ArrangeUI(FB.IsLoggedIn);
    }

    public void openProfile()
    {
        if (firstName.Equals(""))
        {
            Debug.Log("You need to log in first");
        }
        else
        {
            Debug.Log("Opening Profile");
            SceneManager.LoadScene("ProfileScene");
        }
    }

    private void Awake()
    {
        FB.Init(SetInit, OnHideUnity);
    }

    private void Update()
    {
        ArrangeUI(FB.IsLoggedIn);
        SwitchFaceBookButtonImage(FB.IsLoggedIn);
    }

    private void SetInit()
    {
        ArrangeUI(FB.IsLoggedIn);
    }

    private void OnHideUnity(bool isGameShown)
    {

    }

    public void FBLogin()
    {
        if (FB.IsLoggedIn)
        {
            FB.LogOut();
        }
        else
        {
            var perms = new List<string>() { "public_profile", "email" };
            FB.LogInWithReadPermissions(perms, AuthCallback);
        }
    }



    private void AuthCallback(ILoginResult result)
    {

        if (FB.IsLoggedIn)
        {
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            FetchProfilePicture();
        }
        else { }
    }

    private void FetchProfilePicture() {
        FB.API("/me/picture?type=square&height=200&width=200&redirect=false", HttpMethod.GET, ProfilePhotoCallback);
    }

    private void FetchFBProfile(string photoURL)
    {
        tempPhotoURL = photoURL;
        PlayerPrefs.SetString(LocalStorageUtil.Keys.photoUrl.ToString(), photoURL);
        FB.API("/me?fields=first_name,last_name,email", HttpMethod.GET, FetchProfileCallback, new Dictionary<string, string>() { });
    }


    private void FetchProfileCallback(IGraphResult result)
    {
        firstName = result.ResultDictionary["first_name"].ToString();
        lastName = result.ResultDictionary["last_name"].ToString();
        email = result.ResultDictionary["email"].ToString();
        id = result.ResultDictionary["id"].ToString();

        profile_name.text = firstName;

        PlayerPrefs.SetString(LocalStorageUtil.Keys.firstName.ToString(), result.ResultDictionary["first_name"].ToString());
        PlayerPrefs.SetString(LocalStorageUtil.Keys.lastName.ToString(), result.ResultDictionary["last_name"].ToString());
        PlayerPrefs.SetString(LocalStorageUtil.Keys.email.ToString(), result.ResultDictionary["email"].ToString());

        DisplayProfilePanel();

        FacebookLoginRequest facebookLoginRequest = new FacebookLoginRequest();
        facebookLoginRequest.firstName = result.ResultDictionary["first_name"].ToString();
        facebookLoginRequest.lastName = result.ResultDictionary["last_name"].ToString();
        facebookLoginRequest.email = result.ResultDictionary["email"].ToString();
        facebookLoginRequest.id = result.ResultDictionary["id"].ToString();
        facebookLoginRequest.photoUrl = tempPhotoURL;

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

  

    private void DisplayProfilePanel()
    {
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

    private void ProfilePhotoCallback(IGraphResult result)
    {
        if (string.IsNullOrEmpty(result.Error) && !result.Cancelled)
        {
            IDictionary data = result.ResultDictionary["data"] as IDictionary;
            string photoURL = data["url"] as string;
            FetchFBProfile(photoURL);

            StartCoroutine(FetchProfilePic(photoURL));
        }
    }

    private IEnumerator FetchProfilePic(string url)
    {
        WWW www = new WWW(url);
        yield return www;
        avatar.texture = www.texture;
        profilePic = www.texture;

        //Sprite sprites = new Sprite();



        //sprite = Sprite.Create(www.texture, new Rect(0, 0, 50, 50), Vector2.zero);
        //sprites = Sprite.Create(www.texture, new Rect(0, 0, 50, 50), Vector2.zero);

    }


    private void ArrangeUI(bool isLoggedIn) {
        if (isLoggedIn)
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

    private void SwitchFaceBookButtonImage(bool isLoggedIn)
    {
        if (isLoggedIn)
        {
            facebookButtonImage.texture = faceBookLogoutImage.texture;
        }
        else
        {
            facebookButtonImage.texture = faceBookLoginImage.texture;
        }
    }
}


//https://stackoverflow.com/questions/42854992/issue-about-facebook-sdk-is-facebook-image-url-change-each-login-how-to-get-i