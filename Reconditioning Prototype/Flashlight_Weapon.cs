using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class Flashlight_Weapon : MonoBehaviour
{
	
	//Attempted test for faked bounce lighting and bootleg ssgi with the flashlight.
	
    public GameObject light;
    public GameObject bounceLight;

    private Ray ray;
    private RaycastHit hit;
    private RaycastHit reflectedHit;
    private Ray reflectedRay;

    [Header("Light Control Panel")]
    [SerializeField] private float lightStrength;
    [SerializeField] private float bounceStrength;
    [SerializeField] private float bounceRange;
    public float wallCorrection;

    private void Update()
    {
        wallCorrection = Mathf.Clamp(Vector3.Distance(hit.point, reflectedHit.point), 0, 100);
        ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray.origin, ray.direction, out hit))
        {
            reflectedRay = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
            bounceLight.GetComponent<Light>().enabled = true;
            bounceLight.transform.position = hit.point;
            bounceLight.transform.LookAt(reflectedRay.origin + hit.normal);
            bounceLight.GetComponent<Light>().intensity = (light.GetComponent<Light>().intensity / (2 / bounceStrength));
            bounceLight.GetComponent<Light>().range = bounceRange;
        }
        else
        {
            bounceLight.GetComponent<Light>().enabled = false;
        }
    }

}
