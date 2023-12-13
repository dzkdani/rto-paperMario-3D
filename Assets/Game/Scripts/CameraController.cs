using Cinemachine;
using DG.Tweening;
using System.Collections;
using UnityEngine;
public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera frontCamera;
    [SerializeField] CinemachineVirtualCamera backCamera;
    [SerializeField] CinemachineVirtualCamera leftCamera;
    [SerializeField] CinemachineVirtualCamera rightCamera;
    [SerializeField] CinemachineVirtualCamera indoorCamera;

    public CinemachineVirtualCamera FrontCamera { get => frontCamera; set => frontCamera = value; }
    public CinemachineVirtualCamera BackCamera { get => backCamera; set => backCamera = value; }
    public CinemachineVirtualCamera LeftCamera { get => leftCamera; set => leftCamera = value; }
    public CinemachineVirtualCamera RightCamera { get => rightCamera; set => rightCamera = value; }
    public CinemachineVirtualCamera IndoorCamera { get => indoorCamera; set => indoorCamera = value; }

    [SerializeField] float rotationValue;
    // Start is called before the first frame update
    void Start()
    {
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
    public void SwicthCamera(CinemachineVirtualCamera current, CinemachineVirtualCamera after)
    {
        current.Priority = 0;
        after.Priority = 10;

    }

    public void RotateCamera(float value, float duration = 0)
    {
        DOTween.To(() => FrontCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_Heading.m_Bias,
            x => FrontCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_Heading.m_Bias = x,
            value, duration).SetEase(Ease.InQuad);

        //StartCoroutine(RotateCameraIncrementValue(value, duration));
    }
    IEnumerator RotateCameraIncrementValue(float target, float t)
    {
        float value = 0;
        float time = t * Time.deltaTime;
        // Loop selama nilai belum mencapai target
        while (value < target)
        {
            // Hitung nilai baru
            value += (target - value) / time;
            FrontCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_Heading.m_Bias = value;

            // Cek apakah nilai sudah mencapai target
            if (value >= target)
            {
                // Hentikan loop
                break;
            }

            // Tampilkan nilai saat ini
            Debug.Log(value);

            // Tunda coroutine
            yield return null;
        }
    }

}
