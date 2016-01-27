using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;
using System.Text;
using SimpleJSON;

//public class MyUser
//{
//	public int level;
//	public float timeElapsed;
//	public string playerName;
//}

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
		// just testing what happens in JSON usage
//		print ("The packet is composed of " + jsonrepOfPatient.list.Count);
//		var j = (JSONObject)jsonrepOfPatient.list [0];
//		print ("The subpacket is composed of " + j.list.Count);
//		var obj = j["name"];
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
//		JSONObject jsonrepOfPatient = new JSONObject (patientData.text);
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

//		Hashtable headers = form.headers; 
		form.headers["Content-Type"] = "application/json"; 

//		MyUser myObject = new MyUser();
//		myObject.level = 1;
//		myObject.timeElapsed = 47.5f;
//		myObject.playerName = "Dr Charles Francis";

//		if(kii_access_token != null) 
//			headers["Authorization"] = "Bearer " + kii_access_token; 

		string json = "test";
//		string json = JsonUtility.ToJson(myObject);

		Debug.Log("Formatted JSON = " + json);

//		form.AddField( "user", json.ToString());

//		Hashtable data = new Hashtable(); 
//		data["message"] = "Just doing a test"; 
//		string json = JSON.JsonEncode(data); 
//
//		Debug.Log("Sending: " + json); 
//
		byte[] bytes = Encoding.UTF8.GetBytes(json); 
//		WWW userListData = new WWW (baseURL + "/api/v1/users", form);

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