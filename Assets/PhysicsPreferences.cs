/* File PhysicsPreferences C# implementation of class PhysicsPreferences */

/*	      Computer Graphics and Vision Group	  */
/*		  Politecnico di Torino			  */
/*							  */
/*  THIS IS UNPUBLISHED PROPRIETARY SOURCE CODE OF POLITO */
/*  The copyright notice above does not evidence any      */
/*  actual or intended publication of such source code.   */


// global declaration start


using UnityEngine;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System;

// global declaration end

[Serializable]
class PhysicsPreferences : BgPreferences
{
// class declaration start
public static Color ball_Color = Color.yellow;
public static string ball_Color_text = "Yellow";
public static int ball_Size = 10;
public static float ball_Weight = 3f;
public static float ball_Bounciness = 1f;

public static Hashtable ballColors;
public static List<int> ballSizes;

public static bool patientOnly = false;
// class declaration end



    public override void LoadXML(string filename)
    {
       base.LoadXML(filename);
       // reads its own parts
    
       ballColors = new Hashtable();
       ballColors.Add("Red", Color.red);
       ballColors.Add("Green", Color.green);
       ballColors.Add("Blue", Color.blue);
    
       ballSizes = new List<int>();
       ballSizes.Add(10); 
       ballSizes.Add(20); 
       ballSizes.Add(30); 
    
       UpdateSettings();
    }


    public override void SaveXML(string filename)
    {
    	base.SaveXML(filename);
    
    	xmlDoc.SelectSingleNode("//xml/physics/color").InnerText = ball_Color_text;
    	xmlDoc.SelectSingleNode("//xml/physics/size").InnerText = ball_Size.ToString();
    	xmlDoc.SelectSingleNode("//xml/physics/weight").InnerText = ball_Weight.ToString("0.00");
    	xmlDoc.SelectSingleNode("//xml/physics/bounciness").InnerText = ball_Bounciness.ToString("0.00");
    	xmlDoc.SelectSingleNode("//xml/physics/patientOnly").InnerText = patientOnly.ToString();
    
    	xmlDoc.Save(xmlCompletePath);
    	if(PIPars.Debug) Debug.Log("Saving on XML... DONE!");
    }


    public void UpdateSettings()
    {
    ////// Reading Physics Settings
        
    	string color = GetNodeFromXML("xml", "physics", "color");
    	if(!string.IsNullOrEmpty(color))
    	{
    		ball_Color = (Color)ballColors[color];
    		ball_Color_text = color;
    	}
    	else
    		ball_Color = Color.red;
    
    	string size = GetNodeFromXML("xml", "physics", "size");
    	if(!string.IsNullOrEmpty(size) && ballSizes.Contains(int.Parse(size)))
    		ball_Size = int.Parse(size);
    	else
    		ball_Size = 10;
    
    	string weight = GetNodeFromXML("xml", "physics", "weight");
    	if(!string.IsNullOrEmpty(weight))
    		ball_Weight = float.Parse(weight);
    	else
    		ball_Weight = 1f;
    
    	string bounciness = GetNodeFromXML("xml", "physics", "bounciness");
    	if(!string.IsNullOrEmpty(bounciness))
    		ball_Bounciness = float.Parse(bounciness);
    	else
    		ball_Bounciness = 1f;
    
    	string physicsPatientOnly = GetNodeFromXML("xml", "physics", "patientOnly");
    	patientOnly = bool.Parse(physicsPatientOnly);
    
    	if(PIPars.Debug) Debug.Log("PhysicsPreferences :: UpdateSettings :: DONE!");
    }


}
