using UnityEngine;

public class SeaZoneManager : MonoBehaviour
{
    public enum FishType
    {
        Shark,
        Anglerfish,
        Frogfish
    }

    [Header("Player")]
    public Transform playerFish;
    public FirstPersonDrifter fishMovement;
    public FishType fishType;

    [Header("Sea Level Y Positions")]
    public float openWaterY = 30f;
    public float midSeaY = 0f;
    public float deepSeaY = -30f;
    public float levelRange = 12f;

    [Header("Speed")]
    public float normalSpeed = 6f;
    public float slowSpeed = 1.5f;

    [Header("Fog Vision Effect")]
    public Color normalFogColor = new Color(0.05f, 0.35f, 0.5f);
    public Color limitedFogColor = new Color(0.12f, 0.18f, 0.22f);
    public float normalFogDensity = 0.02f;
    public float limitedFogDensity = 0.18f;
    public float fogFadeSpeed = 2f;

    private float targetFogDensity;
    private Color targetFogColor;

    [Header("Lighting")]
    public float normalAmbient = 1f;
    public float darkAmbient = 0.2f;

    public float normalSkyboxExposure = 1f;
    public float darkSkyboxExposure = 0.2f;

    public float lightFadeSpeed = 2f;

    private float targetAmbient;
    private float targetSkybox;


    void Start()
    {
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.ExponentialSquared;

        targetFogDensity = normalFogDensity;
        targetFogColor = normalFogColor;
    }

    void Update()
    {
        if (playerFish == null || fishMovement == null) return;
        float y = playerFish.position.y;

        bool isOpenWater = y >= openWaterY;
        bool isDeepSea = y <= deepSeaY;
        bool isMidSea = y < openWaterY && y > deepSeaY;

        bool limitedVision = false;

        if (fishType == FishType.Shark)
        {
            limitedVision = isDeepSea;
        }
        else if (fishType == FishType.Anglerfish)
        {
            limitedVision = !isDeepSea;
        }
        else // Frogfish
        {
            limitedVision = isOpenWater || isDeepSea;
        }

        fishMovement.walkSpeed = limitedVision ? slowSpeed : normalSpeed;

        SetFogVision(limitedVision);
        UpdateFog();
        UpdateLighting();
    }

    void SetFogVision(bool limited)
    {
        targetFogDensity = limited ? limitedFogDensity : normalFogDensity;
        targetFogColor = limited ? limitedFogColor : normalFogColor;

        targetAmbient = limited ? darkAmbient : normalAmbient;
        targetSkybox = limited ? darkSkyboxExposure : normalSkyboxExposure;
    }

    void UpdateFog()
    {
        RenderSettings.fogDensity = Mathf.Lerp(
            RenderSettings.fogDensity,
            targetFogDensity,
            Time.deltaTime * fogFadeSpeed
        );

        RenderSettings.fogColor = Color.Lerp(
            RenderSettings.fogColor,
            targetFogColor,
            Time.deltaTime * fogFadeSpeed
        );
    }

    void UpdateLighting()
    {
        // Ambient light
        RenderSettings.ambientIntensity = Mathf.Lerp(
            RenderSettings.ambientIntensity,
            targetAmbient,
            Time.deltaTime * lightFadeSpeed
        );

        // Skybox exposure (IMPORTANT)
        if (RenderSettings.skybox != null && RenderSettings.skybox.HasProperty("_Exposure"))
        {
            float currentExposure = RenderSettings.skybox.GetFloat("_Exposure");

            float newExposure = Mathf.Lerp(
                currentExposure,
                targetSkybox,
                Time.deltaTime * lightFadeSpeed
            );

            RenderSettings.skybox.SetFloat("_Exposure", newExposure);
        }
    }
}