using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Attack attack;
    private EnemyRoute Route;

    [System.NonSerialized] public bool order = false;

    [System.NonSerialized] public bool DesthFlag = false;

    public int RouteNo = -1;

    void Start()
    {
        attack = GetComponent<Attack>();
        Route = GameObject.Find("EnemyManager").GetComponent<EnemyRoute>();
    }

    void Update()
    {
        if(RouteNo >=0)
        {
            //ÉÇÅ[ÉhêÿÇËë÷Ç¶
            switch (attack.Mode)
            {
                case 0:
                    RouteCheck();
                    break;
                case 1:
                    attack.ChaseMode();
                    break;
            }
        }

        if (order)
        {
            attack.order = true;
            attack.Mode = 1;
        }
        else
        {
            attack.order = false;
        }

        if (attack.Mode == 1 && Input.GetKeyDown(KeyCode.F2))
        {
            DesthFlag = true;
        }
    }

    private void RouteCheck()
    {
        for(int i = 0; i <= Route.Routes.Count; i++)
        {
            if(RouteNo == i)
            {
                attack.WalkMode(Route.Routes[RouteNo]);

            }
        }
    }
}
