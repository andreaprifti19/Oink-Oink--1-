using UnityEngine;

// Police car projectile: moves forward and damages the player on hit
public class ProjectilePolice : MonoBehaviour
{
    public float speed = 15f;
    public bool firedByPlayer = false; // police projectile by default

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // If this was fired by police and hits the player -> damage the player
        if (!firedByPlayer)
        {
            var player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(1);
                Destroy(gameObject);
                return;
            }
        }

        // Default: destroy projectile on impact
        Destroy(gameObject);
    }
}
