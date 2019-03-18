using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAnimation : MonoBehaviour{
    protected Animator animator;
    protected playerPlataformerController plataformerController;

    public bool currentlyPunching;

    // Start is called before the first frame update
    void Start(){
        animator = GetComponent<Animator>();
        plataformerController = GetComponent<playerPlataformerController>();
    }

    // Update is called once per frame
    void Update(){
        if (Input.GetKey(KeyCode.JoystickButton1)){
            currentlyPunching = true;
        }

        animator.SetBool("soco", currentlyPunching);

        if (plataformerController.grounded){
            animator.SetFloat("speed", Math.Abs(plataformerController.targetVelocity.x));
        }
        else{
            animator.SetFloat("speed", 0);
        }
    }

    void StopPunching(){
        currentlyPunching = false;
    }
}