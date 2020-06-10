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
            javaClass.CallStatic("initPaystack", activity);
        }
        else
        {
            Debug.Log("i am here");
        }
    }
}
