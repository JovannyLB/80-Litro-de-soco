using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dummySpecials : MonoBehaviour{
    
    private playerPlataformerController ownPlayer;

    public int threshold;

    public GameObject[] lightSpecials;
    public GameObject[] hardSpecials;

    void Start(){
        ownPlayer = transform.root.GetChild(0).GetComponent<playerPlataformerController>();
    }

    void Update(){
        // Hadouken
        if (ownPlayer.downTimerSpecial < threshold && ownPlayer.rightTimerSpecial < threshold){
            ownPlayer.canLightPunch = false;
            ownPlayer.canHardPunch = false;
        }
        else{
            ownPlayer.canLightPunch = true;
            ownPlayer.canHardPunch = true;
        }

        if (ownPlayer.downTimerSpecial < threshold && ownPlayer.rightTimerSpecial < threshold && ownPlayer.square && ownPlayer.testeDeSpecial()){
            ownPlayer.StopAllAttack();
            ownPlayer.LightSpecial3();
        } else if (ownPlayer.downTimerSpecial < threshold && ownPlayer.rightTimerSpecial < threshold && ownPlayer.triangle && ownPlayer.testeDeSpecial()){
            ownPlayer.StopAllAttack();
            ownPlayer.HardSpecial3();
        }
        
    }

    public void instantiateLightSpecial3(){
        lightSpecials[2].GetComponent<projectileSpecialCheck>().instantiateProjectile();
    }
    
    public void instantiateHardSpecial3(){
        hardSpecials[2].GetComponent<projectileSpecialCheck>().instantiateProjectile();
    }

}
