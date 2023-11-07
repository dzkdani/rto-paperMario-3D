using UnityEngine;
using DG.Tweening;

public class ProtoMovement : MonoBehaviour
{
    [SerializeField] private float spd = 10f;
    private Rigidbody rb;
    private float horizontalMove;
    private float verticalMove;
    
    private void Awake() {
        rb = GetComponent<Rigidbody>();
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
            transform.DORotate(new Vector3(0, transform.eulerAngles.y + 180f, 0), 0.2f).From(transform.rotation.eulerAngles);
        }

    }

    private void FixedUpdate() {
        transform.position += new Vector3(horizontalMove, 0, verticalMove) * spd * Time.deltaTime;
    }
}
