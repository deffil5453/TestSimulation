using UnityEngine;

public class CrystalSpawner : MonoBehaviour
{
    public GameObject Crystal;
    private int _maxCrystal = 30;
    public float GenerationFrequency = 1f;
    private Vector3 spawnArea = new Vector3(10, 10, 10);
    void Start()
    {
        InitialResource();
        
        InvokeRepeating("SpawnResource", 1f, 1f);
    }

    private void InitialResource()
    {
        for (int i = 0; i < _maxCrystal / 2; i++)
        {
            SpawnResource();
        }
    }

    private void SpawnResource()
    {
        Vector3 spawnPositon = new Vector3
            (
                Random.Range(-spawnArea.x, spawnArea.x),
                0.39f,
                Random.Range(-spawnArea.z, spawnArea.z)
            );
        Instantiate(Crystal, spawnPositon, Quaternion.identity);
    }

}
