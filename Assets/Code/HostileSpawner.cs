using System;
using UnityEngine;

public class HostileSpawner : MonoBehaviour
{
    public Enemy[] enemies;
    float height;

    void Start()
    {
        height = Camera.main.orthographicSize;
        foreach (Enemy e in enemies)
        {
            e.spawnTimer = e.spawnRate;
        }
    }

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
                    Instantiate(e.enemy, Pos(e), Quaternion.LookRotation(Vector3.forward));
                    e.spawnTimer = e.spawnRate + UnityEngine.Random.Range(0f, Mathf.Clamp01(e.spawnRate));
                }
            }
        }
    }
    Vector2 Pos(Enemy e)
    {
        Vector2 position = Vector2.zero;
        if (!e.randSpawn)
        {
            int rand = UnityEngine.Random.Range(0, e.spawnLocations.Length);
            position = e.spawnLocations[rand].position;
        }
        else
        {
            float rand = UnityEngine.Random.Range(-height, height);
            position = new Vector2(height * 2, rand);
        }
        return position;
    }
}

[Serializable]
public class Enemy
{
    // Find out why adding inheriting breaks serialization.
    public GameObject enemy;
    public Transform[] spawnLocations;
    public bool randSpawn;
    public int enemiesPer;
    public float spawnRate;
    [HideInInspector] public float spawnTimer;
}
