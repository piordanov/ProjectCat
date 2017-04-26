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

	void LateUpdate() {
		//avatar.transform.localPosition = playerLocal.position;
		//avatar.transform.localRotation = playerLocal.rotation;

		lHand.transform.position = lHandLocal.position;
		lHand.transform.rotation = lHandLocal.rotation;
		rHand.transform.position = rHandLocal.position;
		rHand.transform.localRotation = rHandLocal.rotation;
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(playerGlobal.position);
			stream.SendNext(playerGlobal.rotation);
			stream.SendNext(playerLocal.localPosition);
			stream.SendNext(playerLocal.localRotation);

			stream.SendNext(lHandLocal.position);
			stream.SendNext(lHandLocal.rotation);
			stream.SendNext(rHandLocal.localPosition);
			stream.SendNext(rHandLocal.localRotation);
		}
		else
		{
			this.transform.position = (Vector3)stream.ReceiveNext();
			this.transform.rotation = (Quaternion)stream.ReceiveNext();
			avatar.transform.localPosition = (Vector3)stream.ReceiveNext();
			avatar.transform.localRotation = (Quaternion)stream.ReceiveNext();

			lHand.transform.localPosition = (Vector3)stream.ReceiveNext();
			lHand.transform.localRotation = (Quaternion)stream.ReceiveNext();
			rHand.transform.localPosition = (Vector3)stream.ReceiveNext();
			rHand.transform.localRotation = (Quaternion)stream.ReceiveNext();
		}
	}
}