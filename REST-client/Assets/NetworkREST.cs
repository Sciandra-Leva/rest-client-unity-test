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

	//---------------------------------------------------------------------
	//---------------------------  VARIABLES  -----------------------------
	//---------------------------------------------------------------------

	static string baseURL = "http://localhost:3000"; 
	static string post_url = baseURL + "/api/v1/users";
	static string login_url = baseURL + "/api/v1/sessions";
//	static string exercise_root = baseURL + "";

	private string token = "";
	private string login_email = "";
	private string login_password = "";

	//---------------------------------------------------------------------
	//-------------------------  PUBLIC METHODS  --------------------------
	//---------------------------------------------------------------------

	// TO DO: a check connection method, in order to screw up later
		
			
	// Use this to POST and get a login
	public IEnumerator LOGINUser (string email, string password) {

		login_email = email;
		login_password = password;

		JSONNode N = new JSONClass(); // Start with JSONArray or JSONClass

		N["user"]["email"] = login_email;
		N["user"]["password"] = login_password;

		string json_test = N.ToString();

		//		Debug.Log("Formatted JSON = " + json_test);

		string result = "";
		using (var client = new WebClient())
		{
			client.Headers[HttpRequestHeader.ContentType] = "application/json";
			yield return result = client.UploadString(login_url, "POST", json_test);
		}

		Debug.Log(result);
		JSONNode R = new JSONClass(); // Start with JSONArray or JSONClass
		R = JSONNode.Parse(result);
		//		Debug.Log("The token is " + R["token"]);
		token = R ["token"];

	}

	// Use this to GET the list of users
	public IEnumerator GETUsersList (List<Person> listOfDoctors) {

		Dictionary<string, string> headers = new Dictionary<string, string>();
		string token_string = "Token token=\"" + token + "\", email=\"" + login_email + "\"";
//		Debug.Log("The header text is " + token_string);
		headers.Add( "Authorization",  token_string);

		WWW usersData = new WWW (baseURL + "/api/v1/users", null, headers);
		yield return usersData;

		Debug.Log("The returned list of users is " + usersData.text);;

		JSONNode R_users = new JSONClass(); // Start with JSONArray or JSONClass
		R_users = JSONNode.Parse(usersData.text);

//		Debug.Log("The name of the first retrived user is " + R_users ["users"][0]["complete_name"]);
		Debug.Log("There are " + R_users["users"].Count + " elements in the array");

		// let's populate an array accessible from outside
		for (int i = 0; i <= R_users["users"].Count; i++)
		{
			int local_age = 0;
			if (R_users ["users"] [i] ["age"] != null) 
			{
				local_age = Int32.Parse (R_users ["users"] [i] ["age"]);
			}
			listOfDoctors.Add(new Person()
				{
					ID = R_users ["users"][i]["id"],
					name = R_users ["users"][i]["complete_name"],
					age = local_age,
					type = Person.Type.Doctor,
					photo = baseURL + R_users ["users"][i]["avatar"]
				}
			);
		}
	}

	// Use this to GET the list of patients
	public IEnumerator GETPatientsList (List<Person> listOfPatients) {

		Dictionary<string, string> headers = new Dictionary<string, string>();
		string token_string = "Token token=\"" + token + "\", email=\"" + login_email + "\"";
//		Debug.Log("The header text is " + token_string);
		headers.Add( "Authorization",  token_string);

		WWW patientsData = new WWW (baseURL + "/api/v1/patients", null, headers);
		yield return patientsData;
		Debug.Log("The returned list of patients is " + patientsData.text);;

		JSONNode R_patients = new JSONClass(); // Start with JSONArray or JSONClass
		R_patients = JSONNode.Parse(patientsData.text);

//		Debug.Log("The name of the first retrived patient is " + R_patients ["patients"][0]["complete_name"]);
		Debug.Log("There are " + R_patients["patients"].Count + " elements in the array");

		// let's populate an array accessible from outside
		for (int i = 0; i <= R_patients["patients"].Count; i++)
		{
			int local_age = 0;
			if (R_patients["patients"] [i] ["age"] != null) 
			{
				local_age = Int32.Parse (R_patients ["patients"] [i] ["age"]);
			}
			listOfPatients.Add(new Person()
				{
					ID = R_patients ["patients"][i]["id"],
					name = R_patients ["patients"][i]["complete_name"],
					age = local_age,
					type = Person.Type.Patient,
					photo = baseURL + R_patients ["patients"][i]["avatar"]
				}
			);
		}

	}

	// Use this to DELETE and do a logout
	public IEnumerator LOGOUTUser () {
		string token_string = "Token token=\"" + token + "\", email=\"" + login_email + "\"";

		HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(login_url);

		myHttpWebRequest.Method = "DELETE";
		myHttpWebRequest.Headers.Add("Authorization", token_string);
		// Sends the HttpWebRequest and waits for the response.			
		HttpWebResponse myHttpWebResponse;
		yield return myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse(); 
		// Gets the stream associated with the response.
		Stream receiveStream = myHttpWebResponse.GetResponseStream();
		Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
		// Pipes the stream to a higher level stream reader with the required encoding format. 
		StreamReader readStream = new StreamReader( receiveStream, encode );
		Debug.Log("\r\nResponse stream received.");
		Char[] read = new Char[256];
		// Reads 256 characters at a time.    
		int count = readStream.Read( read, 0, 256 );
		while (count > 0) 
		{
			// Dumps the 256 characters on a string and displays the string to the console.
			String str = new String(read, 0, count);
			Debug.Log(str);
			count = readStream.Read(read, 0, 256);
		}

		// Releases the resources of the response.
		myHttpWebResponse.Close();
		// Releases the resources of the Stream.
		readStream.Close();
	}


	//---------------------------------------------------------------------
	//-------------------------  PRIVATE METHODS  -------------------------
	//---------------------------------------------------------------------


	//---------------------------------------------------------------------
	//---------------------------------------------------------------------
	//---------------  DOWN HERE ARE JUST TEST METHODS  -------------------
	//---------------------------------------------------------------------
	//---------------------------------------------------------------------

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

