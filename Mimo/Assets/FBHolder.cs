using Facebook.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FBHolder : MonoBehaviour
{
    private void Awake()
    {
        FB.Init(SetInit, OnHideUnity);
    }

    private void SetInit() {

        Debug.Log("FB init done");

        if (FB.IsLoggedIn)
        {

        }
        else {

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
        if (FB.IsLoggedIn)
        {
            // AccessToken class will have session details
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            // Print current access token's User ID
            Debug.Log(aToken.UserId);
            // Print current access token's granted permissions
            foreach (string perm in aToken.Permissions)
            {
                Debug.Log(perm);
            }
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }
}
