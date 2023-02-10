using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuoyancyProbe : MonoBehaviour
{
    public float thickness = 1;
    public float displacementAmount = 1;
    public WaveManager WaveManager { get; set; }
    public Vector3[] probePoints;

    private new Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = gameObject.GetComponentInParent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        foreach(Vector3 probePoint in probePoints)
        {
            Vector3 point = transform.position + probePoint;
            float submergedAmount = Mathf.Clamp01((WaveManager.SampleHeight(point) - point.y) / thickness);
            Vector3 buoyantForce = new Vector3(0, Mathf.Abs(Physics.gravity.y) * submergedAmount, 0);
            rigidbody.AddForceAtPosition(buoyantForce, transform.position, ForceMode.Acceleration);
        }
    }
}
