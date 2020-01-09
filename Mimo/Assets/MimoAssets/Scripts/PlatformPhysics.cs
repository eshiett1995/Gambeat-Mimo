using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPhysics : MonoBehaviour
{
    public Rigidbody2D rb;
    public static float speed;
    public bool used;
    public static Transform respawnPlatform,lastPlatform;
    public static int spawnCount;
    public bool isSpike;

    void Start()
    {
        transform.localScale = new Vector3(Screen.height / 1280f, Screen.height / 1280f, 0);
        lastPlatform = rb.transform;
    }

    void Update()
    {
        if (GameCode.running)
        {
            rb.velocity = new Vector3(rb.velocity.x, speed, 0);
            float y = transform.position.y;
            //Debug.Log("Y: " + y + " Spawn: " + spawnCount);

            if ((y >= Screen.height / 6) && !used)
            {
                if(!isSpike)
                     respawnPlatform = rb.transform;
                used = true;
                // Debug.Log("Respawn Plate Set");

                if (Multiplayer.connection == Multiplayer.Connection.Offline)
                    FindObjectOfType<GameCode>().spawnPlatforms();
                else
                    FindObjectOfType<GameCode>().spawnMultiplayerPlatforms(0);

               // Debug.Log("Y: "+y+" Spawn: " + spawnCount);
                spawnCount++;
            }


            if (rb.transform.position.y > Screen.height)
            {
                Destroy(this.gameObject);
            }
        }
        else
            rb.velocity = Vector2.zero;
    }
}
