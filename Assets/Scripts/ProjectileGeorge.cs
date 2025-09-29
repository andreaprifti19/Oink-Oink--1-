using UnityEngine;

// Player projectile: moves upward (uses MoveRight behaviour) and handles collisions
public class ProjectileGeorge : MonoBehaviour
{
    public float speed = 15f;
    public bool firedByPlayer = true; // player projectile by default

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"ProjectileGeorge hit: {other.gameObject.name} (tag={other.gameObject.tag})");
        // If this was fired by the player and hits a police car -> destroy the police car
        if (firedByPlayer)
        {
            // collider might be on a child; check parents too
            var police = other.GetComponentInParent<PoliceCarShooter>();
            if (police == null && other.transform != null && other.transform.root != null)
            {
                // fallback: check the root object
                police = other.transform.root.GetComponent<PoliceCarShooter>();
            }
            if (police != null)
            {
                Debug.Log($"ProjectileGeorge: found police shooter on {police.gameObject.name}, destroying it.");
                Destroy(police.gameObject);
                Destroy(gameObject);
                return;
            }
        }

        // If this hits the player (shouldn't happen) or any other object, destroy projectile
        Destroy(gameObject);
    }
}
