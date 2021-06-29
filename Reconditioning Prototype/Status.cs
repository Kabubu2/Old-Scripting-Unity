using UnityEngine;

public class Status : MonoBehaviour
{
	//This was the old viewbobber script, it lacked.. use-ability
    CharacterController cc;
    Animator animator;
    public Camera camera_;
    public GameObject neck;

    public bool ascending, falling, walking;

    private bool prevGroundCheck, curGroundCheck;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        animator = camera_.GetComponent<Animator>();
        defaultCamLoc = neck.transform.localPosition;
    }
    private void FixedUpdate()
    {
        //bobControl();
    }
    private void Update()
    {
        stateUpdater();
    }
    private void stateUpdater()
    {
        ascending = cc.velocity.y > 0.2f;
        falling = cc.velocity.y < -0.2f;
        walking = cc.velocity.x + cc.velocity.z != 0;

        //animator.SetBool("isAscending", ascending);
        //animator.SetBool("isFalling", falling);
        //animator.SetBool("isMoving", walking);
    }

    private Vector3 defaultCamLoc;
    public float viewBobIntensity;
    private float viewBobSpeed = 0.08f;
    private float viewBobSpeedDef = 0.08f;
    private float viewBobAmount = 0.2f;
    public float viewBobTimer;
    private float midpoint = 0f;
    private void bobControl()
    {
        //Courtesy of Muuskii -- Headbobber Script
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

        if (ascending)
        {
            neck.transform.localPosition = Vector3.Lerp(neck.transform.localPosition, defaultCamLoc + (Vector3.up * .2f), 5 * Time.fixedDeltaTime);
        }
        else if (falling)
        {
            neck.transform.localPosition = Vector3.Lerp(neck.transform.localPosition, defaultCamLoc + (Vector3.down * .7f), 5 * Time.fixedDeltaTime);
        }
        else if (walking)
        {
            neck.transform.localPosition = Vector3.Lerp(neck.transform.localPosition, cSharpConversion, 5 * Time.fixedDeltaTime);
        }
        else
        {
            neck.transform.localPosition = Vector3.Lerp(neck.transform.localPosition, defaultCamLoc, 5 * Time.fixedDeltaTime);
        }
    }

    private void stateSystem()
    {
        curGroundCheck = Physics.CheckSphere(GetComponent<MovementControllerCC>().groundCheck.transform.position, .1f, ~(1 << LayerMask.NameToLayer("Player")));

        if (curGroundCheck == true && prevGroundCheck == false)
        {
            //land
            Debug.Log("Landed");
        }
        if (curGroundCheck == false && prevGroundCheck == true)
        {
            //jump
            Debug.Log("Jumped");
        }

        prevGroundCheck = curGroundCheck;
    }

}
