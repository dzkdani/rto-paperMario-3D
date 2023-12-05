using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private readonly string[] idles = {"idle_btmLeft", "idle_topLeft"};
    private readonly string[] walks = {"walk_btmLeft", "walk_topLeft"};
    private string[] currentAnims;

    void Awake() {
        anim = GetComponent<Animator>();    
    }

    void Start() {
        InitAnim();
    }

    private void InitAnim()
    {
        anim.Play(idles[0]);
    }

    public void SetAnimation(Vector2 direction)
    {
        if (direction.magnitude < 0.01f)
            currentAnims = idles;
        else
            currentAnims = walks;

        Debug.Log($"current plyr dir : {direction}, {direction.magnitude}");
    }
}
