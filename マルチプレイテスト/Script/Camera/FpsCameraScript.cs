using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsCameraScript : Photon.Pun.MonoBehaviourPun
{
    public GameObject target;
    public float distance;
    public float distanceY;

    [SerializeField] private float Speed = 1f;
    [SerializeField] private float sensitivity_X = 1f;
    [SerializeField] private float sensitivity_Y = 1f;
    [SerializeField] private float Jump = 0.01f;

    bool flag = false;
    int Mode = 0;

    public void SetFlag(bool f)
    {
        flag = f;
    }

    public void SetMode(int i)
    {
        Mode = i;
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        if (!flag) { return; }
        if (!photonView.IsMine) { return; }

        
        switch (Mode)
        {
            case 0://ƒoƒgƒ‹
                //if (Input.GetMouseButtonDown(2))
                //{
                    TargetCamera();
                //}
                break;
            case 1:
                MouseCamera();
                Move();
                break;
        }        

        if (Input.GetKey(KeyCode.F1))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetKey(KeyCode.F2))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void Move()
    {
        float x = Input.GetAxis("Horizontal") * Speed;
        float z = Input.GetAxis("Vertical") * Speed;

        if (Input.GetKey(KeyCode.Space))
        {
            transform.position += new Vector3(0, Jump, 0);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.position += new Vector3(0, -Jump, 0);
        }

        transform.Translate(0.01f * x, 0f, 0.01f * z);
    }

    private void MouseCamera()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector3 newRotation = transform.localEulerAngles;
        newRotation.y += mouseX * sensitivity_X;
        newRotation.x += mouseY * -sensitivity_Y;
        transform.localEulerAngles = newRotation;
    }

    private void TargetCamera()
    {
        if (target == null) { return; }
        Vector3 p = target.transform.position;
        transform.position = p + target.transform.forward * distance * -1 + Vector3.up * distanceY;
        p.y = 1f;
        transform.LookAt(p);
    }
}
