using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashController : Photon.MonoBehaviour
{
    private bool isColliderEnabled = false;
    void Start()
    {
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
        yield return new WaitForSeconds(3);
        PhotonNetwork.Destroy(this.gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale += new Vector3(0.0001f, 0.00f, 0.0001f);
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

    void OnTriggerEnter(Collider other)
    {
        if (photonView.isMine && isColliderEnabled)
        {
			//Debug.Log("going in");
			GameObject obj = other.gameObject;
			//Debug.Log(obj.tag);
			//Debug.Log(obj.name);
			//Debug.Log(obj);
			if (obj.tag == "avatar")
			{
				//Debug.Log("shuriken hit player");
				PlayerNetworkController parent = obj.GetComponentInParent<PlayerNetworkController>();
				if (parent != null)
				{
					parent.dealDamage(20);
				}
				else
				{
					Debug.Log("ERROR playernetworkcontroller not found");
				}
			}
			else if (obj.tag == "shield")
			{
				//Debug.Log ("shuriken hit shield");
				obj.SetActive(false);
			}
			PhotonNetwork.Destroy(this.gameObject);
			//Debug.Log("shuriken destroyed");
        }
    }
}
