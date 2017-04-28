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