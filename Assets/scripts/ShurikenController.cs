using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenController : Photon.MonoBehaviour {

	private float speed = 4.0f;
	private int rotSpeed = 600;
    public Vector3 forwardVec;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (photonView.isMine) {
			//Debug.Log ("a shuriken is in control");
			this.transform.position += forwardVec * Time.deltaTime * speed;
		}
				
		transform.Rotate (0, rotSpeed * Time.deltaTime, 0);
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(this.transform.position);
		}
		else
		{
			this.transform.position = (Vector3)stream.ReceiveNext();
		}
	}
}
