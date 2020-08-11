using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCode : MonoBehaviour
{
    public Rigidbody2D rb;

    void Start()
    {
        transform.localScale = new Vector3(Screen.height / 1280f, Screen.height / 1280f, 0);
    }

    void Update()
    {

        if (rb.transform.position.y > Screen.height)
        {
            Destroy(this.gameObject);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameCode.life < 3 && collision.collider.tag.Equals("Player"))
        {
            GameCode.life++;
            FindObjectOfType<UI>().updateLife();
            FindObjectOfType<GameCode>().playSound(GameCode.Sound.Life); 
            Destroy(this.gameObject);
        }
    }
}
