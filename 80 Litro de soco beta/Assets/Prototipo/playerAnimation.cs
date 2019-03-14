using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAnimation : MonoBehaviour{

    protected Animator animator;

    public bool currentlyPunching;
    
    // Start is called before the first frame update
    void Start(){
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.JoystickButton1)){
            currentlyPunching = true;
        }

        animator.SetBool("soco", currentlyPunching);
    }

    void StopPunching(){
        currentlyPunching = false;
    }
}
