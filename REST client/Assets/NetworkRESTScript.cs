using UnityEngine;
using System.Collections;

public class NetworkRESTScript : MonoBehaviour {

	string baseURL = "0.0.0.0:3000"; 

	// Use this to GET single user data
	IEnumerator GETUser () {
		WWW userData = new WWW (baseURL + "/api/v1/users/3");
		yield return userData;
		string userDataString = userData.text;
		print (userDataString);
	}

	// Use this to GET the users list
	IEnumerator GETUsersList () {
		WWW userListData = new WWW (baseURL + "/api/v1/users");
		yield return userListData;
		string userListDataString = userListData.text;
		print (userListDataString);
	}
}
