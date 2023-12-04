using System.Collections;
using UnityEngine;

namespace TOI2D
{
    public enum TeleportTarget
    {
        None,
        Indoor,
        Outdoor
    }
    [System.Serializable]
    public struct TeleportPreparation
    {
        public string playerAnimationID;
        public Transform target;
        public float durationToTarget;
    }
    public class TeleportSystem : MonoBehaviour
    {
        [SerializeField] private float teleportDelay;
        //private Vector3 lastPos;
        private Vector3 targetPos;
        UITransition uITransition;
        PlayerController player;
        float baseAnimationDuration = 0.25f;

        private void Start()
        {
            uITransition = FindObjectOfType<UITransition>();
            player = GameplayManager.instance.player;
        }
        public void PreparaTeleport(TeleportTarget target, Interactable io, TeleportPreparation[] preparationPos)
        {
            StartCoroutine(SetupTeleport(target, io, preparationPos));
        }

        IEnumerator SetupTeleport(TeleportTarget target, Interactable io, TeleportPreparation[] preparationPos)
        {
            player.CanInteract = false;
            player.CanMove = false;
            for (int i = 0; i < preparationPos.Length; i++)
            {
                StartCoroutine(MoveToTargetWithDuration(preparationPos[i].target, preparationPos[i].durationToTarget));
            }
            yield return new WaitForSeconds(baseAnimationDuration * 4);
            InitTeleport(target, io);
        }

        IEnumerator MoveToTargetWithDuration(Transform target, float duration)
        {
            float distance = GetDistance(target, player.transform);
            float speed = distance / duration;

            while (distance > 0f)
            {
                player.transform.Translate(speed * Time.deltaTime * Vector3.forward);
                distance -= speed * Time.deltaTime;

                yield return null;
            }
            player.transform.position = target.transform.position;
        }
        private float GetDistance(Transform target, Transform player)
        {
            return Vector3.Distance(player.transform.position, target.transform.position);
        }
        public void InitTeleport(TeleportTarget target, Interactable io)
        {
            switch (target)
            {
                case TeleportTarget.Indoor:
                    targetPos = io.GetTeleportPos().position;
                    io.GetTeleportPos().GetComponent<Interactable>().SetLastPosition(GameplayManager.instance.player.GetPlayerPos());
                    break;
                case TeleportTarget.Outdoor:
                    targetPos = io.GetLastPosition();
                    break;
            }
            StartCoroutine(StartTeleport(target));
        }

        private IEnumerator StartTeleport(TeleportTarget target)
        {
            float baseAnimationDuration = 0;

            if (uITransition != null)
            {
                baseAnimationDuration = uITransition.GetBaseAnimationDuration();
                uITransition.FadeOut();
            }

            yield return new WaitForSeconds(baseAnimationDuration * 2);


            //_player.CanMove = false;
            //Debug.Log(_player.canMove);
            player.SetPlayerPos(targetPos);
            //_player.RecalculatingBoundary();
            //FindObjectOfType<CameraVirtualManager>().SetCamera();

            yield return new WaitForSeconds(teleportDelay);

            yield return new WaitForSeconds(teleportDelay);

            if (uITransition != null)
                uITransition.FadeIn();

            yield return new WaitForSeconds(baseAnimationDuration * 2);
            player.CanInteract = true;
            player.CanMove = true;
        }
    }
}


