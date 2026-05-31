using UnityEngine;

public class DObjSpawner : MonoBehaviour
{
    public GameObject[] objectPrefabs;
    public int originalSpawnCount = 30;

    public Vector3 spawnAreaCenter = Vector3.zero;
    public Vector3 spawnAreaSize = new Vector3(40f, 10f, 40f);

    public float checkInterval = 2f;

    private int minimumAmount;

    void Start()
    {
        minimumAmount = Mathf.CeilToInt(originalSpawnCount * 0.3f);

        SpawnObjects(originalSpawnCount);

        InvokeRepeating(nameof(CheckAndRespawn), checkInterval, checkInterval);
    }

    void CheckAndRespawn()
    {
        GameObject[] currentObjects = GameObject.FindGameObjectsWithTag("dobj");

        if (currentObjects.Length < minimumAmount)
        {
            int amountToSpawn = originalSpawnCount - currentObjects.Length;
            SpawnObjects(amountToSpawn);
        }
    }

    void SpawnObjects(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject prefab = objectPrefabs[Random.Range(0, objectPrefabs.Length)];

            Vector3 spawnPos = spawnAreaCenter + new Vector3(
                Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f),
                Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f),
                Random.Range(-spawnAreaSize.z / 2f, spawnAreaSize.z / 2f)
            );

            GameObject obj = Instantiate(prefab, spawnPos, Random.rotation);
            obj.tag = "dobj";

            if (obj.GetComponent<Collider>() == null)
                obj.AddComponent<SphereCollider>();

            if (obj.GetComponent<Rigidbody>() == null)
                obj.AddComponent<Rigidbody>();
        }
    }
}
