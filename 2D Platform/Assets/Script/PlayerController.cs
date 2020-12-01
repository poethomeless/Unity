using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float runSpeed;
    public float jumpSpeed;
    public float doubleJumpSpeed;
    public float restoreTime;

    private Rigidbody2D myRigidbody;
    private Animator myAnim;
    private BoxCollider2D myFeet;
    private bool isGround;
    private bool canDoubleJump;
    private bool isOneWayPlatform;
     void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        myFeet = gameObject.GetComponent<BoxCollider2D>();
        //Debug.Log("1>>>>>>>>>>>>>>>>>>>>>>>>>>"+myFeet);
    }

     void Update()
    {
        if (GameController.isGameAlive)
        {
            Flip();
            Run();
            Jump();
            SwitchAnimation();
            CheckGrounded();
            //Attack();
            OneWayPlatform();
            Courch();
        }
        else
        {
            myAnim.SetBool("Jump", false);
            myAnim.SetBool("DoubleJump", false);
            //myAnim.SetBool("Attack", false);
            myAnim.SetBool("Fall", false);
            myAnim.SetBool("DoubleFall", false);
            myAnim.SetBool("Idle", false);
            myAnim.SetBool("Run", false);
            myAnim.SetBool("Courch", false);
        }

    }
    /// <summary>
    /// 检测是否接触地面
    /// </summary>
    void CheckGrounded()
    {
        isGround = myFeet.IsTouchingLayers(LayerMask.GetMask("Ground")) ||
                   myFeet.IsTouchingLayers(LayerMask.GetMask("MovingPlatform")) ||
                   myFeet.IsTouchingLayers(LayerMask.GetMask("OneWayPlatform"));
        isOneWayPlatform = myFeet.IsTouchingLayers(LayerMask.GetMask("OneWayPlatform"));
        Debug.Log(isGround);
    }
    /// <summary>
    /// 翻转部件
    /// </summary>
    void Flip()
    {
        bool playerHasXAxisSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        //如果X轴有正向速度不翻转，反之，则翻转
        if (playerHasXAxisSpeed) 
        {
            if (myRigidbody.velocity.x > 0.1f)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            if (myRigidbody.velocity.x < -0.1f)
            {
                //部件绕Y轴旋转180°
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }
    void Run() 
    {
        float moveDir = Input.GetAxis("Horizontal");
        Vector2 playerVel = new Vector2(moveDir * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVel;
        bool playerHasXAxisSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnim.SetBool("Run",playerHasXAxisSpeed);
    }
    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGround)
            {
                Vector2 junmpVel = new Vector2(0.0f, jumpSpeed);
                myRigidbody.velocity = Vector2.up * junmpVel;
                myAnim.SetBool("Jump", true);
                myAnim.SetBool("DoubleJump", false);
                canDoubleJump = true;
            }
            else
            {
                if (canDoubleJump)
                {
                    Vector2 doubleJumpVel = new Vector2(0.0f, doubleJumpSpeed);
                    myRigidbody.velocity = Vector2.up * doubleJumpVel;
                    myAnim.SetBool("DoubleJump", true);
                    myAnim.SetBool("Jump", false);
                    canDoubleJump = false;
                }
            }
        }
    }

    //void Attack()
    //{
    //    if (Input.GetButtonDown("Attack"))
    //    {
    //        StartCoroutine(DoAttack());
    //    }
    //}

    //IEnumerator DoAttack()
    //{
    //    myAnim.SetBool("Attack", true);
    //    yield return new WaitForSeconds(0.25f);
    //    myAnim.SetBool("Attack", false);
    //}

    void SwitchAnimation()
    {
        myAnim.SetBool("Idle", false);
        if (myAnim.GetBool("Jump"))
        {
            if (myRigidbody.velocity.y < 0.0f)
            {
                 myAnim.SetBool("Fall", true);
                 myAnim.SetBool("Jump", false);
            }
        }
        else if (isGround) 
        {
            myAnim.SetBool("Fall", false);
            myAnim.SetBool("Idle", true);
        }

        if (myAnim.GetBool("DoubleJump"))
        {
            if (myRigidbody.velocity.y < 0.0f)
            {
               myAnim.SetBool("DoubleFall", true);
               myAnim.SetBool("DoubleJump", false);
            }
        }
        else if (isGround)
        {
            myAnim.SetBool("DoubleFall", false);
            myAnim.SetBool("Idle", true);
        }
    }

    void Courch()
    {
        if (myAnim.GetBool("Idle")) 
        {
            if (Input.GetAxis("Vertical") < -0.01f)
            {
                myAnim.SetBool("Idle",false);
                myAnim.SetBool("Courch", true);
            }
        }
        
    }
    void OneWayPlatform()
    {
        if (isOneWayPlatform) 
        {
            if (Input.GetButtonDown("Jump") && Input.GetKey(KeyCode.S))
            {
                gameObject.layer = LayerMask.NameToLayer("OneWayPlatform");
                Invoke("RestorePlayerLayer", restoreTime);
            }
            
        }
    }

    void RestorePlayerLayer()
    {
        if (!isGround && gameObject.layer != LayerMask.NameToLayer("Player")) 
        {
            gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }
}
