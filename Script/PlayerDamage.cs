using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDamage : MonoBehaviour
{
    MeshRenderer mesh;

    [SerializeField] Color32 DamageColor = new Color32(255, 0, 0, 120);
    Color32 normalColor = new Color32(255, 0, 0, 0);

    public float second = 0.1f;

    [SerializeField] string EnemyBullet = "";

    void Start()
    {
        mesh = transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        mesh.material.color = normalColor;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            mesh.material.color = DamageColor;
        }

        if(mesh.material.color == DamageColor)
        {
             StartCoroutine("Transparent");                
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == EnemyBullet)
        {
            mesh.material.color = DamageColor;
        }
    }

    IEnumerator Transparent()
    {
        for (int i = 0; i < 255; i++)
        {
            //Debug.Log("‰ñ”" + i);
            mesh.material.color -= new Color32(0, 0, 0, 30);
            if(mesh.material.color.a <= 0)
            {
                mesh.material.color = normalColor;
                break;
            }
            yield return new WaitForSeconds(second);
        }
    }

    void Player_HP()
    {
        float HP = 100;
        float _HP;

        _HP = HP;

        if(HP < _HP)
        {
            mesh.material.color = DamageColor;
            _HP = HP;
        }
    }

}
