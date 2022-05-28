using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCheck : MonoBehaviour
{
    [SerializeField] GameObject Hand;
    OnlyHandWarp Hand_Script;

    [SerializeField] GameObject gun;
    hassya1 gun_Script;

    public static bool controllerCheck = false;

    [SerializeField] float buttonTime = 0.2f;

    float time = 0;
    void Awake()
    {
        Hand_Script = Hand.GetComponent<OnlyHandWarp>();

        gun_Script = gun.GetComponent<hassya1>();

        Hand_Script.NewHand();

        Controller_Check();
    }

    void Start()
    {
        //Debug.Log("ハンドオブジェクト："+Hand_Script.hand.handObject);
    }

    void Update()
    {
        Button_Check();
    }

    void Button_Check()
    {
        if (controllerCheck)
        {
            if (OVRInput.Get(OVRInput.RawButton.LHandTrigger))
            {
                time += Time.deltaTime;
            }

            if (!OVRInput.Get(OVRInput.RawButton.LHandTrigger) && Hand_Script.warpCheck == false)
            {
                time = 0;
            }
        }
        else
        {
            if (OVRInput.Get(OVRInput.RawButton.RHandTrigger))
            {
                time += Time.deltaTime;
            }

            if (!OVRInput.Get(OVRInput.RawButton.RHandTrigger) && Hand_Script.warpCheck == false)
            {
                time = 0;
            }
        }

        if (time >= buttonTime)
        {
            Hand_Script.hand.handObject.SetActive(true);
            Hand.SetActive(true);
            gun_Script.Player.SetActive(false);
        }
        if (time <= buttonTime)
        {
            Hand_Script.hand.handObject.SetActive(false);
            Hand.SetActive(false);
            Hand_Script.warpParticle.SetActive(false);
            Hand_Script.hpObject.SetActive(false);
            gun_Script.Player.SetActive(true);
        }
    }

    void Controller_Check()
    {
        if (controllerCheck)
        {
            Hand_Script.hand.handObject = Hand_Script.Lefthand;
            gun_Script.Player.transform.parent = gun_Script.Left.transform;

            Hand_Script.controllerCheck = true;
            gun_Script.controllerCheck = true;

            Hand_Script.Righthand.SetActive(false);
        }
        else
        {
            Hand_Script.hand.handObject = Hand_Script.Righthand;
            gun_Script.Player.transform.parent = gun_Script.Right.transform;

            Hand_Script.controllerCheck = false;
            gun_Script.controllerCheck = false;

            Hand_Script.Lefthand.SetActive(false);
        }

        Hand_Script.warpPoint[0].transform.parent = Hand_Script.hand.handObject.transform;
        Hand_Script.warpPoint[1].transform.parent = Hand_Script.hand.handObject.transform;
    }
}
