using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shuriken : MonoBehaviour
{
	public GameObject sphere;
	//will work with cm
	Vector3 vec, vec2;
	int i = 0;
	public float z_end, z_start, x_end, x_start;
	// Update is called once per frame
	void Update()
	{
		if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
		{
			Debug.Log("RIGHT");
			Debug.Log(OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch));
			i++;
			if (i == 1)
			{
				vec = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
				x_start = vec.x;
				z_start = vec.z;

				//sphere.SetActive(false);
			}
			Debug.Log("time = " + i * Time.deltaTime);
			if (i * Time.deltaTime <= 4)
			{
				vec = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
				x_end = vec.x;
				z_end = vec.z;

				if(x_end - x_start >= .1 && z_end - z_start >= .3)
				{
					Debug.Log("ATTACK!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
					sphere.SetActive(false);
					i = 0;
					StartCoroutine(Example());
				}
			}

		}

		if(OVRInput.Get(OVRInput.RawButton.LIndexTrigger))
		{
			Debug.Log("LEFT");
			Debug.Log(OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch));
			i++;
			if (i == 1)
			{
				vec = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
				x_start = vec.x;
				z_start = vec.z;

			}
			Debug.Log("time = " + i * Time.deltaTime);
			if (i * Time.deltaTime <= 2)
			{
				vec = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
				x_end = vec.x;
				z_end = vec.z;

				if (x_start - x_end >= .2 && z_end - z_start >= .3)
				{
					Debug.Log("ATTACK!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
					sphere.SetActive(false);
					i = 0;
					StartCoroutine(Example());
				}
			}

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