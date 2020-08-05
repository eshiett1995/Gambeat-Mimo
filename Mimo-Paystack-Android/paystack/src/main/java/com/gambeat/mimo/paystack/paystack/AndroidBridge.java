package com.gambeat.mimo.paystack.paystack;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.View;
import android.widget.TextView;
import android.widget.Toast;

import com.gambeat.mimo.paystack.paystack.activity.PaystackActivity;
import com.gambeat.mimo.paystack.paystack.activity.WalletAfricaActivity;


public class AndroidBridge {
    private static Context context;

    public static void initPaystack(Context context, String authKey){
        Intent intent = new Intent(context, PaystackActivity.class);
        intent.putExtra("authKey", authKey);
        context.startActivity(intent);
        Constants.baseContext = context;
    }

    public static void initWalletAfrica(Context context, String authKey){
        Intent intent = new Intent(context, WalletAfricaActivity.class);
        intent.putExtra("authKey", authKey);
        context.startActivity(intent);
        Constants.baseContext = context;
    }

    public static void toast(Context context, String message){
        Toast.makeText(context, message, Toast.LENGTH_LONG).show();
    }

    public static void customToast(Context context, String message){
        LayoutInflater inflater = ((Activity)context).getLayoutInflater();
        View layout = inflater.inflate(R.layout.custom_toast, null);

        TextView text = layout.findViewById(R.id.text);
        text.setText(message);

        Toast toast = new Toast(context);
        toast.setGravity(Gravity.BOTTOM, 0, 40);
        toast.setDuration(Toast.LENGTH_LONG);
        toast.setView(layout);
        toast.show();
    }
}
