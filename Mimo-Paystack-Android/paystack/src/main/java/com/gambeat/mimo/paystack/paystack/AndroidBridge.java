package com.gambeat.mimo.paystack.paystack;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.widget.Toast;

import com.gambeat.mimo.paystack.paystack.activity.PaystackActivity;

import co.paystack.android.Paystack;
import co.paystack.android.PaystackSdk;
import co.paystack.android.Transaction;
import co.paystack.android.model.Card;
import co.paystack.android.model.Charge;


public class AndroidBridge {
    private static Card card;
    private static Charge charge;
    private static Context context;

    public static void initPaystack(Context context){
        Toast.makeText(context, "it is hiting pay", Toast.LENGTH_LONG).show();
        Intent intent = new Intent(context, PaystackActivity.class);
        context.startActivity(intent);
        AndroidBridge.context = context;
//        PaystackSdk.initialize(context);
//        PaystackSdk.setPublicKey("pk_test_3a7af4a93d785ab7fb183ee27eeae4be3755340e");
    }

    public static void toast(Context context){
        Toast.makeText(context, "this is it", Toast.LENGTH_LONG).show();
    }

    public void initPayment(int amount, String email, String cardNumber, int expiryMonth, int expiryYear, String  cvv ){
        card = new Card(cardNumber, expiryMonth, expiryYear, cvv);

        if (card.isValid()) {
            Toast.makeText(AndroidBridge.context, "Card is Valid", Toast.LENGTH_LONG).show();
            performCharge(amount, email);
        } else {
            Toast.makeText(AndroidBridge.context, "Card not Valid", Toast.LENGTH_LONG).show();
        }
    }

    private void performCharge(int amount, String email) {
        //create a Charge object
        charge = new Charge();

        //set the card to charge
        charge.setCard(card);

        charge.setEmail(email); //dummy email address

        charge.setAmount(amount); //test amount

        PaystackSdk.chargeCard((Activity)AndroidBridge.context, charge, new Paystack.TransactionCallback() {
            @Override
            public void onSuccess(Transaction transaction) {
                // This is called only after transaction is deemed successful.
                // Retrieve the transaction, and send its reference to your server
                // for verification.
                String paymentReference = transaction.getReference();
                Toast.makeText(AndroidBridge.context, "Transaction Successful! payment reference: "
                        + paymentReference, Toast.LENGTH_LONG).show();
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
