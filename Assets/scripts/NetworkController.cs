using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkController : MonoBehaviour {

	string _room = "Tutorial_Convrge";
    public GameObject eyeAnchor;
    public OVRPlayerController playerController;

    private Vector3[] positions = new Vector3[] { new Vector3(0, 1, 3), new Vector3(0, 1, 18) };
    int playersJoined = 0;

	void Start()
	{
		PhotonNetwork.ConnectUsingSettings("0.1");
	}

	void OnJoinedLobby()
	{
		//Debug.Log("joined lobby");

		RoomOptions roomOptions = new RoomOptions() { };
		PhotonNetwork.JoinOrCreateRoom(_room, roomOptions, TypedLobby.Default);
	}

	void OnJoinedRoom()
    {
        GameObject player = null;
        playersJoined = PhotonNetwork.countOfPlayers;
        if (playersJoined == 2)
        {
            playerController.transform.rotation = Quaternion.identity;
            playerController.transform.Rotate(0, 180, 0);
            //Debug.Log("second player joined");
            playerController.transform.position = positions[playersJoined - 1];
            player = PhotonNetwork.Instantiate("NetworkedPlayer", Vector3.zero, eyeAnchor.transform.rotation, 0);
           // Debug.Log("First player joined");
        }
        else if (playersJoined > 2)
        {
            //Debug.Log("players joined is " + playersJoined);
            playerController.transform.position = positions[0];
            player = PhotonNetwork.Instantiate("NetworkedPlayer", Vector3.zero, eyeAnchor.transform.rotation, 0);

        }
        else
        {
            playerController.transform.position = positions[playersJoined - 1];
            player = PhotonNetwork.Instantiate("NetworkedPlayer", Vector3.zero, eyeAnchor.transform.rotation, 0);
            //Debug.Log("First player joined");
        }

        if (player != null)
        {
            player.GetComponent<PlayerNetworkController>().id = playersJoined;
        }

        //Debug.Log("joined room");
    }
}
