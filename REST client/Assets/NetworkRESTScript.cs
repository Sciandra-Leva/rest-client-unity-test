using UnityEngine;
using System.Collections;

public class NetworkRESTScript : MonoBehaviour {

	string baseURL = "0.0.0.0:3000"; 

	// Use this for initialization
	IEnumerator Start () {
		WWW userData = new WWW (baseURL + "/api/v1/users/3");
		yield return userData;
		string userDataString = userData.text;
		print (userDataString);
	}

	// Use this for downloading the users list
	IEnumerator GETUsersList () {
		WWW userData = new WWW (baseURL + "/api/v1/users");
		yield return userData;
		string userDataString = userData.text;
		print (userDataString);
	}
}
