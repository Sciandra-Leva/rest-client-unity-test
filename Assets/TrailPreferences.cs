/* File TrailPreferences C# implementation of class TrailPreferences */

/*	      Computer Graphics and Vision Group	  */
/*		  Politecnico di Torino			  */
/*							  */
/*  THIS IS UNPUBLISHED PROPRIETARY SOURCE CODE OF POLITO */
/*  The copyright notice above does not evidence any      */
/*  actual or intended publication of such source code.   */


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
public static string trailsType = "Test type";


public static Color patientDX_trailsColor = Color.red;
public static Color patientSX_trailsColor = Color.green;
public static Color othersDX_trailsColor = Color.magenta;
public static Color othersSX_trailsColor = Color.yellow;

public static float trailsTimeToLive = 5f;

public static bool trailsSpecialFX = false;

public static string trailsMusic = null;
// class declaration end



    public override void LoadXML(string filename)
    {
       base.LoadXML(filename);
    
       // reads its own parts
       UpdateSettings();
    }


    /*public override void SaveXML(string filename)
    {
    	base.SaveXML(filename);
    
    	if(patientDX_trailsEnabled)
    		xmlDoc.SelectSingleNode("//xml/trails/enabled/patientDX").InnerText = "true";
    	else
    		xmlDoc.SelectSingleNode("//xml/trails/enabled/patientDX").InnerText = "false";
    
    	if(patientSX_trailsEnabled)
    		xmlDoc.SelectSingleNode("//xml/trails/enabled/patientSX").InnerText = "true";
    	else
    		xmlDoc.SelectSingleNode("//xml/trails/enabled/patientSX").InnerText = "false";
    
    	if(othersDX_trailsEnabled)
    		xmlDoc.SelectSingleNode("//xml/trails/enabled/othersDX").InnerText = "true";
    	else
    		xmlDoc.SelectSingleNode("//xml/trails/enabled/othersDX").InnerText = "false";
    
    	if(othersSX_trailsEnabled)
    		xmlDoc.SelectSingleNode("//xml/trails/enabled/othersSX").InnerText = "true";
    	else
    		xmlDoc.SelectSingleNode("//xml/trails/enabled/othersSX").InnerText = "false";
    
    	xmlDoc.SelectSingleNode("//xml/trails/dimension").InnerText = trailsDimension.ToString("0.00");
    
    	switch(trailsType)
    	{
    		case PITrailRenderer.PITrailParticlesRenderer.Billboards:
    			xmlDoc.SelectSingleNode("//xml/trails/type").InnerText = "Billboards";
    			break;
    		case PITrailRenderer.PITrailParticlesRenderer.Particles:
    			xmlDoc.SelectSingleNode("//xml/trails/type").InnerText = "Particles";
    			break;	
    		case PITrailRenderer.PITrailParticlesRenderer.Trails:
    			xmlDoc.SelectSingleNode("//xml/trails/type").InnerText = "Trails";
    			break;	
    		case PITrailRenderer.PITrailParticlesRenderer.FashionTrails:
    			xmlDoc.SelectSingleNode("//xml/trails/type").InnerText = "FashionTrails";
    			break;	
    		case PITrailRenderer.PITrailParticlesRenderer.SmoothFashionTrails:
    			xmlDoc.SelectSingleNode("//xml/trails/type").InnerText = "SmoothFashionTrails";
    			break;	
    	}
    
    	xmlDoc.SelectSingleNode("//xml/trails/timeToLive").InnerText = trailsTimeToLive.ToString("0.00");
    
    	if(trailsSpecialFX)
    		xmlDoc.SelectSingleNode("//xml/trails/specialFx").InnerText = "true";
    	else
    		xmlDoc.SelectSingleNode("//xml/trails/specialFx").InnerText = "false";
    
    	xmlDoc.SelectSingleNode("//xml/trails/color/patientDX").InnerText = ConvertColorToHex(patientDX_trailsColor);
    	xmlDoc.SelectSingleNode("//xml/trails/color/patientSX").InnerText = ConvertColorToHex(patientSX_trailsColor);
    	xmlDoc.SelectSingleNode("//xml/trails/color/othersDX").InnerText = ConvertColorToHex(othersDX_trailsColor);
    	xmlDoc.SelectSingleNode("//xml/trails/color/othersSX").InnerText = ConvertColorToHex(othersSX_trailsColor);
    
    	xmlDoc.SelectSingleNode("//xml/trails/music").InnerText = trailsMusic;
    
    	if( !string.IsNullOrEmpty(filename) )
    		xmlDoc.Save(filename);
    	else
    		xmlDoc.Save(xmlCompletePath);
    
    	if(PIPars.Debug) Debug.Log("Saving on XML... DONE! "+xmlCompletePath);
    }
    */

    public void UpdateSettings()
    {
    ////// Reading Trails Settings
        
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
    	if(!string.IsNullOrEmpty(type))
    	{
    		if(string.Compare(type, "Billboards", true) == 0)
    			trailsType = "Billboards";
    		else if(string.Compare(type, "Particles", true) == 0)
    			trailsType = "Particles";
    		else if(string.Compare(type, "Trails", true) == 0)
    			trailsType = "Trails";
    		else if(string.Compare(type, "FashionTrails", true) == 0)
    			trailsType = "FashionTrails";
    		else if(string.Compare(type, "SmoothFashionTrails", true) == 0)
    			trailsType = "SmoothFashionTrails";
    		else
    			Debug.LogError("TrailPrefrences: type not recognized " + type);
    	}
    	else
    		trailsType = "Particles";
    
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
    
    // ---- MUSIC
    	string music = GetNodeFromXML("xml", "trails", "music");
    	trailsMusic = music;
    
    	if(PIPars.Debug) Debug.Log("TrailsPreferences :: UpdateSettings :: DONE!");
    }


}
