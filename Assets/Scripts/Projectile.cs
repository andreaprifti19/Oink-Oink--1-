using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f; // speed of the projectile
    public float lifetime = 5f; // time in seconds before the projectile is destroyed

    void Start()
    {
        Destroy(gameObject, lifetime); // destroy the projectile after its lifetime expires
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime); // move the object forward at a constant rate
    }

    void OnTriggerEnter(Collider other) // detect collision with other objects
    {
        // optional: ignore collisions with the police car itself or other projectiles
        // Destroy on impact
        Destroy(gameObject);
    }
}
