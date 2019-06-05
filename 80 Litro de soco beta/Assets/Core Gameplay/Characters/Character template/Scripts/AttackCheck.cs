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
    [HideInInspector]public ParticleSystem hitSplash;
    
    
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

        if (!transform.root.GetChild(0).GetComponent<playerPlataformerController>().currentlyAttacking){
            isHitting = false;
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
                        spawnParticle(otherPlayer, 0);
                        attackDefended = false;
                    } else if (otherPlayer.isBlockingLow && overHeadAttack){
                        otherPlayer.gotHitHeadStart();
                        setPushBack();
                        spawnParticle(otherPlayer, 0);
                        attackDefended = false;
                    } else if (otherPlayer.isBlockingHigh && lowAttack){
                        otherPlayer.gotHitHeadStart();
                        setPushBack();
                        spawnParticle(otherPlayer, 0);
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
                        spawnParticle(otherPlayer, 1);
                        attackDefended = false;
                    } else if (otherPlayer.isBlockingLow && overHeadAttack){
                        otherPlayer.gotHitTorsoStart();
                        setPushBack();
                        spawnParticle(otherPlayer, 1);
                        attackDefended = false;
                    } else if (otherPlayer.isBlockingHigh && lowAttack){
                        otherPlayer.gotHitTorsoStart();
                        setPushBack();
                        spawnParticle(otherPlayer, 1);
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
                        spawnParticle(otherPlayer, 2);
                        attackDefended = false;
                    } else if (otherPlayer.isBlockingLow && overHeadAttack){
                        otherPlayer.gotHitLegStart();
                        setPushBack();
                        spawnParticle(otherPlayer, 2);
                        attackDefended = false;
                    } else if (otherPlayer.isBlockingHigh && lowAttack){
                        otherPlayer.gotHitLegStart();
                        setPushBack();
                        spawnParticle(otherPlayer, 2);
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
                otherPlayer.lastHitTaken = hitStunFrames;
                otherPlayer.addHitStun(hitStunFrames);
            }
            
            hasHit = true;
            transform.root.GetChild(0).GetComponent<playerPlataformerController>().lastHitHasHit = true;
            transform.root.GetChild(0).GetComponent<playerPlataformerController>().targetVelocity = Vector2.zero;
            enemy = other.gameObject;

            if (!attackDefended){
                otherPlayer.changeHealth(-damage);
            }
            else{
                otherPlayer.changeHealth((int) -(damage * 0.1f));
            }

            if (transform.root.GetChild(0).GetComponent<playerPlataformerController>().roundsWon == 0){
                FindObjectOfType<GameController>().CallHitStop(damage / 8f, 0.05f);
            } else if (damage < otherPlayer.health){
                FindObjectOfType<GameController>().CallHitStop(damage / 8f, 0.05f);
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

    public void spawnParticle(playerPlataformerController otherPlayer, int placeStruck){
        var currentBlood = Instantiate(blood, otherPlayer.transform.root.GetChild(2).GetChild(placeStruck).GetComponent<BoxCollider2D>().transform.position, Quaternion.identity);
        currentBlood.transform.localScale = new Vector3(transform.root.GetChild(0).transform.localScale.x, 1, 1);
        
        ParticleSystem.ShapeModule editableShape = currentBlood.shape;
        editableShape.position = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));

        ParticleSystem.EmissionModule editableCount = currentBlood.emission;
        editableCount.SetBurst(0, new ParticleSystem.Burst(0, damage / 2));

        ParticleSystem.VelocityOverLifetimeModule editableSpeed = currentBlood.velocityOverLifetime;
        editableSpeed.speedModifier =  Mathf.Ceil(pushBackStrengh / 12f);

        if (placeStruck == 0){
            Instantiate(hitSplash, new Vector3(otherPlayer.transform.position.x, otherPlayer.transform.position.y + 4f, 0), Quaternion.identity);
        }
        else if (placeStruck == 1){
            Instantiate(hitSplash, new Vector3(otherPlayer.transform.position.x, otherPlayer.transform.position.y + 2f, 0), Quaternion.identity);
        }
        else{
            Instantiate(hitSplash, new Vector3(otherPlayer.transform.position.x, otherPlayer.transform.position.y - 1.5f, 0), Quaternion.identity);
        }
    }

}
