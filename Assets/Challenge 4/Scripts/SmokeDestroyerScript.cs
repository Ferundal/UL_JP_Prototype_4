using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeDestroyerScript : MonoBehaviour
{
    private ParticleSystem smokeParticleSystem;
    private GameObject player;
    public PlayerControllerX playerControllerX;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerControllerX = player.GetComponent<PlayerControllerX>();
        smokeParticleSystem = this.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = player.transform.position;
        StartCoroutine(DestroyTimer());
        if (!playerControllerX.isExtraSpeed)
        {
            Destroy(this.gameObject);
        }
    }
    IEnumerator DestroyTimer()
    {
        smokeParticleSystem.Play();
        yield return new WaitForSeconds(smokeParticleSystem.main.duration);
        playerControllerX.isExtraSpeed = false;
    }
}
