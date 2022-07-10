using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float moveSpeed = 50.0f;
    public float pushForce = 15.0f;
    private Rigidbody projectileRigidBody;
    public Vector3 moveDirection;
    private Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = this.transform.position;
        projectileRigidBody = this.GetComponent<Rigidbody>();
        projectileRigidBody.AddForce(moveDirection * moveSpeed, ForceMode.VelocityChange);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(startPosition, this.transform.position) > 15)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision");
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Rigidbody>().AddForce(moveDirection * pushForce, ForceMode.Impulse);
            Destroy(this.gameObject);
        }
    }
}
