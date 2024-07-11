using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniamtionEvent : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    public void EndDrinkAnimation()
    {
        animator.SetBool("Drinking", false);
    }
    
    public void EndEatAnimation()
    {
        animator.SetBool("Eating", false);
    }
    
    public void EndCutAnimation()
    {
        animator.SetBool("Attack", false);
    }
}
