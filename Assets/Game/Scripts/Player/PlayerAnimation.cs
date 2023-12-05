using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;

    void Awake() {
        anim = GetComponent<Animator>();    
    }

    public void PlayAnimation(string _state)
    {
        anim.Play(_state);
    }

    public void SetSprite()
    {
        //if nothing, init sprite
        
        //else, set expression sprite
    }

    public void SetReaction()
    {
        //set & animate reaction
    }
}
