/* File TrailPreferences C# implementation of class TrailPreferences */

// THIS a VANILLA VERSION
// USED BY LORENZO SCIANDRA
// TO TEST THE LOAD PART

// global declaration start


using UnityEngine;
using System.Xml;
using System;

// global declaration end

[Serializable]
public class TrailPreferences : BgPreferences
{
// class declaration start
public static bool patientDX_trailsEnabled = true;
public static bool patientSX_trailsEnabled = true;
public static bool othersDX_trailsEnabled = true;
public static bool othersSX_trailsEnabled = true;

public static float trailsDimension = 5f;

//public static PITrailRenderer.PITrailParticlesRenderer trailsType = PITrailRenderer.PITrailParticlesRenderer.Particles;

public static string trailsType = "Billboards";

public static Color patientDX_trailsColor = Color.red;
public static Color patientSX_trailsColor = Color.green;
public static Color othersDX_trailsColor = Color.magenta;
public static Color othersSX_trailsColor = Color.yellow;

public static float trailsTimeToLive = 5f;

public static bool trailsSpecialFX = false;
// class declaration end



    public override void LoadXML(string filename)
    {
       base.LoadXML(filename);
    
       // reads its own parts
       UpdateSettings();
    }


    public void UpdateSettings()
    {
    ////// Reading Trails Settings
		Debug.Log("I get to read the specific data for trails!");
    	string patientDX_enabled = GetNodeFromXML("xml/trails", "enabled", "patientDX");
    	if(!string.IsNullOrEmpty(patientDX_enabled))
    	{
    		if(string.Compare(patientDX_enabled, "true", true) == 0)
    			patientDX_trailsEnabled = true;
    		else
    			patientDX_trailsEnabled = false;
    	}
    	else
    		patientDX_trailsEnabled = true;
    
    	string patientSX_enabled = GetNodeFromXML("xml/trails", "enabled", "patientSX");
    	if(!string.IsNullOrEmpty(patientSX_enabled))
    	{
    		if(string.Compare(patientSX_enabled, "true", true) == 0)
    			patientSX_trailsEnabled = true;
    		else
    			patientSX_trailsEnabled = false;
    	}
    	else
    		patientSX_trailsEnabled = true;
    
    	string othersDX_enabled = GetNodeFromXML("xml/trails", "enabled", "othersDX");
    	if(!string.IsNullOrEmpty(othersDX_enabled))
    	{
    		if(string.Compare(othersDX_enabled, "true", true) == 0)
    			othersDX_trailsEnabled = true;
    		else
    			othersDX_trailsEnabled = false;
    	}
    	else
    		othersDX_trailsEnabled = true;
    
    	string othersSX_enabled = GetNodeFromXML("xml/trails", "enabled", "othersSX");
    	if(!string.IsNullOrEmpty(othersSX_enabled))
    	{
    		if(string.Compare(othersSX_enabled, "true", true) == 0)
    			othersSX_trailsEnabled = true;
    		else
    			othersSX_trailsEnabled = false;
    	}
    	else
    		othersSX_trailsEnabled = true;
    
    // ---- DIMENSION
    	string dimension = GetNodeFromXML("xml", "trails", "dimension");
    	if(!string.IsNullOrEmpty(dimension))
    	{
    		trailsDimension = float.Parse(dimension);
    	}
    	else
    		trailsDimension = 5f;
    
    // ---- TYPE
    	string type = GetNodeFromXML("xml", "trails", "type");
		if( !string.IsNullOrEmpty(type) )
			trailsType = type;
			
    // ---- COLOR
    	string patientDX_color = GetNodeFromXML("xml/trails", "color", "patientDX");
    	if( !string.IsNullOrEmpty(patientDX_color) )
        		patientDX_trailsColor = ConvertHextoColor(patientDX_color, 1f);
        	else
        		patientDX_trailsColor = Color.red;
    
    	string patientSX_color = GetNodeFromXML("xml/trails", "color", "patientSX");
    	if( !string.IsNullOrEmpty(patientSX_color) )
        		patientSX_trailsColor = ConvertHextoColor(patientSX_color, 1f);
        	else
        		patientSX_trailsColor = Color.red;
    
    	string othersDX_color = GetNodeFromXML("xml/trails", "color", "othersDX");
    	if( !string.IsNullOrEmpty(othersDX_color) )
        		othersDX_trailsColor = ConvertHextoColor(othersDX_color, 1f);
        	else
        		othersDX_trailsColor = Color.red;
    
    	string othersSX_color = GetNodeFromXML("xml/trails", "color", "othersSX");
    	if( !string.IsNullOrEmpty(othersSX_color) )
        		othersSX_trailsColor = ConvertHextoColor(othersSX_color, 1f);
        	else
        		othersSX_trailsColor = Color.red;
       
    // ---- TIME TO LIVE
    	string timeToLive = GetNodeFromXML("xml", "trails", "timeToLive");
    	if(!string.IsNullOrEmpty(timeToLive))
    	{
    		trailsTimeToLive = float.Parse(timeToLive);
    	}
    	else
    		trailsTimeToLive = 5f;	
    
    // ---- SPECIAL FX
    	string specialFx = GetNodeFromXML("xml", "trails", "specialFx");
    	if(!string.IsNullOrEmpty(specialFx))
    	{
    		if(string.Compare(specialFx, "true", true) == 0)
    			trailsSpecialFX = true;
    		else
    			trailsSpecialFX = false;
    	}
    	else
    		trailsSpecialFX = false;
    
    	Debug.Log("TrailsPreferences :: UpdateSettings :: DONE!");
    }


}
