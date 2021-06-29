using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif

//This was the harded but I never finished it so there's probably mnothing useful except the ricochet script.
public enum weaponType { melee, gun }
public enum fireType { burst, line }
public class WeaponSystem : MonoBehaviour
{
    public weaponType WeaponTypes;

    public fireType FireType;
    public float fireRate;
    public bool ricochet;
    //public int bounces;
    
       Ray ray;
       RaycastHit hit;
       GameObject lastHit;
       public int reflections;
       public Vector3[] bouncePoints; 
       public Ray[] bounceDirections;
    private void Start()
    {
        bounceDirections = new Ray[reflections];
        bouncePoints = new Vector3[reflections];
    }

    private void Update()
    {
        if (ricochet)
        {
            Ricochet();
        }
    }
    private void Ricochet()
       {
        ray = new Ray(transform.position, transform.forward);

        for (int i = 0; i < reflections; i++) 
           {
            if (Physics.Raycast(ray.origin, ray.direction, out hit)) 
               {
                Debug.DrawLine(ray.origin, hit.point, Color.red, .1f);
                ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
                bounceDirections[i] = ray;
                bouncePoints[i] = ray.origin;
                lastHit = hit.transform.gameObject;
               }
           }
       }

}
#if UNITY_EDITOR
[CustomEditor(typeof(WeaponSystem))]
public class WeaponSystem_Editor : Editor
{ 

}
#endif
