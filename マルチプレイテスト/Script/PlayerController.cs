using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Photon.Pun.MonoBehaviourPun
{
    bool flag = false;

    Animator animator;

    PhotonManager manager;

    [SerializeField]private CapsuleCollider SwordCol;

    Dictionary<string, KeyCode> kmap = new Dictionary<string, KeyCode>
    {
        {"run",KeyCode.LeftShift },
        {"jump",KeyCode.Space },
        {"attack1",KeyCode.E },
        {"attack2",KeyCode.Q }
    };

    public void SetFlag(bool f)
    {
        flag = f;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        manager = GameObject.Find("PhotonManager").GetComponent<PhotonManager>();
    }

    void Update()
    {
        if(!flag) { return; }
        if (!photonView.IsMine) { return; }
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        animator.SetBool("Jump", false);
        animator.SetBool("Attack", false);

        float x = Input.GetAxis("Horizontal") / 2;
        float z = Input.GetAxis("Vertical") / 2;

        if (Input.GetKey(kmap["attack1"]))
        {
            animator.SetBool("Attack",true);
            animator.SetInteger("AttackType", 0);
        }

        if (Input.GetKey(kmap["attack2"]))
        {
            animator.SetBool("Attack", true);
            animator.SetInteger("AttackType", 1);
        }

        if (Input.GetKey(kmap["run"]))
        {
            x = x * 2;
            z = z * 2;
        }

        animator.SetFloat("Forward", z, 0.1f, Time.deltaTime);
        animator.SetFloat("Side", x, 0.1f, Time.deltaTime);

        if (Input.GetKey(kmap["jump"]))
        {
            animator.SetBool("Jump", true);
        }

        if (!stateInfo.IsTag("Attack"))
        {
            transform.Translate(0.01f * x, 0f, 0.01f * z);
        }
        else
        {
            if(stateInfo.normalizedTime >= 0.6f)
            {
                SwordCol.enabled = false;
            }
            else
            {
                SwordCol.enabled = true;
            }
        }

        if (manager.GetEnemy() ==null)
        {
            manager.CheckEnemy();
        }
        else
        {
            transform.LookAt(manager.GetEnemy().transform);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
       // Debug.Log("Hit" + collider.gameObject.name + "/" + collider.gameObject.tag);
        if (collider.gameObject.tag == "Sword")
        {
            Debug.Log("攻撃されました!");
        }
    }
}
