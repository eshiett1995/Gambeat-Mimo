package com.gambeat.mimo.paystack.paystack;

import android.content.Context;
import android.content.Intent;
import android.webkit.JavascriptInterface;

import com.unity3d.player.UnityPlayer;

public class WebInterface {
    private Context mContext;
    public WebInterface(Context c) {
        mContext = c;
    }

    @JavascriptInterface
    public void closePaystackModal() {
        Intent intent = new Intent(mContext, Constants.baseContext.getClass());
        mContext.startActivity(intent);
    }

    @JavascriptInterface
    public void closeWalletAfricaModal(String response) {
        Intent intent = new Intent(mContext, Constants.baseContext.getClass());
        mContext.startActivity(intent);
    }

    @JavascriptInterface
    public void payantOnSuccess(int amount) {
        UnityPlayer. UnitySendMessage("UserProfile","CreditWallet",String.valueOf(amount));
    }

    @JavascriptInterface
    public void payantOnError() {

    }

    @JavascriptInterface
    public void walletsAfricaOnSuccess(int amount) {
        UnityPlayer. UnitySendMessage("UserProfile","DebitWallet",String.valueOf(amount));
    }

    @JavascriptInterface
    public void walletsAfricaOnError() {

    }
}