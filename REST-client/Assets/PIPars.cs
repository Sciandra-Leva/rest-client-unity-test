/* File PIPars C# implementation of class PIPars */



// global declaration start


using UnityEngine;

// global declaration end

class PIPars
{
// class declaration start
public enum Role {
	Patient,
	Operator,
	None
}
// class declaration end



    public static float thresholdForDetectingMovements
    {
      get {return (Screen.width/1920.0f * 21); }
    }


    public static float thresholdForDetectingStrangeMovements
    {
      get { return (Screen.width/1920.0f * 80);}
    }


    public static long hoverClickTime
    {
      get {return 2000; }
    }


    public static float paintMaxStrokeSize
    {
      get {return 30.0f; }
    }


    public static int paintButtonSize
    {
      get {return (int)(Screen.width/1920.0f * 120); }
    }


    public static int paintFloatingVerticalDistance
    {
      // value to set is the last multiplication term
      get {return (int)(Screen.width/1920.0f * 250); }
    }


    public static int paintFloatingHorizontalDistance
    {
      // value to set is the last multiplication term
      get {return (int)(Screen.width/1920.0f * 250); }
    }


    public static int paintFloatingToolbarRay
    {
      // value to set is the last multiplication term
      get {return (int)(Screen.width/1920.0f * 300); }
    }


    public static int paintEraserSize
    {
      get {return (int)(Screen.width/1920.0f * 60); }
    }


    public static int captureFrameRate
    {
      get {return 25; }
    }


    public static bool Debug
    {
      get {return true;}
    }


    public static KeyCode keyEsc
    {
      get{ return KeyCode.Escape; }
    }


    public static KeyCode keyPaintVerticalToolbar
    {
      get{ return KeyCode.F; }
    }


    public static KeyCode keyPaintFloatVertToolbar
    {
      get{ return KeyCode.V; }
    }


    public static KeyCode keyPaintFloatHoriToolbar
    {
      get{ return KeyCode.O; }
    }


    public static KeyCode keyPaintFloatCircToolbar
    {
      get{ return KeyCode.C; }
    }


    public static KeyCode keyPaintEnableToolbar
    {
      get{ return KeyCode.M; }
    }


    public static KeyCode keyVowelsPatientA
    {
      get{ return KeyCode.Z; }
    }


    public static KeyCode keyVowelsPatientE
    {
      get{ return KeyCode.X; }
    }


    public static KeyCode keyVowelsPatientI
    {
      get{ return KeyCode.C; }
    }


    public static KeyCode keyVowelsPatientO
    {
      get{ return KeyCode.V; }
    }


    public static KeyCode keyVowelsPatientU
    {
      get{ return KeyCode.B; }
    }


    public static KeyCode keyVowelsOthersA
    {
      get{ return KeyCode.A; }
    }


    public static KeyCode keyVowelsOthersE
    {
      get{ return KeyCode.S; }
    }


    public static KeyCode keyVowelsOthersI
    {
      get{ return KeyCode.D; }
    }


    public static KeyCode keyVowelsOthersO
    {
      get{ return KeyCode.F; }
    }


    public static KeyCode keyVowelsOthersU
    {
      get{ return KeyCode.G; }
    }


}
