using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileLifeCheck : MonoBehaviour{
    
    [HideInInspector]public int speed;
    [HideInInspector]public int hitStunFrames;
    [HideInInspector]public int blockHitStunFrames;
    [HideInInspector]public int damage;

    private bool goingLeft;
    private bool attackDefended;

    [Header("Particle effect")] 
    public Color mainColor;
    public ParticleSystem projectileDeath;
    
    [HideInInspector]public ParticleSystem blood;

    private Rigidbody2D rb2d;
    [HideInInspector]public playerPlataformerController ownPlayerScript;

    void Start(){
        rb2d = GetComponent<Rigidbody2D>();

        ownPlayerScript.liveProjectile = true;
        
        if (ownPlayerScript.isLeft){
            goingLeft = false;
        }
        else{
            goingLeft = true;
        }

        var partcile = transform.GetChild(0).GetComponent<ParticleSystem>();

        ParticleSystem.MainModule mainModule = partcile.main;

        mainModule.startColor = mainColor;

        Destroy(gameObject, 2);
    }

    void Update(){
        if (!goingLeft){
            rb2d.velocity = new Vector2(speed, 0);
        }
        else{
            rb2d.velocity = new Vector2(-speed, 0);
        }

    }

    private void OnTriggerStay2D(Collider2D other){
        
        if (!other.GetComponent<projectileLifeCheck>() && other.gameObject.layer == 11){
            
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
                    SpawnBlood();
//                    otherPlayer.StopAllAttack();
                    otherPlayer.lastHitTaken = hitStunFrames;
                    otherPlayer.addHitStun(hitStunFrames);
                }


                if (!attackDefended){
                    otherPlayer.changeHealth(-damage);
                }
                else{
                    otherPlayer.changeHealth((int) -(damage * 0.1f));
                }

                SpawnDeathParticle();
                Destroy(gameObject);
                ownPlayerScript.liveProjectile = false;
                
            }
            
        }
        else if (other.GetComponent<projectileLifeCheck>()){
            SpawnDeathParticle();
            Destroy(gameObject);
            ownPlayerScript.liveProjectile = false;
        }

    }

    private void SpawnDeathParticle(){
        var deathParticle = Instantiate(projectileDeath, transform.position, Quaternion.identity);

        ParticleSystem.MainModule deathColor = deathParticle.main;

        deathColor.startColor = mainColor;

        if (!goingLeft){
            deathParticle.transform.localScale = new Vector3(1, 1, 1);
        }
        else{
            deathParticle.transform.localScale = new Vector3(-1, 1, 1);
        }

    }

    private void SpawnBlood(){
        if (!goingLeft){
            var bloodParticle = Instantiate(blood, new Vector3(transform.position.x + 2.5f, transform.position.y), Quaternion.identity);
            
            bloodParticle.transform.localScale = new Vector3(1, 1, 1);
        }
        else{
            var bloodParticle = Instantiate(blood, new Vector3(transform.position.x - 2.5f, transform.position.y), Quaternion.identity);
            
            bloodParticle.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void OnDestroy(){
        ownPlayerScript.liveProjectile = false;
    }
}