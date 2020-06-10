package com.gambeat.mimo.paystack.paystack;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.widget.Toast;

import com.gambeat.mimo.paystack.paystack.activity.PaystackActivity;


public class AndroidBridge {
    private static Context context;

    public static void initPaystack(Context context){
        Toast.makeText(context, "it is hiting pay", Toast.LENGTH_LONG).show();
        Intent intent = new Intent(context, PaystackActivity.class);
        context.startActivity(intent);
        AndroidBridge.context = context;
    }

    public static void toast(Context context){
        Toast.makeText(context, "this is it", Toast.LENGTH_LONG).show();
    }
}
