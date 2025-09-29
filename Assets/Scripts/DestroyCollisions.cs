using UnityEngine;

public class DestoryCollisions : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other) // function that runs when the object collides with another object
    {
        // Safety: don't destroy the player when colliding with triggers.
        // If the other object has a PlayerController, ignore it.
        if (other.GetComponent<PlayerController>() != null)
        {
            // skip destroying player
            return;
        }

        // Optionally, you may want to be more selective here (by tag or component)
        Destroy(gameObject); // destroys the object this script is attached to
        Destroy(other.gameObject); // destroys the other object that collides with this object
    }
}
