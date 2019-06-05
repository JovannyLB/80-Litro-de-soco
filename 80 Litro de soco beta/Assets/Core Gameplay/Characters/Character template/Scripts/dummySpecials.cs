﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dummySpecials : MonoBehaviour{
    
    private playerPlataformerController ownPlayer;

    public int threshold;

    public GameObject[] lightSpecials;
    public GameObject[] hardSpecials;

    private bool special2Moving;
    private bool special1Moving;

    void Start(){
        ownPlayer = transform.root.GetChild(0).GetComponent<playerPlataformerController>();
    }

    void Update(){
        // Special safe
        if (ownPlayer.leftTimerSpecial < threshold && ownPlayer.rightTimerSpecial < threshold){
            ownPlayer.canLightPunch = false;
            ownPlayer.canHardPunch = false;
        } else if (ownPlayer.downTimerSpecial < threshold && ownPlayer.rightTimerSpecial < threshold){
            ownPlayer.canLightPunch = false;
            ownPlayer.canHardPunch = false;
            ownPlayer.canLightKick = false;
            ownPlayer.canHardKick = false;
        }
        else{
            ownPlayer.canLightPunch = true;
            ownPlayer.canHardPunch = true;
            ownPlayer.canLightKick = true;
            ownPlayer.canHardKick = true;
        }
        
        if (special1Moving){
            ownPlayer.velocity.y = 100;
            if (ownPlayer.isLeft){
                ownPlayer.targetVelocity = new Vector2(1, 0) * 50;
            }
            else{
                ownPlayer.targetVelocity = new Vector2(1, 0) * -50;
            }
        }

        if (!ownPlayer.lightSpecial1Currently && !ownPlayer.hardSpecial1Currently){
            special1Moving = false;
        }

        if (ownPlayer.testeDeSpecialCancel()){
            threshold = 40;
        }
        else{
            threshold = 10;
        }

        // Special 1
        if (ownPlayer.downTimerSpecial < threshold && ownPlayer.rightTimerSpecial < threshold &&
            ownPlayer.xButtonTimerSpecial < threshold && (ownPlayer.testeDeSpecial() || ownPlayer.testeDeSpecialCancel()) &&
            ownPlayer.downTimerSpecial > ownPlayer.xButtonTimerSpecial){
            ownPlayer.StopAllAttack();
            ownPlayer.LightSpecial1();
        } else if (ownPlayer.downTimerSpecial < threshold && ownPlayer.rightTimerSpecial < threshold &&
                   ownPlayer.circleTimerSpecial < threshold && (ownPlayer.testeDeSpecial() || ownPlayer.testeDeSpecialCancel()) &&
                   ownPlayer.downTimerSpecial > ownPlayer.circleTimerSpecial){
            ownPlayer.StopAllAttack();
            ownPlayer.HardSpecial1();
        }
        
        // Special 2
        if (ownPlayer.leftTimerSpecial < threshold && ownPlayer.rightTimerSpecial < threshold &&
            ownPlayer.squareTimerSpecial < threshold && (ownPlayer.testeDeSpecial() || ownPlayer.testeDeSpecialCancel()) &&
            ownPlayer.leftTimerSpecial > ownPlayer.squareTimerSpecial){
           ownPlayer.StopAllAttack();
           ownPlayer.LightSpecial2();
       } else if (ownPlayer.leftTimerSpecial < threshold && ownPlayer.rightTimerSpecial < threshold &&
                  ownPlayer.triangleTimerSpecial < threshold && (ownPlayer.testeDeSpecial() || ownPlayer.testeDeSpecialCancel()) &&
                  ownPlayer.leftTimerSpecial > ownPlayer.triangleTimerSpecial){
           ownPlayer.StopAllAttack();
           ownPlayer.HardSpecial2();
       }

        if (special2Moving && ownPlayer.isLeft){
            ownPlayer.targetVelocity = new Vector2(1, 0) * 40;
        } else if (special2Moving && !ownPlayer.isLeft){
            ownPlayer.targetVelocity = new Vector2(1, 0) * -40;
        }

        if (!ownPlayer.lightSpecial2Currently && !ownPlayer.hardSpecial2Currently){
            special2Moving = false;
        }

        // Special 3
        if (ownPlayer.downTimerSpecial < threshold && ownPlayer.rightTimerSpecial < threshold &&
            ownPlayer.squareTimerSpecial < threshold && (ownPlayer.testeDeSpecial() || ownPlayer.testeDeSpecialCancel()) &&
            ownPlayer.downTimerSpecial > ownPlayer.squareTimerSpecial && !ownPlayer.liveProjectile){
            ownPlayer.StopAllAttack();
            ownPlayer.LightSpecial3();
        } else if (ownPlayer.downTimerSpecial < threshold && ownPlayer.rightTimerSpecial < threshold &&
                   ownPlayer.triangleTimerSpecial < threshold && (ownPlayer.testeDeSpecial() || ownPlayer.testeDeSpecialCancel()) &&
                   ownPlayer.downTimerSpecial > ownPlayer.triangleTimerSpecial && !ownPlayer.liveProjectile){
            ownPlayer.StopAllAttack();
            ownPlayer.HardSpecial3();
        }
        
    }

    public void special1MovementStart(){
        ownPlayer.targetVelocity = Vector2.zero;
        special1Moving = true;
    }
    
    public void special1MovementStop(){
        special1Moving = false;
        ownPlayer.targetVelocity = Vector2.zero;
        ownPlayer.velocity.y = 0;
    }
    
    public void special2MovementStart(){
        ownPlayer.targetVelocity = Vector2.zero;
        special2Moving = true;
    }
    
    public void special2MovementStop(){
        special2Moving = false;
        ownPlayer.targetVelocity = Vector2.zero;
    }

    public void instantiateLightSpecial3(){
        lightSpecials[2].GetComponent<projectileSpecialCheck>().instantiateProjectile();
    }
    
    public void instantiateHardSpecial3(){
        hardSpecials[2].GetComponent<projectileSpecialCheck>().instantiateProjectile();
    }

}
