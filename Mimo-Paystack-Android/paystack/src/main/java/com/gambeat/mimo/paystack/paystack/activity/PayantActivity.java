package com.gambeat.mimo.paystack.paystack.activity;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.view.View;
import android.webkit.JavascriptInterface;
import android.webkit.WebSettings;
import android.webkit.WebView;
import android.webkit.WebViewClient;
import android.widget.ProgressBar;

import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatActivity;

import com.gambeat.mimo.paystack.paystack.Constants;
import com.gambeat.mimo.paystack.paystack.R;
import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerActivity;

import java.util.HashMap;

public class PayantActivity extends UnityPlayerActivity {

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_webview);
        final WebView mimoWebview = findViewById(R.id.webView);
        final ProgressBar progressBar = findViewById(R.id.progressBar);
        final String authKey = getIntent().getStringExtra("authKey");
        WebSettings webSettings = mimoWebview.getSettings();
        webSettings.setJavaScriptEnabled(true);
        webSettings.setDomStorageEnabled(true);
        webSettings.setCacheMode(WebSettings.LOAD_NO_CACHE);
        mimoWebview.addJavascriptInterface(new WebInterface(this), "Android");

        mimoWebview.loadUrl("https://gambeat.com.ng/payant");

        mimoWebview.setWebViewClient(new WebViewClient() {

            @Override
            public boolean shouldOverrideUrlLoading(WebView view, String url) {

                Uri parsedUri = Uri.parse(url);
                HashMap<String, String> linkData = parseLinkUriData(parsedUri);
                boolean shouldClose = Boolean.parseBoolean(linkData.get("shouldClose"));
                if (shouldClose) {
                    Intent intent = new Intent(PayantActivity.this, Constants.baseContext.getClass());
                    setResult(Activity.RESULT_OK, intent);
                    finish();
                } else {
                    return false;
                }
                return true;
            }

            public void onPageFinished(WebView view, String weburl){
                progressBar.setVisibility(View.GONE);
                if (android.os.Build.VERSION.SDK_INT >= android.os.Build.VERSION_CODES.KITKAT) {
                    mimoWebview.evaluateJavascript("init("+"'"+authKey+"'"+",'android');", null);
                } else {
                    mimoWebview.loadUrl("init("+"'"+authKey+"'"+", 'android');");
                }
            }
        });
    }

    public HashMap<String, String> parseLinkUriData(Uri linkUri) {
        HashMap<String, String> linkData = new HashMap<String, String>();
        for (String key : linkUri.getQueryParameterNames()) {
            linkData.put(key, linkUri.getQueryParameter(key));
        }
        return linkData;
    }

    @Override
    public void onBackPressed() {
        super.onBackPressed();
        Intent intent = new Intent(PayantActivity.this, Constants.baseContext.getClass());
        startActivity(intent);
    }

    public class WebInterface {
        private Context mContext;
        public WebInterface(Context c) {
            mContext = c;
        }

        @JavascriptInterface
        public void payantOnSuccess(int amount) {
            Intent intent = new Intent(mContext, Constants.baseContext.getClass());
            mContext.startActivity(intent);
            UnityPlayer. UnitySendMessage("UserProfile","CreditWallet",String.valueOf(amount));
        }

        @JavascriptInterface
        public void payantOnError() {}
    }
}
