using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCMove : MonoBehaviour
{
    public Transform[] waypoints;

    private NavMeshAgent navMeshAgent;
    private int currentWaypointIndex = 0;

    private float avoidanceRadius = 2f;

    public Animator anim;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        this.SetNextWaypoint();
    }

    private void Update()
    {
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            float randomNum = Random.value;

            this.SetNextWaypoint();

            if (randomNum <= 0.3f)
            {
                StartCoroutine(Stop());
                //Invoke("SetNextWaypoint", 2f);

            }
        }
    }

    private void SetNextWaypoint()
    {
        if (waypoints.Length == 0) return;

        navMeshAgent.destination = waypoints[currentWaypointIndex].position;

        this.anim.SetInteger("State", 1);

        navMeshAgent.isStopped = false;

        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;

    }

    private IEnumerator Stop()
    {
        this.anim.SetInteger("State", 0);

        navMeshAgent.isStopped = true;

        yield return new WaitForSeconds(2f);

        this.SetNextWaypoint();

        navMeshAgent.isStopped = false;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("npc"))
        {
            Vector3 avoidDirection = transform.position - collision.gameObject.transform.position;
            Vector3 avoidPosition = transform.position + avoidDirection.normalized * avoidanceRadius;

            navMeshAgent.SetDestination(avoidPosition);

            this.anim.SetInteger("State", 2);

            navMeshAgent.isStopped = true;

            new WaitForSeconds(2f);

            this.SetNextWaypoint();

            navMeshAgent.isStopped = false;
        }
    }
}
