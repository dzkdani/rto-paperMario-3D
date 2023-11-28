using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoCamera : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _smoothTime;
    private Vector3 _currentVelocity;

    private Vector3 followPos = Vector3.zero;  

    // Start is called before the first frame update
    void Start() 
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        followPos = transform.position;
        followPos.x = _target.position.x;
        followPos.y = _target.position.y + 5f;
        followPos.z = _target.position.z - 15f;
        transform.position = Vector3.SmoothDamp(transform.position, followPos, ref _currentVelocity, _smoothTime * Time.deltaTime);
    }
}
