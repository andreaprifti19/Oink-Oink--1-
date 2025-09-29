using UnityEngine;
using UnityEngine.UI; // for UI Text (legacy)
using TMPro; // support TextMeshPro UI

public class PlayerController : MonoBehaviour
{

    public float horizontalInput;
    public float verticalInput;
    public float speed = 3.0f;
    public float xRange = 0.9f;
    public float zMax = 17.5f;
    public float zMin = 11.0f;
    public GameObject projectilePrefab; // prefab for projectile
    [Header("Shooting")]
    public Transform firePoint; // where player projectiles spawn (assign a child Transform on the player)

    [Header("Pickup")] // just a header in the inspector for organization
    public int score = 0; // player's score
    public Text scoreText; // optional: assign a Unity UI Text (legacy)
    public TextMeshProUGUI scoreTextTMP; // optional: assign a TextMeshProUGUI if you're using TMP
    [Header("Health")]
    public int maxLives = 3; // maximum lives the player can have
    public int lives = 3; // current lives
    public Text livesText; // optional: assign a Unity UI Text (legacy)
    public TextMeshProUGUI livesTextTMP; // optional: assign a TextMeshProUGUI if you're using TMP

    // internals for pickup
    private DonutPickup _nearbyPickup; // reference to nearby pickup, null if none . What is "DonutPickup"? It's a class we defined in another script that represents a donut pickup item in the game. How does it work? It has methods and properties to handle pickup logic, like detecting when the player is near and allowing the player to pick it up. Is it a built-in class? No, it's a custom class we created for our game. "_nearbyPickup" is the variable name we chose to represent an instance of that class. What is an instance of a class? It's a specific object created from that class, with its own unique data and state. Why? Because we want to keep track of which pickup the player is currently near, so we can interact with it when the player presses the pickup key.

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // initialize score display (support both Unity UI Text and TextMeshPro)
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
        if (scoreTextTMP != null)
        {
            scoreTextTMP.text = "Score: " + score.ToString();
        }
        if (firePoint == null)
        {
            Debug.LogWarning("PlayerController: firePoint is not assigned. Projectiles will spawn at the player's position.", this);
        }

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

        horizontalInput = -Input.GetAxis("Vertical");  // reversed vertical input
        transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * speed);  // move player based on input

        verticalInput = Input.GetAxis("Horizontal");  // horizontal input
        transform.Translate(Vector3.forward * verticalInput * Time.deltaTime * speed);  // move player based on input

        if (Input.GetKeyDown(KeyCode.Space))  // if space key is pressed
        {
            // Launch a projectile from the player's fire point (fallback to player position)
                Vector3 spawnPos = (firePoint != null) ? firePoint.position : transform.position;
                Quaternion spawnRot = (firePoint != null) ? firePoint.rotation : projectilePrefab.transform.rotation;
                var proj = Instantiate(projectilePrefab, spawnPos, spawnRot);
                // mark ownership on whichever projectile component is present
                var pGeneric = proj.GetComponent<ProjectileGeorge>();
                if (pGeneric != null) pGeneric.firedByPlayer = true;
                var pGeorge = proj.GetComponent<ProjectileGeorge>();
                if (pGeorge != null) pGeorge.firedByPlayer = true;
                var pPolice = proj.GetComponent<ProjectilePolice>();
                if (pPolice != null) pPolice.firedByPlayer = true; // in case player prefab uses police script accidentally

                // prevent the projectile from colliding with the shooter (player)
                Collider projCol = proj.GetComponent<Collider>();
                if (projCol == null) projCol = proj.GetComponentInChildren<Collider>();
                Collider myCol = GetComponent<Collider>();
                if (myCol == null) myCol = GetComponentInChildren<Collider>();
                if (projCol != null && myCol != null)
                {
                    Physics.IgnoreCollision(projCol, myCol);
                }
        }

        // Pickup with 'S' when near a donut
        if (Input.GetKeyDown(KeyCode.G))// if 'G key is pressed    
        {
            if (_nearbyPickup != null) // if there is a nearby pickup. How do we know that? Because another script (DonutPickup) will call NotifyNearbyPickup when the player enters its trigger collider, setting _nearbyPickup to that pickup instance. When the player leaves the trigger, it calls NotifyLeftPickup to set _nearbyPickup back to null. Is this automatic? No, we have to implement that logic in the DonutPickup script. Is "pickup" an attribute? No, it's a parameter name we chose for the method. Why? Because we want to keep track of which pickup the player is currently near, so we can interact with it when the player presses the pickup key.
            {
                _nearbyPickup.Pickup(); // call the Pickup method on the nearby pickup. What does this do? It triggers the pickup logic defined in the DonutPickup class, which typically involves playing an effect and destroying the pickup object. Is this built-in code? No, it's our custom method defined in the DonutPickup class.
                score += 1; // increment score
                // update whichever UI text the developer assigned
                if (scoreText != null)
                {
                    scoreText.text = "Score: " + score.ToString();
                }
                if (scoreTextTMP != null)
                {
                    scoreTextTMP.text = "Score: " + score.ToString();
                }
                // initialize lives display
                lives = Mathf.Clamp(lives, 0, maxLives); // ensure lives is within valid range
                if (livesText != null) livesText.text = "Lives: " + lives.ToString(); // update lives display
                if (livesTextTMP != null) livesTextTMP.text = "Lives: " + lives.ToString(); // update lives display
            }
        }
    }

    // Called by projectiles or other damage sources
    public void TakeDamage(int amount) // reduce player lives by the given amount
    {
        lives -= amount;
        lives = Mathf.Max(lives, 0);
        if (livesText != null) livesText.text = "Lives: " + lives.ToString();
        if (livesTextTMP != null) livesTextTMP.text = "Lives: " + lives.ToString();

        if (lives <= 0)
        {
            Die();
        }
    }

    void Die() // handle player death
    {
        // simple death: remove player object
        Destroy(gameObject);
    }

    // Called by DonutPickup when entering/exiting trigger
    public void NotifyNearbyPickup(DonutPickup pickup) // notify that the player is near a pickup
    {
        _nearbyPickup = pickup; // set the nearby pickup reference to the given pickup instance
    }

    public void NotifyLeftPickup(DonutPickup pickup) // notify that the player has left a pickup
    {
        if (_nearbyPickup == pickup) _nearbyPickup = null; // clear the nearby pickup reference if it's the same pickup that was left
    }
}
