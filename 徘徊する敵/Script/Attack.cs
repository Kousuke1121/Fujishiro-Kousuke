using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Attack : MonoBehaviour
{
    Transform player;
    float distance;

    [System.NonSerialized] public int Mode;
    [System.NonSerialized] public bool order = false;
    private bool check = false;

    private int currentRoot;
    [SerializeField] float range = 5f;
    [SerializeField] float Attack_range = 2f;

    [System.NonSerialized] public NavMeshAgent agent;
    [System.NonSerialized] public Animator animator;
    bool isAttacking = false;
    bool judged = false;

    [SerializeField] private float WalkSpeed = 2f;
    [SerializeField] private float ChaseSpeed = 3.5f;

    private float timeReset = 5;
    private float time = 0;
    Vector3 enemy_;

    void Start()
    {
        player = GameObject.Find("Player").transform;
        enemy_ = this.transform.position;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {        
        distance = Vector3.Distance(player.position, transform.position);
        animator.SetFloat("Speed", agent.speed);

        if (!order)
        {
            if (distance > range)
            {
                Mode = 0;//úpújÉÇÅ[Éh
            }
            if (distance <= range)
            {
                Mode = 1;//í«ê’ÉÇÅ[Éh
            }
        }

        AttackMode();
        EnemyStop_error();

        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log(agent.isStopped);
            Debug.Log(agent.speed);
            Debug.Log(Mode);
        }
    }

    /// <summary>
    /// úpúj
    /// </summary>
    public void WalkMode(GameObject Route)
    {
        Vector3[] route = new Vector3[Route.transform.childCount];
        for(int i = 0; i < Route.transform.childCount; i++)
        {
            route[i] = Route.transform.GetChild(i).position;
        }
        //= new Vector3[] { Route.transform.GetChild(0).position };
        Vector3 pos = route[currentRoot];
        agent.speed = WalkSpeed;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("WALK00_F"))
        {
            //Debug.Log("Walk");
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("WAIT02"))
        {
            agent.isStopped = true;
            check = true;
            if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
            {
                agent.isStopped = false;
                check = false;
            }

        }

        if (Vector3.Distance(this.transform.position, pos) < 0.8f)
        {
            
            animator.SetTrigger("Stop");
            currentRoot += 1;
            if (currentRoot > route.Length - 1)
            {
                currentRoot = 0;
            }
        }
        agent.SetDestination(pos);
    }

    /// <summary>
    /// í«ê’
    /// </summary>
    public void ChaseMode()
    {
        //Debug.Log("Chase");

        if (check)
        {
            agent.isStopped = false;
            check = false;
        }

        if (!isAttacking && distance <= Attack_range && Vector3.Dot(player.position - transform.position, transform.forward) >= 0.9f)
        {
            animator.SetTrigger("Attack");
            agent.isStopped = true;
            isAttacking = true;
            judged = false;
        }
        agent.speed = ChaseSpeed;
        agent.destination = player.position;

    }

    /// <summary>
    /// çUåÇ
    /// </summary>
    public void AttackMode()
    {
        //çUåÇäJén
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("SLIDE00"))
        {
            //Debug.Log("Attack");
            if (!judged && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.6f)
            {
                judged = true;
                //çUåÇHit
                if (distance <= 3f && Vector3.Dot(player.position - transform.position, transform.forward) >= 0.7f)
                {
                    Debug.Log("çUåÇHitÅI");                  
                }
                else
                {
                    Debug.Log("ìñÇΩÇÁÇ»Ç©Ç¡ÇΩÅI");
                }

            }

            //çUåÇèIÇÌÇË
            if (isAttacking && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
            {
                agent.isStopped = false;
                isAttacking = false;
                //Debug.Log("çUåÇèIÇÌÇË");
            }
        }
    } 

    private void EnemyStop_error()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("WALK00_F"))
        {            
            time += Time.deltaTime;

            if(time > timeReset)
            {
                if(enemy_ == this.transform.position)
                {
                    Debug.Log("é~Ç‹Ç¡ÇƒÇ¢Ç‹Ç∑ÅI");
                    currentRoot++;
                }
                
                enemy_ = this.transform.position;
                time = 0;
            }
        }
        else
        {
            time = 0;
        }
    }
}
