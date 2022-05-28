using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LateMove : MonoBehaviour
{
    [SerializeField]
    Transform base_object;//これに追従してmy_objectが動く

    [SerializeField]
    Transform my_object;//実際に動くオブジェクト

    [SerializeField]
    Vector3 target_pos;

    [SerializeField]
    Vector3 my_pos;

    [SerializeField]
    Vector3 base_pos;

    [Range(0.0f,1.0f)]//追従する速さ

    [SerializeField]
    float lp_div;

    // Start is called before the first frame update
    void Start()
    {
        if(base_object != null)
            this.transform.Translate(base_object.position);
    }

    // Update is called once per frame
    void Update()
    {
        lateMove();
    }

    void lateMove()
    {
        my_object = this.transform;
        my_pos = my_object.position;
        base_pos = base_object.position;

        target_pos = base_object.position - this.transform.position;
        target_pos *= lp_div;
        this.transform.position += target_pos;
    }
}

////ワールド座標に変換してみる