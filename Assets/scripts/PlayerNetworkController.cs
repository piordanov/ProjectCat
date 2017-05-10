using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerNetworkController : Photon.MonoBehaviour
{
	public GameObject avatar;
	public GameObject lHand;
	public GameObject rHand;
    public GameObject avatarbody;

	public Transform playerGlobal;
	public Transform playerLocal;

	public Transform lHandLocal;
	public Transform rHandLocal;

    public bool shieldOn = false;

    //blocking items
    public GameObject shield;
    Vector3 vecR, vecL;
    int i = 0;                              //i will be used for time, you can only block for 3 seconds at a time??
    private float distance, block_distance;

	//slash objects
	Vector3 vec, vec2;
	public float x1_start, x1_end, x2_start, x2_end;

    //shuriken globals
    float xRstart, xLstart;
    float zRstart, zLstart;
    float xRend, xLend;
    float zRend, zLend;
    int shuriken_flag = 0;

    // triggers
    bool indexLtrigger;
    bool indexRtrigger;
    float lHandTrigger;
    float rHandTrigger;
    bool handTrigger;
    bool indexTrigger;

    //time delay internals
    float fireRate = 1.0f;
    float nextSlash;
    float nextShuriken;

	public int hp = 100;
    public int id;
    private Text northPlayer1Score;
    private Text northPlayer2Score;

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

            GameObject textobj = GameObject.Find("/NorthCanvas/Score1");
            northPlayer1Score = textobj.GetComponent<Text>();
            textobj = GameObject.Find("/NorthCanvas/Score2");
            northPlayer2Score = textobj.GetComponent<Text>();
            if (northPlayer1Score == null)
            {
                Debug.Log("player1score not found");
            }
            if (northPlayer2Score == null)
            {
                Debug.Log("plaer2score not found");
            }


        }
	}
    public static Vector3 getRelativePosition(Transform origin, Vector3 position)
    {
        Vector3 distance = position - origin.position;
        Vector3 relativePosition = Vector3.zero;
        relativePosition.x = Vector3.Dot(distance, origin.right.normalized);
        relativePosition.y = Vector3.Dot(distance, origin.up.normalized);
        relativePosition.z = Vector3.Dot(distance, origin.forward.normalized);

        return relativePosition;
    }
    void Update() {
		if (photonView.isMine) {
            lHand.transform.position = lHandLocal.position;
            lHand.transform.rotation = lHandLocal.rotation;
            rHand.transform.position = rHandLocal.position;
            rHand.transform.rotation = rHandLocal.rotation;

            rHandTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.Touch);
            lHandTrigger = OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger, OVRInput.Controller.Touch);
            indexRtrigger = OVRInput.Get(OVRInput.RawButton.RIndexTrigger);
            indexLtrigger = OVRInput.Get(OVRInput.RawButton.LIndexTrigger);

            handTrigger = (rHandTrigger > 0.5f && lHandTrigger > 0.5f);
            indexTrigger = (indexLtrigger && indexRtrigger);
            if ( handTrigger || indexTrigger)
            {
                //vecR = getRelativePosition(avatar.transform, OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch));
                //vecL = getRelativePosition(avatar.transform, OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch));
                vecR = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
                vecL = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
            }
            if (indexTrigger)
            {
                checkSlash();
                checkShurikenThrow();
            }
            else
            {
                i = 0;
                shuriken_flag = 0;

            }
            if (handTrigger)
            {
                checkBlocking();
            }
            else {
                shield.SetActive(false);
            }

        }
        avatarbody.transform.rotation = Quaternion.identity;
        avatarbody.transform.position = avatar.transform.position;
        avatarbody.transform.Translate(0, -0.5f, 0);
    }
	void checkSlash()
	{
		i++;
		if (i == 1)
		{
			x1_start = vecR.y;
			x2_start = vecL.y;
		} 
		if (i * Time.deltaTime <= 2)
		{
			x1_end = vecR.y;
			x2_end = vecL.y;
			if ((Mathf.Abs(x1_start - x1_end) > .35) && (Mathf.Abs(x2_end - x2_start) > .35) || (Mathf.Abs(x1_end - x1_start) > .35) && (Mathf.Abs(x2_start - x2_end) > .35))
			{
				spawnSlash (avatar.transform.position);
                i = 0;
            
			}
		}
        
	}
    void checkShurikenThrow()
    {
            shuriken_flag++;
            if (shuriken_flag == 1)
            {
               // xLstart = vecL.x;
               // xRstart = vecR.x;
                zLstart = vecL.z;
                zRstart = vecR.z;
            }
            if (shuriken_flag * Time.deltaTime <= 2)
            {
              //  xLend = vecL.x;
               // xRend = vecR.x;
                zLend = vecL.z;
                zRend = vecR.z;
                if ((zRend - zRstart >= .16) || (zLend - zLstart >= .1))
                {
                    spawnShuriken(lHandLocal.position);
                    shuriken_flag = 0;
                }
            }
    }

    void spawnShuriken(Vector3 pos)
    {
		if (Time.time > nextShuriken) {
            Debug.Log("throwing shuriken");
			GameObject projectile = PhotonNetwork.Instantiate ("shuriken", pos, avatar.transform.rotation, 0) as GameObject;
            projectile.GetComponent<Rigidbody>().velocity = avatar.transform.forward * 4.0f;
            nextShuriken = Time.time + fireRate;
		}
    }

    void spawnSlash(Vector3 pos)
    {
        if (Time.time > nextSlash)
        {
            Debug.Log("throw slash");
            GameObject projectile = PhotonNetwork.Instantiate("slash", pos, avatar.transform.rotation, 0) as GameObject;
            projectile.transform.Rotate(0, 90f, 90f);
            projectile.GetComponent<Rigidbody>().velocity = avatar.transform.forward * 6.0f;

            nextSlash = Time.time + fireRate;
        }
        Debug.Log("slash cooldown");
    }

    void checkBlocking()
    {
            //need to check that the 2 controllers are within some distance to indicate a block
            distance = Mathf.Sqrt(((vecR.x - vecL.x) * (vecR.x - vecL.x)) + ((vecR.y - vecL.y) * (vecR.y - vecL.y)) + ((vecR.z - vecL.z) * (vecR.z - vecL.z)));
            if (distance <= .1)
            {
                shield.SetActive(true);
            }
    }

    public void dealDamage(int damage)
    {
        hp -= damage;
        Debug.Log("hp reamaining: " + hp);
        updateScoreBoards(hp, id);

    }
    void updateScoreBoards(int newHP, int player)
    {
        Debug.Log("updating score board of player " + player + " with new hp " + newHP);
        if (player == 0)
        {
            Debug.Log("change");
            northPlayer1Score.text = hp + "/100";
        }
        else
        {
            northPlayer2Score.text = hp + "/100";
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

            stream.SendNext(shield.activeSelf);
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

            shield.SetActive((bool)stream.ReceiveNext());
		}
	}
}