using DG.Tweening;
using UnityEngine;

public enum PlayerDirection
{
    left,
    right
}

public enum PlayerFacing
{
    front,
    back
}

public class PlayerController : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] private GameManager gameManager;

    [Header("Data")]
    [SerializeField] private PlayerData PlayerData;

    [Header("Player Components")]
    [SerializeField] private GameObject playerSprite;
    [SerializeField] private PlayerDirection currentDirection;
    [SerializeField] private PlayerFacing currentFacing;
    [SerializeField] private GameObject notifMark;
    [SerializeField] private bool canMove;
    [SerializeField] private bool canInteract;
    private PlayerAnimation playerAnimation;
    private Ease flipEase;
    private float flipDuration;
    private float smooth;
    private float speed;
    private bool isWalk;

    public bool CanMove { get { return canMove; } set { canMove = value; } }
    public bool CanInteract { get { return canInteract; } set { canInteract = value; } }

    private Rigidbody rb;
    private Animator anim;
    private float horizontalMove;
    private float verticalMove;
    private Vector3 currentVelocity;
    private bool isRotate;
    public IInteractable Interactable { get; set; }

    void Awake()
    {
        // rb = GetComponent<Rigidbody>();
        playerAnimation = playerSprite.GetComponent<PlayerAnimation>();
    }

    void Start()
    {
        InitPlayer();
    }

    private void InitPlayer()
    {
        //load data
        LoadPlayerData();

        //player init state
        CanMove = true;
        isWalk = false;
        CanInteract = true;
        isRotate = false;
        currentDirection = PlayerDirection.left;
    }

    private void LoadPlayerData()
    {
        speed = PlayerData.PlayerSpeed;
        smooth = PlayerData.Smooth;
        flipDuration = PlayerData.Flip;
        flipEase = PlayerData.PlayerEase;
    }

    void Update()
    {
        GetInput();
        SetSprite();
        NotifRotation();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void GetInput()
    {
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.F) && CanInteract)
            Interact();
    }

    public void SetPlayerPos(Vector3 pos) => transform.position = pos;
    
    public Vector3 GetPlayerPos() => transform.position;

    #region Movement

    private void Move()
    {
        Vector3 move = transform.position + (speed * Time.deltaTime * new Vector3(horizontalMove, 0, verticalMove));
        if (CanMove)
        {
            transform.position = Vector3.SmoothDamp(transform.position, move, ref currentVelocity, smooth);
            isWalk = true;
        }
    }
    private void RotateSprite()
    {
        isRotate = true;
        transform.DORotate(new Vector3(0, transform.eulerAngles.y + 180f, 0), flipDuration).From(transform.rotation.eulerAngles).SetEase(flipEase)
            .OnComplete(() => { isRotate = false; });
    }

    private void SetSprite()
    {
        SpriteRotation();
        SpriteAnimation();
    }

    private void SpriteAnimation()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            
        }
    }

    private void SpriteRotation()
    {
        if (isRotate)
            return;

        if (Input.GetKeyDown(KeyCode.A))
        {
            //left dir
            if (currentDirection == PlayerDirection.left)
                return;
            else
            {
                currentDirection = PlayerDirection.left;
                RotateSprite();
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            //right dir
            if (currentDirection == PlayerDirection.right)
                return;
            else
            {
                currentDirection = PlayerDirection.right;
                RotateSprite();
            }
        }
    }
    #endregion

    #region Interaction

    private void NotifRotation()
    {
        if (notifMark.activeInHierarchy)
            notifMark.transform.Rotate(0, 1, 0);
    }

    private void Interact()
    {
        Interactable?.Interact();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Interactable>(out Interactable obj))
        {
            Interactable = obj;
            notifMark.SetActive(true);
            Debug.Log("Player in radius of interactable");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Interactable>(out Interactable obj))
        {
            Interactable = null;
            notifMark.SetActive(false);
            Debug.Log("Player out radius of interactable");
        }
    }
    #endregion
}

public static class Extention
{
    public static Vector2 ToVector2 (this Vector3 move) => new Vector2(move.x, move.z);
}
