using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikePhysics : MonoBehaviour
{
    public Rigidbody2D rb;
    public static float speed;

    void Start()
    {
        speed = Screen.height * 1.56f;
		int side = Random.Range(0, 2);
		if(side == 1)
			speed *= -1;
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(new Vector2(speed, 0));

        if (!GameCode.running)
        {
             Destroy(this.gameObject);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag.Equals("Finish"))
        {
            speed *= -1;
           // Debug.Log("Collided");
        }
    }
}
