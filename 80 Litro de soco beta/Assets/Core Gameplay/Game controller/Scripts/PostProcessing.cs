using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessing : MonoBehaviour{
    
    private Vignette vignette;
    private float vignetteIntensity;
    
    void Start(){
        PostProcessVolume volume = GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out vignette);
    }
    
    public void DeathVignette(bool death){
        if (death){
            vignette.enabled.value = true;

            DOTween.To(() => vignetteIntensity, x => vignetteIntensity = x, 0.4f, 0.5f);

            vignette.intensity.value = vignetteIntensity;
        }
        else{
            vignette.enabled.value = true;

            DOTween.To(() => vignetteIntensity, x => vignetteIntensity = x, 0, 0.5f);

            vignette.intensity.value = vignetteIntensity;
        }
    }

}
