using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerAudio : MonoBehaviour{

    private AudioSource audioSource;

    public AudioClip[] attackVoice;
    public AudioClip[] attackVoice16bit;
    public AudioClip[] painVoice;
    public AudioClip[] painVoice16bit;
    
    public AudioClip walkLoop;
    public AudioClip walkLoop16bit;

    void Awake(){
        audioSource = GetComponent<AudioSource>();
    }

    void Update(){
        if (transform.root.GetChild(0).GetComponent<playerPlataformerController>().moveHRaw != 0 && transform.root.GetChild(0).GetComponent<playerPlataformerController>().ableToMove && !audioSource.isPlaying){
            audioSource.volume = Random.Range(0.1f, 0.2f);
            audioSource.pitch = Random.Range(0.8f, 1.1f);
            audioSource.clip = GameController.mode16bit ? walkLoop16bit : walkLoop;
            audioSource.Play();
        }
        else if (!transform.root.GetChild(0).GetComponent<playerPlataformerController>().ableToMove
                 && !transform.root.GetChild(0).GetComponent<playerPlataformerController>().currentlyAttacking
                 && !transform.root.GetChild(0).GetComponent<playerPlataformerController>().beenHitHead
                 && !transform.root.GetChild(0).GetComponent<playerPlataformerController>().beenHitTorso
                 && !transform.root.GetChild(0).GetComponent<playerPlataformerController>().beenHitLeg){
            audioSource.Stop();
        }
    }

    public void PlayHurtSound(){
        audioSource.volume = Random.Range(0.2f, 0.4f);
        audioSource.pitch = Random.Range(0.8f, 1.1f);
        audioSource.PlayOneShot(GameController.mode16bit ? painVoice16bit[Random.Range(0, painVoice16bit.Length)] : painVoice[Random.Range(0, painVoice.Length)]);
    }
    
    public void PlayAttackSound(){
        audioSource.volume = Random.Range(0.2f, 0.4f);
        audioSource.pitch = Random.Range(0.8f, 1.1f);
        audioSource.PlayOneShot(GameController.mode16bit ? attackVoice16bit[Random.Range(0, attackVoice16bit.Length)] : attackVoice[Random.Range(0, attackVoice.Length)]);
    }

}
