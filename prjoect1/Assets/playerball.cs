using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerball : MonoBehaviour
{
    public float JumpPower = 10; // 
    public int itemCount;
    bool isJump;
    public GameManage manager; // 이를 통해서 find사용을 자제해서 메모리사용량을 더욱 낮춘다 ! 

    Rigidbody rigid;
    AudioSource audio;

    void Awake()
    {
        isJump = false;
        rigid = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump")&& !isJump)
        {
           isJump = true;
           rigid.AddForce(new Vector3(0, JumpPower, 0), ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        rigid.AddForce(new Vector3(v, 0, -h),ForceMode.Impulse);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
            isJump = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "item")
        {
            itemCount++;
            audio.Play();
            other.gameObject.SetActive(false);
            manager.GetItem(itemCount);
        }
        else if (other.tag == "finish")
        {     
            if(itemCount == manager.totalItemCount)
            {
                SceneManager.LoadScene("Example1_"+(manager.stage+1).ToString());
                //Game Clear 
            }
            else
            {
                // ReStart
                SceneManager.LoadScene("Example1");
            }
        }
    }
}
