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
//public static HandInteractionManager.ButtonNumber numberOfColors = HandInteractionManager.ButtonNumber.FourColors;
public static int numberOfColors = 4;
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


    /*public override void SaveXML(string filename)
    {
    	base.SaveXML(filename);
    
    	switch(numberOfColors)
    	{
    		case HandInteractionManager.ButtonNumber.FourColors:
    			xmlDoc.SelectSingleNode("//xml/paint/numColors").InnerText = 4.ToString();
    			break;
    		case HandInteractionManager.ButtonNumber.EightColors:
    			xmlDoc.SelectSingleNode("//xml/paint/numColors").InnerText = 8.ToString();
    			break;
    	}
    
    	xmlDoc.SelectSingleNode("//xml/paint/colors/c01").InnerText = ConvertColorToHex(paintColor[0]);
    	xmlDoc.SelectSingleNode("//xml/paint/colors/c02").InnerText = ConvertColorToHex(paintColor[1]);
    	xmlDoc.SelectSingleNode("//xml/paint/colors/c03").InnerText = ConvertColorToHex(paintColor[2]);
    	xmlDoc.SelectSingleNode("//xml/paint/colors/c04").InnerText = ConvertColorToHex(paintColor[3]);
    	xmlDoc.SelectSingleNode("//xml/paint/colors/c05").InnerText = ConvertColorToHex(paintColor[4]);
    	xmlDoc.SelectSingleNode("//xml/paint/colors/c06").InnerText = ConvertColorToHex(paintColor[5]);
    	xmlDoc.SelectSingleNode("//xml/paint/colors/c07").InnerText = ConvertColorToHex(paintColor[6]);
    	xmlDoc.SelectSingleNode("//xml/paint/colors/c08").InnerText = ConvertColorToHex(paintColor[7]);
    
    	xmlDoc.SelectSingleNode("//xml/paint/dimensions/d01").InnerText = paintDim[0].ToString("0.00");
    	xmlDoc.SelectSingleNode("//xml/paint/dimensions/d02").InnerText = paintDim[1].ToString("0.00");
    	xmlDoc.SelectSingleNode("//xml/paint/dimensions/d03").InnerText = paintDim[2].ToString("0.00");
    
    	xmlDoc.SelectSingleNode("//xml/paint/sounds/s01").InnerText = paintSounds[0];
    	xmlDoc.SelectSingleNode("//xml/paint/sounds/s02").InnerText = paintSounds[1];
    	xmlDoc.SelectSingleNode("//xml/paint/sounds/s03").InnerText = paintSounds[2];
    	xmlDoc.SelectSingleNode("//xml/paint/sounds/s04").InnerText = paintSounds[3];
    	xmlDoc.SelectSingleNode("//xml/paint/sounds/s05").InnerText = paintSounds[4];
    	xmlDoc.SelectSingleNode("//xml/paint/sounds/s06").InnerText = paintSounds[5];
    	xmlDoc.SelectSingleNode("//xml/paint/sounds/s07").InnerText = paintSounds[6];
    	xmlDoc.SelectSingleNode("//xml/paint/sounds/s08").InnerText = paintSounds[7];
    
    	xmlDoc.SelectSingleNode("//xml/paint/emojs/e01").InnerText = paintEmojs[0];
    	xmlDoc.SelectSingleNode("//xml/paint/emojs/e02").InnerText = paintEmojs[1];
    	xmlDoc.SelectSingleNode("//xml/paint/emojs/e03").InnerText = paintEmojs[2];
    	xmlDoc.SelectSingleNode("//xml/paint/emojs/e04").InnerText = paintEmojs[3];
    	xmlDoc.SelectSingleNode("//xml/paint/emojs/e05").InnerText = paintEmojs[4];
    	xmlDoc.SelectSingleNode("//xml/paint/emojs/e06").InnerText = paintEmojs[5];
    	xmlDoc.SelectSingleNode("//xml/paint/emojs/e07").InnerText = paintEmojs[6];
    	xmlDoc.SelectSingleNode("//xml/paint/emojs/e08").InnerText = paintEmojs[7];
    
    	xmlDoc.SelectSingleNode("//xml/paint/patientOnly").InnerText = patientOnly.ToString();
    
    	xmlDoc.Save(xmlCompletePath);
    	if(PIPars.Debug) Debug.Log("Saving on XML... DONE!");
    }
    */

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
