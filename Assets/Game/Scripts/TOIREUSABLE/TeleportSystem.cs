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
        CameraController cameraController;
        float baseAnimationDuration = 0.25f;

        private void Start()
        {
            uITransition = FindObjectOfType<UITransition>();
            player = GameplayManager.instance.player;
            cameraController = GameplayManager.instance.cameraController;

        }
        public void PreparaTeleport(TeleportTarget target, Interactable io, TeleportPreparation[] preparationPos, float teleRot = 0)
        {
            StartCoroutine(SetupTeleport(target, io, preparationPos, teleRot));
        }

        IEnumerator SetupTeleport(TeleportTarget target, Interactable io, TeleportPreparation[] preparationPos, float teleRot = 0)
        {
            player.CanInteract = false;
            player.CanMove = false;
            player.OnTeleport = true;
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
            Debug.Log("Rotate");
            cameraController.RotateCamera(teleRot, baseAnimationDuration * 4);
            if (preparationPos.Length != 0)
            {
                for (int i = 0; i < preparationPos.Length; i++)
                {
                    string animationTarget = CheckPlayerPositionToSetAnimation(preparationPos[i].target);

                    yield return StartCoroutine(MoveToTargetWithDuration(preparationPos[i].target, preparationPos[i].durationToTarget, animationTarget));
                }
                yield return new WaitForSeconds(baseAnimationDuration * 2);
            }
            else
            {
                yield return new WaitForSeconds(baseAnimationDuration * 6);
            }
            //Open Door

            InitTeleport(target);
        }


        string CheckPlayerPositionToSetAnimation(Transform target)
        {
            string animation = "";

            if (player.transform.position.z < target.transform.position.z)
            {
                if (player.transform.position.x > target.transform.position.x)
                {
                    animation = "walk_up_left";
                }
                if (player.transform.position.x < target.transform.position.x)
                {
                    animation = "walk_up_left";
                }
            }
            else
            {
                if (player.transform.position.x > target.transform.position.x)
                {
                    animation = "walk_down_left";
                }
                if (player.transform.position.x < target.transform.position.x)
                {
                    animation = "walk_down_left";
                }
            }


            return animation;
        }
        IEnumerator MoveToTargetWithDuration(Transform target, float duration, string animation = null)
        {
            float distance = GetDistance(target, player.transform);
            float speed = distance / duration;

            while (distance > 0f)
            {
                if (animation != null)
                {
                    player.SetAnimation(animation);
                    //play animation
                }
                player.transform.position = Vector3.MoveTowards(player.transform.position, target.position, speed * Time.deltaTime);
                distance -= speed * Time.deltaTime;

                yield return null;
            }
            player.transform.position = target.transform.position;
        }
        private float GetDistance(Transform target, Transform player)
        {
            return Vector3.Distance(player.transform.position, target.transform.position);
        }

        public void InitTeleport(TeleportTarget target)
        {
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

            player.SetPlayerPos(targetPos);
            yield return new WaitForSeconds(baseAnimationDuration * 2);


            //_player.CanMove = false;
            //Debug.Log(_player.canMove);
            //_player.RecalculatingBoundary();
            //FindObjectOfType<CameraVirtualManager>().SetCamera();

            yield return new WaitForSeconds(teleportDelay);
            cameraController.RotateCamera(0);
            Debug.Log("set camera to 0 bias");
            yield return new WaitForSeconds(teleportDelay);

            if (uITransition != null)
                uITransition.FadeIn();

            yield return new WaitForSeconds(baseAnimationDuration * 2);
            player.CanInteract = true;
            player.CanMove = true;
            player.OnTeleport = false;
        }
    }
}


