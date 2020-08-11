using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreakablePlatformPhysics : MonoBehaviour
{
    private float lastUpdate;
    public RawImage icon;
    private bool breaking;

    void Update()
    {
        if(breaking)
            if (Time.time - lastUpdate >= 1f)
                Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag.Equals("Player") && !breaking)
        {
            icon.GetComponent<Animator>().Play("break");
            FindObjectOfType<GameCode>().playSound(GameCode.Sound.Break);
            breaking = true;

            lastUpdate = Time.time;
        }
    }
}
