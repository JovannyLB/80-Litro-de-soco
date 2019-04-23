using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCheck : MonoBehaviour{

    private bool ownPlayer1;
    private GameObject enemy;

    public int repeatAttackHead;
    public int repeatAttackTorso;
    public int repeatAttackLegs;
    
    public string lastHit;
    
    public bool hasHit;
    public bool isHitting;
    public int hitStunFrames;
    
    
    void Start(){
        ownPlayer1 = transform.root.GetChild(0).GetComponent<playerPlataformerController>().player1;
    }

    void FixedUpdate(){
        if (repeatAttackHead != 0 && !enemy.transform.root.GetChild(0).GetComponent<playerPlataformerController>().beenHitHead){
            enemy.transform.root.GetChild(0).GetComponent<playerPlataformerController>().gotHitHeadStart();
            enemy.transform.root.GetChild(0).GetComponent<playerPlataformerController>().addHitStun(hitStunFrames);
            repeatAttackHead--;
        }
        
        if (repeatAttackTorso != 0 && !enemy.transform.root.GetChild(0).GetComponent<playerPlataformerController>().beenHitTorso){
            enemy.transform.root.GetChild(0).GetComponent<playerPlataformerController>().gotHitTorsoStart();
            enemy.transform.root.GetChild(0).GetComponent<playerPlataformerController>().addHitStun(hitStunFrames);
            repeatAttackTorso--;
        }
        
        if (repeatAttackLegs != 0 && !enemy.transform.root.GetChild(0).GetComponent<playerPlataformerController>().beenHitLeg){
            enemy.transform.root.GetChild(0).GetComponent<playerPlataformerController>().gotHitLegStart();
            enemy.transform.root.GetChild(0).GetComponent<playerPlataformerController>().addHitStun(hitStunFrames);
            repeatAttackLegs--;
        }
    }

    private void OnTriggerStay2D(Collider2D other){
        if (ownPlayer1 != other.transform.root.GetChild(0).GetComponent<playerPlataformerController>().player1 && isHitting && !hasHit){
            if (other.tag != lastHit){
                switch (other.tag){
                    case "Head":
                        other.transform.root.GetChild(0).GetComponent<playerPlataformerController>().gotHitHeadStart();
                        break;
                    case "Torso":
                        other.transform.root.GetChild(0).GetComponent<playerPlataformerController>().gotHitTorsoStart();
                        break;
                    case "Legs":
                        other.transform.root.GetChild(0).GetComponent<playerPlataformerController>().gotHitLegStart();
                        break;
                    default:
                        print("Something went wrong");
                        break;
                }

                other.transform.root.GetChild(0).GetComponent<playerPlataformerController>().addHitStun(hitStunFrames);
            }
            else{
                switch (other.tag){
                    case "Head":
                        repeatAttackHead++;
                        break;
                    case "Torso":
                        repeatAttackTorso++;
                        break;
                    case "Legs":
                        repeatAttackLegs++;
                        break;
                    default:
                        print("Something went wrong");
                        break;
                }
            }

            lastHit = other.tag;
            hasHit = true;
            enemy = other.gameObject;
        }
    }

    public void IsHittingTrue(){
        isHitting = true;
    }

    public void IsHittingFalse(){
        isHitting = false;
        hasHit = false;
    }

}
