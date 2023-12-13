using TOI2D;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;
    public PlayerController player;
    public TeleportSystem teleportSystem;
    public DialogueUI dialogueUI;
    public CameraController cameraController;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        player = FindObjectOfType<PlayerController>();
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
}
