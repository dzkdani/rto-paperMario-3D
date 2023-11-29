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

    public class TeleportSystem : MonoBehaviour
    {
        [SerializeField] private float teleportDelay;
        //private Vector3 lastPos;
        private Vector3 targetPos;
        UITransition uITransition;


        private void Start()
        {
            uITransition = FindObjectOfType<UITransition>();
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
            GameplayManager.instance.player.SetPlayerPos(targetPos);
            //_player.RecalculatingBoundary();
            //FindObjectOfType<CameraVirtualManager>().SetCamera();

            yield return new WaitForSeconds(teleportDelay);

            yield return new WaitForSeconds(teleportDelay);

            if (uITransition != null)
                uITransition.FadeIn();

            yield return new WaitForSeconds(baseAnimationDuration * 2);
            //_player.canMove = true;
        }
    }
}


