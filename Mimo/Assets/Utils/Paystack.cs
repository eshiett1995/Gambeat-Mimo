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
        Debug.Log("carress mem");
        javaClass = new AndroidJavaObject("com.gambeat.mimo.paystack.paystack.PaystackAndroid");
        javaClass.CallStatic("initPaystack", "");
        javaClass.Call("initPayment", "");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
