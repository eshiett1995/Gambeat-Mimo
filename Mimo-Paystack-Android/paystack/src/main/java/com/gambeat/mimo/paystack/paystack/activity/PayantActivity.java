package com.gambeat.mimo.paystack.paystack.activity;

import android.app.Activity;
import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.view.View;
import android.webkit.WebSettings;
import android.webkit.WebView;
import android.webkit.WebViewClient;
import android.widget.ProgressBar;

import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatActivity;

import com.gambeat.mimo.paystack.paystack.Constants;
import com.gambeat.mimo.paystack.paystack.R;
import com.gambeat.mimo.paystack.paystack.WebInterface;

import java.util.HashMap;

public class PayantActivity extends AppCompatActivity {

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

}
