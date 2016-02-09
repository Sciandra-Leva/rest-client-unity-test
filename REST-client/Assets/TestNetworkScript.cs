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

		string error = "";

		// i'm currently doing it as a coroutine in order 
		// to avoid slowing down the process. the yield return is simply
		// to show how to avoid running ahead of time (ex: doing the gets 
		// before the login has been done)
		yield return StartCoroutine(client.LOGINUser ("sciandra@leva.io", "Sementera", error));

		// once we are logged in, it is time to obtain those Doctors and
		// Patients informations.
		// In this example there are two different lists for patients
		// and Doctors but since both are persons, potentially a single
		// list can be used
		List<Person> listOfDoctors = new List<Person> (); 
		yield return StartCoroutine(client.GETUsersList(listOfDoctors));

		List<Person> listOfPatients = new List<Person> ();
		yield return StartCoroutine(client.GETPatientsList(listOfPatients));

		// testing the populated lists with Linq
		Debug.Log("To test the freshly populated lists: " +
			"First Patient registered: " + 
			listOfPatients.ElementAt(0).name +
			" and the first Doctor registered: " +
			listOfDoctors.ElementAt(0).name
		);

		// this is the one used to log out the 
		// current user.
		yield return StartCoroutine(client.LOGOUTUser());

	}
}
