using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class EnemyAI : MonoBehaviourPunCallbacks //IDamagable
{
    PhotonView PV;

    
   //[SerializeField] Item[] items;

    public float speed = 5f;
    public float stoppingDistance = 2f;
    public float retreatDistance = 3f;
    //public float timeBetweenShots = 2f;
    public float patrolDistance = 10f;
    public float attackDistance = 5f;
    public float attackTime = 1f;
    public float enemyDamage = 10f;

    public bool grounded;
    public float currentHealth = 100f;
    public Animator anim;

    private Transform target;
    //private float shotTime;
    private Vector3 patrolPoint;
    private bool patrolling;
    private bool attacking;
    private float attackStartTime;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        SetPatrolPoint();
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;

        if (target != null)
        {
            if (!patrolling && Vector3.Distance(transform.position, target.position) > patrolDistance)
            {
                patrolling = true;
                SetPatrolPoint();
                anim.SetBool("IsMoving", true);
            }

            if (patrolling && Vector3.Distance(transform.position, patrolPoint) < 1f)
            {
                patrolling = false;
                anim.SetBool("IsMoving", false);
            }

            if (!attacking && Vector3.Distance(transform.position, target.position) < attackDistance)
            {
                attacking = true;
                patrolling = false;
                attackStartTime = Time.time;
            }

            if (attacking)
            {
                transform.LookAt(target);
                if (Time.time >= attackStartTime + attackTime)
                {
                    Attack();
                    attackStartTime = Time.time;
                }

                if (Vector3.Distance(transform.position, target.position) > attackDistance + 5f)
                {
                    attacking = false;
                }
            }
            else if (patrolling)
            {
                transform.position = Vector3.MoveTowards(transform.position, patrolPoint, speed * Time.deltaTime);
            }
            else
            {
                if (Vector3.Distance(transform.position, target.position) > stoppingDistance)
                {
                    transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                }
                else if (Vector3.Distance(transform.position, target.position) < stoppingDistance && Vector3.Distance(transform.position, target.position) > retreatDistance)
                {
                    transform.position = this.transform.position;
                }
                else if (Vector3.Distance(transform.position, target.position) < retreatDistance)
                {
                    transform.position = Vector3.MoveTowards(transform.position, target.position, -speed * Time.deltaTime);
                }
            }
        }
    }

    private void SetPatrolPoint()
    {
        float x = Random.Range(-patrolDistance, patrolDistance);
        float z = Random.Range(-patrolDistance, patrolDistance);
        patrolPoint = transform.position + new Vector3(x, 0f, z);
    }

    private void Attack()
    {
        if (Vector3.Distance(transform.position, target.position) <= attackDistance)
        {
            anim.SetBool("IsAttacking", true);
            target.gameObject.GetComponent<IDamagable>()?.TakeDamage(enemyDamage);
        }
    }

    public void SetGroundedState(bool _grounded)
    {
        grounded = _grounded;
    }

    //public void TakeDamage(float damage)
    //{
    //    PV.RPC("RPC_TakeDamage1", RpcTarget.All, damage);
    //    //currentHealth -= damage;

    //    //if (currentHealth <= 0f)
    //    //{
    //    //    Die();
    //    //}
    //}

    //[PunRPC]
    //public void RPC_TakeDamage1(float damage)
    //{
    //    if (!PV.IsMine)
    //        return;

    //    currentHealth -= damage;

    //    if (currentHealth <= 0f)
    //    {
    //        Die();
    //    }
    //}

    //private void Die()
    //{
    //    PhotonNetwork.Destroy(gameObject);
    //}
}