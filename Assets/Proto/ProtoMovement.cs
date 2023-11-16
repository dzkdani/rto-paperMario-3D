using UnityEngine;
using DG.Tweening;

public class ProtoMovement : MonoBehaviour
{
    [SerializeField] private float spd = 10f;
    private Rigidbody rb;
    private SpriteRenderer sprite;
    private float horizontalMove;
    private float verticalMove;
    
    private void Awake() {
        rb = GetComponent<Rigidbody>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.R))
        {
            // Flip();
            transform.DOLocalRotate(new Vector3(0, transform.eulerAngles.y + 180f, 0), 0.3f).From(transform.rotation.eulerAngles).OnComplete(Flip);
        }

        transform.Rotate(0,1,0);
    }

    private void Flip()
    {
        sprite.flipX = !sprite.flipX;
    }

    private void FixedUpdate() {
        transform.position += new Vector3(horizontalMove, 0, verticalMove) * spd * Time.deltaTime;
    }
}
