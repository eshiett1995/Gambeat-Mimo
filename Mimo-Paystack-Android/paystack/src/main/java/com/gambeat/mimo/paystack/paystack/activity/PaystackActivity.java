package com.gambeat.mimo.paystack.paystack.activity;

import android.app.Activity;
import android.os.Bundle;
import android.widget.EditText;
import android.widget.Toast;

import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatActivity;

import com.gambeat.mimo.paystack.paystack.AndroidBridge;
import com.gambeat.mimo.paystack.paystack.R;

import co.paystack.android.Paystack;
import co.paystack.android.PaystackSdk;
import co.paystack.android.Transaction;
import co.paystack.android.model.Card;
import co.paystack.android.model.Charge;

public class PaystackActivity extends AppCompatActivity {

    private static Card card;
    private static Charge charge;
    private EditText cardNumberEt, cvvEt, amountEt, monthEt, yearEt;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_paystack);
        cardNumberEt = findViewById(R.id.card_number);
        cvvEt = findViewById(R.id.cvv);
        amountEt = findViewById(R.id.amountEt);
        monthEt = findViewById(R.id.monthEt);
        yearEt = findViewById(R.id.yearEt);

        PaystackSdk.initialize(this);
        PaystackSdk.setPublicKey("pk_test_3a7af4a93d785ab7fb183ee27eeae4be3755340e");
    }

    public void initPayment(){
        card = new Card(cardNumberEt.getText().toString(),
                Integer.getInteger(monthEt.getText().toString()),
                Integer.getInteger(yearEt.getText().toString()),
                cvvEt.getText().toString());

        if (card.isValid()) {
            Toast.makeText(this, "Card is Valid", Toast.LENGTH_LONG).show();
            performCharge(Integer.getInteger(amountEt.getText().toString()), "");
        } else {
            Toast.makeText(this, "Card not Valid", Toast.LENGTH_LONG).show();
        }
    }

    private void performCharge(int amount, String email) {
        //create a Charge object
        charge = new Charge();

        //set the card to charge
        charge.setCard(card);

        charge.setEmail(email); //dummy email address

        charge.setAmount(amount); //test amount

        PaystackSdk.chargeCard(this, charge, new Paystack.TransactionCallback() {
            @Override
            public void onSuccess(Transaction transaction) {
                // This is called only after transaction is deemed successful.
                // Retrieve the transaction, and send its reference to your server
                // for verification.
                String paymentReference = transaction.getReference();
            }

            @Override
            public void beforeValidate(Transaction transaction) {
                // This is called only before requesting OTP.
                // Save reference so you may send to server. If
                // error occurs with OTP, you should still verify on server.
            }

            @Override
            public void onError(Throwable error, Transaction transaction) {
                //handle error here
            }
        });
    }
}
