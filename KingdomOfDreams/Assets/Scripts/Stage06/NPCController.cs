using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public Transform[] destinationPoints;
    private NavMeshAgent agent;
    private int currentIndex;

    public Animator anim;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        currentIndex = 0;
    }

    public void MoveToNextDestination()
    {
        if (currentIndex >= destinationPoints.Length) return;

        this.anim.SetInteger("State", 1);

        agent.enabled = true;
        agent.SetDestination(destinationPoints[currentIndex].position);
        currentIndex++;
    }

    private void Update()
    {
        if (agent.enabled && agent.remainingDistance <= agent.stoppingDistance)
        {
            MoveToNextDestination();
        }
    }
}
