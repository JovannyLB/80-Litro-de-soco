using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class jumpOverCheck : MonoBehaviour{

    [Header("Makes is so the player is able to jump to the other side without getting stuck")]
    
    [HideInInspector]public bool onTop;
    private GameObject enemy;

    private void Start(){
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject index in enemys){
            if (index.GetComponent<playerPlataformerController>()){
                if (index.GetComponent<playerPlataformerController>().isPlayer1 != transform.root.GetChild(0).GetComponent<playerPlataformerController>().isPlayer1){
                    enemy = index;
                }
            }
        }
    }

    private void FixedUpdate(){
        transform.root.GetChild(0).GetComponent<playerPlataformerController>().onTopP = onTop;
        
        if (onTop){
            
            enemy.GetComponent<playerPlataformerController>().beingJumpedOver = true;
            transform.root.GetChild(0).GetComponent<playerPlataformerController>().jumpingOver = true;
            
            if (transform.root.GetChild(0).GetComponent<playerPlataformerController>().isLeft){

                transform.root.GetChild(0).GetComponent<playerPlataformerController>().targetVelocity = new Vector2(1, 0) * 20;

//                if (!transform.root.GetChild(0).GetComponent<playerPlataformerController>().inCorner){
//                    transform.root.GetChild(0).GetComponent<playerPlataformerController>().targetVelocity = new Vector2(1, 0) * 20;
//                }
//                else if (transform.root.GetChild(0).GetComponent<playerPlataformerController>().inCorner && !enemy.GetComponent<playerPlataformerController>().inCorner){
//                    transform.root.GetChild(0).GetComponent<playerPlataformerController>().targetVelocity = new Vector2(1, 0) * -20;
//                }

                if (enemy.GetComponent<playerPlataformerController>().inCorner){
                    enemy.GetComponent<playerPlataformerController>().StopAllAttack();
                    enemy.GetComponent<playerPlataformerController>().targetVelocity = new Vector2(1, 0) * -60;
                } else{
                    enemy.GetComponent<playerPlataformerController>().targetVelocity = new Vector2(1, 0) * -20;
                }
            } else if (!transform.root.GetChild(0).GetComponent<playerPlataformerController>().isLeft){
                
                transform.root.GetChild(0).GetComponent<playerPlataformerController>().targetVelocity = new Vector2(1, 0) * -20;

//                if (!transform.root.GetChild(0).GetComponent<playerPlataformerController>().inCorner){
//                    transform.root.GetChild(0).GetComponent<playerPlataformerController>().targetVelocity = new Vector2(1, 0) * -20;
//                }
//                else if (transform.root.GetChild(0).GetComponent<playerPlataformerController>().inCorner && !enemy.GetComponent<playerPlataformerController>().inCorner){
//                    transform.root.GetChild(0).GetComponent<playerPlataformerController>().targetVelocity = new Vector2(1, 0) * 20;
//                }

                if (enemy.GetComponent<playerPlataformerController>().inCorner){
                    
                    enemy.GetComponent<playerPlataformerController>().StopAllAttack();
                    enemy.GetComponent<playerPlataformerController>().targetVelocity = new Vector2(1, 0) * 60;
                    
                } else{
                    
                    enemy.GetComponent<playerPlataformerController>().targetVelocity = new Vector2(1, 0) * 20;
                    
                }
            }

            if (enemy.GetComponent<playerPlataformerController>().beingJumpedOver && enemy.GetComponent<playerPlataformerController>().currentlyAttacking && !enemy.GetComponent<playerPlataformerController>().inCorner){
                enemy.GetComponent<playerPlataformerController>().targetVelocity = Vector2.zero;
            }

        }
        else if (enemy != null){
            enemy.GetComponent<playerPlataformerController>().beingJumpedOver = false;
            transform.root.GetChild(0).GetComponent<playerPlataformerController>().jumpingOver = false;
        }
        else{
            transform.root.GetChild(0).GetComponent<playerPlataformerController>().jumpingOver = false;
        }
        
        
//        if (onTop && transform.root.GetChild(0).GetComponent<playerPlataformerController>().inCorner && enemy.GetComponent<playerPlataformerController>().inCorner){
//            if (transform.root.GetChild(0).GetComponent<playerPlataformerController>().posX > enemy.GetComponent<playerPlataformerController>().posX){
//                transform.root.GetChild(0).GetComponent<playerPlataformerController>().targetVelocity = new Vector2(1, 0) * -20;
//                enemy.GetComponent<playerPlataformerController>().targetVelocity = new Vector2(1, 0) * 20;
//            }
//            else{
//                transform.root.GetChild(0).GetComponent<playerPlataformerController>().targetVelocity = new Vector2(1, 0) * 20;
//                enemy.GetComponent<playerPlataformerController>().targetVelocity = new Vector2(1, 0) * -20;
//            }
//        }
        
    }

    private void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.layer == 9){
            if (other.GetComponent<playerPlataformerController>().isPlayer1 != transform.root.GetChild(0).GetComponent<playerPlataformerController>().isPlayer1){
                onTop = true;
//                enemy = other.gameObject;
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
