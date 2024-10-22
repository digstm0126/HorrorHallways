using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimation : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayWalkAnimation()
    {
        animator.SetBool("isWalking", true);
    }

    public void StopWalkAnimation()
    {
        animator.SetBool("isWalking", false);
    }

    public void PlayAttackAnimation()
    {
        animator.SetTrigger("attack");
    }

    public void PlayDeathAnimation()
    {
        animator.SetTrigger("die");
    }
}
