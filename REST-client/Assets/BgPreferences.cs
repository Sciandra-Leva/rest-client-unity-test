/* File BgPreferences C# implementation of class BgPreferences */



// global declaration start


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System;


// global declaration end

[Serializable]
public class BgPreferences
{
// class declaration start
public enum KinectTexture {
	ColorHD,
	ColorGreenScreen,
	SilhouetteGreenScreen
};

public enum BackgroundType {
	Color,
	Image
};



public static string initTime = null;
public static string endTime = null;
public static string patientID = null;
public static string patientName = null;
public static List<string> doctorsIDs = new List<string>();
public static string authenticatedDoctor = null;

public static bool colorFilterEnabled = true;
public static float colorFilterAlpha = 0.1f;
public static Color colorFilter = Color.red;

public static bool kinectEnabled = true;
public static Color kinectSilhouetteColor = Color.blue;
public static float kinectAlpha = 1.0f;
public static KinectTexture kinectTexture = KinectTexture.ColorHD;

public static Texture backgroundTexture = null;
public static string backgroundTexturePath = null;
public static Color backgroundColor = Color.green;
public static bool backgroundIsImage = false;
// class declaration end


    protected XmlDocument xmlDoc = new XmlDocument();
    protected string xmlCompletePath = null;


    protected virtual string GetNodeFromXML(string root, string nodeTag, string nodeAttribute)
	{ 
		string xmlPath = new System.Text.StringBuilder ("/").Append (root).Append ("/").Append (nodeTag).Append ("/").Append (nodeAttribute).ToString (); 
            
		XmlNode xmlNode = xmlDoc.DocumentElement.SelectSingleNode (xmlPath);
            
		if (xmlNode == null || String.IsNullOrEmpty (xmlNode.InnerText))
		{
			Debug.Log ("Something went very wrong very fast");
			return "fuck";
		}
    	else
		{
    		return(xmlNode.InnerText);
    
		}
	}


    protected Color ConvertHextoColor(string HexColor, float alpha)
    {
    	string tmpColor = "";
    	int R = 0;
    	int G = 0;
    	int B = 0;
            
    	if(HexColor.Contains("#")) 
    		tmpColor = HexColor.Substring(1);
    	else 
    		tmpColor = HexColor;
            
    	if( tmpColor.Length == 6 )
    	{
    		R = int.Parse(tmpColor.Substring(0,2), System.Globalization.NumberStyles.AllowHexSpecifier);
    		G = int.Parse(tmpColor.Substring(2,2), System.Globalization.NumberStyles.AllowHexSpecifier);
    		B = int.Parse(tmpColor.Substring(4,2), System.Globalization.NumberStyles.AllowHexSpecifier);
    	}
    	else if ( tmpColor.Length == 3 )
    	{
    		R = int.Parse( (tmpColor.Substring(0,1)+tmpColor.Substring(0,1)), System.Globalization.NumberStyles.AllowHexSpecifier);
    		G = int.Parse(tmpColor.Substring(1,1)+tmpColor.Substring(1,1), System.Globalization.NumberStyles.AllowHexSpecifier);
    		B = int.Parse(tmpColor.Substring(2,1)+tmpColor.Substring(2,1), System.Globalization.NumberStyles.AllowHexSpecifier);
    	}
    	else
    	{
    		Debug.LogError( new System.Text.StringBuilder("ConvertHextoColor :: Error, ").Append(HexColor).Append(" format is invalid!") ); 
    		return Color.black;
    	}
            
    	Color convertedColor = new Color ( R/255f, G/255f, B/255f, alpha );
            
    	return convertedColor;
    }


    protected string ConvertColorToHex(Color col)
    {
    	string tmpColor = "";
    	int R = 0;
    	int G = 0;
    	int B = 0;
    
    	R = Mathf.RoundToInt(col.r * 255);
    	G = Mathf.RoundToInt(col.g * 255);
    	B = Mathf.RoundToInt(col.b * 255);
    	
    	tmpColor = "#"+R.ToString("X2")+G.ToString("X2")+B.ToString("X2");
    
    	return tmpColor;
    }


    protected virtual string[] GetNodesFromXML(string root, string nodeTag, string nodeAttribute)
    { 
        	string xmlPath = new System.Text.StringBuilder("/").Append(root).Append("/").Append(nodeTag).Append("/").Append(nodeAttribute).ToString(); 
            
    	XmlNode xmlNode = xmlDoc.DocumentElement.SelectSingleNode(xmlPath);
            
    	string[] children = new string[xmlNode.ChildNodes.Count];
    
    	for (int i = 0; i < xmlNode.ChildNodes.Count; i++)
          	{
            	children[i] = xmlNode.ChildNodes[i].InnerText;
          	}	
    
    	return(children);
    }




    public virtual void LoadXML(string filename)
    {
    	Debug.Log("Loading xml: "+filename);
    	
    	if( !System.IO.File.Exists(filename) )
    	{
    		Debug.LogError("Error :: LoadXML :: File not found @ "+filename);
    		return;
    	}
    
        	xmlDoc.Load(filename);
    
    	xmlCompletePath = filename;
        
    	initTime = GetNodeFromXML("xml", "Session", "InitTime");
    	endTime = GetNodeFromXML("xml", "Session", "EndTime");
    	patientID = GetNodeFromXML("xml", "Session", "PatientID");
    	doctorsIDs = GetNodesFromXML("xml", "Session", "DoctorsIDs").ToList();
    	
    
    ////// Reading BG Layer Settings
        	string bgLayerImage = GetNodeFromXML("xml/background", "bgLayer", "image");
        	if( !string.IsNullOrEmpty(bgLayerImage) )
        	{
        		backgroundTexturePath = bgLayerImage;
    		backgroundIsImage = true;
        		Debug.Log("LoadXML :: BGLayer :: Image :: "+backgroundTexturePath+" || "+bgLayerImage);
        	}
        	else
        	{
    		backgroundIsImage = false;
        		string bgLayerColor = GetNodeFromXML("xml/background", "bgLayer", "color");
        		//string bgLayerColorAlpha = GetNodeFromXML("xml/background", "bgLayer", "colorAlpha");
        		if( !string.IsNullOrEmpty(bgLayerColor) )
        		{
        			backgroundColor = ConvertHextoColor(bgLayerColor, 1f);
        			Debug.Log("LoadXML :: BGLayer :: Color :: "+backgroundColor);
        		}
        		else
        		{
        			backgroundColor = Color.white;
        			Debug.Log("LoadXML :: BGLayer :: Color :: "+backgroundColor);
        		}
        	}
        
    ////// Reading RGBA Layer Settings
        	string rgbaLayerEnabled = GetNodeFromXML("xml/background", "rgbaLayer", "enabled");
        	if( string.Compare(rgbaLayerEnabled, "true", true) == 0 )
        	{
        		string rgbaLayerAlpha = GetNodeFromXML("xml/background", "rgbaLayer", "alpha");
        		if( !string.IsNullOrEmpty(rgbaLayerAlpha) )
        		{
        			kinectAlpha = float.Parse(rgbaLayerAlpha);
        		}
        		else
        		{
        			kinectAlpha = 1f;
        		}
        		Debug.Log("LoadXML :: RGBALayer :: Alpha :: "+kinectAlpha);
        	}
        	else
        	{
        		Debug.Log("LoadXML :: RGBALayer :: Disabled");
        		kinectEnabled = false;
        	}
        
    ////// Reading Color Filter Layer Settings
        	string filterLayerColor = GetNodeFromXML("xml/background", "filterLayer", "color");
        	string filterLayerAlpha = GetNodeFromXML("xml/background", "filterLayer", "alpha");
        	if( string.IsNullOrEmpty(filterLayerColor) )
        		colorFilterEnabled = false;
        	else
        	{
        		if( !string.IsNullOrEmpty(filterLayerColor) )
        		{
        			if( string.IsNullOrEmpty(filterLayerAlpha) )
        			{
        				colorFilter = ConvertHextoColor(filterLayerColor, 1f);
        				colorFilterAlpha = 1f;
        			}
        			else
        			{
        				colorFilter = ConvertHextoColor(filterLayerColor, float.Parse(filterLayerAlpha));
        				colorFilterAlpha = float.Parse(filterLayerAlpha);
        			}
        		}
        	}
        	Debug.Log("LoadXML :: ColorFilterLayer :: Color :: "+colorFilter);
    }


    public virtual void SaveXML(string filename)
    {
    	// Making a copy of the old configuration file
    	System.IO.File.Copy(xmlCompletePath, xmlCompletePath+".old", true);
    
    	if( !string.IsNullOrEmpty(filename) )
    		xmlCompletePath = filename;
    	
    	Debug.Log("Saving on XML... "+xmlCompletePath);
    
    	//Changing all parameters with new values
    	// SESSION Parameters		
    	xmlDoc.SelectSingleNode("//xml/Session/InitTime").InnerText = initTime;
    	xmlDoc.SelectSingleNode("//xml/Session/EndTime").InnerText = endTime;
    	xmlDoc.SelectSingleNode("//xml/Session/PatientID").InnerText = patientID;
    	xmlDoc.SelectSingleNode("//xml/Session/PatientName").InnerText = patientName;
    	xmlDoc.SelectSingleNode("//xml/Session/DoctorsIDs").RemoveAll();
    	foreach( string s in doctorsIDs )
    	{
    		//Create a new node.
        		XmlElement elem = xmlDoc.CreateElement("DocID");
        		elem.InnerText = s;
    
        		//Add the node to the document.
        		xmlDoc.SelectSingleNode("//xml/Session/DoctorsIDs").AppendChild(elem);
    		//xmlDoc.SelectSingleNode("//xml/Session/DoctorsID/").InnerText = patientID;
    	}
    
    	// BG Layer
    	if(backgroundTexturePath != null)
    		xmlDoc.SelectSingleNode("//xml/background/bgLayer/image").InnerText = System.IO.Path.GetFileName(backgroundTexturePath);
    	else
    		xmlDoc.SelectSingleNode("//xml/background/bgLayer/image").InnerText = "";
    		
    	xmlDoc.SelectSingleNode("//xml/background/bgLayer/color").InnerText = ConvertColorToHex(backgroundColor);
    
    	// RGBA Kinect Layer
    	if(kinectAlpha == 0f)
    	{
    		xmlDoc.SelectSingleNode("//xml/background/rgbaLayer/enabled").InnerText = "false";
    	}
    	else
    	{
    		xmlDoc.SelectSingleNode("//xml/background/rgbaLayer/enabled").InnerText = "true";
    		xmlDoc.SelectSingleNode("//xml/background/rgbaLayer/alpha").InnerText = kinectAlpha.ToString();
    	}
    
    	// Color Filter Layer
    	xmlDoc.SelectSingleNode("//xml/background/filterLayer/color").InnerText = ConvertColorToHex(colorFilter);
    	xmlDoc.SelectSingleNode("//xml/background/filterLayer/alpha").InnerText = colorFilterAlpha.ToString();
    
    
    //	xmlDoc.Save(xmlCompletePath);
    //if(PIPars.Debug) Debug.Log("Saving on XML... DONE!");
    }


}
