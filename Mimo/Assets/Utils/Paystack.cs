using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paystack : MonoBehaviour
{
    private AndroidJavaObject javaClass;

    private void Awake()
    {
        Debug.Log("in my head");
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("carress mem");        com.gambeat.mimo.paystack.android
        //                                 com.gambeat.mimo.paystack.paystack.PaystackAndroid
        //javaClass = new AndroidJavaObject("com.gambeat.mimo.paystack.paystack.AndroidBridge");
        //javaClass.CallStatic("initPaystack", "");
        //javaClass.Call("initPayment", "");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toast()
    {
        AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");
        //AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");
        javaClass = new AndroidJavaClass("com.gambeat.mimo.paystack.paystack.AndroidBridge");
        if (javaClass != null)
        {
            javaClass.CallStatic("toast", activity);
        }
        else {
            Debug.Log("i am here");
        }
    }

    public void paystack()
    {
        AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");
        //AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");
        javaClass = new AndroidJavaClass("com.gambeat.mimo.paystack.paystack.AndroidBridge");
        if (javaClass != null)
        {
            //LocalStorageUtil.getAuthKey()
            javaClass.CallStatic("initPaystack", activity, "eyJhbGciOiJIUzI1NiJ9.eyJwcm92aWRlcl9jcmVkZW50aWFsIjp7ImZpcnN0TmFtZSI6Im90byIsImxhc3ROYW1lIjoiZXNoaWV0dCIsImVtYWlsIjoiZXNoaWV0dDE5OTVAZ21haWwuY29tIiwiaWQiOiIxMjM0NTYifSwicHJvdmlkZXIiOiJmYWNlYm9vayIsImVtYWlsIjoiZXNoaWV0dDE5OTVAZ21haWwuY29tIiwiaXNzIjoiR2FtYmVhdCIsInN1YiI6IkF1dGgifQ.CwspXgmggnt4Eujn0bCYOFmLu9V6KDzU41qLcPKIsyg");
        }
        else
        {
            Debug.Log("i am here");
        }
    }
}
