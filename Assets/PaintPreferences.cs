/* File PaintPreferences C# implementation of class PaintPreferences */



// global declaration start


using UnityEngine;
using System.Xml;
using System;

// global declaration end

[Serializable]
public class PaintPreferences : BgPreferences
{
// class declaration start
public static int numberOfColors = 8;
public static Color[] paintColor = new Color[8]{Color.red,Color.red,Color.red,Color.red,Color.red,Color.red,Color.red,Color.red};
public static float[] paintDim = new float[3]{3f,18f,30f};
public static bool patientOnly = false;
// class declaration end



    public override void LoadXML(string filename)
    {
       base.LoadXML(filename);
       // reads its own parts
       UpdateSettings();
    }


    //public override void SaveXML(string filename)
    //{
    //	base.SaveXML(filename);
    
    //	switch(numberOfColors)
    //	{
    //		case HandInteractionManager.ButtonNumber.FourColors:
    //			xmlDoc.SelectSingleNode("//xml/paint/numColors").InnerText = 4.ToString();
    //			break;
    //		case HandInteractionManager.ButtonNumber.EightColors:
    //			xmlDoc.SelectSingleNode("//xml/paint/numColors").InnerText = 8.ToString();
    //			break;
    //	}
    
    //	xmlDoc.SelectSingleNode("//xml/paint/colors/c01").InnerText = ConvertColorToHex(paintColor[0]);
    //	xmlDoc.SelectSingleNode("//xml/paint/colors/c02").InnerText = ConvertColorToHex(paintColor[1]);
    //	xmlDoc.SelectSingleNode("//xml/paint/colors/c03").InnerText = ConvertColorToHex(paintColor[2]);
    //	xmlDoc.SelectSingleNode("//xml/paint/colors/c04").InnerText = ConvertColorToHex(paintColor[3]);
    //	xmlDoc.SelectSingleNode("//xml/paint/colors/c05").InnerText = ConvertColorToHex(paintColor[4]);
    //	xmlDoc.SelectSingleNode("//xml/paint/colors/c06").InnerText = ConvertColorToHex(paintColor[5]);
    //	xmlDoc.SelectSingleNode("//xml/paint/colors/c07").InnerText = ConvertColorToHex(paintColor[6]);
    //	xmlDoc.SelectSingleNode("//xml/paint/colors/c08").InnerText = ConvertColorToHex(paintColor[7]);
    
    //	xmlDoc.SelectSingleNode("//xml/paint/dimensions/d01").InnerText = paintDim[0].ToString("0.00");
    //	xmlDoc.SelectSingleNode("//xml/paint/dimensions/d02").InnerText = paintDim[1].ToString("0.00");
    //	xmlDoc.SelectSingleNode("//xml/paint/dimensions/d03").InnerText = paintDim[2].ToString("0.00");
    
    
    //	xmlDoc.SelectSingleNode("//xml/paint/patientOnly").InnerText = patientOnly.ToString();
    
    //	xmlDoc.Save(xmlCompletePath);
    //	if(PIPars.Debug) Debug.Log("Saving on XML... DONE!");
    //}


    public void UpdateSettings()
    {
    ////// Reading Paint Settings
    
    	string paintNumColors = GetNodeFromXML("xml", "paint", "numColors");
    	if(!string.IsNullOrEmpty(paintNumColors))
    	{
    		int cols = int.Parse(paintNumColors);
    		if(cols == 8)
    			numberOfColors =8;
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
    
    
    /*	string paintColor01 = GetNodeFromXML("xml/paint", "colors", "c01");
    	if(!string.IsNullOrEmpty(paintColor01))
    		paintColor_1 = ConvertHextoColor(paintColor01, 1f);
    	else
    		paintColor_1 = Color.red;
    
    	string paintColor02 = GetNodeFromXML("xml/paint", "colors", "c02");
    	if(!string.IsNullOrEmpty(paintColor02))
    		paintColor_2 = ConvertHextoColor(paintColor02, 1f);
    	else
    		paintColor_2 = Color.green;
    
    	string paintColor03 = GetNodeFromXML("xml/paint", "colors", "c03");
    	if(!string.IsNullOrEmpty(paintColor03))
    		paintColor_3 = ConvertHextoColor(paintColor03, 1f);
    	else
    		paintColor_3 = Color.cyan;
    
    	string paintColor04 = GetNodeFromXML("xml/paint", "colors", "c04");
    	if(!string.IsNullOrEmpty(paintColor04))
    		paintColor_4 = ConvertHextoColor(paintColor04, 1f);
    	else
    		paintColor_4 = Color.blue;
    
    
    	string paintDim01 = GetNodeFromXML("xml/paint", "dimensions", "d01");
    	paintDim_1 = float.Parse(paintDim01);
    	string paintDim02 = GetNodeFromXML("xml/paint", "dimensions", "d02");
    	paintDim_2 = float.Parse(paintDim02);
    	string paintDim03 = GetNodeFromXML("xml/paint", "dimensions", "d03");
    	paintDim_3 = float.Parse(paintDim03);
    */
    
    	string paintPatientOnly = GetNodeFromXML("xml", "paint", "patientOnly");
    	patientOnly = bool.Parse(paintPatientOnly);
    	
    	if(PIPars.Debug) Debug.Log("PaintPreferences :: UpdateSettings :: DONE!");
    }


}
