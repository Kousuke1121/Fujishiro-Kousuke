using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject Pre_Attacker;
    private EnemyManager manager;
    private EnemyRoute route;

    void Start()
    {
        manager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();        
        route = GameObject.Find("EnemyManager").GetComponent<EnemyRoute>();        
    }

    void Update()
    {
        if (manager.SpawnFlag)
        {
            Create_Pre();            
        }
    }

    public void Create_Pre()
    {
        manager.SpawnFlag = false;
        GameObject NewEnemy = Instantiate(Pre_Attacker) as GameObject;
        Enemy enemy = NewEnemy.GetComponent<Enemy>();
        for(int i =0; i <= route.Routes.Count; i++)
        {
            if(manager.Spawn_No == i)
            {
                NewEnemy.transform.position = route.Routes[manager.Spawn_No].transform.GetChild(0).position;

            }
        }
        enemy.RouteNo = manager.Spawn_No;
        NewEnemy.transform.parent = manager.parent.transform;

        manager.enemies.Add(NewEnemy.GetComponent<Enemy>());
    }
}
