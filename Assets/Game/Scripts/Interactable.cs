using System.Collections;
using TOI2D;
using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    public InteractType Type;
    [Tooltip("Interaction Components")]
    [SerializeField] private Dialogue dialogueObj;
    [SerializeField] private Transform teleportPos;
    [SerializeField] private Vector3 lastpos;
    [SerializeField] private TeleportPreparation[] preparationPosition;
    [SerializeField] private TeleportTarget teleportTarget;
    [SerializeField] private float teleRotation;

    private void InitInteractable(PlayerController player)
    {
        switch (Type)
        {
            case InteractType.dialogue:
                DialogueInteraction();
                break;
            case InteractType.teleport:
                TeleportInteraction();
                break;
            case InteractType.npc:
                StartCoroutine(NPCInteraction(player));
                break;
            case InteractType.other:
                Debug.Log("other interaction");
                break;
        }
    }

    public void Interact(PlayerController player)
    {
        InitInteractable(player);
    }

    private void DialogueInteraction()
    {
        Debug.Log("dialog interaction");
        if (GameplayManager.instance.dialogueUI != null)
            GameplayManager.instance.dialogueUI.InitDialogue(dialogueObj, null, this);
    }

    private void TeleportInteraction()
    {
        Debug.Log("teleport interaction");
        if (GameplayManager.instance.teleportSystem != null)
            GameplayManager.instance.teleportSystem.PreparaTeleport(teleportTarget, this, preparationPosition, teleRotation);
    }

    private IEnumerator NPCInteraction(PlayerController player)
    {
        Debug.Log("npc interaction");
        if (TryGetComponent<NPCManager>(out NPCManager npc))
        {
            if (!npc.OnInteract)
            {
                npc.InitInteractionNPC();
                if (player.transform.position.z > transform.position.z)
                    npc.dialogFacing = Facing.up;
                else
                    npc.dialogFacing = Facing.down;

                if (npc.GetCurrentDirection() == Direction.left)
                {
                    if (player.transform.position.x > transform.position.x)
                        transform.GetChild(0).transform.FlipSprite(onComplete: DialogueInteraction);
                    else
                        DialogueInteraction();
                }
                if (npc.GetCurrentDirection() == Direction.right)
                {
                    if (player.transform.position.x < transform.position.x)
                        transform.GetChild(0).transform.FlipSprite(onComplete: DialogueInteraction);
                    else
                        DialogueInteraction();
                }

                yield return new WaitUntil(() => GameplayManager.instance.dialogueUI.IsOpen == true);
                yield return new WaitUntil(() => GameplayManager.instance.dialogueUI.IsOpen == false);

                if (npc.GetCurrentDirection() == Direction.left)
                {
                    if (player.transform.position.x > transform.position.x)
                        transform.GetChild(0).transform.FlipSprite(onComplete: npc.EndInteractionNPC);
                    else
                        npc.EndInteractionNPC();
                }
                if (npc.GetCurrentDirection() == Direction.right)
                {
                    if (player.transform.position.x < transform.position.x)
                        transform.GetChild(0).transform.FlipSprite(onComplete: npc.EndInteractionNPC);
                    else
                        npc.EndInteractionNPC();
                }
            }
        }
        else
            Debug.Log("no npc found");
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
