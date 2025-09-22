using UnityEngine;

public class MoveRight : MonoBehaviour
{
    public float speed = 15.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);  // move the object forward at a constant rate
    }
}
