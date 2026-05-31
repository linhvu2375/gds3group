using UnityEngine;
using TMPro;

public class SharkAbility : MonoBehaviour
{
    [Header("Movement")]
    public FirstPersonDrifter fishMovement;
    public float speedBoostMultiplier = 1.5f;

    [Header("Visual")]
    public Renderer sharkRenderer;
    public float transparentAlpha = 0.45f;

    [Header("Ability")]
    public float abilityDuration = 3f;
    public float cooldown = 5f;

    [Header("UI")]
    public TMP_Text skillText;

    private float originalWalkSpeed;
    private float originalRunSpeed;
    private Color originalColor;

    private bool abilityActive = false;
    private float abilityTimer = 0f;
    private float cooldownTimer = 0f;

    public SeaLevelsManager seaLevelsManager;

    void Start()
    {
        if (fishMovement != null)
        {
            originalWalkSpeed = fishMovement.walkSpeed;
            originalRunSpeed = fishMovement.runSpeed;
        }

        if (sharkRenderer != null)
            originalColor = sharkRenderer.material.color;

        HideSkillText();
    }

    void OnDisable()
    {
        StopAbility();
        HideSkillText();
    }

    void Update()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        bool canUseSkill =
        !abilityActive &&
        cooldownTimer <= 0f &&
        IsOpenWater();

        if (skillText != null)
            skillText.gameObject.SetActive(canUseSkill);

        if (canUseSkill && Input.GetKeyDown(KeyCode.E))
            StartAbility();

        if (abilityActive)
        {
            abilityTimer -= Time.deltaTime;

            if (abilityTimer <= 0f)
                StopAbility();
        }

        if (abilityActive && !IsOpenWater())
        {
            StopAbility();
        }
    }

    void StartAbility()
    {
        abilityActive = true;
        abilityTimer = abilityDuration;

        if (fishMovement != null)
        {
            fishMovement.walkSpeed = originalWalkSpeed * speedBoostMultiplier;
            fishMovement.runSpeed = originalRunSpeed * speedBoostMultiplier;
        }

        SetTransparency(transparentAlpha);

        HideSkillText();

        Debug.Log("Shark countershade sprint started.");
    }

    void StopAbility()
    {
        if (abilityActive)
            cooldownTimer = cooldown;

        abilityActive = false;

        if (fishMovement != null)
        {
            fishMovement.walkSpeed = originalWalkSpeed;
            fishMovement.runSpeed = originalRunSpeed;
        }

        SetTransparency(1f);
    }

    void SetTransparency(float alpha)
    {
        if (sharkRenderer == null) return;

        Color c = sharkRenderer.material.color;
        c.a = alpha;
        sharkRenderer.material.color = c;
    }

    void HideSkillText()
    {
        if (skillText != null)
            skillText.gameObject.SetActive(false);
    }

    bool IsOpenWater()
    {
        if (seaLevelsManager == null)
            return false;

        return transform.position.y >= seaLevelsManager.openWaterY;
    }
}
