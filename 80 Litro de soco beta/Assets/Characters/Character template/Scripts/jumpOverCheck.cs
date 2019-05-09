using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpOverCheck : MonoBehaviour{

    [Header("Makes is so the player is able to jump to the other side without getting stuck")]
    
    [HideInInspector]public bool onTop;
    private GameObject enemy;

    private void FixedUpdate(){
        if (onTop){
            enemy.GetComponent<playerPlataformerController>().beingJumpedOver = true;
            transform.root.GetChild(0).GetComponent<playerPlataformerController>().jumpingOver = true;
            if (transform.root.GetChild(0).GetComponent<playerPlataformerController>().isLeft){
                transform.root.GetChild(0).GetComponent<playerPlataformerController>().targetVelocity = new Vector2(1, 0) * 20;
                if (enemy.GetComponent<playerPlataformerController>().inCorner){
                    enemy.GetComponent<playerPlataformerController>().targetVelocity = new Vector2(1, 0) * -60;
                }
                else{
                    enemy.GetComponent<playerPlataformerController>().targetVelocity = new Vector2(1, 0) * -20;
                }
            } else if (!transform.root.GetChild(0).GetComponent<playerPlataformerController>().isLeft){
                transform.root.GetChild(0).GetComponent<playerPlataformerController>().targetVelocity = new Vector2(1, 0) * -20;
                if (enemy.GetComponent<playerPlataformerController>().inCorner){
                    enemy.GetComponent<playerPlataformerController>().targetVelocity = new Vector2(1, 0) * 60;
                }
                else{
                    enemy.GetComponent<playerPlataformerController>().targetVelocity = new Vector2(1, 0) * 20;
                }
            }
        }
        else if (enemy != null){
            enemy.GetComponent<playerPlataformerController>().beingJumpedOver = false;
            transform.root.GetChild(0).GetComponent<playerPlataformerController>().jumpingOver = false;
        }
        else{
            transform.root.GetChild(0).GetComponent<playerPlataformerController>().jumpingOver = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.layer == 9){
            if (other.GetComponent<playerPlataformerController>().isPlayer1 != transform.root.GetChild(0).GetComponent<playerPlataformerController>().isPlayer1){
                onTop = true;
                enemy = other.gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other){
        if (other.gameObject.layer == 9){
            if (other.GetComponent<playerPlataformerController>().isPlayer1 != transform.root.GetChild(0).GetComponent<playerPlataformerController>().isPlayer1){
                onTop = false;
            }
        }
    }
}
