using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ControllerCheck : MonoBehaviour
{
    GameObject Event;

    GameObject Left;
    GameObject Right;
    GameObject Warp;
    GameObject Button;

    [SerializeField] float second = 11f;

    void Awake()
    {
        Left = GameObject.Find("LeftHandAnchor");
        Right = GameObject.Find("RightHandAnchor");
        Warp = GameObject.Find("WarpManager");
        Button = GameObject.Find("ButtonCheck");
        Event = GameObject.Find("TL_StartEvent");
    }
    private void Start()
    {
        Controller_Off();
        StartCoroutine(Delay(second, () =>
        {
            Controller_On();
        }));
    }

    void Update()
    {
        /*if (Event.activeSelf == true)
        {
            Controller_Off();
        }
        else
        {
            Controller_On();
        }*/
    }

    void Controller_Off()
    {
        //Debug.Log("コントローラー停止");
        Left.SetActive(false);
        Right.SetActive(false);
        Warp.SetActive(false);
        Button.SetActive(false);
    }

    void Controller_On()
    {
        //Debug.Log("コントローラー起動");
        Left.SetActive(true);
        Right.SetActive(true);
        Warp.SetActive(true);
        Button.SetActive(true);
    }

    private IEnumerator Delay(float seconds, UnityAction action)
    {
        yield return new WaitForSeconds(seconds);
        action?.Invoke();
    }
}
