
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;


public class EnemyAiTutorial : MonoBehaviour, IDamagable
{
    PhotonView PV;
    public float enemyDamage = 10f;

    public NavMeshAgent agent;

    private Transform player;
    private Rigidbody rb;
    private Collider col;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;

    public Animator anim;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    //public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;

            if (!PV.IsMine)
                return;

            if (player != null)
            {
                //Check for sight and attack range
                playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
                playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

                if (!playerInSightRange && !playerInAttackRange) Patroling();
                if (playerInSightRange && !playerInAttackRange) ChasePlayer();
                if (playerInAttackRange && playerInSightRange) AttackPlayer();
            }
        }
    }

    private void Patroling()
    {
        


            if (!walkPointSet) SearchWalkPoint();

            if (walkPointSet)
            {
                agent.SetDestination(walkPoint);
            }


            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            //Walkpoint reached
            if (distanceToWalkPoint.magnitude < 1f)
                walkPointSet = false;

    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        //transform.LookAt(player.position); 
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            ///Attack code here
            //Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            //rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            //rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            anim.SetTrigger("Attack");
            player.gameObject.GetComponent<IDamagable>()?.TakeDamage(enemyDamage);
            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
        anim.ResetTrigger("Attack");
    }

    //public void TakeDamage(int damage)
    //{
    //    health -= damage;

    //    if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    //}
    //private void DestroyEnemy()
    //{
    //    Destroy(gameObject);
    //}


    public void TakeDamage(float damage)
    {
        PV.RPC("RPC_TakeDamage1", RpcTarget.All, damage);
        //currentHealth -= damage;

        //if (currentHealth <= 0f)
        //{
        //    Die();
        //}
    }

    [PunRPC]
    public void RPC_TakeDamage1(float damage)
    {
        if (!PV.IsMine)
            return;

        health -= damage;

        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        //PhotonNetwork.Destroy(gameObject);
        anim.SetBool("IsDead", true);
        agent.enabled = false;
        col.enabled = false;
        rb.useGravity = false;
        enemyDamage = 0f;
        sightRange = 0f;
        attackRange = 0f;
        rb.velocity = Vector3.zero;
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, attackRange);
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, sightRange);
    //}
}
