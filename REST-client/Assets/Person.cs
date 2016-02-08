/* File Person C# implementation of class Person */



// global declaration start


using UnityEngine;

// global declaration end

public class Person
{
// class declaration start
public enum Type {
	Patient,
	Doctor
};
// class declaration end


    protected string _name;
    protected int _age;
    protected string _ID;
    protected string _photo;
    protected Type _type;
    protected bool _selected = false;


    public string name
    {
      set; get;
    }


    public int age
    {
      set; get;
    }


    public string ID
    {
      set; get;
    }


    public string photo
    {
      set; get;
    }


    public Type type
    {
      set; get;
    }


    public bool selected
    {
      set
      {
        _selected = value; 
      }
      get
      {
        return _selected;
      }
    }


}
