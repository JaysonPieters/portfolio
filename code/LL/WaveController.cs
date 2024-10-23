using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public WaveSO[] waves;
    [SerializeField] Transform player;

    float waveTime;

    int currentWave = 0;
    public float timeBetweenWaves = 5;
    // Start is called before the first frame update
    void Start()
    {
        timeBetweenWaves = 5;
        spawnWave();
    }

    // Update is called once per frame
    void Update()
    {
        waveTime += Time.deltaTime;
        if (waveTime >= timeBetweenWaves)
        {
            waveTime = 0;
            spawnWave();
            //Debug.Log("Spawing wave");
            
            if(timeBetweenWaves > 0.5f)
            {
                timeBetweenWaves *= 0.97f;
            }
        }
    }

    public void spawnWave()
    {
        currentWave++;
        foreach (var enemy in waves[Random.Range(0,waves.Length)].enemies)
        {
            SpawnEnemy(enemy);
        }
    }

    private void SpawnEnemy(GameObject enemyGameObject)
    {
        Vector3 spawnDir = Random.onUnitSphere;
        spawnDir.y = 0;
        spawnDir.Normalize();
        
        var spawnPos = player.transform.position + spawnDir * 20;
        Instantiate(enemyGameObject, spawnPos, Quaternion.identity);
    }
}
