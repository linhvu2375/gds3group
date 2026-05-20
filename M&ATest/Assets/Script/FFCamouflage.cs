using UnityEngine;

public class FFCamouflage : MonoBehaviour
{
    public Renderer fishRenderer;
    public float detectDistance = 6f;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        RaycastHit[] hits = Physics.RaycastAll(ray, detectDistance);

        foreach (RaycastHit hit in hits)
        {
            if (!hit.collider.CompareTag("denvi")) continue;

            Renderer targetRenderer = hit.collider.GetComponent<Renderer>();

            if (targetRenderer != null)
            {
                fishRenderer.material.color = targetRenderer.material.color;
                fishRenderer.material.mainTexture = targetRenderer.material.mainTexture;
            }

            break;
        }

        Debug.DrawRay(transform.position, transform.forward * detectDistance, Color.red);
    }
}
