using UnityEngine;

[DisallowMultipleComponent] // only one allowed, because it makes no sense to have more than one PickupIdle script on a single object, just to be explicit)      
public class PickupIdle : MonoBehaviour
{
    [Header("Rotation")] // Unity attribute that adds a header in the Inspector window for better organization and readability.
    public Vector3 rotationSpeed = new Vector3(0f, 25f, 0f); // deg/sec

    [Header("Bob (Up/Down)")] // simple harmonic motion
    public float bobAmplitude = 0.05f;   // meters
    public float bobFrequency = 1.2f;   // Hz (oscillations per second)

    [Header("Emission (Optional Glow)")] // requires material with emission property (e.g. URP/Lit)
    public bool pulseEmission = false;
    public Color emissionColor = Color.yellow;
    public float emissionMin = 0.4f;    // intensity (HDR if using URP/HDRP)
    public float emissionMax = 1.2f;
    public float emissionPulseSpeed = 2f;

    // Internals
    Vector3 _startPos; // default is private
    float _phaseOffset;
    Renderer _renderer; //what is Renderer? A component that is responsible for rendering the visual representation of a GameObject in the scene. How is it used? It allows you to access and modify various properties related to the appearance of the object, such as its material, color, texture, and visibility.
    MaterialPropertyBlock _mpb; // what is MaterialPropertyBlock? A class in Unity that allows you to modify the properties of a material for a specific renderer without creating a new material instance. How is it used? It is used to change material properties like color, texture, or shader parameters on a per-object basis, which is more efficient than creating separate material instances for each object.
    static readonly int _EmissionColorID = Shader.PropertyToID("_EmissionColor"); // what is Shader.PropertyToID? A method in Unity that converts a shader property name (string) into an integer ID. How is it used? It is used to optimize performance when accessing shader properties, as using integer IDs is faster than using string names.

    void Awake() // Awake is called when the script instance is being loaded. It is used to initialize variables or states before the game starts. It is called only once during the lifetime of the script instance. This is different from Start, which is called just before any of the Update methods are called the first time. What is an script instance? An instance of a class that derives from MonoBehaviour and is attached to a GameObject in the scene. Wha is instance?  An individual occurrence of a class or object in programming.
    {
        _startPos = transform.position; // record starting position for bobbing
        _phaseOffset = Random.value * Mathf.PI * 2f; // desync multiple donuts by randomizing phase offset
        _renderer = GetComponentInChildren<Renderer>();
        if (_renderer != null)
        {
            _mpb = new MaterialPropertyBlock();
        }
    }

    void Update()
    {
        // 1) Slow spin
        transform.Rotate(rotationSpeed * Time.deltaTime, Space.World);

        // 2) Gentle bob
        float y = _startPos.y + Mathf.Sin((Time.time * bobFrequency) + _phaseOffset) * bobAmplitude;
        var p = transform.position;
        p.y = y;
        transform.position = p;

        // 3) Optional shimmer / emission pulse (no material instancing)
        if (pulseEmission && _renderer != null)
        {
            float t = (Mathf.Sin(Time.time * emissionPulseSpeed + _phaseOffset) + 1f) * 0.5f; // 0..1
            float intensity = Mathf.Lerp(emissionMin, emissionMax, t);

            _renderer.GetPropertyBlock(_mpb);
            _mpb.SetColor(_EmissionColorID, emissionColor * intensity);
            _renderer.SetPropertyBlock(_mpb);
        }
    }
}
