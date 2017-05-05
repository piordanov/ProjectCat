using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{
    public GameObject sphere;
    //will work with cm
    Vector3 vec, vec2;
    int i = 0;
    public float y1_start, y1_end, y2_start, y2_end;
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
                y1_start = vec.y;
                y2_start = vec2.y;

                //sphere.SetActive(false);
            }
            Debug.Log("time = " + i * Time.deltaTime);
            if (i * Time.deltaTime <= 2)
            {
                vec = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
                vec2 = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
                y1_end = vec.y;
                y2_end = vec2.y;
                if (Mathf.Abs(y1_end - y1_start) > .4 && Mathf.Abs(y2_end - y2_start) > .4)
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
