using UnityEngine;
using System.Collections;
using System.Linq;


public class TestNetworkScript : MonoBehaviour {

	public NetworkREST client;

	// Use this for initialization
	void Start (){
		// first step, add it to the gameobject
		client = gameObject.AddComponent<NetworkREST>();

		// i'm currently doing it as a coroutine in order 
		// to avoid slowing down the process

		// to login, add the email and password
		client.login_email = "sciandra@leva.io";
		client.login_password = "Sementera";

		// then start the LOGIN
		// in this call there is also the population
		// of two Lists, one with the Doctors
		// and the other one with the Patients
		StartCoroutine(client.LOGINUser());

		// if needed, these are the calls for the
		// two lists
//		StartCoroutine(client.GETUsersList());
//		StartCoroutine(client.GETPatientsList());


		// this is the one used to log out the 
		// current user. It won't start if there is
		// another call to the server running 
		// but it should not happen. Ever.
		StartCoroutine(client.LOGOUTUser());

		// example of interaction
//		Debug.Log("To test the freshly populated arrays: First Patient registered: " + 
//			listOfPatients.ElementAt(0).name +
//			" and the first Doctor registered: " +
//			listOfDoctors.ElementAt(0).name
//		);
	}
}
