//---------------------------------------------------------------------
//--------------------------  NetworkREST  ----------------------------
// A small library to handle communication with our RESTful services
// in the backend server.
// Developed by: Lorenzo Sciandra
// v0.4 - added the drawing in Paint, and some fixes. Moreover picture's url should be enough
// v0.32 - fixed path of background image and added a complete name field
// v0.31 - added new url
// v0.3 - fixed server not online error handling
// v0.2 - changed JSON library
//---------------------------------------------------------------------


using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;

public enum RestError
{
	AllGood,
	WrongMail,
	WrongPassword,
	ServerError,
	TokenError,
	ZeroDoctors,
	ZeroPatients,
	UnAuthorized,
    NotLoggedIn,
    XMLNotPresent,
    GenericLoginError,
    GenericGetUserListError,
    GenericGetPatientListError,
    GenericLogoutError,
    GenericForceLogoutError,
    GenericFinalLogoutError,
    GenericPostTrailError,
    GenericPostPaintError
}

public enum RestSession
{
	AllGood,
	MultipleActive
}


public class NetworkREST : MonoBehaviour
{

    //---------------------------------------------------------------------
    //---------------------------  VARIABLES  -----------------------------
    //---------------------------------------------------------------------

    static string baseURL = "http://ec2-52-58-50-250.eu-central-1.compute.amazonaws.com/";
    // static string baseURL = "http://dev.painteraction.org";
    // static string baseURL = "http://localhost:3000";
    // static string baseURL = "http://painteraction:3000/";

    static string login_url = baseURL + "/api/v1/sessions";
    static string force_logout_url = baseURL + "/api/v1/force_logout";
    static string trails_url = baseURL + "/api/v1/trails";
    static string paints_url = baseURL + "/api/v1/paints";

    //	static string balls_url = baseURL + "/api/v1/balls";
    //	static string vowels_url = baseURL + "/api/v1/vowels";

    private string token = "";
    private string login_email = "";
    private string login_password = "";

    public RestError errorHandler = RestError.AllGood;
    public RestSession sessionsHandler = RestSession.AllGood;
    public string logged_user_complete_name;
    public string logged_user_id;

    //---------------------------------------------------------------------
    //-------------------------  PUBLIC METHODS  --------------------------
    //---------------------------------------------------------------------

    // TO DO: a check connection method, in order to not screw up later

    // Use this to do a POST and create a session
    public IEnumerator LOGINUser(string email, string password)
    {
        bool allProper = true;

        // I need to store those informations for other calls
        login_email = email;
        login_password = password;

        JSONObject nested_fields = new JSONObject(JSONObject.Type.OBJECT);
        nested_fields.AddField("email", login_email);
        nested_fields.AddField("password", login_password);
        JSONObject root_field = new JSONObject(JSONObject.Type.OBJECT);
        root_field.AddField("user", nested_fields);

        string encodedString = root_field.ToString();

        string result = "";

        // the actual call, in a try catch
        try
        {
            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                result = client.UploadString(login_url, "POST", encodedString);
            }
        }
        catch (WebException ex)
        {
            Debug.Log("TESTexception: " + ex);
            var response = ex.Response as HttpWebResponse;
            errorHandler = RestError.GenericLoginError;

            if (response != null)
            {
                Debug.Log("HTTP Status Code: " + (int)response.StatusCode);
                switch ((int)response.StatusCode)
                {

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
                        break;
                }
            }

            allProper = false;
        }

        yield return result;

        if (allProper)
        {
            Debug.Log(result);
            JSONObject j = new JSONObject(result);
            // this won't work everytime
            Dictionary<string, string> decoded_response = j.ToDictionary();
            token = decoded_response["token"];
            logged_user_complete_name = decoded_response["complete_name"];
            logged_user_id = decoded_response["id"];


            int sessionCounter = int.Parse(decoded_response["sessions_counter"]);

            if (sessionCounter > 0)
            {
                sessionsHandler = RestSession.MultipleActive;
            }
        }
    }

    // Use this to GET the list of users
    public IEnumerator GETUsersList(List<Person> listOfDoctors)
    {
        bool allProper = true;

        string token_string = "Token token=\"" + token + "\", email=\"" + login_email + "\"";

        string result = "";
        string answer_text = string.Empty;

        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseURL + "/api/v1/users");
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
            errorHandler = RestError.GenericGetUserListError;

            if (response != null)
            {
                Debug.Log("HTTP Status Code: " + (int)response.StatusCode);
                switch ((int)response.StatusCode)
                {

                    case 401:
                        errorHandler = RestError.ZeroDoctors;
                        break;
                    case 500:
                        errorHandler = RestError.ServerError;
                        break;
                    default:
                        break;
                }
            }

            allProper = false;
        }

        yield return result;

        if (allProper)
        {
            //Debug.Log(result);

            JSONObject root_users = new JSONObject(answer_text);

            JSONObject nested_users = root_users.list[0];

            foreach (JSONObject user in nested_users.list)
            {
                Dictionary<string, string> decoded_user = user.ToDictionary();
                listOfDoctors.Add(new Person()
                {
                    ID = decoded_user["id"],
                    name = decoded_user["complete_name"],
                    age = int.Parse(decoded_user["age"]),
                    type = Person.Type.Doctor,
                    //photo = baseURL + decoded_user["user_avatar"]
                    photo = System.Text.RegularExpressions.Regex.Unescape(decoded_user["user_avatar_url"])
                    }
                );
            }
        }
    }

    // Use this to GET the list of patients
    public IEnumerator GETPatientsList(List<Person> listOfPatients)
    {

        bool allProper = true;

        string token_string = "Token token=\"" + token + "\", email=\"" + login_email + "\"";

        string result = "";
        string answer_text = string.Empty;

        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseURL + "/api/v1/patients");
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
            errorHandler = RestError.GenericGetPatientListError;

            if (response != null)
            {
                Debug.Log("HTTP Status Code: " + (int)response.StatusCode);
                switch ((int)response.StatusCode)
                {

                    case 401:
                        errorHandler = RestError.ZeroPatients;
                        break;
                    case 500:
                        errorHandler = RestError.ServerError;
                        break;
                    default:
                        break;
                }
            }

            allProper = false;
        }

        yield return result;

        if (allProper)
        {
            //Debug.Log(result);

            JSONObject root_patients = new JSONObject(answer_text);

            JSONObject nested_patients = root_patients.list[0];

            foreach (JSONObject patient in nested_patients.list)
            {
                Dictionary<string, string> decoded_patient = patient.ToDictionary();
                listOfPatients.Add(new Person()
                {
                    ID = decoded_patient["id"],
                    name = decoded_patient["complete_name"],
                    age = int.Parse(decoded_patient["age"]),
                    type = Person.Type.Patient,
                    photo = System.Text.RegularExpressions.Regex.Unescape(decoded_patient["patient_avatar_url"])
                    //photo = baseURL + decoded_patient["patient_avatar"]
                }
                );
            }
        }
    }

    // Use this to DELETE and do a logout
    public IEnumerator LOGOUTUser()
    {
        if (token.Equals(""))
        {
            errorHandler = RestError.NotLoggedIn;
        }
        else
        {
            bool allProper = true;

            string token_string = "Token token=\"" + token + "\", email=\"" + login_email + "\"";

            string answer_text = string.Empty;

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
                StreamReader readStream = new StreamReader(receiveStream, encode);
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
                errorHandler = RestError.GenericLogoutError;

                if (response != null)
                {
                    Debug.Log("HTTP Status Code: " + (int)response.StatusCode);
                    switch ((int)response.StatusCode)
                    {
                        case 401:
                            errorHandler = RestError.UnAuthorized;
                            break;
                        case 500:
                            errorHandler = RestError.ServerError;
                            break;
                        default:
                            break;
                    }
                }
                allProper = false;
            }

            yield return myHttpWebResponse;
            if (allProper)
            {
                Debug.Log(answer_text);
            }
        }
    }

    // Use this to DELETE and force a logout
    public IEnumerator ForceLOGOUTUser()
    {
        if (token.Equals(""))
        {
            errorHandler = RestError.NotLoggedIn;
        }
        else
        {
            bool allProper = true;
            string token_string = "Token token=\"" + token + "\", email=\"" + login_email + "\"";

            string answer_text = string.Empty;

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
                StreamReader readStream = new StreamReader(receiveStream, encode);
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
                errorHandler = RestError.GenericForceLogoutError;

                if (response != null)
                {
                    Debug.Log("HTTP Status Code: " + (int)response.StatusCode);
                    switch ((int)response.StatusCode)
                    {
                        case 401:
                            errorHandler = RestError.UnAuthorized;
                            break;
                        case 500:
                            errorHandler = RestError.ServerError;
                            break;
                        default:
                            break;
                    }
                }

                allProper = false;

            }

            yield return myHttpWebResponse;
            if (allProper)
            {
                Debug.Log(answer_text);
                token = "";
            }
        }
    }

    // Use this to DELETE and do a logout
    public void FinalLOGOUTUser()
    {
        if (token.Equals(""))
        {
            errorHandler = RestError.NotLoggedIn;
        }
        else
        {
            bool allProper = true;

            string token_string = "Token token=\"" + token + "\", email=\"" + login_email + "\"";

            string answer_text = string.Empty;

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
                StreamReader readStream = new StreamReader(receiveStream, encode);
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
                errorHandler = RestError.GenericFinalLogoutError;

                if (response != null)
                {
                    Debug.Log("HTTP Status Code: " + (int)response.StatusCode);
                    switch ((int)response.StatusCode)
                    {
                        case 401:
                            errorHandler = RestError.UnAuthorized;
                            break;
                        case 500:
                            errorHandler = RestError.ServerError;
                            break;
                        default:
                            break;
                    }
                }

                allProper = false;
            }

            if (allProper)
            {
                Debug.Log(answer_text);
                token = "";
            }
        }
    }


    // Use this to do a POST of a trails exercise
    public IEnumerator POSTTrailExercise(string exercisePath)
    {
        bool allProper = true;
        string result = "";

        TrailPreferences xmldata = new TrailPreferences();

        // let's start with the easy part: they tell me
        // where the .xml is, I open it and read it
        string mainFilePath = exercisePath + "\\main.xml";
        //Debug.Log("The path i search for the xml is " + mainFilePath);

        if (File.Exists(mainFilePath))
        {
            xmldata.LoadXML(mainFilePath);
            //Debug.Log("I actually read it!");

            // since it's really working we can
            // create the JSON structure to send

            JSONObject nested_fields_lvl1 = new JSONObject(JSONObject.Type.OBJECT);

            nested_fields_lvl1.AddField("patient_id", TrailPreferences.patientID);
            nested_fields_lvl1.AddField("time_to_live", TrailPreferences.trailsTimeToLive);
            nested_fields_lvl1.AddField("typology", TrailPreferences.trailsType.ToString());
            nested_fields_lvl1.AddField("start_datetime", TrailPreferences.initTime);
            nested_fields_lvl1.AddField("end_datetime", TrailPreferences.endTime);
            nested_fields_lvl1.AddField("special_fx", TrailPreferences.trailsSpecialFX.ToString().ToLower());

            // I have to get all the doctors involved
            // and pick out the first since it is the logged one
            string list_of_doctors = string.Join(", ", TrailPreferences.doctorsIDs.Skip(1).ToArray());
            nested_fields_lvl1.AddField("other_doctors", "[" + list_of_doctors + "]");

            // ok, let's start with the big issues... trails colors
            nested_fields_lvl1.AddField("enabled_therapist_left", TrailPreferences.othersSX_trailsEnabled.ToString().ToLower());
            if (TrailPreferences.othersSX_trailsEnabled == true)
            {
                JSONObject nested_fields_lvl2TL = new JSONObject(JSONObject.Type.OBJECT);
                nested_fields_lvl2TL.AddField("d", TrailPreferences.trailsDimension);
                nested_fields_lvl2TL.AddField("a", TrailPreferences.othersSX_trailsColor.a.ToString());
                nested_fields_lvl2TL.AddField("r", TrailPreferences.othersSX_trailsColor.r.ToString());
                nested_fields_lvl2TL.AddField("g", TrailPreferences.othersSX_trailsColor.g.ToString());
                nested_fields_lvl2TL.AddField("b", TrailPreferences.othersSX_trailsColor.b.ToString());

                nested_fields_lvl1.AddField("therapist_left_trail_color", nested_fields_lvl2TL);
            }

            nested_fields_lvl1.AddField("enabled_therapist_right", TrailPreferences.othersDX_trailsEnabled.ToString().ToLower());
            if (TrailPreferences.othersDX_trailsEnabled == true)
            {
                JSONObject nested_fields_lvl2TR = new JSONObject(JSONObject.Type.OBJECT);
                nested_fields_lvl2TR.AddField("d", TrailPreferences.trailsDimension);
                nested_fields_lvl2TR.AddField("a", TrailPreferences.othersDX_trailsColor.a.ToString());
                nested_fields_lvl2TR.AddField("r", TrailPreferences.othersDX_trailsColor.r.ToString());
                nested_fields_lvl2TR.AddField("g", TrailPreferences.othersDX_trailsColor.g.ToString());
                nested_fields_lvl2TR.AddField("b", TrailPreferences.othersDX_trailsColor.b.ToString());

                nested_fields_lvl1.AddField("therapist_right_trail_color", nested_fields_lvl2TR);
            }

            nested_fields_lvl1.AddField("enabled_patient_left", TrailPreferences.patientSX_trailsEnabled.ToString().ToLower());
            if (TrailPreferences.patientSX_trailsEnabled == true)
            {
                JSONObject nested_fields_lvl2PL = new JSONObject(JSONObject.Type.OBJECT);
                nested_fields_lvl2PL.AddField("d", TrailPreferences.trailsDimension);
                nested_fields_lvl2PL.AddField("a", TrailPreferences.patientSX_trailsColor.a.ToString());
                nested_fields_lvl2PL.AddField("r", TrailPreferences.patientSX_trailsColor.r.ToString());
                nested_fields_lvl2PL.AddField("g", TrailPreferences.patientSX_trailsColor.g.ToString());
                nested_fields_lvl2PL.AddField("b", TrailPreferences.patientSX_trailsColor.b.ToString());

                nested_fields_lvl1.AddField("patient_left_trail_color", nested_fields_lvl2PL);
            }

            nested_fields_lvl1.AddField("enabled_patient_right", TrailPreferences.patientDX_trailsEnabled.ToString().ToLower());
            if (TrailPreferences.patientDX_trailsEnabled == true)
            {
                JSONObject nested_fields_lvl2PR = new JSONObject(JSONObject.Type.OBJECT);
                nested_fields_lvl2PR.AddField("d", TrailPreferences.trailsDimension);
                nested_fields_lvl2PR.AddField("a", TrailPreferences.patientDX_trailsColor.a.ToString());
                nested_fields_lvl2PR.AddField("r", TrailPreferences.patientDX_trailsColor.r.ToString());
                nested_fields_lvl2PR.AddField("g", TrailPreferences.patientDX_trailsColor.g.ToString());
                nested_fields_lvl2PR.AddField("b", TrailPreferences.patientDX_trailsColor.b.ToString());

                nested_fields_lvl1.AddField("patient_right_trail_color", nested_fields_lvl2PR);
            }

            if (TrailPreferences.colorFilterEnabled == true)
            {
                JSONObject nested_fields_lvl2CF = new JSONObject(JSONObject.Type.OBJECT);
                nested_fields_lvl2CF.AddField("a", TrailPreferences.colorFilterAlpha.ToString());
                nested_fields_lvl2CF.AddField("r", TrailPreferences.colorFilter.r.ToString());
                nested_fields_lvl2CF.AddField("g", TrailPreferences.colorFilter.g.ToString());
                nested_fields_lvl2CF.AddField("b", TrailPreferences.colorFilter.b.ToString());

                nested_fields_lvl1.AddField("color_filter", nested_fields_lvl2CF);
            }

            // now the part which is going to be a mess, about the backgrounds
            if (TrailPreferences.backgroundIsImage == false)
            {
                JSONObject nested_fields_lvl2BG = new JSONObject(JSONObject.Type.OBJECT);
                nested_fields_lvl2BG.AddField("a", TrailPreferences.backgroundColor.a.ToString());
                nested_fields_lvl2BG.AddField("r", TrailPreferences.backgroundColor.r.ToString());
                nested_fields_lvl2BG.AddField("g", TrailPreferences.backgroundColor.g.ToString());
                nested_fields_lvl2BG.AddField("b", TrailPreferences.backgroundColor.b.ToString());

                nested_fields_lvl1.AddField("background_color", nested_fields_lvl2BG);
            }
            else
            {
                string fullPath = Path.Combine(exercisePath, Path.GetFileName(TrailPreferences.backgroundTexturePath));

                byte[] bytes = File.ReadAllBytes(fullPath);

                string base64String = System.Convert.ToBase64String(bytes);
                // Debug.Log("codifica dell'immagine: " + base64String);

                JSONObject nested_fields_lvl2BI = new JSONObject(JSONObject.Type.OBJECT);
                nested_fields_lvl2BI.AddField("filename", TrailPreferences.backgroundTexturePath);
                nested_fields_lvl2BI.AddField("content", base64String);
                nested_fields_lvl2BI.AddField("content_type", "image/jpeg");

                nested_fields_lvl1.AddField("background_image", nested_fields_lvl2BI);
            }

            // finally, everything goes back into trail
            JSONObject root_trail = new JSONObject(JSONObject.Type.OBJECT);
            root_trail.AddField("trail", nested_fields_lvl1);

            string encodedString = root_trail.ToString();
            Debug.Log(encodedString);

            //the actual call, in a try catch
            try
            {
                using (var client = new WebClient())
                {
                    string token_string = "Token token=\"" + token + "\", email=\"" + login_email + "\"";
                    client.Headers[HttpRequestHeader.Authorization] = token_string;
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    result = client.UploadString(trails_url, "POST", encodedString);
                }
            }
            catch (WebException ex)
            {
                Debug.Log("exception: " + ex);
                var response = ex.Response as HttpWebResponse;
                errorHandler = RestError.GenericPostTrailError;

                if (response != null)
                {
                    Debug.Log("HTTP Status Code: " + (int)response.StatusCode);
                    switch ((int)response.StatusCode)
                    {
                        case 500:
                            errorHandler = RestError.ServerError;
                            break;
                        default:
                            break;
                    }
                }

                allProper = false;
            }

            yield return result;

            if (allProper)
            {
                Debug.Log(result);
                // not really much to do if the exercise has been uploaded properly
            }

        }
        else
        {
            errorHandler = RestError.XMLNotPresent;
        }

        yield return result;

        if (allProper)
        {
            Debug.Log(result);
        }

    }

    // Use this to do a POST for a paint exercise
    public IEnumerator POSTPaintExercise(string exercisePath)
    {
        bool allProper = true;
        string result = "";

        PaintPreferences xmldata = new PaintPreferences();

        // let's start with the easy part: they tell me
        // where the .xml is, I open it and read it
        string mainFilePath = exercisePath + "\\main.xml";
        //Debug.Log("The path i search for the xml is " + mainFilePath);

        if (File.Exists(mainFilePath))
        {
            xmldata.LoadXML(mainFilePath);
            //Debug.Log("I actually read it!");

            // since it's really working we can
            // create the JSON structure to send

            JSONObject nested_fields_lvl1 = new JSONObject(JSONObject.Type.OBJECT);

            nested_fields_lvl1.AddField("patient_id", PaintPreferences.patientID);
            nested_fields_lvl1.AddField("start_datetime", PaintPreferences.initTime);
            nested_fields_lvl1.AddField("end_datetime", PaintPreferences.endTime);
            nested_fields_lvl1.AddField("patient_only", PaintPreferences.patientOnly.ToString().ToLower());

            // I have to get all the doctors involved
            // and pick out the first since it is the logged one
            string list_of_doctors = string.Join(", ", PaintPreferences.doctorsIDs.Skip(1).ToArray());
            nested_fields_lvl1.AddField("other_doctors", "[" + list_of_doctors + "]");

            JSONObject nested_fields_lvl2D = new JSONObject(JSONObject.Type.OBJECT);
            int count = 0;
            foreach (float dimension in PaintPreferences.paintDim)
            {
                count += 1;
                nested_fields_lvl2D.AddField("d" + count.ToString(), dimension);
            }
            nested_fields_lvl1.AddField("dimension", nested_fields_lvl2D);

            count = 0;
            foreach (Color single_color in PaintPreferences.paintColor)
            {
                JSONObject nested_fields_lvl2C = new JSONObject(JSONObject.Type.OBJECT);
                count += 1;
                nested_fields_lvl2C.AddField("a", single_color.a);
                nested_fields_lvl2C.AddField("r", single_color.r);
                nested_fields_lvl2C.AddField("g", single_color.g);
                nested_fields_lvl2C.AddField("b", single_color.b);

                nested_fields_lvl1.AddField("color" + count.ToString(), nested_fields_lvl2C);
            }

            if (PaintPreferences.colorFilterEnabled == true)
            {
                JSONObject nested_fields_lvl2CF = new JSONObject(JSONObject.Type.OBJECT);
                nested_fields_lvl2CF.AddField("a", PaintPreferences.colorFilterAlpha.ToString());
                nested_fields_lvl2CF.AddField("r", PaintPreferences.colorFilter.r.ToString());
                nested_fields_lvl2CF.AddField("g", PaintPreferences.colorFilter.g.ToString());
                nested_fields_lvl2CF.AddField("b", PaintPreferences.colorFilter.b.ToString());

                nested_fields_lvl1.AddField("color_filter", nested_fields_lvl2CF);
            }

            // I have to set it here because srly that is fucked up
            // PaintPreferences.backgroundIsImage = false;
            // now the part which is going to be a mess, about the backgrounds
            if (PaintPreferences.backgroundIsImage == false)
            {
                JSONObject nested_fields_lvl2BG = new JSONObject(JSONObject.Type.OBJECT);
                nested_fields_lvl2BG.AddField("a", PaintPreferences.backgroundColor.a.ToString());
                nested_fields_lvl2BG.AddField("r", PaintPreferences.backgroundColor.r.ToString());
                nested_fields_lvl2BG.AddField("g", PaintPreferences.backgroundColor.g.ToString());
                nested_fields_lvl2BG.AddField("b", PaintPreferences.backgroundColor.b.ToString());

                nested_fields_lvl1.AddField("background_color", nested_fields_lvl2BG);
            }
            else
            {
                string fullPath = Path.Combine(exercisePath, Path.GetFileName(TrailPreferences.backgroundTexturePath));

                byte[] bytes = File.ReadAllBytes(fullPath);

                string base64String = Convert.ToBase64String(bytes);
                //Debug.Log("codifica dell'immagine: " + base64String);

                JSONObject nested_fields_lvl2BI = new JSONObject(JSONObject.Type.OBJECT);
                nested_fields_lvl2BI.AddField("filename", PaintPreferences.backgroundTexturePath);
                nested_fields_lvl2BI.AddField("content", base64String);
                nested_fields_lvl2BI.AddField("content_type", "image/jpeg");

                nested_fields_lvl1.AddField("background_image", nested_fields_lvl2BI);
            }

            string drawingFilePath = exercisePath + "\\Paint_Drawing.png";

            if (File.Exists(drawingFilePath))
            {
                byte[] bytes = File.ReadAllBytes(drawingFilePath);

                string base64String = Convert.ToBase64String(bytes);
                //Debug.Log("codifica dell'immagine: " + base64String);

                JSONObject nested_fields_lvl2PD= new JSONObject(JSONObject.Type.OBJECT);
                nested_fields_lvl2PD.AddField("filename", "Paint_Drawing.png");
                nested_fields_lvl2PD.AddField("content", base64String);
                nested_fields_lvl2PD.AddField("content_type", "image/png");

                nested_fields_lvl1.AddField("paint_drawing", nested_fields_lvl2PD);
            }

            // finally, everything goes back in to trails
            JSONObject root_paint = new JSONObject(JSONObject.Type.OBJECT);
            root_paint.AddField("paint", nested_fields_lvl1);

            string encodedString = root_paint.ToString();
            Debug.Log(encodedString);


            //the actual call, in a try catch
            try
            {
                using (var client = new WebClient())
                {
                    string token_string = "Token token=\"" + token + "\", email=\"" + login_email + "\"";
                    client.Headers[HttpRequestHeader.Authorization] = token_string;
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    result = client.UploadString(paints_url, "POST", encodedString);
                }
            }
            catch (WebException ex)
            {
                Debug.Log("exception: " + ex);
                var response = ex.Response as HttpWebResponse;
                errorHandler = RestError.GenericPostPaintError;

                if (response != null)
                {
                    Debug.Log("HTTP Status Code: " + (int)response.StatusCode);
                    switch ((int)response.StatusCode)
                    {

                        //			case 400:
                        //				errorHandler = RestError.WrongMail;
                        //				break;
                        //			case 401:
                        //				errorHandler = RestError.WrongPassword;
                        //				break;
                        case 500:
                            errorHandler = RestError.ServerError;
                            break;
                        default:
                            break;
                    }
                }
                allProper = false;
            }

            yield return result;

            if (allProper)
            {
                //Debug.Log(result);
            }

        }
        else
        {
            errorHandler = RestError.XMLNotPresent;
        }

        yield return result;

        if (allProper)
        {
            Debug.Log(result);
        }

    }

    //---------------------------------------------------------------------
    //------------------------  TESTING METHODS  --------------------------
    //---------------------------------------------------------------------


}
