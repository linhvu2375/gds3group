using UnityEngine;
using TMPro;

public class FFCamouflage : MonoBehaviour
{
    [Header("Camouflage")]
    public Renderer fishRenderer;
    public float detectDistance = 6f;

    [Header("Magnet")]
    public float magnetRadius = 4f;
    public Transform stickPoint;

    [Header("UI")]
    public TMP_Text skillText;

    private Renderer targetRenderer;
    private Collider[] magnetHits;

    void Start()
    {
        HideSkillText();
    }

    void OnDisable()
    {
        HideSkillText();
    }

    void Update()
    {
        bool canCamouflage = CheckCamouflageTarget();
        bool canMagnet = CheckMagnetTargets();

        bool canUseSkill = canCamouflage || canMagnet;

        if (skillText != null)
            skillText.gameObject.SetActive(canUseSkill);

        if (canUseSkill && Input.GetKeyDown(KeyCode.E))
        {
            if (canCamouflage)
                UseCamouflage();

            if (canMagnet)
                UseMagnet();

            Debug.Log("Frogfish skill used.");
        }

        Debug.DrawRay(transform.position, transform.forward * detectDistance, Color.red);
    }

    bool CheckCamouflageTarget()
    {
        targetRenderer = null;

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit[] hits = Physics.RaycastAll(ray, detectDistance);

        foreach (RaycastHit hit in hits)
        {
            if (!hit.collider.CompareTag("denvi")) continue;

            Renderer r = hit.collider.GetComponent<Renderer>();

            if (r != null)
            {
                targetRenderer = r;
                return true;
            }
        }

        return false;
    }

    bool CheckMagnetTargets()
    {
        magnetHits = Physics.OverlapSphere(transform.position, magnetRadius);

        foreach (Collider hit in magnetHits)
        {
            if (hit.CompareTag("dobj"))
                return true;
        }

        return false;
    }

    void UseCamouflage()
    {
        if (fishRenderer == null || targetRenderer == null) return;

        fishRenderer.material.color = targetRenderer.material.color;
        fishRenderer.material.mainTexture = targetRenderer.material.mainTexture;

        Debug.Log("Camouflage used.");
    }

    void UseMagnet()
    {
        if (magnetHits == null) return;

        foreach (Collider hit in magnetHits)
        {
            if (hit.CompareTag("dobj"))
            {
                StickObject(hit.gameObject);
            }
        }

        Debug.Log("Magnet used.");
    }

    void StickObject(GameObject obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        obj.transform.SetParent(stickPoint != null ? stickPoint : transform);

        obj.transform.localPosition = Random.insideUnitSphere * 0.6f;
        obj.transform.localRotation = Random.rotation;

        obj.tag = "Untagged";
    }

    void HideSkillText()
    {
        if (skillText != null)
            skillText.gameObject.SetActive(false);
    }
}