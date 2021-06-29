using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveTowardPoint : MonoBehaviour
{
	//this was a test for a phantom pull ability.
    public Transform point;
    public float force_ = 2000, StableFac = 15;
    private void FixedUpdate()
    {
       var rb = GetComponent<Rigidbody>();

        Vector3 force = point.position - transform.position;
        if (force.magnitude < StableFac) force = force.normalized * Mathf.Sqrt(force.magnitude);

        rb.AddForce(force * force_);
    }
}
