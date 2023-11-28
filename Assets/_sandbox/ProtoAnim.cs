using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoAnim : MonoBehaviour
{
    [SerializeField] private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }
 
    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Vertical") > 0)
        {
            animator.Play("test_lt");
        }
        
        if (Input.GetAxis("Vertical") < 0)
        {
            animator.Play("test_lb");
        }
    }
}
