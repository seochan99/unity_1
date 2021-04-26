using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class p1 : MonoBehaviour
{
    Rigidbody rigid;

    void start()
    {
        rigid = GetComponent<Rigidbody>();
    }


    void FixedUpdate ()
    {
        if(Input.GetButtonDown("Jump")){
            rigid.AddForce(Vector3.up*25,ForceMode.Impulse);
        }
        Vector3 vec = new Vector3(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
        rigid.AddForce(vec,ForceMode.Impuse);
    }
}