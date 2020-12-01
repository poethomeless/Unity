using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public int health;
    public int damage;
    public float flashTime;
    public GameObject bloodEffect;

    private SpriteRenderer sr;
    private Color originalColor;
    private PlayerHealth playerHealth;
    // Start is called before the first frame update
    public void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    // Update is called once per frame
    public void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        FlashColor(flashTime);
        //产生血液特效
        Instantiate(bloodEffect, transform.position, Quaternion.identity);
        //镜头震动
        GameController.camShake.Shake();
    }

    void FlashColor(float time)
    {
        sr.color = Color.red;
        Invoke("ResetColor", time);
    }

    void ResetColor()
    {
        sr.color = originalColor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //碰撞体标签是“Player” 且类型是 “UnityEngine.CapsuleCollider2D”
        if (collision.gameObject.CompareTag("Player") && collision.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            if (playerHealth != null) 
            {
                playerHealth.DamagePlayer(damage);
            }
            
        }
        
    }
}
