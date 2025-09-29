using UnityEngine;

[RequireComponent(typeof(Collider))] // ensure there is a collider component
public class DonutPickup : MonoBehaviour
{
    public bool picked = false; // whether this pickup has been collected
    public GameObject pickupEffect; // optional effect to spawn on pickup

    void Reset() // called when the script is first added or reset in the editor. Why? To ensure the collider is set as a trigger by default when the script is added.
    {
        // Make sure collider is a trigger by default
        var col = GetComponent<Collider>(); // get the collider component on this game object
        if (col != null) col.isTrigger = true; // set it to be a trigger. 
    }

    void OnTriggerEnter(Collider other) // detect when another collider enters this trigger
    {
        // Only respond to objects tagged as Player
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerController>(); // get the PlayerController component from the player object
            if (player != null) // if the player component exists
            {
                player.NotifyNearbyPickup(this); // notify the player that they are near this pickup
            }
        }
    }

    void OnTriggerExit(Collider other) // detect when another collider exits this trigger
    {
        if (other.CompareTag("Player")) // only respond to objects tagged as Player
        {
            var player = other.GetComponent<PlayerController>(); // get the PlayerController component from the player object
            if (player != null) // if the player component exists
            {
                player.NotifyLeftPickup(this); // notify the player that they have left this pickup
            }
        }
    }

    // Called by player when pickup is triggered
    public void Pickup() // method to handle the pickup action
    {
        if (picked) return;  // already picked up, do nothing
        picked = true; // mark as picked to prevent double pickup
        if (pickupEffect != null) // if there is a pickup effect assigned
        {
            Instantiate(pickupEffect, transform.position, Quaternion.identity); // spawn the effect at the pickup's position with no rotation. What is Quaternion.identity? It means no rotation, or the default rotation.
        }
        Destroy(gameObject); // destroy the pickup object
    }
}
