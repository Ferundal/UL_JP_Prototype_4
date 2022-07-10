using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject follower;
    public int followerAmount = 2;
    private GameObject spawnManager;
    private SpawnManagerScript spawnManagerScript;
    private bool isCreatingFollower = false;

    // Start is called before the first frame update
    void Start()
    {
        spawnManager = GameObject.Find("SpawnManager");
        spawnManagerScript = spawnManager.GetComponent<SpawnManagerScript>();
        for (int followersCounter = 0; followersCounter < followerAmount; ++followersCounter)
        {
            Instantiate(follower, spawnManagerScript.GenerateRandomSpawnPosition(), follower.transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCreatingFollower && GameObject.FindGameObjectsWithTag("Enemy").Length < followerAmount + 1)
        {
            StartCoroutine(CreateFollower());
        }
    }

    IEnumerator CreateFollower()
    {
        isCreatingFollower = true;
        yield return new WaitForSeconds(5);
        Instantiate(follower, spawnManagerScript.GenerateRandomSpawnPosition(), follower.transform.rotation);
        isCreatingFollower = false;
    }


}
