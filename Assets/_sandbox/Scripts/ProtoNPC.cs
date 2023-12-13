using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public enum NPCType 
{
    walk,
    idle 
}

public enum NPCFacing
{
    left,
    right
}

public enum NPCDirection
{
    up,
    down
}

public class ProtoNPC : MonoBehaviour
{

    //General
    [Tooltip("NPC Components")]
    private GameObject sprite;
    public NPCType Type;
    //Movement
    public Transform[] walkPoints;
    private Transform targetPoint;
    private NavMeshAgent agent;
    private int walkPointIdx = 1;
    private bool reverseRoute = false;
    private bool IsEndRoute = false;
    private bool IsWalking = true;
    private string lastFacing = "";
    private string currFacing = "";

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
        if (vector == Vector2.zero)
            return;

        

        if (vector.x >= 0f && vector.y <= 0f)
        {
            if (currFacing == "left")
            {
                RotateSprite();
            }
            sprite.GetComponent<Animator>().Play("walk_down_right");
            lastFacing = "right";
            currFacing = "right";
        }
        if (vector.x >= 0f && vector.y >= 0f)
        {
            if (currFacing == "left")
            {
                RotateSprite();
            }
            sprite.GetComponent<Animator>().Play("walk_up_right");
            lastFacing = "right";
            currFacing = "right";
        }
        if (vector.x <= 0f && vector.y <= 0f)
        {
            if (currFacing == "right")
            {
                RotateSprite();
            }
            sprite.GetComponent<Animator>().Play("walk_down_left");
            lastFacing = "left";
            currFacing = "left";
        }
        if (vector.x <= 0f && vector.y >= 0f)
        {
            if (currFacing == "right")
            {
                RotateSprite();
            }
            sprite.GetComponent<Animator>().Play("walk_up_left");
            lastFacing = "left";
            currFacing = "left";
        }
    }

    private void RotateSprite()
    {
        sprite.transform.DORotate(new Vector3(0, transform.eulerAngles.y + 180f, 0), 0.75f).From(transform.rotation.eulerAngles).SetEase(Ease.InQuad);
    }

    private void CheckNPCFlip()
    {
        
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

