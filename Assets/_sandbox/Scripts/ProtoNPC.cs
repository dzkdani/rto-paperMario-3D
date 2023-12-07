using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum NPCType 
{
    walk,
    idle 
}

public class ProtoNPC : MonoBehaviour
{
    [SerializeField] private GameObject sprite;

    //General
    public NPCType Type;
 
    //Movement
    public Transform[] walkPoints;
    private Transform targetPoint;
    private NavMeshAgent agent;
    private int walkPointIdx = 1;
    private bool reverseRoute = false;
    private bool IsEndRoute = false;
    private bool IsWalking = true;

    private void Awake() {
        agent = Type == NPCType.walk ? GetComponent<NavMeshAgent>() : null;
        sprite = transform.GetChild(0).gameObject;
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
                LookAtDestination();
            }
        }
    }

    private void Update() {
        
        // transform.rotation = Quaternion.Euler(Vector3.zero);
    
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

    private void SetAnim(Vector2 vector)
    {
        Vector2 up = Vector2.up;
        Vector2 down = Vector2.down;
        Vector2 left = Vector2.left;
        Vector2 right = Vector2.right;

        if (vector.x > 0f)
            sprite.GetComponent<Animator>().Play("walk_down_right");
        if (vector.x < 0f)
            sprite.GetComponent<Animator>().Play("walk_down_left");

        // if (vector.x > 0f && vector.y < 0f)
        //     sprite.GetComponent<Animator>().Play("walk_down_right");
        // if (vector.x > 0f && vector.y > 0f)
        //     sprite.GetComponent<Animator>().Play("walk_up_right");
        // if (vector.x < 0f && vector.y < 0f)
        //     sprite.GetComponent<Animator>().Play("walk_down_left");
        // if (vector.x < 0f && vector.y > 0f)
        //     sprite.GetComponent<Animator>().Play("walk_up_left");
    }

    private void LookAtDestination()
    {
        Debug.Log($"npc turn to {agent.destination.normalized.ToVector2()}");
        SetAnim(agent.destination.normalized.ToVector2());

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
        LookAtDestination();
        IsWalking = true;
    }
}

public static class Extention
{
    public static Vector2 ToVector2 (this Vector3 move) => new Vector2(move.x, move.z);
}

