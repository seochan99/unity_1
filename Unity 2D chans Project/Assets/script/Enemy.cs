using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    Rigidbody2D rigid;
    public int nextMove; // 행동 지표를 결정할 변수 하나 생성
    Animator anim;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        Invoke("Think", 3); //주어진 시간이 지난뒤 지정된 함수를 실행하는 함수 
    }

    void FixedUpdate()
    {
        rigid.velocity = new Vector2(nextMove,rigid.velocity.y);

        //지형 체크
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove*0.4f,rigid.position.y);

        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0)); //에디터 상에서만 레이를 그려준다
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null) // 바닥 감지를 위해서 레이저를 쏜다! 없다면!!! 바닥이
        {
            Turn();
        }
    }

    void Think()
    {
        nextMove = Random.Range(-2, 3);


        //애
        anim.SetInteger("WalkSpeed", nextMove);

        // 방향
        if(nextMove != 0)//안서있을때만 방향 바꾸게 설정
            spriteRenderer.flipX = nextMove == 1; // 1이면 실행

        //재귀함수 
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);
    }
    void Turn()
    {
        nextMove = nextMove * -1;
        spriteRenderer.flipX = nextMove == 1; // 1이면 실행

        CancelInvoke();
        Invoke("Think", 5);
    }
}
