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
                " and the first Doctor registered: " +
                listOfDoctors.ElementAt(0).name
            );

            // this is the one used to log out the 
            // current user.
            //			yield return StartCoroutine(client.LOGOUTUser());	
            //			if (client.errorHandler != RestError.AllGood) // this check should be done after every command.
            //			{
            //				Debug.Log ("There has been an error: " + client.errorHandler);
            //			} 

            // Ok, let's try now to get the post exercise to work.
            //string path_of_exercise = "/users/lorenzosciandra/documents/workspace-testing-lorenzo" +
            //	"/unity-projects/rest-client-testing/rest-client/assets/sessions/20160203/aldobo/" +
            //	"trails_1138";
            string path_of_exercise = "C:\\Users\\Lorenzo\\Documents\\rest-client-unity-test\\REST-client\\Assets\\Sessions\\20160203\\AldoBo\\TRAILS_1138";
            yield return StartCoroutine(client.POSTTrailExercise(path_of_exercise));
            if (client.errorHandler != RestError.AllGood) // this check should be done after every command.
            {
                Debug.Log("There has been an error: " + client.errorHandler);
            }
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
