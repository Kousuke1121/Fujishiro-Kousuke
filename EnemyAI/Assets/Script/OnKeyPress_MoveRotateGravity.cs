using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnKeyPress_MoveRotateGravity : MonoBehaviour
{
    public float normalspeed = 3;
    public float speed;
    public float dashspeed = 5;
    public float rotateSpeed = 90;
    public float jumppower = 6;

    float vz = 0;
    float angle = 0;
    bool pushFlag = false;
    bool jumpFlag = false;
    bool groundFlag = false;
    Rigidbody rbody;

    void Start()
    {
        rbody = this.GetComponent<Rigidbody>();
        rbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY|RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            speed = dashspeed;
        }
        else
        {
            speed = normalspeed;
        }
        angle = Input.GetAxisRaw("Horizontal") * rotateSpeed;
        vz = Input.GetAxisRaw("Vertical") * speed;

        if(Input.GetKey("space") && groundFlag)
        {
            if(pushFlag == false)
            {
                pushFlag = true;
                jumpFlag = true;
            }
        }
        else
        {
            pushFlag = false;
        }

       
    }
    private void FixedUpdate()
    {
        if (vz != 0)
        {
            this.transform.Translate(0, 0, vz / 50);
        }
        if (angle != 0)
        {
            this.transform.Rotate(0, angle / 50, 0);
        }

        if (jumpFlag)
        {
            jumpFlag = false;
            rbody.AddForce(new Vector3(0, jumppower, 0), ForceMode.Impulse);
        }
    }
    private void OnTriggerStay(Collider collision)
    {
        groundFlag = true;
    }
    private void OnTriggerExit(Collider collision)
    {
        groundFlag = false;
    }
}
