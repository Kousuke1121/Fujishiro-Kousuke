using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Searcher : MonoBehaviour
{
    [SerializeField] private SphereCollider searchArea;
    [SerializeField] private float searchAngle = 45f;
    [System.NonSerialized] public bool order = false;

    [SerializeField] private bool SearchFlag = true;

    public float call_dis = 50f;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("1エリア内です。");
            Vector3 playerDirection = other.transform.position - transform.position;
            float angle = Vector3.Angle(transform.forward, playerDirection);
            if (angle <= searchAngle)
            {
                Debug.Log("2エリア内です。");
                order = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            order = false;
        }
    }
#if UNITY_EDITOR
    /// <summary>
    /// サーチする角度表示
    /// </summary>
    private void OnDrawGizmos()
    {
        if (SearchFlag)
        {
            Handles.color = Color.red;
            Handles.DrawSolidArc(transform.position, Vector3.up , Quaternion.Euler(0f, -searchAngle, 0f) * transform.forward, searchAngle * 2f, searchArea.radius*2.15f);
        }
    }
#endif
}
