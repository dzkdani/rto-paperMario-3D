using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoInteraction : MonoBehaviour
{
    public GameObject notif;
    // Start is called before the first frame update
    void Start() 
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        notif.SetActive(true);
    }

    private void OnTriggerExit(Collider other) {
        notif.SetActive(false);
    }
}
