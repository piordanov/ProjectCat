using UnityEngine;
using System.Collections;

public class PlayerNetworkController : Photon.MonoBehaviour
{
	public GameObject avatar;
	public GameObject lHand;
	public GameObject rHand;

	public Transform playerGlobal;
	public Transform playerLocal;

	public Transform lHandLocal;
	public Transform rHandLocal;

    //blocking items
    public GameObject shield;
    Vector3 vecR, vecL;
    int i = 0;                              //i will be used for time, you can only block for 3 seconds at a time??
    private float distance, block_distance;

    void Start ()
	{
		Debug.Log("i'm instantiated");

		if (photonView.isMine)
		{
			Debug.Log("player is mine");

			playerGlobal = GameObject.Find("OVRPlayerController").transform;
			if (playerGlobal == null) {
				Debug.Log("cannot find playerGlobal");
			}
			playerLocal = playerGlobal.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor").transform;
			lHandLocal = playerGlobal.Find("OVRCameraRig/TrackingSpace/LeftHandAnchor").transform;
			rHandLocal = playerGlobal.Find("OVRCameraRig/TrackingSpace/RightHandAnchor").transform;
			if (lHandLocal == null) {
				Debug.Log ("lHandLocal not instantiated");
			}
			if (rHandLocal == null) {
				Debug.Log ("rHandLocal not instantiated");
			}

			this.transform.SetParent(playerLocal);

			this.transform.localPosition = Vector3.zero;

		}
	}

	void Update() {
		if (photonView.isMine) {
			lHand.transform.position = lHandLocal.position;
			lHand.transform.rotation = lHandLocal.rotation;
			rHand.transform.position = rHandLocal.position;
			rHand.transform.rotation = rHandLocal.rotation;
            checkBlocking();
            checkShurikenThrow();
		}
	}
    void checkShurikenThrow()
    {
        Vector3 vec;
        float z_end = 0, z_start = 0, x_end = 0, x_start = 0;
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

                if (x_end - x_start >= .1 && z_end - z_start >= .3)
                {
                    Debug.Log("ATTACK!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    spawnShuriken(lHandLocal.position);
                    i = 0;
                }
            }

        }

        if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger))
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
                    i = 0;
                    spawnShuriken(lHandLocal.position);
                }
            }

        }

        else
        {
            i = 0;
        }
    }

    [PunRPC]
    void spawnShuriken(Vector3 pos)
    {
        Debug.Log("throw shuriken");
        Instantiate(Resources.Load("shuriken"), pos, Quaternion.identity);
    }

    void checkBlocking()
    {
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

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(playerGlobal.position);
			stream.SendNext(playerGlobal.rotation);
			stream.SendNext(playerLocal.position);
			stream.SendNext(playerLocal.rotation);

			stream.SendNext(lHandLocal.position);
			stream.SendNext(lHandLocal.rotation);
			stream.SendNext(rHandLocal.position);
			stream.SendNext(rHandLocal.rotation);
		}
		else
		{
			this.transform.position = (Vector3)stream.ReceiveNext();
			this.transform.rotation = (Quaternion)stream.ReceiveNext();
			avatar.transform.position = (Vector3)stream.ReceiveNext();
			avatar.transform.rotation = (Quaternion)stream.ReceiveNext();

			lHand.transform.position = (Vector3)stream.ReceiveNext();
			lHand.transform.rotation = (Quaternion)stream.ReceiveNext();
			rHand.transform.position = (Vector3)stream.ReceiveNext();
			rHand.transform.rotation = (Quaternion)stream.ReceiveNext();
		}
	}
}