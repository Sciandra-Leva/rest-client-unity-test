//---------------------------------------------------------------------
//--------------------------  NetworkREST  ----------------------------
// A small library to handle communication with our RESTful services
// in the backend server.
// Developed by: Lorenzo Sciandra
// v0.1
//---------------------------------------------------------------------


using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;
using SimpleJSON;

public enum RestError
{
	AllGood,
	WrongMail,
	WrongPassword,
	ServerError,
	TokenError,
	ZeroDoctors,
	ZeroPatients,
	UnAuthorized
}

public enum RestSession
{
	AllGood,
	MultipleActive
}


public class NetworkREST  : MonoBehaviour {

	//---------------------------------------------------------------------
	//---------------------------  VARIABLES  -----------------------------
	//---------------------------------------------------------------------

//	static string baseURL = "http://dev.painteraction.org";
	static string baseURL = "http://localhost:3000";
//	static string post_url = baseURL + "/api/v1/users";
	static string login_url = baseURL + "/api/v1/sessions";
	static string force_logout_url = baseURL + "/api/v1/force_logout";
	static string exercise_url = baseURL + "/api/v1/exercises";

	private string token = "";
	private string login_email = "";
	private string login_password = "";

	public RestError errorHandler = RestError.AllGood;
	public RestSession sessionsHandler = RestSession.AllGood;

	public string exercise_root = "/Users/lorenzosciandra/Documents/workspace-testing-lorenzo/unity-projects/rest-client-testing/REST-client/Assets";

	//---------------------------------------------------------------------
	//-------------------------  PUBLIC METHODS  --------------------------
	//---------------------------------------------------------------------

	// TO DO: a check connection method, in order to not screw up later
		
			
	// Use this to do a POST and create a session
	public IEnumerator LOGINUser (string email, string password) {

		bool allProper = true;

		// I need to store those informations for other calls
		login_email = email;
		login_password = password;

		// create the JSON structure to send
		JSONNode N = new JSONClass();
		N["user"]["email"] = login_email;
		N["user"]["password"] = login_password;

		// and convert it to string
		string json_parameters = N.ToString();

		string result = "";

		// the actual call, in a try catch
		try 
		{
			using (var client = new WebClient())
			{
				client.Headers[HttpRequestHeader.ContentType] = "application/json";
				result = client.UploadString(login_url, "POST", json_parameters);
			}
		}
		catch (WebException ex)
		{
			Debug.Log("exception: " + ex);
			var response = ex.Response as HttpWebResponse;
			if (response != null)
			{
				Debug.Log("HTTP Status Code: " + (int)response.StatusCode);
			}

			switch ((int)response.StatusCode) {

			case 400:
				errorHandler = RestError.WrongMail;
				break;
			case 401:
				errorHandler = RestError.WrongPassword;
				break;
			case 500:
				errorHandler = RestError.ServerError;
				break;
			default:
				Debug.Log ("OH SHIT");
				break;
			}
			allProper = false;
		}

		yield return result;

		if (allProper) 
		{
			Debug.Log(result);
			// now I have to parse the json with the result
			JSONNode R = new JSONClass();
			R = JSONNode.Parse(result);
			token = R ["token"];
			int sessionCounter = R ["sessions_counter"].AsInt;
			if (sessionCounter > 0)
			{
				sessionsHandler = RestSession.MultipleActive;
			}
		}

	}

	// Use this to GET the list of users
	public IEnumerator GETUsersList (List<Person> listOfDoctors) {

		bool allProper = true;

		string token_string = "Token token=\"" + token + "\", email=\"" + login_email + "\"";
//		Debug.Log("The header text is " + token_string);

		string result = "";
		String answer_text = String.Empty;

		try 
		{
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(baseURL + "/api/v1/users");
			request.Method = "GET";
			request.Headers[HttpRequestHeader.Authorization] = token_string;
			using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
			{
				Stream dataStream = response.GetResponseStream();
				StreamReader reader = new StreamReader(dataStream);
				answer_text = reader.ReadToEnd();
				reader.Close();
				dataStream.Close();
			}
		}
		catch (WebException ex)
		{
			Debug.Log("exception: " + ex);
			var response = ex.Response as HttpWebResponse;
			if (response != null)
			{
				Debug.Log("HTTP Status Code: " + (int)response.StatusCode);
			}
			switch ((int)response.StatusCode) {

			case 401:
				errorHandler = RestError.ZeroDoctors;
				break;
			case 500:
				errorHandler = RestError.ServerError;
				break;
			default:
				Debug.Log ("OH SHIT");
				break;
			}
			allProper = false;
		}

		yield return result;

		if (allProper) 
		{
			Debug.Log("The returned list of users is " + answer_text);;

			JSONNode R_users = new JSONClass(); // Start with JSONArray or JSONClass
			R_users = JSONNode.Parse(answer_text);

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
	}

	// Use this to GET the list of patients
	public IEnumerator GETPatientsList (List<Person> listOfPatients) {

		bool allProper = true;

		string token_string = "Token token=\"" + token + "\", email=\"" + login_email + "\"";
		//		Debug.Log("The header text is " + token_string);

		string result = "";
		String answer_text = String.Empty;

		try 
		{
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(baseURL + "/api/v1/patients");
			request.Method = "GET";
			request.Headers[HttpRequestHeader.Authorization] = token_string;
			using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
			{
				Stream dataStream = response.GetResponseStream();
				StreamReader reader = new StreamReader(dataStream);
				answer_text = reader.ReadToEnd();
				reader.Close();
				dataStream.Close();
			}
		}
		catch (WebException ex)
		{
			Debug.Log("exception: " + ex);
			var response = ex.Response as HttpWebResponse;
			if (response != null)
			{
				Debug.Log("HTTP Status Code: " + (int)response.StatusCode);
			}
			switch ((int)response.StatusCode) {

			case 401:
				errorHandler = RestError.ZeroPatients;
				break;
			case 500:
				errorHandler = RestError.ServerError;
				break;
			default:
				Debug.Log ("OH SHIT");
				break;
			}
			allProper = false;
		}

		yield return result;

		if (allProper) 
		{
			Debug.Log ("The returned list of patients is " + answer_text);

			JSONNode R_patients = new JSONClass (); // Start with JSONArray or JSONClass
			R_patients = JSONNode.Parse (answer_text);

			//		Debug.Log("The name of the first retrived patient is " + R_patients ["patients"][0]["complete_name"]);
			Debug.Log ("There are " + R_patients ["patients"].Count + " elements in the array");

			// let's populate an array accessible from outside
			for (int i = 0; i <= R_patients ["patients"].Count; i++) {
				int local_age = 0;
				if (R_patients ["patients"] [i] ["age"] != null) {
					local_age = Int32.Parse (R_patients ["patients"] [i] ["age"]);
				}
				listOfPatients.Add (new Person () {
					ID = R_patients ["patients"] [i] ["id"],
					name = R_patients ["patients"] [i] ["complete_name"],
					age = local_age,
					type = Person.Type.Patient,
					photo = baseURL + R_patients ["patients"] [i] ["avatar"]
				}
				);
			}
		}
	}

	// Use this to DELETE and do a logout
	public IEnumerator LOGOUTUser () {
		bool allProper = true;

		string token_string = "Token token=\"" + token + "\", email=\"" + login_email + "\"";

		String answer_text = String.Empty;

		HttpWebResponse myHttpWebResponse = null;
		try
		{
			HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(login_url);

			myHttpWebRequest.Method = "DELETE";
			myHttpWebRequest.Headers.Add("Authorization", token_string);
			// Sends the HttpWebRequest and waits for the response.			
			myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse(); 
			// Gets the stream associated with the response.
			Stream receiveStream = myHttpWebResponse.GetResponseStream();
			Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
			// Pipes the stream to a higher level stream reader with the required encoding format. 
			StreamReader readStream = new StreamReader( receiveStream, encode );
			answer_text = readStream.ReadToEnd();

			// Releases the resources of the response.
			myHttpWebResponse.Close();
			// Releases the resources of the Stream.
			readStream.Close();
		}
		catch (WebException ex)
		{
			Debug.Log("exception: " + ex);
			var response = ex.Response as HttpWebResponse;
			if (response != null)
			{
				Debug.Log("HTTP Status Code: " + (int)response.StatusCode);
			}
			switch ((int)response.StatusCode) {
				// I don't really have many ideas about the kind of error here
			case 401:
				errorHandler = RestError.UnAuthorized;
				break;
			case 500:
				errorHandler = RestError.ServerError;
				break;
			default:
				Debug.Log ("OH SHIT");
				break;
			}
			allProper = false;
		}

		yield return myHttpWebResponse;
		if (allProper) 
		{
			Debug.Log (answer_text);
		}
	}

	// Use this to DELETE and force a logout
	public IEnumerator ForceLOGOUTUser () {
		bool allProper = true;
		string token_string = "Token token=\"" + token + "\", email=\"" + login_email + "\"";

		String answer_text = String.Empty;

		HttpWebResponse myHttpWebResponse = null;
		try
		{
			HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(force_logout_url);

			myHttpWebRequest.Method = "DELETE";
			myHttpWebRequest.Headers.Add("Authorization", token_string);
			// Sends the HttpWebRequest and waits for the response.			
			myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse(); 
			// Gets the stream associated with the response.
			Stream receiveStream = myHttpWebResponse.GetResponseStream();
			Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
			// Pipes the stream to a higher level stream reader with the required encoding format. 
			StreamReader readStream = new StreamReader( receiveStream, encode );
			answer_text = readStream.ReadToEnd();

			// Releases the resources of the response.
			myHttpWebResponse.Close();
			// Releases the resources of the Stream.
			readStream.Close();
		}
		catch (WebException ex)
		{
			Debug.Log("exception: " + ex);
			var response = ex.Response as HttpWebResponse;
			if (response != null)
			{
				Debug.Log("HTTP Status Code: " + (int)response.StatusCode);
			}
			switch ((int)response.StatusCode) {
			// I don't really have many ideas about the kind of error here
			case 401:
				errorHandler = RestError.UnAuthorized;
				break;
			case 500:
				errorHandler = RestError.ServerError;
				break;
			default:
				Debug.Log ("OH SHIT");
				break;
			}
			allProper = false;

		}

		yield return myHttpWebResponse;
		if (allProper) 
		{
			Debug.Log (answer_text);
		}
	}

	// Use this to DELETE and do a logout
	public void FinalLOGOUTUser () {
		bool allProper = true;

		string token_string = "Token token=\"" + token + "\", email=\"" + login_email + "\"";

		String answer_text = String.Empty;

		HttpWebResponse myHttpWebResponse = null;
		try
		{
			HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(login_url);

			myHttpWebRequest.Method = "DELETE";
			myHttpWebRequest.Headers.Add("Authorization", token_string);
			// Sends the HttpWebRequest and waits for the response.			
			myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse(); 
			// Gets the stream associated with the response.
			Stream receiveStream = myHttpWebResponse.GetResponseStream();
			Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
			// Pipes the stream to a higher level stream reader with the required encoding format. 
			StreamReader readStream = new StreamReader( receiveStream, encode );
			answer_text = readStream.ReadToEnd();

			// Releases the resources of the response.
			myHttpWebResponse.Close();
			// Releases the resources of the Stream.
			readStream.Close();
		}
		catch (WebException ex)
		{
			Debug.Log("exception: " + ex);
			var response = ex.Response as HttpWebResponse;
			if (response != null)
			{
				Debug.Log("HTTP Status Code: " + (int)response.StatusCode);
			}
			switch ((int)response.StatusCode) {
			// I don't really have many ideas about the kind of error here
			case 401:
				errorHandler = RestError.UnAuthorized;
				break;
			case 500:
				errorHandler = RestError.ServerError;
				break;
			default:
				Debug.Log ("OH SHIT");
				break;
			}
			allProper = false;
		}

		if (allProper) 
		{
			Debug.Log (answer_text);
		}
	}

	// Use this to do a POST and create a session
	public IEnumerator POSTExercise () {

		bool allProper = true;

		// 

		// create the JSON structure to send
		JSONNode N = new JSONClass();
		N["user"]["email"] = login_email;
		N["user"]["password"] = login_password;

		// and convert it to string
		string json_parameters = N.ToString();

		string result = "";

		// the actual call, in a try catch
		try 
		{
			using (var client = new WebClient())
			{
				client.Headers[HttpRequestHeader.ContentType] = "application/json";
				result = client.UploadString(exercise_url, "POST", json_parameters);
			}
		}
		catch (WebException ex)
		{
			Debug.Log("exception: " + ex);
			var response = ex.Response as HttpWebResponse;
			if (response != null)
			{
				Debug.Log("HTTP Status Code: " + (int)response.StatusCode);
			}

			switch ((int)response.StatusCode) {

			case 400:
				errorHandler = RestError.WrongMail;
				break;
			case 401:
				errorHandler = RestError.WrongPassword;
				break;
			case 500:
				errorHandler = RestError.ServerError;
				break;
			default:
				Debug.Log ("OH SHIT");
				break;
			}
			allProper = false;
		}

		yield return result;

		if (allProper) 
		{
			Debug.Log(result);
		}

	}
}

