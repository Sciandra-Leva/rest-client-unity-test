using UnityEngine;
using System;
using System.Linq;
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

	public List<Person> listOfDoctors = new List<Person>(); 
	public List<Person> listOfPatients = new List<Person>();

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

		// Now we write the token somewhere offline, so if we crash we are gucci.

		StartCoroutine(GETUserList());
	}

	// Use this to GET the list of users
	public IEnumerator GETUserList () {
		Dictionary<string, string> headers = new Dictionary<string, string>();
		string token_string = "Token token=\"" + token + "\", email=\"" + login_email + "\"";
		Debug.Log("The header text is " + token_string);;
		headers.Add( "Authorization",  token_string);

		WWW userData = new WWW (baseURL + "/api/v1/users", null, headers);
		yield return userData;
		Debug.Log("The returned list of users is " + userData.text);;

		JSONNode R = new JSONClass(); // Start with JSONArray or JSONClass
		R = JSONNode.Parse(userData.text);

		Debug.Log("The name of the first retrived user is " + R ["users"][0]["email"]);
		Debug.Log("There are " + R["users"].Count + " elements in the array");

		// let's populate an array accessible from outside
		for (int i = 0; i <= R["users"].Count; i++)
		{
			Debug.Log ("Just testing");
			int local_age = 0;
			if (R ["users"] [i] ["age"] != null) 
			{
				local_age = Int32.Parse (R ["users"] [i] ["age"]);
			}
			listOfDoctors.Add(new Person()
				{
					ID = R ["users"][i]["id"],
					name = R ["users"][i]["complete_name"],
					age = local_age,
					type = Person.Type.Doctor,
					photo = R ["users"][i]["avatar"]
				}
			);
		}

	}

	//---------------------------------------------------------------------
	//---------------------------------------------------------------------
	//---------------  DOWN HERE ARE JUST TEST METHODS  -------------------
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

