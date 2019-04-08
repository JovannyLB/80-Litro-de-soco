using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCheck : MonoBehaviour{

    private bool ownPlayer1;

    public string lastHit;
    
    void Start(){
        ownPlayer1 = transform.root.GetChild(0).GetComponent<playerPlataformerController>().player1;
    }

    private void OnTriggerEnter2D(Collider2D other){
        if (ownPlayer1 != other.transform.root.GetChild(0).GetComponent<playerPlataformerController>().player1){
            lastHit = other.tag;
            print(name + " : " + other.transform.name + " : Enter");
        }
    }

    private void OnTriggerExit2D(Collider2D other){
        if (ownPlayer1 != other.transform.root.GetChild(0).GetComponent<playerPlataformerController>().player1){
            print(name + " : " + other.transform.name + " : Exit");
        }
    }
}
