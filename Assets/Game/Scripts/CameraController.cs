using Cinemachine;
using DG.Tweening;
using TOI2D;
using UnityEngine;

[System.Serializable]
public enum CameraPriorityLocation
{
    none,
    indoor1,
    indoor2,
    indoor3
}

public class CameraController : MonoBehaviour
{
    [System.Serializable]
    public struct VirtualIndoorCamera
    {
        public CinemachineVirtualCamera indoorBase;
        public CinemachineVirtualCamera indoorSide;
    }

    [SerializeField] CinemachineVirtualCamera virtualCamera;
    public CameraPriorityLocation vcamLoc;
    public TeleportTarget cameraState;

    public VirtualIndoorCamera[] virtualIndoorCameras;
    public CinemachineVirtualCamera VirtualCamera { get => virtualCamera; set => virtualCamera = value; }

    [SerializeField] float rotationValue;
    // Start is called before the first frame update
    void Start()
    {
        InitCamera();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RotateCamera(0, 0);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            RotateCamera(rotationValue, 1);
        }
    }
    public void RotateAroundCamera()
    {

    }

    public void InitCamera()
    {
        cameraState = TeleportTarget.Outdoor;
        SetupCamera();
    }

    public void SetupCamera()
    {
        if (cameraState == TeleportTarget.Outdoor)
        {
            SwicthCamera(VirtualCamera, 10);

            for (int i = 0; i < virtualIndoorCameras.Length; i++)
            {
                SwicthCamera(virtualIndoorCameras[i].indoorBase, 0);
                SwicthCamera(virtualIndoorCameras[i].indoorSide, 0);
            }
        }
        else
        {
            SwicthCamera(VirtualCamera, 0);
            if (vcamLoc == CameraPriorityLocation.indoor1)
            {
                SwicthCamera(virtualIndoorCameras[0].indoorBase, 10);
                SwicthCamera(virtualIndoorCameras[1].indoorBase, 0);
                SwicthCamera(virtualIndoorCameras[2].indoorBase, 0);

            }
            else if (vcamLoc == CameraPriorityLocation.indoor2)
            {
                SwicthCamera(virtualIndoorCameras[1].indoorBase, 10);
                SwicthCamera(virtualIndoorCameras[0].indoorBase, 0);
                SwicthCamera(virtualIndoorCameras[2].indoorBase, 0);

            }
            else if (vcamLoc == CameraPriorityLocation.indoor3)
            {
                SwicthCamera(virtualIndoorCameras[2].indoorBase, 10);
                SwicthCamera(virtualIndoorCameras[0].indoorBase, 0);
                SwicthCamera(virtualIndoorCameras[1].indoorBase, 0);

            }
        }
    }

    public void indoorToOutdoor()
    {
        if (vcamLoc == CameraPriorityLocation.indoor1)
        {
            SwicthCamera(virtualIndoorCameras[0].indoorBase, 0);
            SwicthCamera(virtualIndoorCameras[0].indoorSide, 10);
        }
        if (vcamLoc == CameraPriorityLocation.indoor2)
        {
            SwicthCamera(virtualIndoorCameras[1].indoorBase, 0);
            SwicthCamera(virtualIndoorCameras[1].indoorSide, 10);
        }
        if (vcamLoc == CameraPriorityLocation.indoor3)
        {
            SwicthCamera(virtualIndoorCameras[2].indoorBase, 0);
            SwicthCamera(virtualIndoorCameras[2].indoorSide, 10);
        }
    }

    public void SwicthCamera(CinemachineVirtualCamera target, int priorityTarget)
    {
        if (target != null)
            target.Priority = priorityTarget;
    }

    public void RotateCamera(float value, float duration = 0)
    {
        if (cameraState == TeleportTarget.Outdoor)
        {
            Debug.Log("Rotate value " + value + " with duration " + duration);
            DOTween.To(() => VirtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_Heading.m_Bias,
                x => VirtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_Heading.m_Bias = x,
                value, duration).SetEase(Ease.InQuad);
        }
        else
        {

        }

    }
}
