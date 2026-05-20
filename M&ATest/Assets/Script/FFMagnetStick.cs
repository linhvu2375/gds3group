using UnityEngine;

public class FFMagnetStick : MonoBehaviour
{
    public float magnetRadius = 4f;
    public Transform stickPoint;

    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, magnetRadius);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("dobj"))
            {
                StickObject(hit.gameObject);
            }
        }
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
}
