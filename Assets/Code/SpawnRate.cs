using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnRate : MonoBehaviour
{
    [SerializeField] private Vector2 minPoses;
    [SerializeField] private Vector2 maxPoses;
    
    public GameObject spawnObject;
    public AnimationCurve spawnCurve;

    public float maxSpawnTime = 20;
    public float currentSpawnTime = 0;
    
    private void Awake()
    {
        StartCoroutine(Counter());
    }

    public IEnumerator Counter()
    {
        while (true)
        {
            float spawnRate = currentSpawnTime / maxSpawnTime;
            float delay = 1.2f - spawnCurve.Evaluate(spawnRate);

            currentSpawnTime += delay;
            
            yield return new WaitForSeconds(delay);
            
            Vector3 randomPos = new Vector3(
                Random.Range(minPoses.x, maxPoses.x), 
                0, 
                Random.Range(minPoses.y, maxPoses.y));
            
            GameObject spawn = Instantiate(spawnObject, randomPos, Quaternion.identity);
        }
    }
}
