using UnityEngine;

public class PlayerController : MonoBehaviour
{

     public float horizontalInput; 
     public float verticalInput;
        public float speed = 3.0f;
        public float xRange = 0.9f;
        public float zMax = 17.5f;
        public float zMin = 11.0f;
        public GameObject projectilePrefab; // prefab for projectile

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -xRange) // bottom boundary (I have reversed the input axis)
        {
            transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);  // keep player within boundary
        }

        if (transform.position.x > xRange)    // top boundary (I have reversed the input axis)
        {
            transform.position = new Vector3(xRange, transform.position.y, transform.position.z);  // keep player within boundary 
        }

        if (transform.position.z < zMin) // left boundary (I have reversed the input axis)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zMin);  // keep player within boundary
        }
        if (transform.position.z > zMax) // right boundary (I have reversed the input axis)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zMax);
        }

        horizontalInput = - Input.GetAxis("Vertical");  // reversed vertical input
        transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * speed);  // move player based on input

        verticalInput = Input.GetAxis("Horizontal");  // horizontal input
        transform.Translate(Vector3.forward * verticalInput * Time.deltaTime * speed);  // move player based on input
        
        if (Input.GetKeyDown(KeyCode.Space))  // if space key is pressed
        {
            // Launch a projectile from the player
            Instantiate(projectilePrefab, transform.position, projectilePrefab.transform.rotation);
        }
    }
}
