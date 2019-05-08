using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCheck : MonoBehaviour{

    private bool ownPlayer1;
    private GameObject enemy;
    
    private bool hasHit;
    private bool isHitting;
    [Header("Attack frame data")]
    public int hitStunFrames;
    public int blockHitStunFrames;
    public int pushBackStrengh = 12;
    public int damage;
    private int pushBackTotal = 1;
    private int pushBackAtual;
    private int pushBackAtualSelf;

    [Header("Attack type")]
    public bool overHeadAttack;
    public bool lowAttack;
    private bool attackDefended;
    
    
    void Start(){
        ownPlayer1 = transform.root.GetChild(0).GetComponent<playerPlataformerController>().isPlayer1;
    }

    void FixedUpdate(){
        if (pushBackAtual > 0 && transform.root.GetChild(0).GetComponent<playerPlataformerController>().isLeft){
            enemy.transform.root.GetChild(0).GetComponent<playerPlataformerController>().targetVelocity = new Vector2(1, 0) * pushBackStrengh;
            pushBackAtual--;
        } else if (pushBackAtual > 0 && !transform.root.GetChild(0).GetComponent<playerPlataformerController>().isLeft){
            enemy.transform.root.GetChild(0).GetComponent<playerPlataformerController>().targetVelocity = new Vector2(1, 0) * -pushBackStrengh;
            pushBackAtual--;
        }

        if (pushBackAtualSelf > 0 && transform.root.GetChild(0).GetComponent<playerPlataformerController>().isLeft){
            transform.root.GetChild(0).GetComponent<playerPlataformerController>().targetVelocity = new Vector2(1, 0) * -(pushBackStrengh / 2.5f);
            pushBackAtualSelf--;
        } else if (pushBackAtualSelf > 0 && !transform.root.GetChild(0).GetComponent<playerPlataformerController>().isLeft){
            transform.root.GetChild(0).GetComponent<playerPlataformerController>().targetVelocity = new Vector2(1, 0) * (pushBackStrengh / 2.5f);
            pushBackAtualSelf--;
        }
    }

    private void OnTriggerStay2D(Collider2D other){
        var otherPlayer = other.transform.root.GetChild(0).GetComponent<playerPlataformerController>();
        
        if (ownPlayer1 != otherPlayer.isPlayer1 && isHitting && !hasHit){
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
                            setPushBackSelf();
                        }
                        else if (otherPlayer.isBlockingLow){
                            otherPlayer.gotHitBlockLowStart();
                            setPushBackSelf();
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
                            setPushBackSelf();
                        }
                        else if (otherPlayer.isBlockingLow){
                            otherPlayer.gotHitBlockLowStart();
                            setPushBackSelf();
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
                            setPushBackSelf();
                        }
                        else if (otherPlayer.isBlockingLow){
                            otherPlayer.gotHitBlockLowStart();
                            setPushBackSelf();
                        }
                        attackDefended = true;
                    }
                    break;
                default:
                    print("Something went wrong");
                    break;
            }

            if (attackDefended){
                otherPlayer.addHitStunBlock(blockHitStunFrames);
            }
            else{
                otherPlayer.addHitStun(hitStunFrames);
            }
            
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

    public void setPushBackSelf(){
        pushBackAtualSelf = pushBackTotal;
    }

}
