using TMPro;
using UnityEngine;

public class AFLure : MonoBehaviour
{
    [Header("Sea Zone")]
    public SeaLevelsManager seaLevelsManager;

    [Header("Lure Light")]
    public Light lureLight;

    [Header("Attract Settings")]
    public float attractRadius = 10f;
    public float attractSpeed = 5f;
    public float abilityDuration = 4f;
    public float cooldown = 6f;

    [Header("UI")]
    public TMP_Text skillText;

    private bool abilityActive = false;
    private float abilityTimer = 0f;
    private float cooldownTimer = 0f;

    void Start()
    {
        if (lureLight != null)
            lureLight.enabled = false;

        HideSkillText();
    }

    void OnDisable()
    {
        if (lureLight != null)
            lureLight.enabled = false;

        HideSkillText();
    }

    void Update()
    {
        bool isDeepSea = CheckDeepSea();
        bool abilityReady = cooldownTimer <= 0f && !abilityActive;
        bool canUseAbility = isDeepSea && abilityReady;

        if (skillText != null)
            skillText.gameObject.SetActive(canUseAbility);

        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        if (canUseAbility && Input.GetKeyDown(KeyCode.E))
            StartAbility();

        if (abilityActive)
            RunAbility();
    }

    bool CheckDeepSea()
    {
        if (seaLevelsManager == null) return false;

        return transform.position.y <= seaLevelsManager.deepSeaY;
    }

    void StartAbility()
    {
        abilityActive = true;
        abilityTimer = abilityDuration;

        if (lureLight != null)
            lureLight.enabled = true;

        HideSkillText();

        Debug.Log("Anglerfish lure ability started.");
    }

    void RunAbility()
    {
        abilityTimer -= Time.deltaTime;

        PullCollectibles();

        if (abilityTimer <= 0f)
            StopAbility();
    }

    void PullCollectibles()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, attractRadius);

        foreach (Collider hit in hits)
        {
            if (!hit.CompareTag("collectible")) continue;

            Vector3 direction = transform.position - hit.transform.position;

            hit.transform.position += direction.normalized * attractSpeed * Time.deltaTime;
        }
    }

    void StopAbility()
    {
        abilityActive = false;
        cooldownTimer = cooldown;

        if (lureLight != null)
            lureLight.enabled = false;

        Debug.Log("Anglerfish lure ability ended.");
    }

    void HideSkillText()
    {
        if (skillText != null)
            skillText.gameObject.SetActive(false);
    }
}
