using UnityEngine;
using System.Collections;

public class TestNetworkScript : MonoBehaviour {

	public NetworkREST client;

	// Use this for initialization
	void Start (){
		client = gameObject.AddComponent<NetworkREST>();

		StartCoroutine(client.LOGINUser());
	}
}
