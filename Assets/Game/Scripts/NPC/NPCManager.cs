using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// public enum NPCType 
// {
//     walk,
//     idle 
// }

[RequireComponent(typeof(Interactable))]
public class NPCManager : MonoBehaviour
{

    //General
    [SerializeField] private GameObject sprite;
    public NPCType Type;
 
    //Patrol
    public Transform[] walkPoints;
    private Transform targetPoint;
    private NavMeshAgent agent;
    private int walkPointIdx = 1;
    private bool reverseRoute = false;
    private bool IsEndRoute = false;
    private bool IsWalking = true;

    private void Awake() {
        sprite = transform.GetChild(0).gameObject;
        agent = Type == NPCType.walk ? GetComponent<NavMeshAgent>() : null;
    }

    private void Start() {
        InitNPC();
    }

    public void NPCInteractionTest()
    {
        if (Type == NPCType.walk)
        {
            agent.isStopped = true;
        }
    }

    private void InitNPC()
    {
        if (Type == NPCType.walk)
        {
            if (walkPoints.Length > 0 && walkPoints[walkPointIdx] != null)
            {
                targetPoint = walkPoints[1];
                agent.SetDestination(targetPoint.position);
            }
        }
    }

    private void Update() {
        
        transform.rotation = Quaternion.Euler(Vector3.zero);
    
        if (Type == NPCType.idle)
            return;

        if (targetPoint != null)
        {
            if(Vector3.Distance(transform.position, targetPoint.position) <= 1f && IsWalking)
            {
                IsWalking = false;
                StartCoroutine(WalkToNextPoint());
            }
        }
    }

    private IEnumerator WalkToNextPoint() {
        if (!reverseRoute)
            walkPointIdx++;

        if (walkPointIdx < walkPoints.Length && !reverseRoute)
        {
            if (walkPointIdx == 1)
                yield return new WaitForSeconds(.5f);
            targetPoint = walkPoints[walkPointIdx];
        }
        else
        {
            if (!IsEndRoute)
            {
                IsEndRoute = true;
                yield return new WaitForSeconds(.5f);
            }
            walkPointIdx--;
            reverseRoute = true;

            if (walkPointIdx == 0)
            {
                reverseRoute = false;
                IsEndRoute = false;
            }
            targetPoint = walkPoints[walkPointIdx];
        }
        agent.SetDestination(targetPoint.position);
        IsWalking = true;
    }
}

