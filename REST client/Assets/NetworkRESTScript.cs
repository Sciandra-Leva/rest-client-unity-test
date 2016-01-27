using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;
using System.Text;
using SimpleJSON;

public class NetworkRESTScript : MonoBehaviour {

	public string baseURL = "0.0.0.0:3000"; 

	void Start() {
		StartCoroutine(GETUser(2));
		StartCoroutine(POSTUser());
	}

	// Use this to GET single user data
	IEnumerator GETUser (int userID) {
		WWW userData = new WWW (baseURL + "/api/v1/users/" + userID.ToString());
		yield return userData;
		string userDataString = userData.text;
		var jsonrepOfPatient = JSON.Parse(userData.text);
		print ("The subpacket name is " + jsonrepOfPatient["user"]["name"].Value);
		print (userDataString);
	}

	// Use this to GET the users list
	IEnumerator GETUsersList () {
		WWW userListData = new WWW (baseURL + "/api/v1/users");
		yield return userListData;
		string userListDataString = userListData.text;
		print (userListDataString);
	}

	// Use this to GET single patient data
	IEnumerator GETPatient (int patientID) {
		WWW patientData = new WWW (baseURL + "/api/v1/patients/" + patientID.ToString());
		yield return patientData;
		string patientDataString = patientData.text;
		print (patientDataString);
	}

	// Use this to GET the patients list
	IEnumerator GETPatientsList () {
		WWW patientListData = new WWW (baseURL + "/api/v1/patients");
		yield return patientListData;
		string patientListDataString = patientListData.text;
		print (patientListDataString);
	}

	// Use this to POST a new user
	IEnumerator POSTUser () {
		// first thing for a POST is to do a form
		WWWForm form = new WWWForm();

		form.headers["Content-Type"] = "application/json; ";
		form.headers["Accept"] = "application/json"; 


		JSONNode N = new JSONClass(); // Start with JSONArray or JSONClass

		N["user"]["name"] = "Another";
		N["user"]["surname"] = "Test";
		N["user"]["email"] = "mc@test.test";
		N["user"]["password"] = "Sementera";
		N["user"]["role"] = "Admin";

		string json = N.ToString();

		Debug.Log("Formatted JSON = " + json);

		string JsonArraystring = "{\"user\": [{\"Id\":\"101\",\"Name\":\"Unity4.6\"},{\"Id\":\"102\",\"Name\":\"Unity5\"}]}";

		byte[] bytes = Encoding.UTF8.GetBytes(JsonArraystring); 

		WWW userListData = new WWW (baseURL + "/api/v1/users", bytes, form.headers);
		yield return userListData;

		if(!string.IsNullOrEmpty(userListData.error)) {
			print( "Error downloading: " + userListData.error );
		} else {
			// show the highscores
			Debug.Log(userListData.text);
			string userListDataString = userListData.text;
		}
	}
}