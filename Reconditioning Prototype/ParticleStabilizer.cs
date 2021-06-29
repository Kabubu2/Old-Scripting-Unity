using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleStabilizer : MonoBehaviour
{
	
	//for some reason, unity would always play particles in world coords regardless of setting the setting to local so I made this to ammend that.
    private Vector3 startPos;
    private Quaternion startRot;
    private void Start()
    {
        startPos = transform.localPosition;
        startRot = transform.localRotation;
    }
    void Update()
    {
        if (transform.parent && gameObject.isStatic)
        {
            transform.localPosition = startPos;
            transform.localRotation = startRot;
        }
    }
}
