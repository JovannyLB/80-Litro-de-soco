using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialsBase : MonoBehaviour
{
    
    [HideInInspector]public playerPlataformerController ownPlayer;

    protected int threshold = 10;
    private int totalThreshold;

    public GameObject[] lightSpecials;
    public GameObject[] hardSpecials;
    public GameObject super;
    
    void Start() {
        ownPlayer = transform.root.GetChild(0).GetComponent<playerPlataformerController>();
        totalThreshold = threshold;
    }

    void Update() {
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
        
        // Ajuda os ataques cancelaveis
        if (ownPlayer.testeDeSpecialCancel()){
            threshold = 40;
        }
        else{
            threshold = totalThreshold;
        }
        
        // Especiais
        EspecialUpdate();
        Special1();
        Special2();
        Special3();
        Super();
    }

    protected virtual void Special1(){
    }
    
    protected virtual void Special2(){
    }
    
    protected virtual void Special3(){
    }

    protected virtual void Super(){
    }

    protected virtual void EspecialUpdate(){
    }

}
