using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessing : MonoBehaviour{
    
    private Vignette vignette;
    private float vignetteIntensity;

    private ChromaticAberration chromaticAberration;
    private float chromaticAberrationIntensity;

    private LensDistortion lensDistortion;
    private float lensDistortionIntensity;

    private float cameraIntensity;

    public bool doneEffect;
    
    void Start(){
        // Pega os efeitos do post processing
        PostProcessVolume volume = GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out vignette);
        volume.profile.TryGetSettings(out chromaticAberration);
        volume.profile.TryGetSettings(out lensDistortion);
    }

    public void DeathVignette(bool death){
        if (death){
            // Torna todos efeitos verdadeiros
            vignette.enabled.value = true;
            chromaticAberration.enabled.value = true;
            lensDistortion.enabled.value = true;
            
            // Desativa a hud
            transform.GetChild(0).gameObject.SetActive(false);
            
            // Faz os efeitos aparecerem lentamente
            DOTween.To(() => vignetteIntensity, x => vignetteIntensity = x, 0.6f, 0.1f).OnComplete(() => {doneEffect = true;});
            DOTween.To(() => chromaticAberrationIntensity, x => chromaticAberrationIntensity = x, 1f, 0.1f);
            DOTween.To(() => lensDistortionIntensity, x => lensDistortionIntensity = x, -30f, 0.1f);
            DOTween.To(() => cameraIntensity, x => cameraIntensity = x, 7.5f, 0.1f);

            // Aplica os efeitos
            vignette.intensity.value = vignetteIntensity;
            chromaticAberration.intensity.value = chromaticAberrationIntensity;
            lensDistortion.intensity.value = lensDistortionIntensity;
            GameObject.FindWithTag("MainCamera").GetComponent<Camera>().orthographicSize = cameraIntensity;
        }
        else{
            // Torna todos efeitos verdadeiros
            vignette.enabled.value = true;
            chromaticAberration.enabled.value = true;
            lensDistortion.enabled.value = true;

            // Faz os efeitos desaparecerem lentamente
            DOTween.To(() => vignetteIntensity, x => vignetteIntensity = x, 0, 0.1f).OnComplete(() => {transform.GetChild(0).gameObject.SetActive(true);});
            DOTween.To(() => chromaticAberrationIntensity, x => chromaticAberrationIntensity = x, 0, 0.1f);
            DOTween.To(() => lensDistortionIntensity, x => lensDistortionIntensity = x, 0, 0.1f);
            DOTween.To(() => cameraIntensity, x => cameraIntensity = x, 10f, 0.1f);

            // Aplica os efeitos
            vignette.intensity.value = vignetteIntensity;
            chromaticAberration.intensity.value = chromaticAberrationIntensity;
            lensDistortion.intensity.value = lensDistortionIntensity;
            GameObject.FindWithTag("MainCamera").GetComponent<Camera>().orthographicSize = cameraIntensity;
        }
    }

}
