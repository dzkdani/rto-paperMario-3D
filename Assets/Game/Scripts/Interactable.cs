using DG.Tweening;
using System;
using TOI2D;
using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    [SerializeField] private InteractType Type;

    public static event Action OnInteract;
    [SerializeField] private Dialogue dialogueObj;
    [SerializeField] private Transform teleportPos;
    public Vector3 lastpos;
    public TeleportPreparation[] preparationPosition;
    public TeleportTarget teleportTarget;
    private GameplayManager gameplayManager;

    private void Start()
    {
        //InitInteractable();
    }

    private void InitInteractable()
    {
        Debug.Log("Interact");
        switch (Type)
        {
            case InteractType.dialogue:
                Test4();
                if (GameplayManager.instance.dialogueUI != null)
                    GameplayManager.instance.dialogueUI.InitDialogue(dialogueObj, null, this);
                break;
            case InteractType.teleport:
                if (GameplayManager.instance.teleportSystem != null)
                    GameplayManager.instance.teleportSystem.PreparaTeleport(teleportTarget, this, preparationPosition);
                break;
            case InteractType.other:
                OnInteract = Test3;
                break;
        }
    }

    public void Interact()
    {
        //OnInteract?.Invoke();
        InitInteractable();
    }

    private void Test1()
    {
        Debug.Log("dialog interaction");
    }

    private void Test2()
    {
        Debug.Log("teleport interaction");
    }

    private void Test3()
    {
        Debug.Log("other interaction");
    }

    private void Test4()
    {
        Debug.Log("npc interaction");
        if (transform.GetChild(0).TryGetComponent<ProtoNPCAnim>(out ProtoNPCAnim npc))
        {
            npc.transform.DORotate(new Vector3(0, transform.eulerAngles.y + 180f, 0), 0.25f).From(transform.rotation.eulerAngles).SetEase(Ease.InQuad)
                .OnComplete(() => { npc.GetComponent<SpriteRenderer>().flipX = true; });

            npc.GetComponentInParent<ProtoNPC>().NPCInteractionTest();
        }
    }

    public void SetLastPosition(Vector3 posTarget)
    {
        lastpos = posTarget;
    }
    public Vector3 GetLastPosition()
    {
        Vector3 posTarget = lastpos;
        return posTarget;
    }

    public Transform GetTeleportPos()
    {
        return teleportPos;
    }
}
