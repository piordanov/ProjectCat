using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossController : MonoBehaviour
{
    public GameObject sphere;
    //will work with cm
    Vector3 vec, vec2;
    int i = 0;
    public float x1_start, x1_end, x2_start, x2_end;
    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger) && OVRInput.Get(OVRInput.RawButton.LIndexTrigger))
        {
            Debug.Log("RIGHT");
            Debug.Log(OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch));
            Debug.Log("LEFT");
            Debug.Log(OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch));
            i++;
            if (i == 1)
            {
                vec = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
                vec2 = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
                x1_start = vec.x;
                x2_start = vec2.x;

                //sphere.SetActive(false);
            } 
            Debug.Log("time = " + i * Time.deltaTime);
            if (i * Time.deltaTime <= 2)
            {
                vec = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
                vec2 = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
                x1_end = vec.x;
                x2_end = vec2.x;
				if ((((x1_start - x1_end) > .5) && ((x2_end - x2_start) > .5)) || (((x1_end - x1_start) > .5) && ((x2_start - x2_end) > .5)))
                {
                    Debug.Log("ATTACK!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    sphere.SetActive(false);
                    i = 0;
                    StartCoroutine(Example());
                }
            }

            // Debug.Log(OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch));
        }
        else
        {
            i = 0;
        }

    }
    IEnumerator Example()
    {
        print(Time.time);
        yield return new WaitForSeconds(2);
        sphere.SetActive(true);
        print(Time.time);
    }
}

