using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Showcase : PhysicsObject{

    private Animator animator;
    private int chance;
    
    void Start(){
        animator = GetComponent<Animator>();
        chance = Random.Range(0, 2);
    }

    void Update()
    {
        animator.SetBool("grounded", grounded);
        animator.SetInteger("chance", chance);
    }

    public void DestroyFade(){
        GetComponent<SpriteRenderer>().DOColor(Color.clear, 1);
        Destroy(gameObject, 1);
    }
}
