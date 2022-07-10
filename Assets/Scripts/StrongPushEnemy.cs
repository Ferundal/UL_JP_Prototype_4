using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongPushEnemy : MonoBehaviour
{
    public float speed = 3.0f;
    public float pushForce = 5.0f;
    private Rigidbody enemyRigidbody;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        enemyRigidbody = this.GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.y < -10)
        {
            Destroy(this.gameObject);
        }
        Vector3 moveDirection = (player.transform.position - this.transform.position).normalized;
        enemyRigidbody.AddForce(moveDirection * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Vector3 pushDirection = (collision.transform.position - this.transform.position);
            collision.gameObject.GetComponent<Rigidbody>().AddForce(pushDirection * pushForce, ForceMode.Impulse);
        }
    }
}
