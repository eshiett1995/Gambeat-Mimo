using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPhysics : MonoBehaviour
{
    public Rigidbody2D rb;
    public RawImage icon;
    private float horizontalSpeed;
    private bool isMoving,isDying;
    private float lastUpdate;
    public static float X, Y;
    public static int invincible;
    Collision2D col;


    void Start()
    {
        horizontalSpeed = Screen.height / 2.4f;
        transform.localScale = new Vector3(Screen.height / 2000f, Screen.height / 2000f, 0);
        rb.gravityScale = 0f;
    }

    void Update()
    {
        //Debug.Log("Position X: " + X + " Y: " + Y);
        X = rb.transform.position.x;
        Y = rb.transform.position.y;

        if (invincible == 0)
        {
            if (col != null)
            {
                OnCollisionEnter2D(col);
            }
        }

#if UNITY_EDITOR
        if (Input.anyKey)
            disableTutorials();
#elif UNITY_ANDROID
        if (Input.touchCount > 0)
        {
           disableTutorials();
        }
#endif

        if (GameCode.running)
        {
            rb.gravityScale = Screen.height / 9f;
#if UNITY_EDITOR
            setDesktopInput();
#elif UNITY_ANDROID
            setMobileInput();
#endif

        }

        if (isDying) {

            //Debug.Log("Time Start:" + lastUpdate + "Current Time:" + Time.time);
            
            if (Time.time - lastUpdate >= 1f)
                 finalizeDeath();

            rb.velocity = Vector3.zero;
        }

        if (!isMoving)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    void disableTutorials()
    {
        if (Multiplayer.ui != null)
        {
            if (Multiplayer.ui.tutorialPanel.activeSelf)
            {
                GameCode.running = true;
                Multiplayer.ui.tutorialPanel.SetActive(false);
            }
        }
    }

    private void setDesktopInput()
    {
        
        if (Input.GetKey("a"))
        {
            isMoving = true;
            rb.velocity = new Vector3(-horizontalSpeed, rb.velocity.y, 0);
        }
        if (Input.GetKey("d"))
        {
            isMoving = true;
            rb.velocity = new Vector3(horizontalSpeed, rb.velocity.y, 0);
        }
        if (Input.GetKeyUp("a") || Input.GetKeyUp("d"))
        {
            isMoving = false;
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    private void setMobileInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended)
            {
                isMoving = false;
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
            }
            else
            {
                if (touch.position.x < (Screen.width / 2))
                {
                    isMoving = true;
                    rb.velocity = new Vector3(-horizontalSpeed, rb.velocity.y, 0);
                }
                if (touch.position.x > (Screen.width / 2))
                {
                    isMoving = true;
                    rb.velocity = new Vector3(horizontalSpeed, rb.velocity.y, 0);
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision != null && collision.collider!=null)
        {
            if (collision.collider.tag.Equals("Platform"))
            {
                // Debug.Log("Blink ma nigga");
                if (!isDying)
                    icon.GetComponent<Animator>().Play("blink");
            }
            if (collision.collider.tag.Equals("Enemy") && invincible <= 0)
            {
                die();
            }
            if (invincible > 0)
                col = collision;
        }
    }

    void die()
    {
        
        if (!isDying)
        {
            lastUpdate = Time.time;
            GameCode.life--;
            if (GameCode.life <= 0 && Multiplayer.oppLife > 0)
                Multiplayer.winner = false;

                // Debug.Log("Time Start:" + lastUpdate);
                icon.GetComponent<Animator>().Play("poof");
            isDying = true;
            rb.gravityScale = 0f;
            gameObject.layer = 8;
           // Debug.Log("Layer:"+gameObject.layer.ToString());
            // Debug.Log("Die");
        }
    }

    private void finalizeDeath()
    {
        //Debug.Log("Time Start:" + lastUpdate + " Current Time:" + Time.time);

        isDying = false;
        rb.gravityScale = Screen.height / 9f;
        gameObject.layer = 9;
        //Debug.Log("Layer:" + gameObject.layer.ToString());
        
        FindObjectOfType<UI>().updateLife();

        if (GameCode.life > 0)
        {
            //  FindObjectOfType<GameCode>().playSound(GameCode.Sound.Die);
            respawn();
        }
        else
            FindObjectOfType<GameCode>().gameOver();

    }

    public void respawn()
    {

        gameObject.layer = 9;
        invincible = 3;

        float dead = (float)(Screen.height / 1.25);
		
		  if (PlatformPhysics.lastPlatform == null || PlatformPhysics.lastPlatform.transform.position.y >= Screen.height / 4)
        {
            float randPos = UnityEngine.Random.Range((float)(Screen.width / 8.5), (float)(Screen.width / 1.18));

            Instantiate(FindObjectOfType<GameCode>().platform, new Vector3(randPos, 0, 0), Quaternion.identity, GameObject.FindGameObjectWithTag("Panel").transform);
          
            Debug.Log("Calling Backup");

        }

        if (PlatformPhysics.respawnPlatform != null)
        {
            if (PlatformPhysics.respawnPlatform.position.y < dead)
                Instantiate(transform.gameObject, new Vector3(PlatformPhysics.respawnPlatform.position.x, PlatformPhysics.respawnPlatform.position.y + transform.localScale.y, 0), Quaternion.identity, GameObject.FindGameObjectWithTag("Panel").transform);
            else
                Instantiate(transform.gameObject, new Vector3((Screen.width / 2), (float)(Screen.height / 1.28), 0), Quaternion.identity, GameObject.FindGameObjectWithTag("Panel").transform);
        }
        else
            Instantiate(transform.gameObject, new Vector3((Screen.width / 2), (float)(Screen.height / 1.28), 0), Quaternion.identity, GameObject.FindGameObjectWithTag("Panel").transform);

        Destroy(this.gameObject);
    }

   
}
