using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationDialog : MonoBehaviour
{
    public Text displayText;
    public Button yesBtn, noBtn;
    void Start()
    {
        noBtn.onClick.AddListener(() => {
            var confirmDialig = GameObject.FindGameObjectWithTag("ConfirmDialog");
            this.gameObject.SetActive(false);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
