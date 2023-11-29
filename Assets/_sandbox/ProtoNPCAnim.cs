using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoNPCAnim : MonoBehaviour
{
    float camRotY;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().flipX = true;
        camRotY = Camera.main.transform.rotation.y;
        Debug.Log($"get camrotY : {camRotY}");
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0,camRotY,0);
    }
}
