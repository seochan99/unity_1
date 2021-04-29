using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public GameManager gameManager;
    public float maxSpeed;// 최대속도 설정
    public float jumpPower; // 점프파워 
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    CircleCollider2D CircleCollider;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        CircleCollider = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        //Jump
        if (Input.GetButtonDown("Jump")&& !anim.GetBool("isJumping"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse); // 이르케 점프한다 !
            anim.SetBool("isJumping", true);
        }
        // Stop Speed 
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);//float곱할때는 f붙여줘야한다.

        }

        // change Direction
        if(Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        // work animation
        if (Mathf.Abs( rigid.velocity.x)< 0.3) //절댓값 설정 
            anim.SetBool("isWorking", false);
        else
            anim.SetBool("isWorking", true);

    }


    void FixedUpdate()
    {
        // Move by Control
        float h = Input.GetAxisRaw("Horizontal"); // 횡으로 키를 누르면 
        rigid.AddForce(Vector2.right*h,ForceMode2D.Impulse); // 이르케 이동한다 !


        // MaxSpeed Limit
        if (rigid.velocity.x > maxSpeed)// right
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1)) // Left Maxspeed
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);

        // Lending Platform
        if(rigid.velocity.y < 0)
        {
            //Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0)); //에디터 상에서만 레이를 그려준다
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null) // 바닥 감지를 위해서 레이저를 쏜다! 
            {
                if (rayHit.distance < 0.5f)
                {
                    anim.SetBool("isJumping", false);
                }
            }
        }

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag =="Enemy")
        {
            // Attack
            if(rigid.velocity.y<0 && transform.position.y > collision.transform.position.y)
            {
                OnAttack(collision.transform);
            }
            else // damaged
            {
                OnDamaged(collision.transform.position);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag =="item")
        {
            //point
            bool isBronze = collision.gameObject.name.Contains("item");
            bool isSilver = collision.gameObject.name.Contains("silver");
            bool isGold = collision.gameObject.name.Contains("gold");

            if (isBronze)
                gameManager.stagePoint += 50;
            else if (isSilver)
                gameManager.stagePoint += 100;
            else if (isGold)
                gameManager.stagePoint += 200;

            //Deactive item
            collision.gameObject.SetActive(false);
        }
        else if(collision.gameObject.tag == "Finish")
        {
            // next stage
            gameManager.NextStage();
        }
    }

    void OnAttack(Transform enemy)
    {
        // point
        gameManager.stagePoint += 100;
        //jump reaction
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        //Enemy Die
        Enemy enemyMove = enemy.GetComponent<Enemy>();
        enemyMove.OnDamaged();

    }

    void OnDamaged(Vector2 targetPos)
    {
        gameObject.layer = 9;

        //health
        gameManager.HealthDown();

        //view alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // Reaction Force
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1)*7, ForceMode2D.Impulse);

        // Animation
        anim.SetTrigger("doDamaged");

        Invoke("OffDamaged", 3);
    }

    void OffDamaged()
    {
        gameObject.layer = 8;
        spriteRenderer.color = new Color(1, 1, 1, 1);

    }
    public void Ondie()
    {
        //Sprite Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        //Sprite FLip Y
        spriteRenderer.flipY = true;
        //Collider Disable
        CircleCollider.enabled = false;
        //Die Effect Jump
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
    }

    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }
}
