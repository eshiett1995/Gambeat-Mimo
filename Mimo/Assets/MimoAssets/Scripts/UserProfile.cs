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


    void Start()
    {
        profile.transform.localScale = new Vector3(Screen.width / 720f, Screen.width / 720f, 0);
        background.transform.localScale = new Vector3(Screen.width / 720f, Screen.height / 1280f, 0);

        wallet.onClick.AddListener(() => openWallet());
        closeWal.onClick.AddListener(() => closeWallet());
        back.onClick.AddListener(() => openMenu());
        
        getData(true);

        AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject javaClass = new AndroidJavaClass("com.gambeat.mimo.paystack.paystack.AndroidBridge");
        if (javaClass != null)
        {
            //LocalStorageUtil.getAuthKey()
            javaClass.CallStatic("toast", activity, "in start method");
        }
    }

    private void Awake()
    {
        AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject javaClass = new AndroidJavaClass("com.gambeat.mimo.paystack.paystack.AndroidBridge");
        if (javaClass != null)
        {
            //LocalStorageUtil.getAuthKey()
            javaClass.CallStatic("toast", activity, "in awake method");
        }
    }

    public void getData(bool isProfile){
        
        UserProfile.isProfile = isProfile;

        SetProfileDataFromLocalStorage();
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
            if(isProfile){
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
            
            }else{

                FindObjectOfType<UI>().cashText.text = $"N{(profileResponse.walletBalance/100).ToString("N0")}";
            }
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

    public void CreditWallet(string amount) {
        var amountToCredit = float.Parse(amount);
        var availableCash = PlayerPrefs.GetFloat(LocalStorageUtil.Keys.cash.ToString(), 0);
        cash.text = $"N{((availableCash + amountToCredit)/ 100).ToString("N0")}";
        PlayerPrefs.SetFloat(LocalStorageUtil.Keys.cash.ToString(), availableCash + amountToCredit);
    }

    public void DebitWallet(string amount)
    {
        var amountToDebit = float.Parse(amount);
        var availableCash = PlayerPrefs.GetFloat(LocalStorageUtil.Keys.cash.ToString(), 0);
        cash.text = $"N{((availableCash - amountToDebit) / 100).ToString("N0")}";
        PlayerPrefs.SetFloat(LocalStorageUtil.Keys.cash.ToString(), availableCash - amountToDebit);
    }

    public void makeDeposit()
    {
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
            //Generate URL
            paymentUrl = "https://gambeat.com.ng/payant";

            closeWallet();
            PaymentPanel.SetActive(true);
            FindObjectOfType<SampleWebView>().webViewObject.SetVisibility(true);
            FindObjectOfType<SampleWebView>().webViewObject.enabled = true;
        }
    }

    public void makeCashoutOut()
    {
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
       /** AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

        AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        AndroidJavaObject intent = currentActivity.Call<AndroidJavaObject>("getIntent");

        bool credit = intent.Call<bool>("getBooleanExtra", "credit");

        int amount = intent.Call<int>("getIntExtra", "amount");

        Debug.Log("this is the credit " + credit);

        Debug.Log("this is the amount " + amount);
        AndroidJavaObject javaClass = new AndroidJavaClass("com.gambeat.mimo.paystack.paystack.AndroidBridge");
        if (javaClass != null)
        {
            //LocalStorageUtil.getAuthKey()
            javaClass.CallStatic("initWalletAfrica", activity, LocalStorageUtil.getAuthKey());
        }**/
    }
}
