using UnityEngine;

public class SeaLevelsManager : MonoBehaviour
{
    public enum FishType
    {
        Shark,
        Anglerfish,
        Frogfish
    }
    public EnergySystem energySystem;

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

    [Header("Sea Level Lighting")]
    public float openWaterAmbient = 1.2f;
    public float midSeaAmbient = 0.8f;
    public float deepSeaAmbient = 0.35f;

    public float openWaterSkyboxExposure = 1.3f;
    public float midSeaSkyboxExposure = 0.9f;
    public float deepSeaSkyboxExposure = 0.35f;

    [Header("Fish Abilities")]
    public SharkAbility sharkAbility;
    public AFLure anglerfishAbility;
    public FFCamouflage frogfishAbility;

    void Start()
    {
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.ExponentialSquared;

        targetFogDensity = normalFogDensity;
        targetFogColor = normalFogColor;

        UpdateActiveAbility();
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
            limitedVision = !isOpenWater;
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

        if (energySystem != null)
        {
            if (limitedVision)
                energySystem.UseWrongEnvironmentEnergy(Time.deltaTime);
            else
                energySystem.ResetWrongEnvironmentTimer();
        }

        SetFogVision(limitedVision, isOpenWater, isMidSea, isDeepSea);
        UpdateFog();
        UpdateLighting();
    }

    void SetFogVision(bool limited, bool isOpenWater, bool isMidSea, bool isDeepSea)
    {
        targetFogDensity = limited ? limitedFogDensity : normalFogDensity;
        targetFogColor = limited ? limitedFogColor : normalFogColor;

        float zoneAmbient = midSeaAmbient;
        float zoneSkybox = midSeaSkyboxExposure;

        if (isOpenWater)
        {
            zoneAmbient = openWaterAmbient;
            zoneSkybox = openWaterSkyboxExposure;
        }
        else if (isDeepSea)
        {
            zoneAmbient = deepSeaAmbient;
            zoneSkybox = deepSeaSkyboxExposure;
        }

        if (limited)
        {
            targetAmbient = darkAmbient;
            targetSkybox = darkSkyboxExposure;
        }
        else
        {
            targetAmbient = zoneAmbient;
            targetSkybox = zoneSkybox;
        }
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

    public void UpdateActiveAbility()
    {
        if (sharkAbility != null)
            sharkAbility.enabled = fishType == FishType.Shark;

        if (anglerfishAbility != null)
            anglerfishAbility.enabled = fishType == FishType.Anglerfish;

        if (frogfishAbility != null)
            frogfishAbility.enabled = fishType == FishType.Frogfish;
    }
}