using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;


public class TestNetworkScript : MonoBehaviour {

	public NetworkREST client;

	// Use this for initialization
	IEnumerator Start (){

		// first step, add it to the gameobject
		client = gameObject.AddComponent<NetworkREST>();

		// i'm currently doing it as a coroutine in order 
		// to avoid slowing down the process. the yield return is simply
		// to show how to avoid running ahead of time (ex: doing the gets 
		// before the login has been done)
		yield return StartCoroutine(client.LOGINUser ("sciandra@leva.io", "Sementera"));

		if (client.errorHandler != RestError.AllGood) // this check should be done after every command.
		{
			Debug.Log ("There has been an error: " + client.errorHandler);
		} 
		else if (client.sessionsHandler == RestSession.MultipleActive) 
		{
			Debug.Log ("There are multiple sessions with this user");
			// you may want to do something when multiple session are active...
			// like invoking this to force all logouts

			yield return StartCoroutine(client.ForceLOGOUTUser());
			// which of course requires that you do a new login...
		}
		else
		{
			//// once we are logged in, it is time to obtain those Doctors and
			//// Patients informations.
			//// In this example there are two different lists for patients
			//// and Doctors but since both are persons, potentially a single
			//// list can be used
			List<Person> listOfDoctors = new List<Person> (); 
			yield return StartCoroutine(client.GETUsersList(listOfDoctors));

            if (client.errorHandler != RestError.AllGood) // this check should be done after every command.
            {
                Debug.Log("There has been an error: " + client.errorHandler);
            }

            List<Person> listOfPatients = new List<Person>();
            yield return StartCoroutine(client.GETPatientsList(listOfPatients));
            if (client.errorHandler != RestError.AllGood) // this check should be done after every command.
            {
                Debug.Log("There has been an error: " + client.errorHandler);
            }

            // testing the populated lists with Linq
            Debug.Log("To test the freshly populated lists: " +
                "First Patient registered: " +
                listOfPatients.ElementAt(0).name +
                "And a photo url is:" +
                listOfPatients.ElementAt(0).photo +
                " and the first Doctor registered: " +
                listOfDoctors.ElementAt(0).name +
                "And a photo url is:" +
                listOfDoctors.ElementAt(0).photo
            );

            // this is the one used to log out the 
            // current user.
            //			yield return StartCoroutine(client.LOGOUTUser());	
            //			if (client.errorHandler != RestError.AllGood) // this check should be done after every command.
            //			{
            //				Debug.Log ("There has been an error: " + client.errorHandler);
            //			} 


            // Ok, let's try now to get the post exercise to work.
           string path_of_exercise = "C:\\Users\\Lorenzo\\Documents\\workspace-leva\\rest-client-unity-test\\Assets\\Sessions\\20160203\\OlivieroManzari\\TRAILS_0939";
            yield return StartCoroutine(client.POSTTrailExercise(path_of_exercise));
            if (client.errorHandler != RestError.AllGood) // this check should be done after every command.
            {
                Debug.Log("There has been an error Trail: " + client.errorHandler);
            }

            // the paint POST 
            //path_of_exercise = "C:\\Users\\Lorenzo\\Documents\\workspace-leva\\rest-client-unity-test\\Assets\\Sessions\\20160203\\OlivieroManzari\\PAINT_0939";
            //yield return StartCoroutine(client.POSTPaintExercise(path_of_exercise));
            //if (client.errorHandler != RestError.AllGood) // this check should be done after every command.
            //{
            //    Debug.Log("There has been an error Paint: " + client.errorHandler);
            //}

            //// the ball POST 
            //path_of_exercise = "C:\\Users\\Lorenzo\\Documents\\workspace-leva\\rest-client-unity-test\\Assets\\Sessions\\20160203\\AldoBo\\PHYSICS_1247";
            //yield return StartCoroutine(client.POSTPhysicsExercise(path_of_exercise));
            //if (client.errorHandler != RestError.AllGood) // this check should be done after every command.
            //{
            //    Debug.Log("There has been an error Ball: " + client.errorHandler);
            //}

        }
	}

	void OnDestroy()
	{
        client.FinalLOGOUTUser();
        if (client.errorHandler != RestError.AllGood) // this check should be done after every command.
        {
            Debug.Log("There has been an error: " + client.errorHandler);
        }
    }

}
