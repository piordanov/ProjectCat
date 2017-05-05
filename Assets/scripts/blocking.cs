using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class blocking : Photon.MonoBehaviour {

    //will work with cm
    public GameObject shield;
    Vector3 vecR, vecL;
    int i = 0;                              //i will be used for time, you can only block for 3 seconds at a time??
    private float distance, block_distance;
    //block_distance = 0.2;
    // Update is called once per frame

    void Update()
    {
        Debug.Log("check blocking");
        if (!photonView.isMine)
        {
            Debug.Log("not me");
            return;
        }

        float rHandTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.Touch);
        float lHandTrigger = OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger, OVRInput.Controller.Touch);
        if (rHandTrigger > 0.5f && lHandTrigger > 0.5f)
        {
            Debug.Log("HELD");
            /*
            if (i * Time.deltaTime <= 3)
            {
                //can only block for 3 seconds
                //what do we want to do here?
                i = 0;
                return;
            }*/
            i++;
            //need to check that the 2 controllers are within some distance to indicate a block
            vecR = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
            vecL = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
            distance = Mathf.Sqrt(((vecR.x - vecL.x) * (vecR.x - vecL.x)) + ((vecR.y - vecL.y) * (vecR.y - vecL.y)) + ((vecR.z - vecL.z) * (vecR.z - vecL.z)));
            Debug.Log("distance: " + distance);
            if (distance <= .1)
            {
                Debug.Log("BLOCKING");
                shield.SetActive(true);
            }
        }
        else
        {
            shield.SetActive(false);
        }
    }

}
