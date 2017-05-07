using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenController : Photon.MonoBehaviour {

	//private float speed = 4.0f;
	private int rotSpeed = 600;
    private bool isColliderEnabled = false;
   // public Vector3 forwardVec;
	// Use this for initialization
	void Start () {
        if (photonView.isMine)
        {
            StartCoroutine(waitAndDestroy());
            StartCoroutine(enableCollision());
        }
    }

    IEnumerator enableCollision()
    {
        yield return new WaitForSeconds(.02f);
        isColliderEnabled = true;
    }

    IEnumerator waitAndDestroy()
    {
        yield return new WaitForSeconds(5);
        PhotonNetwork.Destroy(this.gameObject);

    }
	
	// Update is called once per frame
	void Update () {
				
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

	void OnTriggerEnter(Collider other) {
		if (photonView.isMine && isColliderEnabled) {
			GameObject obj = other.gameObject;
			if (obj.tag == "avatar") {
				Debug.Log ("shuriken hit player");
				obj.GetComponent<PlayerNetworkController> ().hp -= 5;
			} else if (obj.tag == "shield") {
				Debug.Log ("shuriken hit shield");
				obj.SetActive (false);
			}
			PhotonNetwork.Destroy (this.gameObject);
			Debug.Log ("shuriken destroyed");
		}
	}
}
