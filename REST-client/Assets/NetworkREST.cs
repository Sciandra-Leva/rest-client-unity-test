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
using System.Xml;
using UnityEngine.UI;

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
//	static string baseURL = "http://localhost:3000";
    static string baseURL = "http://painteraction:3000/";


    //	static string post_url = baseURL + "/api/v1/users";
    static string login_url = baseURL + "/api/v1/sessions";
	static string force_logout_url = baseURL + "/api/v1/force_logout";
	static string trails_url = baseURL + "/api/v1/trails";
//	static string balls_url = baseURL + "/api/v1/balls";
//	static string paints_url = baseURL + "/api/v1/paints";
//	static string vowels_url = baseURL + "/api/v1/vowels";


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
						photo = baseURL + R_users ["users"][i]["user_avatar"]
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
					photo = baseURL + R_patients ["patients"] [i] ["patient_avatar"]
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

	//---------------------------------------------------------------------
	//------------------------  TESTING METHODS  --------------------------
	//---------------------------------------------------------------------

	// Use this to do a POST and create a session
	// apparently for now I do one for each
	public IEnumerator POSTTrailsExercise (string exercisePath) {

		bool allProper = true;
		TrailPreferences xmldata = new TrailPreferences ();
		// let's start with the easy part: they tell me
		// where the .xml is, I open it and read it
		string mainFilePath = exercisePath + "/main.xml";
		Debug.Log ("The path i search for the xml is " + mainFilePath);

		if (System.IO.File.Exists (mainFilePath)) {
			xmldata.LoadXML (mainFilePath);
			Debug.Log ("I actually read it!");
		}
		// since it's really working we can
		// create the JSON structure to send
		JSONNode N = new JSONClass ();

		N ["trails"] ["patient_id"] = TrailPreferences.patientID;
		N ["trails"] ["time_to_live"] = TrailPreferences.trailsTimeToLive.ToString ();

		// I have to get all the doctors involved
		// and pick out the first since it is the logged one
		string list_of_doctors = String.Join (", ", TrailPreferences.doctorsIDs.Skip (1).ToArray ());
		N ["trails"] ["other_doctors"] = "[" + list_of_doctors + "]";

		N ["trails"] ["typology"] = TrailPreferences.trailsType;
		N ["trails"] ["start_datetime"] = TrailPreferences.initTime;
		N ["trails"] ["end_datetime"] = TrailPreferences.endTime;
		N ["trails"] ["special_fx"] = TrailPreferences.trailsSpecialFX.ToString ().ToLower ();

		N ["trails"] ["enabled_therapist_left"] = TrailPreferences.othersSX_trailsEnabled.ToString ().ToLower ();
		if (TrailPreferences.othersSX_trailsEnabled == true) {
			N ["trails"] ["therapist_left_trail_color"] ["d"] = TrailPreferences.trailsDimension.ToString ();
			N ["trails"] ["therapist_left_trail_color"] ["a"] = TrailPreferences.othersSX_trailsColor.a.ToString ();
			N ["trails"] ["therapist_left_trail_color"] ["r"] = TrailPreferences.othersSX_trailsColor.r.ToString ();
			N ["trails"] ["therapist_left_trail_color"] ["g"] = TrailPreferences.othersSX_trailsColor.g.ToString ();
			N ["trails"] ["therapist_left_trail_color"] ["b"] = TrailPreferences.othersSX_trailsColor.b.ToString ();
		}

		N ["trails"] ["enabled_therapist_right"] = TrailPreferences.othersDX_trailsEnabled.ToString ().ToLower ();
		if (TrailPreferences.othersDX_trailsEnabled == true) {
			N ["trails"] ["therapist_right_trail_color"] ["d"] = TrailPreferences.trailsDimension.ToString ();
			N ["trails"] ["therapist_right_trail_color"] ["a"] = TrailPreferences.othersDX_trailsColor.a.ToString ();
			N ["trails"] ["therapist_right_trail_color"] ["r"] = TrailPreferences.othersDX_trailsColor.r.ToString ();
			N ["trails"] ["therapist_right_trail_color"] ["g"] = TrailPreferences.othersDX_trailsColor.g.ToString ();
			N ["trails"] ["therapist_right_trail_color"] ["b"] = TrailPreferences.othersDX_trailsColor.b.ToString ();
		}

		N ["trails"] ["enabled_patient_left"] = TrailPreferences.patientSX_trailsEnabled.ToString ().ToLower ();
		if (TrailPreferences.patientSX_trailsEnabled == true) {
			N ["trails"] ["patient_left_trail_color"] ["d"] = TrailPreferences.trailsDimension.ToString ();
			N ["trails"] ["patient_left_trail_color"] ["a"] = TrailPreferences.patientSX_trailsColor.a.ToString ();
			N ["trails"] ["patient_left_trail_color"] ["r"] = TrailPreferences.patientSX_trailsColor.r.ToString ();
			N ["trails"] ["patient_left_trail_color"] ["g"] = TrailPreferences.patientSX_trailsColor.g.ToString ();
			N ["trails"] ["patient_left_trail_color"] ["b"] = TrailPreferences.patientSX_trailsColor.b.ToString ();
		}

		N ["trails"] ["enabled_patient_right"] = TrailPreferences.patientDX_trailsEnabled.ToString ().ToLower ();
		if (TrailPreferences.patientDX_trailsEnabled == true) {
			N ["trails"] ["patient_right_trail_color"] ["d"] = TrailPreferences.trailsDimension.ToString ();
			N ["trails"] ["patient_right_trail_color"] ["a"] = TrailPreferences.patientDX_trailsColor.a.ToString ();
			N ["trails"] ["patient_right_trail_color"] ["r"] = TrailPreferences.patientDX_trailsColor.r.ToString ();
			N ["trails"] ["patient_right_trail_color"] ["g"] = TrailPreferences.patientDX_trailsColor.g.ToString ();
			N ["trails"] ["patient_right_trail_color"] ["b"] = TrailPreferences.patientDX_trailsColor.b.ToString ();
		}

		// to do the first tests i will pass this anyway
		N ["trails"] ["background_color"] ["a"] = TrailPreferences.backgroundColor.a.ToString ();
		N ["trails"] ["background_color"] ["r"] = TrailPreferences.backgroundColor.r.ToString ();
		N ["trails"] ["background_color"] ["g"] = TrailPreferences.backgroundColor.g.ToString ();
		N ["trails"] ["background_color"] ["b"] = TrailPreferences.backgroundColor.b.ToString ();

		// now the part which is going to be a mess, about the backgrounds
		if (TrailPreferences.backgroundIsImage == false) {
			N ["trails"] ["background_color"] ["a"] = TrailPreferences.backgroundColor.a.ToString ();
			N ["trails"] ["background_color"] ["r"] = TrailPreferences.backgroundColor.r.ToString ();
			N ["trails"] ["background_color"] ["g"] = TrailPreferences.backgroundColor.g.ToString ();
			N ["trails"] ["background_color"] ["b"] = TrailPreferences.backgroundColor.b.ToString ();
		} else {
			// I HAVE TO FIND A WAY TO UPLOAD IMAGES FFS backgroundTexturePath
			// send it as base64 in JSON FUCK ME


			string fullPath = Path.Combine(exercisePath, TrailPreferences.backgroundTexturePath);
//			string test = @TrailPreferences.backgroundTexturePath;
			//string image = "/Users/lorenzosciandra/Documents/workspace-testing-lorenzo/unity-projects/rest-client-testing/REST-client/Assets/Sessions/20160203/AldoBo/TRAILS_1138/northern-lights-christmas-background-1366-768-618491.jpeg";
			//byte[] bytes = System.Text.ASCIIEncoding.ASCII.GetBytes(image);
			byte[] bytes = File.ReadAllBytes(fullPath);

			string base64String = System.Convert.ToBase64String(bytes);
			Debug.Log("codifica dell'immagine: " + base64String);

//			string test64json = N.SaveToCompressedBase64 ();

//			string base64String = "no";
			//Console.WriteLine("Base 64 string: " + base64String);
			N ["trails"] ["background_image"] ["filepath"] = fullPath;
			N ["trails"] ["background_image"] ["filename"] = TrailPreferences.backgroundTexturePath;
			N ["trails"] ["background_image"] ["content"] = " "; // bitstream here
//			JSONNode K = new JSONClass();
//			K["content"] = base64String;
			Debug.Log("1");
//			Debug.Log("K value = " + K.ToString());

			N ["trails"] ["background_image"] ["content"] = base64String;
			Debug.Log("2");
			N ["trails"] ["background_image"] ["content_type"] = "image/jpeg";
			Debug.Log("3");
//			using (Image image = Image.FromFile(TrailPreferences.backgroundTexturePath))
//			{                 
//				using (MemoryStream m = new MemoryStream())
//				{
//					image.Save(m, image.RawFormat);
//					byte[] imageBytes = m.ToArray();
//
//					// Convert byte[] to Base64 String
//					string base64String = Convert.ToBase64String(imageBytes);
//					return base64String;
//				}                  
		//}
		}

		if (TrailPreferences.colorFilterEnabled == true) {
			N ["trails"] ["color_filter"] ["a"] = TrailPreferences.colorFilterAlpha.ToString ();
			N ["trails"] ["color_filter"] ["r"] = TrailPreferences.colorFilter.r.ToString ();
			N ["trails"] ["color_filter"] ["g"] = TrailPreferences.colorFilter.g.ToString ();
			N ["trails"] ["color_filter"] ["b"] = TrailPreferences.colorFilter.b.ToString ();
		} 
		// and convert it to string
//		string json_parameters = N.ToString();
//		Debug.Log ("This is what i wrote til aaaaaaand now: " + json_parameters);
		string result = "";

		// the actual call, in a try catch
//		try 
//		{
//			using (var client = new WebClient())
//			{
//				string token_string = "Token token=\"" + token + "\", email=\"" + login_email + "\"";
//				client.Headers[HttpRequestHeader.Authorization] = token_string;
//				client.Headers[HttpRequestHeader.ContentType] = "application/json";
//				result = client.UploadString(trails_url, "POST", json_parameters);
//			}
//		}
//		catch (WebException ex)
//		{
//			Debug.Log("exception: " + ex);
//			var response = ex.Response as HttpWebResponse;
//			if (response != null)
//			{
//				Debug.Log("HTTP Status Code: " + (int)response.StatusCode);
//			}
//
//			switch ((int)response.StatusCode) {
//
////			case 400:
////				errorHandler = RestError.WrongMail;
////				break;
////			case 401:
////				errorHandler = RestError.WrongPassword;
////				break;
////			case 500:
////				errorHandler = RestError.ServerError;
////				break;
//			default:
//				Debug.Log ("OH SHIT");
//				break;
//			}
//			allProper = false;
//		}
//
		yield return result;
////
//		if (allProper) 
//		{
//			Debug.Log(result);
//		}

	}
}

