using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;
using SimpleJSON;

public class NetworkREST  : MonoBehaviour {

	public string baseURL = "http://localhost:3000"; 
	public string post_url = "http://localhost:3000/api/v1/users";
	public string login_url = "http://localhost:3000/api/v1/sessions";
	public string exercise_root = "";
	public string token = "";

	public string login_email = "sciandra@leva.io";
	public string login_password = "Sementera";

	// Use this to POST and get a login
	public IEnumerator LOGINUser () {

		JSONNode N = new JSONClass(); // Start with JSONArray or JSONClass

		N["user"]["email"] = login_email;
		N["user"]["password"] = login_password;

		string json_test = N.ToString();

		Debug.Log("Formatted JSON = " + json_test);

		string result = "";
		using (var client = new WebClient())
		{
			client.Headers[HttpRequestHeader.ContentType] = "application/json";
			yield return result = client.UploadString(login_url, "POST", json_test);
		}
		Debug.Log(result);
		JSONNode R = new JSONClass(); // Start with JSONArray or JSONClass
		R = JSONNode.Parse(result);
		Debug.Log("The token is " + R["token"]);
		token = R ["token"];
		StartCoroutine(GETUser(2));
	}

	//---------------------------------------------------------------------
	//---------------------------------------------------------------------
	//---------------------------------------------------------------------
	//---------------------------------------------------------------------
	//---------------------------------------------------------------------


	// Use this to GET single user data
	public IEnumerator GETUser (int userID) {
		Dictionary<string, string> headers = new Dictionary<string, string>();
		string token_string = "Token token=\"" + token + "\", email=\"" + login_email + "\"";
		Debug.Log("The header text is " + token_string);;
		headers.Add( "Authorization",  token_string);

		WWW userData = new WWW (baseURL + "/api/v1/users/" + userID.ToString(), null, headers);
		yield return userData;
		Debug.Log("The returned text is " + userData.text);;
//		JSONNode R = new JSONClass(); // Start with JSONArray or JSONClass
//		R = JSONNode.Parse(userData.text);
//		Debug.Log("The name of the retrived user is " + R["name"]);;
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

