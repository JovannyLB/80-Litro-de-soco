using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip[] hardHitHits, hardHitBlock, weakHitHit, weakHitBlock, whiff, hardHitHits16bit, hardHitBlock16bit, weakHitHit16bit, weakHitBlock16bit, whiff16bit;
    public AudioClip super, super16bit;

    private static AudioSource audioSourceS;
    public static AudioClip[] hardHitHitsS, hardHitBlockS, weakHitHitS, weakHitBlockS, whiffS, hardHitHitsS16bit, hardHitBlockS16bit, weakHitHitS16bit, weakHitBlockS16bit, whiffS16bit;
    public static AudioClip superS, superS16bit;
    
    public AudioMixerSnapshot noEcho;
    public AudioMixerSnapshot echo;
    
    public static AudioMixerSnapshot noEchoS;
    public static AudioMixerSnapshot echoS;
    
    void Awake(){
        audioSource = GetComponent<AudioSource>();
        audioSourceS = audioSource;

        hardHitHitsS = hardHitHits;
        hardHitBlockS = hardHitBlock;

        hardHitHitsS16bit = hardHitHits16bit;
        hardHitBlockS16bit = hardHitBlock16bit;

        weakHitHitS = weakHitHit;
        weakHitBlockS = weakHitBlock;
        
        weakHitHitS16bit = weakHitHit16bit;
        weakHitBlockS16bit = weakHitBlock16bit;

        whiffS = whiff;
        whiffS16bit = whiff16bit;

        superS = super;
        superS16bit = super16bit;

        noEchoS = noEcho;
        echoS = echo;
    }

    private static void SetAudio(AudioClip[] audios){
        audioSourceS.volume = Random.Range(0.4f, 0.6f);
        audioSourceS.pitch = Random.Range(0.8f, 1.1f);
        audioSourceS.PlayOneShot(audios[Random.Range(0, audios.Length)]);
    }

    public static void PlayHardHit(bool hit){
        if (hit){
            SetAudio(GameController.mode16bit ? hardHitHitsS16bit : hardHitHitsS);
        }
        else{
            SetAudio(GameController.mode16bit ? hardHitBlockS16bit : hardHitBlockS);
        }
    }
    
    public static void PlayWeakHit(bool hit){
        if (hit){
            SetAudio(GameController.mode16bit ? weakHitHitS16bit : weakHitHitS);
        }
        else{
            SetAudio(GameController.mode16bit ? weakHitBlockS16bit : weakHitBlockS);
        }
    }

    public static void PlayWhiff(){
        SetAudio(GameController.mode16bit ? whiffS16bit : whiffS);
    }

    public static void PlaySuper(){
        audioSourceS.volume = Random.Range(0.8f, 1f);
        audioSourceS.pitch = Random.Range(0.9f, 1.1f);
        audioSourceS.PlayOneShot(GameController.mode16bit ? superS16bit : superS);
    }

    public static void PlaySpecificSound(AudioClip sound){
        audioSourceS.volume = Random.Range(0.4f, 0.8f);
        audioSourceS.pitch = Random.Range(0.8f, 1.1f);
        audioSourceS.PlayOneShot(sound);
    }

    public static void StartEcho(bool start){
        if (start){
            echoS.TransitionTo(0);
        }
        else{
            noEchoS.TransitionTo(0);
        }
    }
}
