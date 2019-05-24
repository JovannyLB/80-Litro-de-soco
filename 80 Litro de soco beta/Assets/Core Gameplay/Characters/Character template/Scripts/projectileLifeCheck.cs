using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileLifeCheck : MonoBehaviour{
    
    [HideInInspector]public int speed;
    [HideInInspector]public int hitStunFrames;
    [HideInInspector]public int blockHitStunFrames;
    [HideInInspector]public int damage;
    [HideInInspector]public ParticleSystem blood;

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
                        
                        spawnParticle(otherPlayer, 0);
                        
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
                        
                        spawnParticle(otherPlayer, 1);
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
                        
                        spawnParticle(otherPlayer, 2);

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
    
    public void spawnParticle(playerPlataformerController otherPlayer, int placeStruck){
        var currentBlood = Instantiate(blood, otherPlayer.transform.root.GetChild(2).GetChild(placeStruck).GetComponent<BoxCollider2D>().transform.position, Quaternion.identity);
        currentBlood.transform.localScale = new Vector3(transform.root.GetChild(0).transform.localScale.x, 1, 1);
        
        ParticleSystem.ShapeModule editableShape = currentBlood.shape;
        editableShape.position = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));

        ParticleSystem.EmissionModule editableCount = currentBlood.emission;
        editableCount.SetBurst(0, new ParticleSystem.Burst(0, damage / 2));

        ParticleSystem.VelocityOverLifetimeModule editableSpeed = currentBlood.velocityOverLifetime;
        editableSpeed.speedModifier =  Mathf.Ceil(damage / 12f);

        ParticleSystem.CollisionModule editableBounce = currentBlood.collision;
        editableBounce.bounce = new ParticleSystem.MinMaxCurve(currentBlood.collision.bounce.constantMin * (damage / 12f) > 0.75f ? 0.75f : currentBlood.collision.bounce.constantMin * (damage / 12f), currentBlood.collision.bounce.constantMax * (damage / 12f) > 1.5f ? 1.5f : currentBlood.collision.bounce.constantMax * (damage / 12f));
    }
    
}