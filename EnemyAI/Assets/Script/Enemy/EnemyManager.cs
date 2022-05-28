using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [System.NonSerialized] public GameObject parent;
    public List<Enemy> enemies;
    [SerializeField] private Searcher searchers;
    private EnemyRoute route;

    [System.NonSerialized] public bool SpawnFlag = false;
    [System.NonSerialized] public bool SpawnFlag_2 = false;
    [System.NonSerialized] public int Spawn_No;

    void Start()
    {
        parent = GameObject.Find("Attackers");
        route = gameObject.GetComponent<EnemyRoute>();
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            enemies.Add(parent.transform.GetChild(i).GetComponent<Enemy>());
        }
        //Debug.Log(enemies);
    }

    void Update()
    {
        Search_Order();

        if (Input.GetKeyDown(KeyCode.F1))
        {
            SpawnEnemy_Route();   
        }

        Enemy_Desth();
    }

    /// <summary>
    /// スポーンさせるエネミーのルートを決める関数
    /// </summary>
    private void SpawnEnemy_Route()
    {
        bool[] routeFlag = new bool[route.Routes.Count];
        int[] routeNomber = new int[route.Routes.Count];
        for(int i = 0; i < route.Routes.Count; i++)
        {
            routeNomber[i] = i;
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            for(int j = 0; j <= route.Routes.Count; j++)
            {
               if(enemies[i].RouteNo == j)
                {
                    routeFlag[j] = true;
                }
            }
        }


        for (int i = 0; i < routeFlag.Length; i++)
        {
            if (!routeFlag[i])
            {
                Spawn_No = i;
                SpawnFlag_2 = true;
                break;
            }
        }

        if (SpawnFlag_2)
        {
            Debug.Log(string.Format("スポーンNo：{0}がスポーンしました！", Spawn_No));
            SpawnFlag_2 = false;
            SpawnFlag = true;
        }
        else
        {
            Debug.Log("スポーン出来ません！");
        }
    }
    
    /// <summary>
    /// エネミーの死亡させる関数
    /// </summary>
    private void Enemy_Desth()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].DesthFlag == true)
            {
                GameObject enemy = enemies[i].gameObject;
                enemies.RemoveAt(i);
                Destroy(enemy);
            }
        }
    }
    /// <summary>
    /// サーチエネミーがPlayerを見つけた時
    /// </summary>
    private void Search_Order()
    {
        if (searchers.order)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                float Distance = Vector3.Distance(searchers.transform.position, enemies[i].transform.position);
                if (Distance < searchers.call_dis)
                {
                    enemies[i].order = true;
                }
            }
        }
        else
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].order = false;
            }
        }
    }
}
