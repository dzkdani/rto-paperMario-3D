using DG.Tweening;
using UnityEngine;

public enum PlayerDirection
{
    left,
    right
}

public enum PlayerFacing
{
    up,
    down
}

public enum PlayerState
{
    idle,
    walk,
    interact,
    teleport
}

public class PlayerController : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] private GameManager gameManager;

    [Header("Data")]
    [SerializeField] private PlayerData PlayerData;

    [Header("Player Components")]
    [SerializeField] private PlayerState currentState;
    [SerializeField] private PlayerDirection currentDirection;
    [SerializeField] private PlayerFacing currentFacing;
    [SerializeField] private GameObject notifMark;
    [SerializeField] private GameObject sprite;
    [SerializeField] private bool canMove;
    [SerializeField] private bool canInteract;
    [SerializeField] private bool onTeleport = false;

    private Ease flipEase;
    private float flipDuration;
    private float smooth;
    private float speed;

    private Rigidbody rb;
    private float horizontalMove;
    private float verticalMove;
    private Vector3 movement;
    private Vector3 currentVelocity;
    private bool isRotate;

    public bool CanMove { get { return canMove; } set { canMove = value; } }
    public bool CanInteract { get { return canInteract; } set { canInteract = value; } }
    public bool OnTeleport { get { return onTeleport; } set { onTeleport = value; } }

    public IInteractable Interactable { get; set; }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        sprite = transform.GetChild(0).gameObject;
    }

    void Start()
    {
        InitPlayer();
    }

    public void InitPlayer()
    {
        //load player
        LoadPlayerData();
        //player init state
        canMove = true;
        canInteract = true;
        isRotate = false;
        onTeleport = false;
        currentState = PlayerState.idle;
        currentFacing = PlayerFacing.down;
        currentDirection = PlayerDirection.left;
        SetAnimation(currentState.ToString() + "_" + currentFacing.ToString() + "_" + currentDirection.ToString());
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
        if (!onTeleport)
        {
            GetInput();
            SetSprite();
            NotifRotation();
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    private void GetInput()
    {
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");
        movement = new Vector3(horizontalMove, 0, verticalMove).normalized;

        if (Input.GetKeyDown(KeyCode.F) && CanInteract)
            Interact();
        if (Input.GetKeyDown(KeyCode.Q))
            Debug.Log($"Rot Cam to Left");
        if (Input.GetKeyDown(KeyCode.E))
            Debug.Log($"Rot Cam to Right");
    }

    public void SetAnimation(string Animation) => sprite.GetComponent<PlayerAnimation>().PlayAnimation(Animation);
    public void SetPlayerPos(Vector3 pos) => transform.position = pos;
    public Vector3 GetPlayerPos() => transform.position;

    #region Movement
    private void Move()
    {
        if (!CanMove && !isRotate)
            return;
        currentState = movement != Vector3.zero ? PlayerState.walk : PlayerState.idle;
        Vector3 targetPos = transform.position + (movement * speed * Time.deltaTime);
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref currentVelocity, smooth);
        SetAnimation(currentState.ToString() + "_" + currentFacing.ToString() + "_" + "left");
    }
    #endregion

    #region Sprite
    private void RotateSprite()
    {
        isRotate = true;
        transform.DORotate(new Vector3(0, transform.eulerAngles.y + 180f, 0), flipDuration).From(transform.rotation.eulerAngles).SetEase(flipEase)
            .OnComplete(() => { isRotate = false; });
    }

    private void SetSprite()
    {
        SpriteRotation();
        SpriteDirection();
    }

    private void SpriteDirection()
    {
        if (movement == Vector3.zero)
            return;

        if (movement.z > 0f)
            currentFacing = PlayerFacing.up;
        if (movement.z < 0f)
            currentFacing = PlayerFacing.down;

        string _anim = "walk" + "_" + currentFacing.ToString() + "_" + PlayerDirection.left;
        SetAnimation(_anim);
    }

    private void SpriteRotation()
    {
        if (!canInteract)
            return;
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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Interactable>(out Interactable obj))
        {
            Interactable = null;
            notifMark.SetActive(false);
        }
    }
    #endregion
}
