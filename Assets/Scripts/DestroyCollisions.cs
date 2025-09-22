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
        Destroy(gameObject); // destroys the object this script is attached to
        Destroy(other.gameObject); // destroys the other object that collides with this object
    }
}
