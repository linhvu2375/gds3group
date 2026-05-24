using UnityEngine;
using TMPro;

public class FFCamouflage : MonoBehaviour
{
    public Renderer fishRenderer;
    public float detectDistance = 6f;

    public TMP_Text skillText;

    void Start()
    {
        if (skillText != null)
            skillText.gameObject.SetActive(false);
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit[] hits = Physics.RaycastAll(ray, detectDistance);

        bool canUseSkill = false;
        Renderer targetRenderer = null;

        foreach (RaycastHit hit in hits)
        {
            if (!hit.collider.CompareTag("denvi")) continue;

            targetRenderer = hit.collider.GetComponent<Renderer>();

            if (targetRenderer != null)
            {
                canUseSkill = true;
                break;
            }
        }

        if (skillText != null)
            skillText.gameObject.SetActive(canUseSkill);

        if (canUseSkill && Input.GetKeyDown(KeyCode.E))
        {
            fishRenderer.material.color = targetRenderer.material.color;
            fishRenderer.material.mainTexture = targetRenderer.material.mainTexture;
            Debug.Log("Frogfish camouflage used.");
        }

        Debug.DrawRay(transform.position, transform.forward * detectDistance, Color.red);
    }
}
