using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dummySpecials : SpecialsBase{

    private bool special2Moving;
    private bool special1Moving;
    
    protected override void EspecialUpdate(){
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
    }

    protected override void Special1(){
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
    }
    
    protected override void Special2(){
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
    }
    
    protected override void Special3(){
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
