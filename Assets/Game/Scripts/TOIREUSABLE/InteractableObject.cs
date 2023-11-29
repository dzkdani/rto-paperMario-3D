using UnityEngine;
namespace TOI2D
{
    public interface IInteracable
    {
        void Interact(PlayerController player);
    }

    public enum InteractableType
    {
        Dialogue,
        Teleport,
        Cutscene,
        none
    }

    public class InteractableObject : MonoBehaviour, IInteracable
    {
        public GameObject interactableNotif;
        public InteractableType interactType;
        [SerializeField] DialogueUI dialogueUI;
        [SerializeField] private Dialogue dialogObj;
        [SerializeField] private Transform teleportPos;
        public Vector3 lastpos;
        public TeleportTarget teleportTarget;
        public bool npc = false;
        //NPCController nPCController;
        private void Start()
        {
            if (interactableNotif != null)
                interactableNotif.SetActive(false);

            if (interactType != InteractableType.none)
            {
                //nPCController = gameObject.GetComponent<NPCController>();
                //if (nPCController != null)
                //    npc = true;
            }
        }
        public void Interact(PlayerController player)
        {
            if (interactableNotif != null)
                interactableNotif.SetActive(false);

            if (interactType != InteractableType.none)
                switch (interactType)
                {
                    case InteractableType.Dialogue:
                        if (!npc)
                            dialogueUI.InitDialogue(dialogObj, interactableNotif);
                        else
                            dialogueUI.InitDialogue(dialogObj, interactableNotif, this, player.gameObject);

                        break;
                    case InteractableType.Teleport:
                        //if (player.teleportSystem != null)
                        //    player.teleportSystem.InitTeleport(teleportTarget, this);
                        break;
                    case InteractableType.Cutscene:
                        //if (player.cutsceneSystem != null)
                        //    player.cutsceneSystem.InitCutscene(dialogObj);
                        break;
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

        //private void OnTriggerEnter2D(Collider2D other)
        //{
        //    if (interactType != InteractableType.none)
        //        if (other.TryGetComponent<PlayerController>(out PlayerController player))
        //        {
        //            if (!player.onInteraction)
        //            {
        //                player.onInteraction = true;
        //                player.Interacable = this;
        //                if (interactableNotif != null)
        //                    interactableNotif?.SetActive(true);

        //                //if (nPCController != null)
        //                //    nPCController.Interupted(true);
        //            }
        //        }
        //}

        //private void OnTriggerStay2D(Collider2D other)
        //{
        //    //if (interactType != InteractableType.none)
        //    //    if (other.TryGetComponent<PlayerController>(out PlayerController player))
        //    //    {
        //    //        player.teleportSystem.InitTeleport(teleportTarget, this);
        //    //        if (!player.onInteraction)
        //    //        {
        //    //            player.onInteraction = true;
        //    //            player.Interacable = this;
        //    //            if (interactableNotif != null)
        //    //                interactableNotif?.SetActive(true);

        //    //            //if (nPCController != null)
        //    //            //    nPCController.Interupted(true);
        //    //        }
        //    //    }
        //}

        //private void OnTriggerExit2D(Collider2D other)
        //{
        //    if (interactType != InteractableType.none)
        //        if (interactableNotif != null)
        //            if (other.TryGetComponent<PlayerController>(out PlayerController player))
        //            {
        //                player.Interacable = null;
        //                if (interactableNotif != null)
        //                    interactableNotif.SetActive(false);

        //                //if (nPCController != null)
        //                //    nPCController.Interupted(false, 1);

        //                player.onInteraction = false;
        //            }

        //}
    }
}

