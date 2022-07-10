using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    public GameObject projectilePrefab;
    private Vector3 startPosition;

    enum PowerUpStatus {NO_POWER_UP, COOLDOWN, STRONG_PUNCH, ROCKETS, EXPOSION};
    private PowerUpStatus powerUpStatus = PowerUpStatus.NO_POWER_UP;

    public float pushForce = 5.0f;
    private Rigidbody playerRigidbody;
    private float verticalInput;
    private GameObject focalPoint;
    private float powerUpStrength = 20.0f;
    public GameObject powerUpIndicator;
    public GameObject strongPunchIndicator;
    public GameObject rocketsIndicator;
    public GameObject explosionIndicator;
    private Explosion explosionParametrs;
    public Vector3 powerUpOffsetPosition = new Vector3(0, -0.5f, 0);
    public Vector3 powerUpTypeOffsetPosition = new Vector3(0, 1.5f, 0);
    public bool isOnGround = false;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = this.transform.position;
        playerRigidbody = this.GetComponent<Rigidbody>();
        explosionParametrs = explosionIndicator.GetComponent<Explosion>();
        focalPoint = GameObject.Find("FocalPoint");
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.y < -10)
        {
            this.transform.SetPositionAndRotation(startPosition, this.transform.rotation);
        }
        if (isOnGround)
        {
            verticalInput = Input.GetAxis("Vertical");
            playerRigidbody.AddForce(pushForce * verticalInput * focalPoint.transform.forward);
        }
        powerUpIndicator.transform.position = this.transform.position + powerUpOffsetPosition;
        strongPunchIndicator.transform.position = this.transform.position + powerUpTypeOffsetPosition;
        rocketsIndicator.transform.position = strongPunchIndicator.transform.position;
        explosionIndicator.transform.position = strongPunchIndicator.transform.position;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (powerUpStatus == PowerUpStatus.ROCKETS)
            {
                powerUpStatus = PowerUpStatus.COOLDOWN;
                rocketsIndicator.SetActive(false);
                StartCoroutine(ShootProjectiles(3));
            } else if (powerUpStatus == PowerUpStatus.EXPOSION)
            {
                powerUpStatus = PowerUpStatus.COOLDOWN;
                explosionIndicator.SetActive(false);
                MakeExplosion();

            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (powerUpStatus == PowerUpStatus.NO_POWER_UP)
        {
            if (other.CompareTag("PowerUp"))
            {
                powerUpStatus = PowerUpStatus.STRONG_PUNCH;
                Destroy(other.gameObject);
                powerUpIndicator.SetActive(true);
                strongPunchIndicator.SetActive(true);
                StartCoroutine(PowerUpCountdownRoutine());
            } else if (other.CompareTag("Rockets"))
            {
                powerUpStatus = PowerUpStatus.ROCKETS;
                Destroy(other.gameObject);
                powerUpIndicator.SetActive(true);
                rocketsIndicator.SetActive(true);
                StartCoroutine(PowerUpCountdownRoutine());
            } else if (other.CompareTag("Explosion"))
            {
                powerUpStatus = PowerUpStatus.EXPOSION;
                Destroy(other.gameObject);
                powerUpIndicator.SetActive(true);
                explosionIndicator.SetActive(true);
                StartCoroutine(PowerUpCountdownRoutine());
            }
        }
    }

    IEnumerator PowerUpCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        powerUpStatus = PowerUpStatus.NO_POWER_UP;
        powerUpIndicator.SetActive(false);
        strongPunchIndicator.SetActive(false);
        explosionIndicator.SetActive(false);
    }

    IEnumerator ShootProjectiles(int rocketsWavesAmount)
    {
        if (rocketsWavesAmount > 0)
        {
            SpawnProjectileWave();
        }
        for (int counter = 1; counter < rocketsWavesAmount; ++counter)
        {
            yield return new WaitForSeconds(1);
            SpawnProjectileWave();
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Equals("Island"))
        {
            isOnGround = true;
        } else if (powerUpStatus == PowerUpStatus.STRONG_PUNCH && collision.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - this.transform.position);
            enemyRigidbody.AddForce(awayFromPlayer * powerUpStrength, ForceMode.Impulse);
            Debug.Log("Player collide with " + collision.gameObject.name + " with powerup set " + powerUpStatus);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name.Equals("Island"))
        {
            isOnGround = false;
        }
    }

    void SpawnProjectileWave()
    {
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        for (int enemysCounter = 0; enemysCounter < enemys.Length; ++enemysCounter)
        {
            ShootProjectileToTarget(enemys[enemysCounter]);
        }
    }

    void ShootProjectileToTarget(GameObject target)
    {
        Vector3 direction = (target.transform.position - this.transform.position);
        direction.y = 0;
        direction = direction.normalized;
        Vector3 spawnPosition = this.transform.position + direction * 2.0f;
        spawnPosition.y = 0.2f;

        GameObject newProjectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.LookRotation(direction));
        newProjectile.GetComponent<Projectile>().moveDirection = direction;
        newProjectile.transform.Rotate(Vector3.left, 90, Space.Self);
    }

    void MakeExplosion()
    {
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        playerRigidbody.velocity = Vector3.zero;
        playerRigidbody.AddForce(Vector3.up * explosionParametrs.explosionPower / 2, ForceMode.Impulse);
        for (int enemysCounter = 0; enemysCounter < enemys.Length; ++enemysCounter)
        {
            float distance = Vector3.Distance(this.gameObject.transform.position, enemys[enemysCounter].transform.position);
            if (distance < explosionParametrs.explosionRadius)
            {
                Rigidbody enemyRigidbody = enemys[enemysCounter].gameObject.GetComponent<Rigidbody>();
                Vector3 direction = (enemys[enemysCounter].transform.position - this.transform.position).normalized;
                enemyRigidbody.AddForce(direction * explosionParametrs.explosionPower * (explosionParametrs.explosionRadius - distance), ForceMode.Impulse);
            }
        }
    }
}
