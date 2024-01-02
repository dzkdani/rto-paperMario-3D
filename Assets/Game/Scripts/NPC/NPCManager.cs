using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum NPCType
{
    walk,
    idle
}

[RequireComponent(typeof(Interactable))]
public class NPCManager : MonoBehaviour
{

    //General
    [Tooltip("NPC Components")]
    public NPCType Type;
    [SerializeField] private Direction direction;
    [SerializeField] private Facing facing = Facing.down;
    [SerializeField] public Facing dialogFacing = Facing.down;
    public bool OnInteract = false;
    public GameObject expressionObject;
    Animator expressionObjectAnimator;
    PlayerController player;
    //Patrol
    public Transform[] walkPoints;
    private Transform targetPoint;
    private GameObject sprite;
    private NavMeshAgent agent;
    private int walkPointIdx = 1;
    private bool reverseRoute = false;
    private bool IsEndRoute = false;
    private bool IsWalking = true;
    private Animator anim;

    private void Awake()
    {
        sprite = transform.GetChild(0).gameObject;
        agent = Type == NPCType.walk ? GetComponent<NavMeshAgent>() : null;
        anim = sprite.GetComponent<Animator>();
        GetComponent<Interactable>().Type = InteractType.npc;
    }

    private void Start()
    {
        player = GameplayManager.instance.player;
        expressionObjectAnimator = expressionObject.GetComponentInChildren<Animator>();
        expressionObject.gameObject.SetActive(false);
        InitNPC();
    }

    private void InitNPC()
    {
        //patrol
        if (Type == NPCType.walk)
        {
            if (walkPoints.Length > 0 && walkPoints[walkPointIdx] != null)
            {
                targetPoint = walkPoints[1];
                agent.SetDestination(targetPoint.position);
                LookAtTargetDir(agent.destination.normalized.ToVector2());
            }
        }
        else
        {
            InitRotateSprite();
            anim.Play("idle_down_left");
        }
    }

    private void Update()
    {

        transform.rotation = Quaternion.Euler(Vector3.zero);

        if (Type == NPCType.idle)
            return;

        if (targetPoint != null)
        {
            if (Vector3.Distance(transform.position, targetPoint.position) <= 1f && IsWalking)
            {
                IsWalking = false;
                StartCoroutine(WalkToNextPoint());
            }
        }
    }

    public void InitExpression(string exspression)
    {
        Debug.Log($"init Expression {expressionObjectAnimator.gameObject.name} with {exspression}");
        expressionObject.gameObject.SetActive(true);
        expressionObjectAnimator.Play(exspression);
        if (dialogFacing != Facing.down)
        {
            anim.Play("behind_LB");
        }
        else
        {
            if (exspression != "calm")
            {
                anim.Play(exspression + "_LF");
            }
            else
            {
                anim.Play("calm_LF");
            }

        }
    }

    void InitRotateSprite()
    {
        if (direction == Direction.left)
        {
            transform.GetChild(0).transform.DORotate(new Vector3(0, 0, 0), 0.4f).From(transform.rotation.eulerAngles).SetEase(Ease.InQuad);
        }
        if (direction == Direction.right)
        {
            transform.GetChild(0).transform.DORotate(new Vector3(0, transform.eulerAngles.y + 180f, 0), 0.4f).From(transform.rotation.eulerAngles).SetEase(Ease.InQuad);
        }
    }

    public void SetInteraction(bool val) { OnInteract = val; }

    #region Interaction
    public void InitInteractionNPC()
    {
        if (agent != null)
            agent.isStopped = true;
        anim.Play("idle_down_left");
        OnInteract = true;
        //anim.Play("Idle");
    }

    public void EndInteractionNPC()
    {
        if (agent != null)
            agent.isStopped = false;
        expressionObject.gameObject.SetActive(false);

        if (Type != NPCType.idle)
            anim.Play("walk_down_left");
        else
            anim.Play("idle_down_left");
        OnInteract = false;
    }

    public Direction GetCurrentDirection() => direction;
    public Facing GetCurrentFacing() => facing;

    #endregion

    #region Patrol
    private IEnumerator WalkToNextPoint()
    {
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
        LookAtTargetDir(agent.destination.normalized.ToVector2());
        IsWalking = true;
    }

    private void LookAtTargetDir(Vector2 destination)
    {
        Debug.Log($"npc next target : {destination}");
        if (destination == Vector2.left)
            direction = Direction.left;
        if (destination == Vector2.right)
            direction = Direction.right;
        SetSprite(destination);
    }

    private void SetSprite(Vector2 vector)
    {
        //every facing is down, biar assets kelihatan muka
        if (vector.x >= 0f)
        {
            if (direction == Direction.left)
                sprite.transform.FlipSprite(onComplete: () => { Debug.Log("FinishFlip"); });
            direction = Direction.right;
        }
        if (vector.x <= 0f)
        {
            if (direction == Direction.right)
                sprite.transform.FlipSprite(onComplete: () => { Debug.Log("FinishFlip"); });
            direction = Direction.left;
        }
        facing = Facing.down;
        anim.Play("walk_down_left");
    }
    #endregion
}

