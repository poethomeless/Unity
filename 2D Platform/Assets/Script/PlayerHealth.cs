using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int numBlinks;
    public float seconds;
    public float dieTime;
    public float HitBoxCdTime;

    private Animator anim;
    private Renderer myRender;
    private ScreenFlash sf;
    private Rigidbody2D rb2d;
    private PolygonCollider2D pc2d;

    // Start is called before the first frame update
    void Start()
    {
        HealthBar.HealthMax = health;
        HealthBar.HealthCurrent = health;
        myRender = gameObject.GetComponent<Renderer>();
        anim = gameObject.GetComponent<Animator>();
        sf = GetComponent<ScreenFlash>();
        rb2d = GetComponent<Rigidbody2D>();
        pc2d = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamagePlayer(int damage)
    {
        if (GameController.isGameAlive) 
        {
            sf.FlashScreen();
            health -= damage;
            if (health < 0)
            {
                health = 0;
            }
            HealthBar.HealthCurrent = health;
            if (health <= 0)
            {
                rb2d.velocity = new Vector2(0, 0);
                //重力为0，不掉下来
                //rb2d.gravityScale = 0.0f;
                GameController.isGameAlive = false;
                anim.SetTrigger("Die");
                Invoke("KillPlayer", dieTime);
            }
            BlinkPlayer(numBlinks, seconds);
            //关闭PolygonCollider2D碰撞，使地刺先造成一次伤害
            pc2d.enabled = false;
            StartCoroutine(ShowPlayerHitBox());
        }
        
    }
    IEnumerator ShowPlayerHitBox()
    {
        yield return new WaitForSeconds(HitBoxCdTime);
        //等待HitBoxCdTime后地刺可以再次造成伤害
        pc2d.enabled = true;
    }
    void KillPlayer()
    {
        //Destroy(gameObject);
    }
    void BlinkPlayer(int numBlinks, float seconds)
    {
        StartCoroutine(DoBlink(numBlinks, seconds));
    }

    IEnumerator DoBlink(int numBlinks, float seconds)
    {
        for (int i = 0; i < numBlinks * 2; i++) 
        {
            myRender.enabled = !myRender.enabled;
            yield return new WaitForSeconds(seconds);
        }
        myRender.enabled = true;
    }
}
