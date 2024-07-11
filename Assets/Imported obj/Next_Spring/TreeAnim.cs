using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeAnim : MonoBehaviour
{
    Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void EndFallAnimation()
    {
        _animator.SetBool("Falling", false);
        gameObject.layer = 6;
        transform.rotation = new Quaternion(0, 0, 0, 0);
        gameObject.SetActive(false);
    }
}
