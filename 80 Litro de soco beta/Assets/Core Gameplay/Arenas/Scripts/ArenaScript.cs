using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ArenaScript : MonoBehaviour{

    public GameObject interactablePrefab;
    private GameObject interactable;
    
    public GameObject backgroundMovingObjectPrefab;
    private GameObject BGO;

    private AudioSource audioSource;
    public AudioClip ambience, ambience16bit;

    void Start(){
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = GameController.mode16bit ? ambience16bit : ambience;
        audioSource.Play();
    }

    public void SpawnBGO(Vector3 spawnPos){
        BGO = Instantiate(backgroundMovingObjectPrefab, spawnPos, Quaternion.identity);
    }
    
    public void SpawnInteractable(Vector3 spawnPos){
        interactable = Instantiate(interactablePrefab, spawnPos, Quaternion.identity);
    }

}
