package com.gambeat.mimo.paystack.paystack;

import android.content.Context;
import android.content.Intent;
import android.webkit.JavascriptInterface;

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
    public void closeWalletAfricaModal() {
        Intent intent = new Intent(mContext, Constants.baseContext.getClass());
        mContext.startActivity(intent);
    }
}