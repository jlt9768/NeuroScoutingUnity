using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Linq;

public class ExerciseData : GameData {

    //	// Use this for initialization
    //	void Start () {

    //	}

    //	// Update is called once per frame
    //	void Update () {

    //	}
    const string ATTRIBUTE_GUESS_TIMELIMIT = "guessTimeLimit";
    const string ATTRIBUTE_RESPONSE_TIMELIMIT = "responseTimeLimit";
    public const string ATTRIBUTE_DURATION = "duration";
    public const string ATTRIBUTE_RANDOM = "random";
    public const string ATTRIBUTE_MINX = "minX";
    public const string ATTRIBUTE_MAXX = "maxX";
    public const string ATTRIBUTE_MINY = "minY";
    public const string ATTRIBUTE_MAXY = "maxY";
    /// <summary>
    /// The amount of time that needs to pass before the player can respond without being penalized.
    /// </summary>
    private float guessTimeLimit = 0;
    /// <summary>
    /// The amount of time that the user has to respond; 
    /// Starts when input becomes enabled during a Trial. 
    /// Responses that fall within this time constraint will be marked as Successful.
    /// </summary>
    private float responseTimeLimit = 0;
    /// <summary>
    /// The visibility Duration for the Stimulus.
    /// </summary>
    private float duration = 0;
    private bool random = false;
    private float minX = 0;
    private float maxX = 0;
    private float minY = 0;
    private float maxY = 0;
    #region ACCESSORS

    public float GuessTimeLimit
    {
        get
        {
            return guessTimeLimit;
        }
    }
    public float ResponseTimeLimit
    {
        get
        {
            return responseTimeLimit;
        }
    }
    public float GeneratedDuration
    {
        get
        {
            return duration;
        }
    }
    public bool IsRandom
    {
        get
        {
            return random;
        }
    }
    public float MinX
    {
        get
        {
            return minX;
        }
    }
    public float MaxX
    {
        get
        {
            return maxX;
        }
    }
    public float MinY
    {
        get
        {
            return minY;
        }
    }
    public float MaxY
    {
        get
        {
            return maxY;
        }
    }

    #endregion


    public ExerciseData(XmlElement elem) 
		: base(elem)
	{
    }


    public override void ParseElement(XmlElement elem)
    {
        base.ParseElement(elem);
        XMLUtil.ParseAttribute(elem, ATTRIBUTE_DURATION, ref duration);
        XMLUtil.ParseAttribute(elem, ATTRIBUTE_RESPONSE_TIMELIMIT, ref responseTimeLimit);
        XMLUtil.ParseAttribute(elem, ATTRIBUTE_GUESS_TIMELIMIT, ref guessTimeLimit);
        XMLUtil.ParseAttribute(elem, ATTRIBUTE_RANDOM, ref random);
        XMLUtil.ParseAttribute(elem, ATTRIBUTE_MINX, ref minX);
        XMLUtil.ParseAttribute(elem, ATTRIBUTE_MAXX, ref maxX);
        XMLUtil.ParseAttribute(elem, ATTRIBUTE_MINY, ref minY);
        XMLUtil.ParseAttribute(elem, ATTRIBUTE_MAXY, ref maxY);


        //Debug.Log(duration);
        //XMLUtil.ParseAttribute(elem, ATTRIBUTE_RANDOM, ref random);
    }


    public override void WriteOutputData(ref XElement elem)
    {
        base.WriteOutputData(ref elem);
        XMLUtil.CreateAttribute(ATTRIBUTE_GUESS_TIMELIMIT, guessTimeLimit.ToString(), ref elem);
        XMLUtil.CreateAttribute(ATTRIBUTE_RESPONSE_TIMELIMIT, responseTimeLimit.ToString(), ref elem);
        XMLUtil.CreateAttribute(ATTRIBUTE_DURATION, duration.ToString(), ref elem);
        XMLUtil.CreateAttribute(ATTRIBUTE_RANDOM, random.ToString(), ref elem);
        XMLUtil.CreateAttribute(ATTRIBUTE_MINX, minX.ToString(), ref elem);
        XMLUtil.CreateAttribute(ATTRIBUTE_MAXX, maxX.ToString(), ref elem);
        XMLUtil.CreateAttribute(ATTRIBUTE_MINY, minY.ToString(), ref elem);
        XMLUtil.CreateAttribute(ATTRIBUTE_MAXY, maxY.ToString(), ref elem);

    }
}
