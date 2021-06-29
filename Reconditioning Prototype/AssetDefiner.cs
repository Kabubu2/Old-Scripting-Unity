using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AssetDefiner : AssetDetail
{	

	//Various props should have different features if not deemed a default static object. This would be that definer.
    public assetType AssetType;

    private bool canClimb;
    public bool canCover;
    private GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if (AssetType == assetType.Ladder)
        {
            if (other.tag == "Player")
            {
                canClimb = true;
                player = other.gameObject;
                player.GetComponent<MovementControllerCC>().canClimb = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (AssetType == assetType.Ladder)
        {
            if (other.tag == "Player" && player != null)
            {
                canClimb = false;
                player.GetComponent<MovementControllerCC>().canClimb = false;
                player = null;
            }
        }
    }
    private void FixedUpdate()
    {
        if (canClimb)
        {
            if (player != null )
            {
                if (Input.GetButtonDown("Jump"))
                {
                    canClimb = false;
                    player.GetComponent<MovementControllerCC>().canClimb = false;
                    player = null;
                }
                if (player.GetComponent<MovementControllerCC>() != null)
                {
                    if (Input.GetAxis("Vertical") > 0)
                    {
                        player.GetComponent<CharacterController>().Move(new Vector3(0, 1, 0) * Time.fixedDeltaTime * 5);
                    }
                    if (Input.GetAxis("Vertical") < 0)
                    {
                        player.GetComponent<CharacterController>().Move(new Vector3(0, -1, 0) * Time.fixedDeltaTime * 5);
                    }
                }
                
            }
        }
		//This particular definition is what was used to allow the player to phase through holes objects in phantom mode.
        if (AssetType == assetType.Grate)
        {
            if (GameObject.Find("EventSystem").GetComponent<SceneTargets>())
            {
                SceneTargets tmpST = GameObject.Find("EventSystem").GetComponent<SceneTargets>();
                if (tmpST.GetComponent<SceneTargets>().Player)
                {
                    if (tmpST.GetComponent<SceneTargets>().Player.GetComponent<MovementControllerCC>().checkAbilitiesDash)
                    {
                        GetComponent<Collider>().isTrigger = true;
                    }
                    else
                    {
                        GetComponent<Collider>().isTrigger = false;
                    }
                }
            }
        }
    }


}
public class AssetDetail : MonoBehaviour
{
    public enum assetType { Null, Ladder, Pickupable, Grate, Weapon }
}
