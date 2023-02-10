using UnityEngine;

public class CameraOrbiter : MonoBehaviour
{
    // Public members
    public float zoomSensitivity = 1f;
    public float orbitSensitivity = 3f;
    public Vector3 TargetOrigin => targetTransform != null ? targetTransform.position : targetPosition;
    public Quaternion TargetRotation => Quaternion.Euler(90 * Mathf.Pow(elevation / 90, 0.75f), azimuth, 0);
    public Vector3 TargetPosition => TargetOrigin + Quaternion.Euler(elevation, azimuth, 0) * new Vector3(0, 0, -distance);
    public Vector3 targetPosition;
    [Range(10, 1000)]
    public float distance = 100;
    [Range(0, 360)]
    public float azimuth = 0;
    [Range(0.5f, 45)]
    public float orbitSpeed = 5;
    [Range(0, 90)]
    public float elevation = 60f;

    // Private members
    private const float distanceMin = 10;
    private const float distanceMax = 1000;
    private const float elevationMin = 0;
    private const float elevationMax = 90;
    private new Camera camera;
    private new Rigidbody rigidbody;
    private Transform targetTransform;
    private float manualControlExpirationTime = 0;
    private bool manualControl = false;

    public void SetTarget(Transform target) => targetTransform = target;
    public void SetTarget(Vector3 target) => targetPosition = target;

    private void Start()
    {
        camera = GetComponent<Camera>();
        camera.transform.parent = transform;
        camera.transform.rotation = Quaternion.Euler(elevation, azimuth, 0);
        Vector3 offset = new Vector3(0, 0, -distance);
        camera.transform.localPosition = camera.transform.rotation * offset;
        rigidbody = gameObject.GetOrAddComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.mass = 1;
        rigidbody.drag = rigidbody.angularDrag = 0.1f;
    }
    private void Update()
    {
        // Get rotation input.
        /*if(Input.GetAxis("MouseMiddle") > 0)
        {
            azimuth += Input.GetAxis("Mouse X") * orbitSensitivity;
            elevation = Mathf.Clamp(elevation - Input.GetAxis("Mouse Y") * orbitSensitivity, elevationMin, elevationMax);
            camera.transform.localRotation = Quaternion.Euler(elevation, azimuth, 0);
            manualControl = true;
            manualControlExpirationTime = Time.time + 10;
        }

        // Get zoom input.
        if(Input.GetAxisRaw("MouseWheel") != 0)
        {
            distance = Mathf.Clamp(distance - Input.GetAxis("MouseWheel") * zoomSensitivity * distance, distanceMin, distanceMax);
            manualControl = true;
            manualControlExpirationTime = Time.time + 10;
        }*/

        // Move the camera.
        //rigidbody.AddForce((TargetPosition - transform.position) * 0.1f, ForceMode.Acceleration);
        transform.position = TargetPosition;
        //Quaternion deltaRotation = Quaternion.Inverse(transform.rotation) * TargetRotation;
        //rigidbody.AddTorque(deltaRotation.eulerAngles * 0.01f, ForceMode.Acceleration);
        transform.rotation = TargetRotation;

        // Check manual control.
        if(manualControl && Time.time > manualControlExpirationTime)
        {
            manualControl = false;
        }
        if(!manualControl)
        {
            azimuth += Mathf.Repeat(Time.deltaTime * orbitSpeed, 360);
        }
    }
}
