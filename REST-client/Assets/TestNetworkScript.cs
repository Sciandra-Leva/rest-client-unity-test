using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;


public class TestNetworkScript : MonoBehaviour {

	public NetworkREST client;


	// Use this for initialization
	IEnumerator Start (){

		// first step, add it to the gameobject
		client = gameObject.AddComponent<NetworkREST>();

		// i'm currently doing it as a coroutine in order 
		// to avoid slowing down the process
		yield return StartCoroutine(client.LOGINUser ("sciandra@leva.io", "Sementera"));

		// if needed, these are the calls for the
		// two lists
		List<Person> listOfDoctors = new List<Person> (); 
		List<Person> listOfPatients = new List<Person> ();
		yield return StartCoroutine(client.GETUsersList(listOfDoctors));
		yield return StartCoroutine(client.GETPatientsList(listOfPatients));

		Debug.Log("To test the freshly populated arrays: First Patient registered: " + 
			listOfPatients.ElementAt(0).name +
			" and the first Doctor registered: " +
			listOfDoctors.ElementAt(0).name
		);

		// this is the one used to log out the 
		// current user. It won't start if there is
		// another call to the server running 
		// but it should not happen. Ever.
		yield return StartCoroutine(client.LOGOUTUser());
		// example of interaction

	}
}
