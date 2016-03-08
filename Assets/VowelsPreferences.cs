/* File VowelsPreferences C# implementation of class VowelsPreferences */



// global declaration start



using System;
using System.Xml;

// global declaration end

[Serializable]
public class VowelsPreferences : BgPreferences
{


    public override void LoadXML(string filename)
    {
       base.LoadXML(filename);
       // reads its own parts
    }


}
