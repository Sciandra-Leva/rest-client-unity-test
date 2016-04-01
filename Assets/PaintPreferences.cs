/* File PaintPreferences C# implementation of class PaintPreferences */

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
public class PaintPreferences : BgPreferences
{
    // class declaration start
    // public static HandInteractionManager.ButtonNumber numberOfColors = HandInteractionManager.ButtonNumber.FourColors;
public static int numberOfColors; 
public static Color[] paintColor = new Color[8]{Color.red,Color.red,Color.red,Color.red,Color.red,Color.red,Color.red,Color.red};
public static float[] paintDim = new float[3]{3f,18f,30f};
public static bool patientOnly = false;
public static string[] paintSounds = new string[8]{null,null,null,null,null,null,null,null};
public static string[] paintEmojs = new string[8]{null,null,null,null,null,null,null,null};
// class declaration end



    public override void LoadXML(string filename)
    {
       base.LoadXML(filename);
       // reads its own parts
       UpdateSettings();
    }

    public void UpdateSettings()
    {
    ////// Reading Paint Settings
    
    	string paintNumColors = GetNodeFromXML("xml", "paint", "numColors");
    	if(!string.IsNullOrEmpty(paintNumColors))
    	{
    		int cols = int.Parse(paintNumColors);
    		if(cols == 8)
    			numberOfColors = 8;
    		else 
    			numberOfColors = 4;
    	}
    	else
    		numberOfColors = 4;
    
    	string[] colors = GetNodesFromXML("xml", "paint", "colors");
    
    	for(int i = 0; i < colors.Length; i++)
    	{
    		if(!string.IsNullOrEmpty(colors[i]))
    			paintColor[i] = ConvertHextoColor(colors[i], 1f);
    		else
    			paintColor[i] = Color.red;
    	}
    
    	string[] dims = GetNodesFromXML("xml", "paint", "dimensions");
    	for(int i = 0; i < dims.Length; i++)
    	{
    		if(!string.IsNullOrEmpty(dims[i]))
    			paintDim[i] = float.Parse(dims[i]);
    	}
    
    	string[] sounds = GetNodesFromXML("xml", "paint", "sounds");
    	for(int i = 0; i < sounds.Length; i++)
    	{
    		paintSounds[i] = sounds[i];
    	}
    
    	string[] emojs = GetNodesFromXML("xml", "paint", "emojs");
    	for(int i = 0; i < emojs.Length; i++)
    	{
    		paintEmojs[i] = emojs[i];
    	}
    
    	string paintPatientOnly = GetNodeFromXML("xml", "paint", "patientOnly");
    	patientOnly = bool.Parse(paintPatientOnly);
    
    	if(PIPars.Debug) Debug.Log("PaintPreferences :: UpdateSettings :: DONE!");
    }


}
