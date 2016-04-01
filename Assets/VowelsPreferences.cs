/* File VowelsPreferences C# implementation of class VowelsPreferences */

/*	      Computer Graphics and Vision Group	  */
/*		  Politecnico di Torino			  */
/*							  */
/*  THIS IS UNPUBLISHED PROPRIETARY SOURCE CODE OF POLITO */
/*  The copyright notice above does not evidence any      */
/*  actual or intended publication of such source code.   */


// global declaration start


using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml;
using UnityEngine;

// global declaration end

[Serializable]
public class VowelsPreferences : BgPreferences
{
// class declaration start
public static int vowels_Size = 10;
public static string vowels_Size_Text = "S";
public static Dictionary<string, int> vowelsSizes;
public static bool vowelsSpecialFX = true;
public static float lifespan = 1f;
public static Color[] vowelsColors = new Color[5];
// class declaration end



    public override void LoadXML(string filename)
    {
       base.LoadXML(filename);
       // reads its own parts
    
       vowelsSizes = new Dictionary<string, int>();
       vowelsSizes.Add("S",10); 
       vowelsSizes.Add("M",20); 
       vowelsSizes.Add("L",30);
    
       UpdateSettings();
    
       Debug.Log("VowelsPreferences succesfully loaded!");
    }


    public override void SaveXML(string filename)
    {
    	if(PIPars.Debug) Debug.Log("VowelsPreferences.SaveXML");
    	base.SaveXML(filename);
    
    	xmlDoc.SelectSingleNode("//xml/vowels/size").InnerText = vowels_Size_Text;
    	xmlDoc.SelectSingleNode("//xml/vowels/lifespan").InnerText = lifespan.ToString("0.00");
    	xmlDoc.SelectSingleNode("//xml/vowels/specialFX").InnerText = vowelsSpecialFX.ToString();
    	xmlDoc.SelectSingleNode("//xml/vowels/colors/c01").InnerText = ConvertColorToHex(vowelsColors[0]);
    	xmlDoc.SelectSingleNode("//xml/vowels/colors/c02").InnerText = ConvertColorToHex(vowelsColors[1]);
    	xmlDoc.SelectSingleNode("//xml/vowels/colors/c03").InnerText = ConvertColorToHex(vowelsColors[2]);
    	xmlDoc.SelectSingleNode("//xml/vowels/colors/c04").InnerText = ConvertColorToHex(vowelsColors[3]);
    	xmlDoc.SelectSingleNode("//xml/vowels/colors/c05").InnerText = ConvertColorToHex(vowelsColors[4]);
    
    	xmlDoc.Save(xmlCompletePath);
    	if(PIPars.Debug) Debug.Log("Saving on XML... DONE!");
    }


    public void UpdateSettings()
    {
    //// Load SIZE
       string size = GetNodeFromXML("xml", "vowels", "size");
       if(!string.IsNullOrEmpty(size) && vowelsSizes.ContainsKey(size))
       {
       	int val = 0;
       	vowelsSizes.TryGetValue(size, out val);
       	vowels_Size = val;
       	vowels_Size_Text = size;
       }
       else
       {
       	vowels_Size = 10;
       	vowels_Size_Text = "S";
       }     
    
    //// Load LIFESPAN
       string life = GetNodeFromXML("xml", "vowels", "lifespan");
       if(!string.IsNullOrEmpty(life))
       	lifespan = float.Parse(life);
       else
          lifespan = 1f;
    
    //// Load SPECIALFX
       string fx = GetNodeFromXML("xml", "vowels", "specialFX");
       vowelsSpecialFX = bool.Parse(fx);
    
    //// Load COLORS
       string[] colors = GetNodesFromXML("xml", "vowels", "colors");
       for(int i = 0; i < colors.Length; i++)
       {
    	if(!string.IsNullOrEmpty(colors[i]))
    		vowelsColors[i] = ConvertHextoColor(colors[i], 1f);
       	else
    		vowelsColors[i] = Color.red;
       }
    }


}
