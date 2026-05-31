using UnityEngine;

public class EnergyCollectible : MonoBehaviour
{
    public int energyAmount = 1;

    void OnTriggerEnter(Collider other)
    {
        EnergySystem energy = other.GetComponent<EnergySystem>();

        if (energy != null)
        {
            energy.AddEnergy(energyAmount);
            gameObject.SetActive(false);
        }
    }
}