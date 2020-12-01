using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int damage;
    public float time;
    public float startTime;
    private Animator anim;
    private PolygonCollider2D coll2D;
    // Start is called before the first frame update
    void Start()
    {
        anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        coll2D = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.isGameAlive) 
        {
            Attack();
        }
            
    }

    void Attack()
    {
        coll2D.enabled = false;
        if (Input.GetButtonDown("Attack"))
        {
            anim.SetTrigger("Attack");
            StartCoroutine(StartAttack());
        }
    }

    IEnumerator StartAttack()
    {
        yield return new WaitForSeconds(startTime);
        coll2D.enabled = true;
        StartCoroutine(DisableHitBox());
    }

    IEnumerator DisableHitBox()
    {
        yield return new WaitForSeconds(time);
        coll2D.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyBat"))
        {
            collision.GetComponent<Enemy>().TakeDamage(damage);
        }
    }
}
