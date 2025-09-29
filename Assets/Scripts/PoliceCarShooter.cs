using UnityEngine;

public class PoliceCarShooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint; // where the projectile spawns (assign in prefab). How does it know to fire from the police car? Because this script will be attached to the police car prefab, and the firePoint will be a child transform of the police car prefab.
    public float minFireInterval = 1.0f; // minimum time between each fire
    public float maxFireInterval = 2.0f; // maximum time between each fire

    private float nextFireTime = 0f; // time when the police car can fire next. 0 means it can fire immediately when the game starts.

    void Start()
    {
        ScheduleNextFire(); // schedule the first fire time. Why is it on start? Because we want the police car to start firing as soon as the game starts.
        
    }

    void Update()
    {
        if (projectilePrefab == null || firePoint == null) return; // safety check.How? If either the projectilePrefab or firePoint is not assigned in the inspector, we simply return and do nothing to avoid errors.

        if (Time.time >= nextFireTime) // check if it's time to fire. How? Time.time gives the time in seconds since the start of the game. If the current time is greater than or equal to nextFireTime, it's time to fire. But nextFierTime is initially 0, so the police car can fire immediately when the game starts.
        {
            Fire(); // fire a projectile. Is it automatic? Yes, because this is in Update which runs every frame. Is this build in code? No, it's our custom method defined below.
            ScheduleNextFire(); // schedule the next fire time. How? We call the ScheduleNextFire method defined below which sets nextFireTime to the current time plus a random interval between minFireInterval and maxFireInterval.
        }
    }

    void ScheduleNextFire() // schedule when the police car can fire next
    {
        nextFireTime = Time.time + Random.Range(minFireInterval, maxFireInterval);
    }

    void Fire() // actually fire the projectile
    {
        var proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // set ownership on known projectile types
        var pGeneric = proj.GetComponent<ProjectilePolice>();
        if (pGeneric != null) pGeneric.firedByPlayer = false;
        var pGeorge = proj.GetComponent<ProjectileGeorge>();
        if (pGeorge != null) pGeorge.firedByPlayer = false;
        var pPolice = proj.GetComponent<ProjectilePolice>();
        if (pPolice != null) pPolice.firedByPlayer = false;

        // prevent projectile from colliding with the police car itself
        Collider projCol = proj.GetComponent<Collider>();
        if (projCol == null) projCol = proj.GetComponentInChildren<Collider>();
        Collider carCol = GetComponent<Collider>();
        if (carCol == null) carCol = GetComponentInChildren<Collider>();
        if (projCol != null && carCol != null)
        {
            Physics.IgnoreCollision(projCol, carCol);
        }
    }
}
