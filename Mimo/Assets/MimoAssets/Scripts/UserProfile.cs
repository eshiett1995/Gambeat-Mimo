using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UserProfile : MonoBehaviour
{
    public Text username, fullName, email, games, wins, draws, winnings, cash, cash2;
    public Button wallet, back, closeWal, withdraw, deposit;
    public RawImage avatar;
    public GameObject profile, background, walletDialog;
   

    void Start()
    {
        profile.transform.localScale = new Vector3(Screen.width / 720f, Screen.width / 720f, 0);
        background.transform.localScale = new Vector3(Screen.width / 720f, Screen.height / 1280f, 0);

        wallet.onClick.AddListener(() => openWallet());
        closeWal.onClick.AddListener(() => closeWallet());
        back.onClick.AddListener(() => openMenu());
        fullName.text = FBHolder.firstName + " " + FBHolder.lastName;
        email.text = FBHolder.email;
        avatar.texture = FBHolder.profilePic;
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

    void Update()
    {
        
    }
}
