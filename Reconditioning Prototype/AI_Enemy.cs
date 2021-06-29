using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AI_Enemy : MonoBehaviour
{

    public Transform targetObject;
    Vector3 moveTo;
    NavMeshAgent NMAgent;
    GameObject EventSystem;
    // Start is called before the first frame update
    void Start()
    {
        NMAgent = GetComponent<NavMeshAgent>();
        moveTo = NMAgent.destination;
        EventSystem = GameObject.Find("EventSystem");
    }

	//Press 'f' to test on input.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            getToCover(true, 10);
        }
        if (Vector3.Distance(transform.position, targetObject.transform.position) > 1f)
        {
            NMAgent.SetDestination(moveTo);
        }
    }

    public GameObject coverObject;
    public bool lookingForCover;
    public bool mustSeePlayer;
	
	//A crude way of searching for a good place to go to. It will randomly pick a point in a radius and check LOS to the player. If he deems it blocking, he will go to it.
    public void findCover()
    {
            lookingForCover = true;
            Collider[] nearCover = Physics.OverlapSphere(transform.position, 10);
            int tmpSelectNum = Random.Range(0, nearCover.Length);
        if (nearCover[tmpSelectNum].GetComponent<AssetDefiner>())
        {
            if (nearCover[tmpSelectNum].GetComponent<AssetDefiner>().canCover)
            {
                coverObject = nearCover[tmpSelectNum].gameObject;
            }
            else
            {
                lookingForCover = false;
            }
        }
        else
        {
            lookingForCover = false;
        }
    }
	//The place to player check.
    private void getToCover(bool canSeeTarget, float Radius)
    {
        if (coverObject)
        {
            lookingForCover = false;
            if (mustSeePlayer && NMAgent.isStopped)
            {
                Vector3 checkPoint1 = coverObject.transform.position + new Vector3(Radius, 0, Radius), checkPoint2 = coverObject.transform.position + new Vector3(-Radius, 0, -Radius);
                Vector3 locationSelect = new Vector3(Random.Range(checkPoint1.x, checkPoint2.x), transform.position.y, Random.Range(checkPoint1.z, checkPoint2.z));
                Debug.Log("Get to cover:1");

                RaycastHit hit;
                if (Physics.Raycast(locationSelect, (EventSystem.GetComponent<SceneTargets>().Player.transform.position - locationSelect), out hit))
                {
                    Debug.Log(hit.transform.name);
                    if (hit.transform.gameObject == EventSystem.GetComponent<SceneTargets>().Player)
                    {
                        moveTo = locationSelect;
                        Debug.Log("Get to cover:2");
                    }
                }
            }
            else
            {
                Vector3 checkPoint1 = coverObject.transform.position + new Vector3(Radius, 0, Radius), checkPoint2 = coverObject.transform.position + new Vector3(-Radius, 0, -Radius);
                Vector3 locationSelect = new Vector3(Random.Range(checkPoint1.x, checkPoint2.x), transform.position.y, Random.Range(checkPoint1.z, checkPoint2.z));
                Debug.Log("Get to cover:1");

                RaycastHit hit;
                if (Physics.Raycast(locationSelect, (EventSystem.GetComponent<SceneTargets>().Player.transform.position - locationSelect), out hit))
                {
                    Debug.Log(hit.transform.name);
                    if (hit.transform.gameObject != EventSystem.GetComponent<SceneTargets>().Player)
                    {
                        moveTo = locationSelect;
                        Debug.Log("Get to cover:2");
                    }
                }
            }
            
        }
        else
        {
            if (!lookingForCover)
            {
                StartCoroutine("findCover");
            }
        }
    }
}
