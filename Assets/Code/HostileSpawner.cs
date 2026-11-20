using System;
using UnityEngine;

public class HostileSpawner : MonoBehaviour
{
    public Enemy[] enemies;

    // Update is called once per frame
    void Update()
    {
        foreach (Enemy e in enemies)
        {
            e.spawnTimer -= Time.deltaTime;
            if (e.spawnTimer <= 0)
            {
                for (int i = 0; i < e.enemiesPer; i++)
                {
                    int rand = UnityEngine.Random.Range(0, e.spawnLocations.Length);
                    Instantiate(e.enemy, e.spawnLocations[rand].position, Quaternion.LookRotation(Vector3.forward));
                    e.spawnTimer = e.spawnRate;
                }
            }
        }
    }
}

[Serializable]
public class Enemy
{
    // Find out why adding inheriting breaks serialization.
    public GameObject enemy;
    public Transform[] spawnLocations;
    public int enemiesPer;
    public float spawnRate;
    [HideInInspector] public float spawnTimer;
}
