using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public bool isTouchTop;
    public bool isTouchBottom;
    public bool isTouchRight;
    public bool isTouchLeft;
    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if ((isTouchRight && h == 1) || ((isTouchLeft && h == -1)))
            h = 0;

        float v = Input.GetAxisRaw("Vertical");
        if ((isTouchTop && h == 1) || ((isTouchBottom && h == -1)))
            v = 0;
        Vector3 curPos = transform.position; //ㅎㅕㄴ재 위치
        Vector3 nextPos = new Vector3(h, v, 0)*speed*Time.deltaTime; //transform이동시 델타타임 꼭 곱해주기.
        transform.position = curPos + nextPos; //현재위치에 다음 위치 더해주기 -> 이동한다.

        if(Input.GetButtonDown("Horizontal")|| Input.GetButtonUp("Horizontal"))
        {
            anim.SetInteger("Input", (int)h);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = true;
                    break;
                case "Bottom":
                    isTouchBottom = true;
                    break;
                case "Right":
                    isTouchRight = true;
                    break;
                case "Left":
                    isTouchLeft = true;
                    break;
            }
        }
        
    }
     void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = false;
                    break;
                case "Bottom":
                    isTouchBottom = false;
                    break;
                case "Right":
                    isTouchRight = false;
                    break;
                case "Left":
                    isTouchLeft = false;
                    break;
            }
        }
    }
}
