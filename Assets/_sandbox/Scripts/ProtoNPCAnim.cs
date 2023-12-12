using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoNPCAnim : MonoBehaviour
{
    [SerializeField] private Animator anim;
    float camRotY;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        camRotY = Camera.main.transform.rotation.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0,camRotY,0);
    }
}
