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

    [HideInInspector]public ParticleSystem blood;
    
    
    void Start(){
        ownPlayer1 = transform.root.GetChild(0).GetComponent<playerPlataformerController>().isPlayer1;
    }

    void FixedUpdate(){
        if (pushBackAtual > 0 && transform.root.GetChild(0).GetComponent<playerPlataformerController>().isLeft){
            if (enemy.transform.root.GetChild(0).GetComponent<playerPlataformerController>().inCorner){
                transform.root.GetChild(0).GetComponent<playerPlataformerController>().targetVelocity = new Vector2(1, 0) * -(pushBackStrengh / 2.0f);
                pushBackAtual--;
            }
            else{
                enemy.transform.root.GetChild(0).GetComponent<playerPlataformerController>().targetVelocity = new Vector2(1, 0) * pushBackStrengh;
                pushBackAtual--;
            }
        } else if (pushBackAtual > 0 && !transform.root.GetChild(0).GetComponent<playerPlataformerController>().isLeft){
            if (enemy.transform.root.GetChild(0).GetComponent<playerPlataformerController>().inCorner){
                transform.root.GetChild(0).GetComponent<playerPlataformerController>().targetVelocity = new Vector2(1, 0) * (pushBackStrengh / 2.0f);
                pushBackAtual--;
            }
            else{
                enemy.transform.root.GetChild(0).GetComponent<playerPlataformerController>().targetVelocity = new Vector2(1, 0) * -pushBackStrengh;
                pushBackAtual--;
            }
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

                    spawnParticle(otherPlayer, 0);
                    
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
                    
                    spawnParticle(otherPlayer, 1);

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
                    
                    spawnParticle(otherPlayer, 2);

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

            FindObjectOfType<GameController>().CallHitStop(damage / 4f, 0.05f);

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

    public void spawnParticle(playerPlataformerController otherPlayer, int placeStruck){
        var currentBlood = Instantiate(blood, otherPlayer.transform.root.GetChild(2).GetChild(placeStruck).GetComponent<BoxCollider2D>().transform.position, Quaternion.identity);
        currentBlood.transform.localScale = new Vector3(transform.root.GetChild(0).transform.localScale.x, 1, 1);
        
        ParticleSystem.ShapeModule editableShape = currentBlood.shape;
        editableShape.position = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));

        ParticleSystem.EmissionModule editableCount = currentBlood.emission;
        editableCount.SetBurst(0, new ParticleSystem.Burst(0, damage / 2));

        ParticleSystem.VelocityOverLifetimeModule editableSpeed = currentBlood.velocityOverLifetime;
        editableSpeed.speedModifier =  Mathf.Ceil(pushBackStrengh / 12f);

        ParticleSystem.CollisionModule editableBounce = currentBlood.collision;
        editableBounce.bounce = new ParticleSystem.MinMaxCurve(currentBlood.collision.bounce.constantMin * (pushBackStrengh / 12f) > 0.75f ? 0.75f : currentBlood.collision.bounce.constantMin * (pushBackStrengh / 12f), currentBlood.collision.bounce.constantMax * (pushBackStrengh / 12f) > 1.5f ? 1.5f : currentBlood.collision.bounce.constantMax * (pushBackStrengh / 12f));
    }

}
