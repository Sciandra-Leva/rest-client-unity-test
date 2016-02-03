using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;
using System.Text;
using SimpleJSON;

public class NetworkRESTScript : MonoBehaviour {

	public string baseURL = "http://localhost:3000"; 
	public string post_url = "http://localhost:3000/api/v1/users";
	public string sessions_root = "/Users/lorenzosciandra/Documents/workspace-testing-lorenzo/unity-projects/rest-client-testing/REST-client/Assets";

	public string login_email = "sciandra@leva.io";
	public string login_password = "Sementera";

	void Start() {
		
		readFromXML ();
		StartCoroutine(GETUser(2));
		StartCoroutine(POSTUser());

	}

	void readFromXML ()
	{
		
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

		JSONNode N = new JSONClass(); // Start with JSONArray or JSONClass

		N["user"]["name"] = "Another";
		N["user"]["surname"] = "Test";
		N["user"]["email"] = "mc@test.test";
		N["user"]["password"] = "Sementera";
		N["user"]["role"] = "Admin";

		string json_test = N.ToString();

		Debug.Log("Formatted JSON = " + json_test);

		string result = "";
		using (var client = new WebClient())
		{
			client.Headers[HttpRequestHeader.ContentType] = "application/json";
//			try{
//				yield return result = client.UploadString(post_url, "POST", json_test);
//			}
//			catch (WebException x)
//			{
//				// we can end here, to say, when the user is already registered or some shit
//				// btw it can't happen in our REST server
//				Debug.Log ("Error: " + x.Message);
//			}
			yield return result = client.UploadString(post_url, "POST", json_test);
		}
		Debug.Log(result);
	}
}

