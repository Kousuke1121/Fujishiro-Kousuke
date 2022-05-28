using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_BossWarp : MonoBehaviour
{
    [SerializeField] GameObject Player;

    [SerializeField] Vector3 Boss_Position;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.T))
        {
            //Player.transform.position = new Vector3(Boss_Position.x, Boss_Position.y, Boss_Position.z);
            Player.transform.position = this.gameObject.transform.position;
        }
    }
}
