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
        yield return new WaitForSeconds(8);
        PhotonNetwork.Destroy(this.gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);
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
            GameObject obj = other.gameObject;
            if (obj.tag == "avatar")
            {
                Debug.Log("slash hit player");
                obj.GetComponent<PlayerNetworkController>().hp -= 5;
            }
            else if (obj.tag == "shield")
            {
                Debug.Log("slash hit shield");
                obj.SetActive(false);
            }
            PhotonNetwork.Destroy(this.gameObject);
            Debug.Log("slash destroyed");
        }
    }
}
