using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;

public class NetworkRESTScript : MonoBehaviour {

	public string baseURL = "0.0.0.0:3000"; 

	void Start() {
		StartCoroutine(GETUser(2));
	}

	// Use this to GET single user data
	IEnumerator GETUser (int userID) {
		WWW userData = new WWW (baseURL + "/api/v1/users/" + userID.ToString());
		yield return userData;
		string userDataString = userData.text;
		JSONObject jsonrepOfPatient = new JSONObject (userData.text);
		// just testing what happens in JSON usage
		print ("The packet is composed of " + jsonrepOfPatient.list.Count);
		JSONObject j = (JSONObject)jsonrepOfPatient.list [0];
		print ("The subpacket is composed of " + j.list.Count);
		JSONObject obj = j["name"];
		print ("The subpacket name is " + obj.str);
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
		JSONObject jsonrepOfPatient = new JSONObject (patientData.text);
		print (patientDataString);
	}

	// Use this to GET the patients list
	IEnumerator GETPatientsList () {
		WWW patientListData = new WWW (baseURL + "/api/v1/patients");
		yield return patientListData;
		string patientListDataString = patientListData.text;
		print (patientListDataString);
	}

	// Use this to GET the patients list
//	IEnumerator POSTuser () {
//
//		// Create a form object for sending high score data to the server
//		WWWForm form = new WWWForm();
//		// Assuming the perl script manages high scores for different games
//		form.AddField( "game", "MyGameName" );
//		// The name of the player submitting the scores
//		form.AddField( "playerName", playName );
//		// The score
//		form.AddField( "score", score );
//
//		// Create a download object
//		WWW download = new WWW( highscore_url, form );
//
//		// Wait until the download is done
//		yield return download;
//
//		if(!string.IsNullOrEmpty(download.error)) {
//			print( "Error downloading: " + download.error );
//		} else {
//			// show the highscores
//			Debug.Log(download.text);
//		}
//	}


//	public static WWW RunServerExtension (string appId, string appKey, string endpoint, string kii_access_token, string msg) 
//	{ 
//		WWWForm form = new WWWForm(); 
//		Hashtable headers = form.headers; 
//		headers["Content-Type"] = "application/json"; 
//		headers["x-kii-appid"] = appId; 
//		headers["x-kii-appkey"] = appKey; 
//		if(kii_access_token != null) 
//			headers["Authorization"] = "Bearer " + kii_access_token; 
//		Hashtable data = new Hashtable(); 
//		data["message"] = msg; 
//		string json = JSON.JsonEncode(data); 
//		Debug.Log("Sending: " + json); 
//		byte[] bytes = Encoding.UTF8.GetBytes(json); 
//		return new WWW("https://api.kii.com/api/apps/" + appId + "/server-code/versions/current/" + endpoint, bytes, headers); 
//	}
}