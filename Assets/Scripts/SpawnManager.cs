using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Donuts")]
    public GameObject[] donutPrefabs;    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Police Car")]
    public GameObject policeCarPrefab;
    public float policeSpawnRangeX = 0.6f; // x range to spawn the police car
    public float policeSpawnZ = 9.0f; // z  to spawn the police car
    public float policeSpawnY = -0.108f; // y position to spawn the police car
    public float policeSpawnMinInterval = 3.0f; // minimum time between each spawn
    public float policeSpawnMaxInterval = 5.0f; // maximum time between each spawn
    private float spawnRangeX = 0.8f;
    private float spawnMaxZ = 15.0f;
    private float spawnMinZ = 11.0f;
    private float startDelay = 2.0f;
    private float spawnInterval = 3.0f; //  time between each spawn
    private float nextPoliceSpawnTime = 0f;
    void Start()
    {
        InvokeRepeating("SpawnRandomDonut", startDelay, spawnInterval);  // call the SpawnRandomDonut function repeatedly after a delay 
        ScheduleNextPoliceSpawn(); // schedule the first police car spawn
    }

    // (Update is implemented below with police spawn logic)
    void SpawnRandomDonut()  // function to spawn a random donut
    {
        int donutIndex = Random.Range(0, donutPrefabs.Length);  // get a random index for the donut prefabs array
        Vector3 spawnPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 0, Random.Range(spawnMinZ, spawnMaxZ));  // get a random position to spawn the donut
        Instantiate(donutPrefabs[donutIndex], spawnPos, donutPrefabs[donutIndex].transform.rotation);  // spawn the donut at the position of the SpawnManager object
    }

    void Update()
    {
        // police car spawn timer
        if (Time.time >= nextPoliceSpawnTime && policeCarPrefab != null) // check if it's time to spawn a police car and if the prefab is assigned
        {
            SpawnPoliceCar(); // spawn the police car
            ScheduleNextPoliceSpawn(); // schedule the next police car spawn
        }
    }

    void ScheduleNextPoliceSpawn() // schedule when the next police car can spawn
    {
        float interval = Random.Range(policeSpawnMinInterval, policeSpawnMaxInterval); // get a random interval between min and max
        nextPoliceSpawnTime = Time.time + interval; // set the next spawn time
    }

    void SpawnPoliceCar() // actually spawn the police car
    {
        float x = Random.Range(-policeSpawnRangeX, policeSpawnRangeX); // get a random x position within the spawn range
        Vector3 spawnPos = new Vector3(x, policeSpawnY, policeSpawnZ); // set the spawn position using the random x and fixed z
        Instantiate(policeCarPrefab, spawnPos, policeCarPrefab.transform.rotation); // spawn the police car prefab at the calculated position with its default rotation
    }
}
