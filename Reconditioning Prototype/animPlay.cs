using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animPlay : MonoBehaviour
{
	
	//A script to override the existing animation tools glitchiness.
	
    bool isPlayed;
    void Update()
    {
        if (GetComponent<Animation>().clip != null && !isPlayed)
        {
            GetComponent<Animation>().Play();
            isPlayed = true;

        }
        if (isPlayed && !GetComponent<Animation>().isPlaying)
        {
            GetComponent<Animation>().clip = null;
            isPlayed = false;
        }
    }
}
