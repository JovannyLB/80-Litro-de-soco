using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCheck : MonoBehaviour{

    private bool ownPlayer1;
    private GameObject enemy;

    public int repeatAttackHead;
    public int repeatAttackTorso;
    public int repeatAttackLegs;
    public int repeatBlockHigh;
    public int repeatBlockLow;
    
    public string lastHit;
    
    public bool hasHit;
    public bool isHitting;
    public int hitStunFrames;
    public int blockHitStunFrames;
    public int pushBackStrengh = 12;
    private int pushBackTotal = 1;
    private int pushBackAtual;

    public bool overHeadAttack;
    public bool lowAttack;
    public int damage;
    public bool attackDefended;
    
    
    void Start(){
        ownPlayer1 = transform.root.GetChild(0).GetComponent<playerPlataformerController>().isPlayer1;
    }

    void FixedUpdate(){
        /*
        if (repeatAttackHead != 0 && !enemy.transform.root.GetChild(0).GetComponent<playerPlataformerController>().beenHitHead){
            enemy.transform.root.GetChild(0).GetComponent<playerPlataformerController>().gotHitHeadStart();
            enemy.transform.root.GetChild(0).GetComponent<playerPlataformerController>().addHitStun(hitStunFrames);
            setPushBack();
            repeatAttackHead--;
        }
        
        if (repeatAttackTorso != 0 && !enemy.transform.root.GetChild(0).GetComponent<playerPlataformerController>().beenHitTorso){
            enemy.transform.root.GetChild(0).GetComponent<playerPlataformerController>().gotHitTorsoStart();
            enemy.transform.root.GetChild(0).GetComponent<playerPlataformerController>().addHitStun(hitStunFrames);
            setPushBack();
            repeatAttackTorso--;
        }
        
        if (repeatAttackLegs != 0 && !enemy.transform.root.GetChild(0).GetComponent<playerPlataformerController>().beenHitLeg){
            enemy.transform.root.GetChild(0).GetComponent<playerPlataformerController>().gotHitLegStart();
            enemy.transform.root.GetChild(0).GetComponent<playerPlataformerController>().addHitStun(hitStunFrames);
            setPushBack();
            repeatAttackLegs--;
        }
        */

        if (pushBackAtual > 0){
            enemy.transform.root.GetChild(0).GetComponent<playerPlataformerController>().targetVelocity = new Vector2(1, 0) * pushBackStrengh;
            pushBackAtual--;
        }
    }

    private void OnTriggerStay2D(Collider2D other){
        var otherPlayer = other.transform.root.GetChild(0).GetComponent<playerPlataformerController>();
        
        if (ownPlayer1 != otherPlayer.isPlayer1 && isHitting && !hasHit){
            //if (other.tag != lastHit){
                switch (other.tag){
                    case "Head":
                        if (!otherPlayer.isBlockingHigh && !otherPlayer.isBlockingLow){
                            otherPlayer.gotHitHeadStart();
                            setPushBack();
                            attackDefended = false;
                        } else if (otherPlayer.isBlockingLow && overHeadAttack){
                            otherPlayer.gotHitHeadStart();
                            setPushBack();
                            attackDefended = false;
                        } else if (otherPlayer.isBlockingHigh && lowAttack){
                            otherPlayer.gotHitHeadStart();
                            setPushBack();
                            attackDefended = false;
                        }
                        else{
                            if (otherPlayer.isBlockingHigh){
                                otherPlayer.gotHitBlockHighStart();
                            }
                            else if (otherPlayer.isBlockingLow){
                                otherPlayer.gotHitBlockLowStart();
                            }
                            attackDefended = true;
                        }
                        break;
                    case "Torso":
                        if (!otherPlayer.isBlockingHigh && !otherPlayer.isBlockingLow){
                            otherPlayer.gotHitTorsoStart();
                            setPushBack();
                            attackDefended = false;
                        } else if (otherPlayer.isBlockingLow && overHeadAttack){
                            otherPlayer.gotHitTorsoStart();
                            setPushBack();
                            attackDefended = false;
                        } else if (otherPlayer.isBlockingHigh && lowAttack){
                            otherPlayer.gotHitTorsoStart();
                            setPushBack();
                            attackDefended = false;
                        }
                        else{
                            if (otherPlayer.isBlockingHigh){
                                otherPlayer.gotHitBlockHighStart();
                            }
                            else if (otherPlayer.isBlockingLow){
                                otherPlayer.gotHitBlockLowStart();
                            }
                            attackDefended = true;
                        }
                        break;
                    case "Legs":
                        if (!otherPlayer.isBlockingHigh && !otherPlayer.isBlockingLow){
                            otherPlayer.gotHitLegStart();
                            setPushBack();
                            attackDefended = false;
                        } else if (otherPlayer.isBlockingLow && overHeadAttack){
                            otherPlayer.gotHitLegStart();
                            setPushBack();
                            attackDefended = false;
                        } else if (otherPlayer.isBlockingHigh && lowAttack){
                            otherPlayer.gotHitLegStart();
                            setPushBack();
                            attackDefended = false;
                        }
                        else{
                            if (otherPlayer.isBlockingHigh){
                                otherPlayer.gotHitBlockHighStart();
                            }
                            else if (otherPlayer.isBlockingLow){
                                otherPlayer.gotHitBlockLowStart();
                            }
                            attackDefended = true;
                        }
                        break;
                    default:
                        print("Something went wrong");
                        break;
                }

                if (attackDefended){
                    otherPlayer.addHitStun(blockHitStunFrames);
                }
                else{
                    otherPlayer.addHitStun(hitStunFrames);
                }
            //}
            /*else{
                switch (other.tag){
                    case "Head":
                        if (!otherPlayer.isBlockingHigh && !otherPlayer.isBlockingLow){
                            repeatAttackHead++;
                            attackDefended = false;
                        } else if (otherPlayer.isBlockingLow && overHeadAttack){
                            repeatAttackHead++;
                            attackDefended = false;
                        } else if (otherPlayer.isBlockingHigh && lowAttack){
                            repeatAttackHead++;
                            attackDefended = false;
                        }
                        else{
                            if (otherPlayer.isBlockingHigh){
                                otherPlayer.gotHitBlockHighStart();
                            }
                            else if (otherPlayer.isBlockingLow){
                                otherPlayer.gotHitBlockLowStart();
                            }
                            attackDefended = true;
                        }
                        break;
                    case "Torso":
                        if (!otherPlayer.isBlockingHigh && !otherPlayer.isBlockingLow){
                            repeatAttackTorso++;
                            attackDefended = false;
                        } else if (otherPlayer.isBlockingLow && overHeadAttack){
                            repeatAttackTorso++;
                            attackDefended = false;
                        } else if (otherPlayer.isBlockingHigh && lowAttack){
                            repeatAttackTorso++;
                            attackDefended = false;
                        }
                        else{
                            if (otherPlayer.isBlockingHigh){
                                otherPlayer.gotHitBlockHighStart();
                            }
                            else if (otherPlayer.isBlockingLow){
                                otherPlayer.gotHitBlockLowStart();
                            }
                            attackDefended = true;
                        }
                        break;
                    case "Legs":
                        if (!otherPlayer.isBlockingHigh && !otherPlayer.isBlockingLow){
                            repeatAttackLegs++;
                            attackDefended = false;
                        } else if (otherPlayer.isBlockingLow && overHeadAttack){
                            repeatAttackLegs++;
                            attackDefended = false;
                        } else if (otherPlayer.isBlockingHigh && lowAttack){
                            repeatAttackLegs++;
                            attackDefended = false;
                        }
                        else{
                            if (otherPlayer.isBlockingHigh){
                                otherPlayer.gotHitBlockHighStart();
                            }
                            else if (otherPlayer.isBlockingLow){
                                otherPlayer.gotHitBlockLowStart();
                            }
                            attackDefended = true;
                        }
                        break;
                    default:
                        print("Something went wrong");
                        break;
                }
            }*/

            lastHit = other.tag;
            hasHit = true;
            enemy = other.gameObject;

            if (!attackDefended){
                otherPlayer.changeHealth(-damage);
            }
            else{
                otherPlayer.changeHealth((int) -(damage * 0.1f));
            }

        }
    }

    public void IsHittingTrue(){
        isHitting = true;
    }

    public void IsHittingFalse(){
        isHitting = false;
        hasHit = false;
    }

    public void setPushBack(){
        pushBackAtual = pushBackTotal;
    }

}
