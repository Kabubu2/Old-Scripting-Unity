using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUse : MonoBehaviour
{
    public Camera camera_;
    private MovementControllerCC movementController;
    private CharacterController cc;
    [SerializeField] public GameObject heldObject;
    //This was the phantom pull ability/ the use function. It would pull an object to the player if too far other wise it would interact with an object by picking it up and using interactables.
    private void Start()
    {
        movementController = GetComponent<MovementControllerCC>();
        cc = GetComponent<CharacterController>();
    }
    private void Update()
    {
        if (!movementController.checkAbilitiesDash)
        {
            Pickup();
            
        }
    }
    private void FixedUpdate()
    {
        holdingObject();
        Ability_Pull();
    }

    public float pickupDistanceNormal;
    public float pickupDistancePull;

    private void Pickup()
    {
        if (heldObject == null)
        {
            if (Input.GetButtonDown("Use"))
            {
                if (viewedObject(pickupDistanceNormal) != null && viewedObject(pickupDistanceNormal).GetComponent<AssetDefiner>())
                {
                    if (viewedObject(pickupDistanceNormal).GetComponent<AssetDefiner>().AssetType == AssetDetail.assetType.Pickupable)
                    {
                        Debug.Log("Pickup");
                        heldObject = viewedObject(pickupDistanceNormal);
                        drag = heldObject.GetComponent<Rigidbody>().drag;
                        angularDrag = heldObject.GetComponent<Rigidbody>().angularDrag;
                    }
                }
            }
        }
        else
        {
            if (Input.GetButtonDown("Use"))
            {
                //if it touches the player it must be isTrigger
                //heldObject.transform.position = transform.position + transform.forward * 2;
                heldObject.GetComponent<Rigidbody>().drag = drag;
                heldObject.GetComponent<Rigidbody>().angularDrag = angularDrag;
                heldObject = null;
            }
        }
    }

    public float drag, angularDrag;
    public float force_ = 2000, StableFac = 15;
    private void holdingObject()
    {
        if (heldObject != null)
        {
            Vector3 point = camera_.transform.position + (camera_.transform.forward * 2);
            var rb = heldObject.GetComponent<Rigidbody>();

            Vector3 force = point - heldObject.transform.position;
            if (force.magnitude < StableFac) force = force.normalized * Mathf.Sqrt(force.magnitude);

            rb.drag = 10;
            rb.angularDrag = .5f;

            rb.AddForce(force * (force_ * rb.mass));

            if (Vector3.Distance(heldObject.transform.position, transform.position) > pickupDistanceNormal + 4f)
            {
                heldObject = null;
                rb.drag = drag;
                rb.angularDrag = angularDrag;
            }
        }
    }
    private GameObject viewedObject(float pickUpDistance)
    {
        RaycastHit hit;
        if (Physics.Raycast(camera_.transform.position, camera_.transform.forward, out hit, pickUpDistance, ~(1 << LayerMask.NameToLayer("Player"))))
        {
            return hit.transform.gameObject;
        }
        else
        {
            return null;
        }
    }
    private GameObject viewedObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(camera_.transform.position, camera_.transform.forward, out hit, ~(1 << LayerMask.NameToLayer("Player"))))
        {
            return hit.transform.gameObject;
        }
        else
        {
            return null;
        }
    }

    public float pullStrength;
    private bool Pulled;
    private GameObject PulledObject;
    public ParticleSystem pullEffect;
    private int PulledTimer, PulledTimerMax = 50;
    private void Ability_Pull()
    {
        if (viewedObject(pickupDistancePull) && heldObject == null)
        {
            if (viewedObject(pickupDistancePull).GetComponent<AssetDefiner>() && Input.GetButtonDown("PullAbility"))
            {
                GameObject tmp = viewedObject(pickupDistancePull);
                if (tmp.GetComponent<AssetDefiner>().AssetType == AssetDetail.assetType.Pickupable)
                {
                    Debug.Log("Pull Ability");
                    PulledObject = tmp;
                    tmp.GetComponent<Rigidbody>().AddForce((transform.position - tmp.transform.position).normalized * ((pullStrength * tmp.GetComponent<Rigidbody>().mass) * Vector3.Distance(tmp.transform.position, transform.position)), ForceMode.Force);
                    Pulled = true;
                }
            }
        }
        if (Pulled)
        {
            if (PulledTimer == 0)
            {
                PulledTimer = PulledTimerMax;
                pullEffect.transform.parent = PulledObject.transform;
                pullEffect.Play();
            }
            else
            {
                if (PulledTimer == 1)
                {
                    Pulled = false;
                    PulledObject = null;
                    pullEffect.transform.parent = null;
                    pullEffect.Stop();
                }
                PulledTimer--;
            }

        }
    }
}
