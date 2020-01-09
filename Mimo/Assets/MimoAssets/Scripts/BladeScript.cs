using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeScript : MonoBehaviour
{
    public Rigidbody2D rb;
    void Start()
    {
        transform.localScale = new Vector3(Screen.height / 1280f, Screen.height / 1280f, 0);
        transform.position = new Vector3(-Screen.width/2.6f, transform.position.y, 0);
        rb.gravityScale = 0f;

        int side = Random.Range(0, 2);
        
        if (side == 1)
        {
            transform.position = new Vector3(Screen.width/1.3f, transform.position.y, 0);
        }
        Debug.Log("Blade Spawned at side " + side);

    }

    // Update is called once per frame
    void Update()
    {
        if (GameCode.running)
        {
            rb.velocity = new Vector3(rb.velocity.x, PlatformPhysics.speed, 0);
        }

        if (!GameCode.running || rb.transform.position.y > Screen.height*1.5f)
        {
            Destroy(this.gameObject);
        }

    }
}
