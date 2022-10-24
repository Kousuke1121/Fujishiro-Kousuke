using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoute : MonoBehaviour
{    
    public GameObject Routeparent;
    public List<GameObject> Routes;

    private void Start()
    {
        for(int i = 0; i < Routeparent.transform.childCount; i++)
        {
            Routes.Add(Routeparent.transform.GetChild(i).gameObject);
        }
    }
}
