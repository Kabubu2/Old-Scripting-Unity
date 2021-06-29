using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentMovementEffect : MonoBehaviour
{
	//When this was enabled, it would handle the various worldly effects that would affect the character. Things like explosion rumble, viewbob, inertia displacement, etc.
	
    [Header("Viewbob")]
    [SerializeField]
    private bool viewBobber;
    [SerializeField]
    private GameObject neck;
    [SerializeField]
    private CharacterController cc;

    private void Start()
    {
        defaultCamLoc = neck.transform.localPosition;
        viewbobRemember = viewBobber;
    }
    private void FixedUpdate()
    {
        bobControl();
        explosionControl();
    }

    private Vector3 defaultCamLoc;
    public float viewBobIntensity;
    private float viewBobSpeed = 0.08f;
    private float viewBobSpeedDef = 0.08f;
    private float viewBobAmount = 0.2f;
    private float viewBobTimer;
    private float midpoint = 0f;
    private void bobControl()
    {
        //Courtesy of Muuskii -- Headbobber Script
        if (viewBobber)
        {
            viewBobSpeed = viewBobSpeedDef * ((Mathf.Abs(cc.velocity.x) + Mathf.Abs(cc.velocity.z)) / 1.5f);
            float waveslice = 0.0f;
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 cSharpConversion = neck.transform.localPosition;

            if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
            {
                viewBobTimer = 0.0f;
            }
            else
            {
                waveslice = Mathf.Sin(viewBobTimer);
                viewBobTimer = viewBobTimer + viewBobSpeed;
                if (viewBobTimer > Mathf.PI * 2)
                {
                    viewBobTimer = viewBobTimer - (Mathf.PI * 2);
                }
            }
            if (waveslice != 0)
            {
                float translateChange = waveslice * viewBobAmount;
                float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
                totalAxes = Mathf.Clamp(totalAxes, 0.1f, 1.0f);
                translateChange = totalAxes * translateChange;
                cSharpConversion.y = midpoint + translateChange;
            }
            else
            {
                cSharpConversion.y = midpoint;
            }

            neck.transform.localRotation = Quaternion.Euler(new Vector3(-Input.GetAxis("Vertical") * viewBobIntensity, 0, -Input.GetAxis("Horizontal") * viewBobIntensity));


            if (GetComponent<Status>().ascending)
            {
                neck.transform.localPosition = Vector3.Lerp(neck.transform.localPosition, defaultCamLoc + (Vector3.up * .2f), 5 * Time.fixedDeltaTime);
            }
            else if (GetComponent<Status>().falling)
            {
                neck.transform.localPosition = Vector3.Lerp(neck.transform.localPosition, defaultCamLoc + (Vector3.down * .7f), 5 * Time.fixedDeltaTime);
            }
            else if (GetComponent<Status>().walking)
            {
                neck.transform.localPosition = Vector3.Lerp(neck.transform.localPosition, cSharpConversion, 5 * Time.fixedDeltaTime);
            }
            else
            {
                neck.transform.localPosition = Vector3.Lerp(neck.transform.localPosition, defaultCamLoc, 5 * Time.fixedDeltaTime);
            }
        }
    }
    [Header("Explosion")]
    [SerializeField]
    private bool explosionSuseptable;
    [SerializeField]
    private float explosionIntensity;
    private bool viewbobRemember;
    private void explosionControl()
    {
        explosionSuseptable = (explosionIntensity == 0) ? false : true;
        if (explosionSuseptable)
        {
            Vector3 VibrationLocationTarget = defaultCamLoc + new Vector3(Random.Range(-1, 1) * explosionIntensity, Random.Range(-1, 1) * explosionIntensity, Random.Range(-1, 1) * explosionIntensity);

                viewBobber = false;
                neck.transform.localPosition = Vector3.Lerp(neck.transform.localPosition, VibrationLocationTarget, 10 * Time.fixedDeltaTime);
            
        }
        else if (!viewbobRemember)
        {
            neck.transform.localPosition = Vector3.Lerp(neck.transform.localPosition, defaultCamLoc, 10 * Time.fixedDeltaTime);
        }
        else if (viewbobRemember)
        {
            viewBobber = viewbobRemember;
        }
    }
}
