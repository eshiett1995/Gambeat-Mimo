using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class UserProfile : MonoBehaviour
{

    public Text username, fullName, email, games, wins, draws, losses, cash, cash2;
    public Button wallet, back, closeWal, withdraw, deposit;
    public RawImage avatar;
    public GameObject profile, background, walletDialog, PaymentPanel, withdrawDialog;
    public static string paymentUrl;
    private static bool isProfile;
    public float targetTime = 60.0f;
    public bool startTimer = false;


    void Start()
    {
        profile.transform.localScale = new Vector3(Screen.width / 720f, Screen.width / 720f, 0);
        background.transform.localScale = new Vector3(Screen.width / 720f, Screen.height / 1280f, 0);

        wallet.onClick.AddListener(() => openWallet());
        closeWal.onClick.AddListener(() => closeWallet());
        back.onClick.AddListener(() => openMenu());
        
        getData();
    }

    public void getData(){
        SetProfileDataFromLocalStorage();
        StartCoroutine(HttpUtil.Get(HttpUtil.userProfileUrl, getProfileCallback));
    }

    public void GetProfileData() {
        StartCoroutine(HttpUtil.Get(HttpUtil.userProfileUrl, getProfileCallback));
    }

    private void getProfileCallback(UnityWebRequest response)
    {
        ProfileResponse profileResponse = new ProfileResponse();
        profileResponse = JsonUtility.FromJson<ProfileResponse>(response.downloadHandler.text);
        Debug.Log("here comes the profile");
        Debug.Log(response.downloadHandler.text);
        if (profileResponse.isSuccessful || profileResponse.successful)
        {
                username.text = $"{profileResponse.firstName} {profileResponse.lastName}";

                fullName.text = $"{profileResponse.firstName} {profileResponse.lastName}";
                PlayerPrefs.SetString(LocalStorageUtil.Keys.firstName.ToString(), profileResponse.firstName);
                PlayerPrefs.SetString(LocalStorageUtil.Keys.lastName.ToString(), profileResponse.lastName);

                email.text = profileResponse.email;
                PlayerPrefs.SetString(LocalStorageUtil.Keys.email.ToString(), profileResponse.email);

                games.text = (profileResponse.wins + profileResponse.draws + profileResponse.losses).ToString();
                PlayerPrefs.SetFloat(LocalStorageUtil.Keys.games.ToString(), profileResponse.wins + profileResponse.draws + profileResponse.losses);

                wins.text = profileResponse.wins.ToString();
                PlayerPrefs.SetFloat(LocalStorageUtil.Keys.wins.ToString(), profileResponse.wins);

                draws.text = profileResponse.draws.ToString();
                PlayerPrefs.SetFloat(LocalStorageUtil.Keys.draws.ToString(), profileResponse.draws);

                losses.text = profileResponse.draws.ToString();
                PlayerPrefs.SetFloat(LocalStorageUtil.Keys.losses.ToString(), profileResponse.losses);

                cash.text = $"N{(profileResponse.walletBalance/100).ToString("N0")}";
                PlayerPrefs.SetFloat(LocalStorageUtil.Keys.cash.ToString(), profileResponse.walletBalance);
            
        }
        else
        {
            Debug.Log("this is the message: " + profileResponse.message);
        }
    }

    void openMenu()
    {
        SceneManager.LoadScene("MimoScene");
    }

    void openWallet()
    {
        walletDialog.SetActive(true);
    }

    void closeWallet()
    {
        walletDialog.SetActive(false);
    }


    public void makeDeposit()
    {
        startTimer = true;
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");
            //AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");
            AndroidJavaObject javaClass = new AndroidJavaClass("com.gambeat.mimo.paystack.paystack.AndroidBridge");
            if (javaClass != null)
            {
                //LocalStorageUtil.getAuthKey()
                javaClass.CallStatic("initPayant", activity, LocalStorageUtil.getAuthKey());
            }
            
        }
        else
        {
         
            paymentUrl = "https://gambeat.com.ng/payant";

            closeWallet();
            PaymentPanel.SetActive(true);
            FindObjectOfType<SampleWebView>().webViewObject.SetVisibility(true);
            FindObjectOfType<SampleWebView>().webViewObject.enabled = true;
        }
    }

    public void makeCashoutOut()
    {
        startTimer = true;
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");
            //AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");
            AndroidJavaObject javaClass = new AndroidJavaClass("com.gambeat.mimo.paystack.paystack.AndroidBridge");
            if (javaClass != null)
            {
                //LocalStorageUtil.getAuthKey()
                javaClass.CallStatic("initWalletAfrica", activity, LocalStorageUtil.getAuthKey());
            }

        }
        else
        {
            //Generate URL
            paymentUrl = "https://gambeat.com.ng/wallets.africa";

            closeWallet();
            PaymentPanel.SetActive(true);
            FindObjectOfType<SampleWebView>().webViewObject.SetVisibility(true);
            FindObjectOfType<SampleWebView>().webViewObject.enabled = true;
        }
    }

    public void endPayment()
    {
        FindObjectOfType<SampleWebView>().webViewObject.SetVisibility(false);
        FindObjectOfType<SampleWebView>().webViewObject.enabled = false;
        PaymentPanel.SetActive(false);
    }
    public void withdrawFunds()
    {
        withdrawDialog.SetActive(true);
    }
    public void closewithdrawDialog()
    {
        withdrawDialog.SetActive(false);
    }
    public void processWithdrawal()
    {
        closewithdrawDialog();
    }

    private void SetProfileDataFromLocalStorage() {
        username.text = $"{LocalStorageUtil.get("firstName")} {LocalStorageUtil.get("lastName")}";
        fullName.text = $"{LocalStorageUtil.get("firstName")} {LocalStorageUtil.get("lastName")}";
        email.text = LocalStorageUtil.get("email");
        avatar.texture = FBHolder.profilePic;

        games.text = PlayerPrefs.GetFloat(LocalStorageUtil.Keys.games.ToString()).ToString();
     
        wins.text = PlayerPrefs.GetFloat(LocalStorageUtil.Keys.wins.ToString()).ToString();

        draws.text = PlayerPrefs.GetFloat(LocalStorageUtil.Keys.draws.ToString()).ToString();

        losses.text = PlayerPrefs.GetFloat(LocalStorageUtil.Keys.losses.ToString()).ToString();

        cash.text = $"N{PlayerPrefs.GetFloat(LocalStorageUtil.Keys.cash.ToString()) / 100:N0}";
    }

    private void Update()
    {
       if (startTimer) {
          targetTime -= Time.deltaTime;
           if (targetTime <= 0.0f)
           {
               GetProfileData();
               targetTime = 60.0f;
           }
       }
    }
}
