using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerScript : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject boss;
    public GameObject [] rareEnamyPrefabs;
    public GameObject [] powerUpPrefab;
    private float spawnRange = 9.0f;
    private int spawnWaveSize = 1;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectsOfType<Enemy>().Length == 0 && FindObjectsOfType<StrongPushEnemy>().Length == 0)
        {
            ++spawnWaveSize;
            if (spawnWaveSize % 5 != 0)
            {
                SpawnWave(spawnWaveSize, enemyPrefab);
            } else {
                SpawnBoss();
                SpawnWave(spawnWaveSize - 3, enemyPrefab);
            }
            SpawnPowerUp();
        }
    }
    private void SpawnBoss()
    {
        Instantiate(boss, GenerateRandomSpawnPosition(), boss.transform.rotation);
    }

    private void SpawnWave(int waveSize, GameObject prefab)
    {
        for (int counter = 0; counter < waveSize / 4; ++counter)
        {
            int currRareEnemy = Random.Range(0, rareEnamyPrefabs.Length);
            Instantiate(rareEnamyPrefabs[currRareEnemy], GenerateRandomSpawnPosition(), rareEnamyPrefabs[currRareEnemy].transform.rotation);
        }
        for (int counter = waveSize / 4; counter < waveSize; ++counter)
        {
            Instantiate(prefab, GenerateRandomSpawnPosition(), enemyPrefab.transform.rotation);
        }
    }

    private void SpawnPowerUp()
    {
        int currentPowerUp = Random.Range(0, powerUpPrefab.Length);
        Instantiate(powerUpPrefab[currentPowerUp], GenerateRandomSpawnPosition(), powerUpPrefab[currentPowerUp].transform.rotation);
    }
    public Vector3 GenerateRandomSpawnPosition()
    {
        float spawnPositionX = Random.Range(-spawnRange, spawnRange);
        float spawnPositionZ = Random.Range(-spawnRange, spawnRange);
        return new Vector3(spawnPositionX, 0, spawnPositionZ);
    }


}
