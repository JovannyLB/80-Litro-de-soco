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

    private float cameraIntensity = 10;

    public bool doneEffect;

    [HideInInspector]public float startSpeed = 0.1f;
    
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
            DOTween.To(() => vignetteIntensity, x => vignetteIntensity = x, 0.6f, startSpeed).OnComplete(() => {doneEffect = true;});
            DOTween.To(() => chromaticAberrationIntensity, x => chromaticAberrationIntensity = x, 1f, startSpeed);
            DOTween.To(() => lensDistortionIntensity, x => lensDistortionIntensity = x, -30f, startSpeed);
            DOTween.To(() => cameraIntensity, x => cameraIntensity = x, 7.5f, startSpeed);

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
            DOTween.To(() => vignetteIntensity, x => vignetteIntensity = x, 0, 1f).SetDelay(0.1f);
            DOTween.To(() => chromaticAberrationIntensity, x => chromaticAberrationIntensity = x, 0, 1f).SetDelay(0.1f);
            DOTween.To(() => lensDistortionIntensity, x => lensDistortionIntensity = x, 0, 1f).SetDelay(0.1f);
            DOTween.To(() => cameraIntensity, x => cameraIntensity = x, 10f, 1f).SetDelay(0.1f).OnPlay(() => {
                transform.GetChild(0).gameObject.SetActive(true);
            });

            // Aplica os efeitos
            vignette.intensity.value = vignetteIntensity;
            chromaticAberration.intensity.value = chromaticAberrationIntensity;
            lensDistortion.intensity.value = lensDistortionIntensity;
            GameObject.FindWithTag("MainCamera").GetComponent<Camera>().orthographicSize = cameraIntensity;
        }
    }

}
