using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileLifeCheck : MonoBehaviour{
    
    [HideInInspector]public int speed;
    [HideInInspector]public int hitStunFrames;
    [HideInInspector]public int blockHitStunFrames;
    [HideInInspector]public int damage;

    private bool attackDefended;

    private Rigidbody2D rb2d;
    [HideInInspector]public playerPlataformerController ownPlayerScript;

    void Start(){
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update(){
        if (ownPlayerScript.isLeft){
            rb2d.velocity = new Vector2(speed, 0);
        }
        else{
            rb2d.velocity = new Vector2(-speed, 0);
        }

        Destroy(gameObject, 5);
    }

    private void OnTriggerStay2D(Collider2D other){
        var otherPlayer = other.transform.root.GetChild(0).GetComponent<playerPlataformerController>();

        if (ownPlayerScript.isPlayer1 != otherPlayer.isPlayer1){
            switch (other.tag){
                case "Head":
                    if (!otherPlayer.isBlockingLow && !otherPlayer.isBlockingHigh){
                        otherPlayer.gotHitHeadStart();
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
                    if (!otherPlayer.isBlockingLow && !otherPlayer.isBlockingHigh){
                        otherPlayer.gotHitTorsoStart();
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
                    if (!otherPlayer.isBlockingLow && !otherPlayer.isBlockingHigh){
                        otherPlayer.gotHitLegStart();
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
                otherPlayer.addHitStunBlock(blockHitStunFrames);
            }
            else{
                otherPlayer.addHitStun(hitStunFrames);
            }
            

            if (!attackDefended){
                otherPlayer.changeHealth(-damage);
            }
            else{
                otherPlayer.changeHealth((int) -(damage * 0.1f));
            }
            
            Destroy(gameObject);
        }
    }
    
}