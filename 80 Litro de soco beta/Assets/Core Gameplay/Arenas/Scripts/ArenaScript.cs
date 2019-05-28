using System;
using UnityEngine;

public class ArenaScript : MonoBehaviour{

    public GameObject interactablePrefab;
    private GameObject interactable;
    
    public GameObject backgroundMovingObjectPrefab;
    private GameObject BGO;

    public void SpawnBGO(Vector3 spawnPos){
        BGO = Instantiate(backgroundMovingObjectPrefab, spawnPos, Quaternion.identity);
    }
    
    public void SpawnInteractable(Vector3 spawnPos){
        interactable = Instantiate(interactablePrefab, spawnPos, Quaternion.identity);
    }

}
