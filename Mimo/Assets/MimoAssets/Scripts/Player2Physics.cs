using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player2Physics : MonoBehaviour
{
    public Rigidbody2D rb;
    public RawImage icon;
    public int life;
    public static float X, Y;


    void Start()
    {
        transform.localScale = new Vector3(Screen.height / 2000f, Screen.height / 2000f, 0);
       
        Debug.Log("Player 2 Spawned");
    }

    void Update()
    {
        gameObject.layer = 8;
        rb.gravityScale = Screen.height / 9f;
        rb.mass = 1;

        rb.transform.position = new Vector3(X, Y, 0);

    }
  
   
}
