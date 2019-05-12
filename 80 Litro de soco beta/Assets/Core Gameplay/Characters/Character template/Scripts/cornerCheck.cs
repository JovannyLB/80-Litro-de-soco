using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cornerCheck : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other){
        if (other.tag == "Wall"){
            transform.root.GetChild(0).GetComponent<playerPlataformerController>().inCorner = true;
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if (other.tag == "Wall"){
            transform.root.GetChild(0).GetComponent<playerPlataformerController>().inCorner = false;
        }
    }
}
