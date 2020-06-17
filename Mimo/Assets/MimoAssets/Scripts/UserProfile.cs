using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class UserProfile : MonoBehaviour
{

    public Text username, fullName, email, games, wins, draws, winnings, cash, cash2;
    public Button wallet, back, closeWal, withdraw, deposit;
    public RawImage avatar;
    public GameObject profile, background, walletDialog, PaymentPanel, withdrawDialog;
    public static string paymentUrl;


    void Start()
    {
        Debug.Log("inside profile page");
        profile.transform.localScale = new Vector3(Screen.width / 720f, Screen.width / 720f, 0);
        background.transform.localScale = new Vector3(Screen.width / 720f, Screen.height / 1280f, 0);

        wallet.onClick.AddListener(() => openWallet());
        closeWal.onClick.AddListener(() => closeWallet());
        back.onClick.AddListener(() => openMenu());
        fullName.text = FBHolder.firstName + " " + FBHolder.lastName;
        email.text = FBHolder.email;
        avatar.texture = FBHolder.profilePic;

        StartCoroutine(HttpUtil.Get(HttpUtil.userProfileUrl, getProfileCallback));
    }

    private void getProfileCallback(UnityWebRequest response)
    {
        ProfileResponse profileResponse = new ProfileResponse();
        profileResponse = JsonUtility.FromJson<ProfileResponse>(response.downloadHandler.text);
        if (profileResponse.isSuccessful)
        {
            username.text = profileResponse.email;
            PlayerPrefs.SetString(LocalStorageUtil.Keys.email.ToString(), profileResponse.email);

            fullName.text = profileResponse.firstName + " " + profileResponse.lastName;
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
            
            cash.text = profileResponse.walletBalance.ToString();
            cash2.text = profileResponse.walletBalance.ToString();
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
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");
            //AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");
            AndroidJavaObject javaClass = new AndroidJavaClass("com.gambeat.mimo.paystack.paystack.AndroidBridge");
            if (javaClass != null)
            {
                //LocalStorageUtil.getAuthKey()
                javaClass.CallStatic("initPaystack", activity, "eyJhbGciOiJIUzI1NiJ9.eyJwcm92aWRlcl9jcmVkZW50aWFsIjp7ImZpcnN0TmFtZSI6Im90byIsImxhc3ROYW1lIjoiZXNoaWV0dCIsImVtYWlsIjoiZXNoaWV0dDE5OTVAZ21haWwuY29tIiwiaWQiOiIxMjM0NTYifSwicHJvdmlkZXIiOiJmYWNlYm9vayIsImVtYWlsIjoiZXNoaWV0dDE5OTVAZ21haWwuY29tIiwiaXNzIjoiR2FtYmVhdCIsInN1YiI6IkF1dGgifQ.CwspXgmggnt4Eujn0bCYOFmLu9V6KDzU41qLcPKIsyg");
            }
            
        }
        else
        {
            //Generate URL
            paymentUrl = "http://c39ddeff0013.ngrok.io/paystack";

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
                javaClass.CallStatic("initWalletAfrica", activity, "eyJhbGciOiJIUzI1NiJ9.eyJwcm92aWRlcl9jcmVkZW50aWFsIjp7ImZpcnN0TmFtZSI6Im90byIsImxhc3ROYW1lIjoiZXNoaWV0dCIsImVtYWlsIjoiZXNoaWV0dDE5OTVAZ21haWwuY29tIiwiaWQiOiIxMjM0NTYifSwicHJvdmlkZXIiOiJmYWNlYm9vayIsImVtYWlsIjoiZXNoaWV0dDE5OTVAZ21haWwuY29tIiwiaXNzIjoiR2FtYmVhdCIsInN1YiI6IkF1dGgifQ.CwspXgmggnt4Eujn0bCYOFmLu9V6KDzU41qLcPKIsyg");
            }

        }
        else
        {
            //Generate URL
            paymentUrl = "http://c39ddeff0013.ngrok.io/wallets.africa";

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
}
