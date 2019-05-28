using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableScript : MonoBehaviour
{

    private bool isLeft;

    void Start(){
        
        if (transform.position.x < 0){
            isLeft = true;
        }
        else{
            isLeft = false;
            GetComponent<SpriteRenderer>().flipX = true;
        }

    }
    
}
