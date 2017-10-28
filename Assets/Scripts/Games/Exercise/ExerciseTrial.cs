using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Linq;

public class ExerciseTrial : Trial {

    const string ATTRIBUTE_POSX = "posX";
    const string ATTRIBUTE_POSY = "posY";
    const string ATTRIBUTE_ISRED = "isRed";
    /// <summary>
    /// The distance ratio that will be targeted.
    /// </summary>
    public float duration = 0;
    /// <summary>
    /// Whether or not the stimulus position is random
    /// </summary>
    public bool random = false;

    private float x = 0;
    private float y = 0;
    private bool isRed = false;
    private float minX = 0;
    private float maxX = 0;
    private float minY = 0;
    private float maxY = 0;
    #region ACCESSORS

    public float Duration
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
    public float X
    {
        get
        {
            return x;
        }
        set
        {
            x = value;
        }
    }
    public float Y
    {
        get
        {
            return y;
        }
        set
        {
            y = value;
        }
    }
    public bool IsRed
    {
        get
        {
            return isRed;
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


    public ExerciseTrial(SessionData data, XmlElement n = null) 
		: base(data, n)
	{
    }


    /// <summary>
    /// Parses Game specific variables for this Trial from the given XmlElement.
    /// If no parsable attributes are found, or fail, then it will generate some from the given GameData.
    /// Used when parsing a Trial that IS defined in the Session file.
    /// </summary>
    public override void ParseGameSpecificVars(XmlNode n, SessionData session)
    {
        base.ParseGameSpecificVars(n, session);

        ExerciseData data = (ExerciseData)(session.gameData);
        if (!XMLUtil.ParseAttribute(n, ExerciseData.ATTRIBUTE_DURATION, ref duration, true))
        {
            duration = data.GeneratedDuration;
        }
        if (!XMLUtil.ParseAttribute(n, ExerciseData.ATTRIBUTE_RANDOM, ref random, true))
        {
            random = data.IsRandom;
        }
        //Gets the x and y positions from the trial if they don't exist the default position is 0,0
        if (!XMLUtil.ParseAttribute(n, ATTRIBUTE_POSX, ref x, true))
        {
            x = 0;
        }
        if (!XMLUtil.ParseAttribute(n, ATTRIBUTE_POSY, ref y, true))
        {
            y = 0;
        }

        if (!XMLUtil.ParseAttribute(n, ATTRIBUTE_ISRED, ref isRed, true))
        {
            isRed = false;
        }
        if (random)
        {
            minX = data.MinX;
            maxX = data.MaxX;
            minY = data.MinY;
            maxY = data.MaxY;
        }


    }


    /// <summary>
    /// Writes any tracked variables to the given XElement.
    /// </summary>
    public override void WriteOutputData(ref XElement elem)
    {
        base.WriteOutputData(ref elem);
        XMLUtil.CreateAttribute(ExerciseData.ATTRIBUTE_DURATION, duration.ToString(), ref elem);
        XMLUtil.CreateAttribute(ExerciseData.ATTRIBUTE_RANDOM, random.ToString(), ref elem);
        XMLUtil.CreateAttribute(ATTRIBUTE_POSX, x.ToString(), ref elem);
        XMLUtil.CreateAttribute(ATTRIBUTE_POSY, y.ToString(), ref elem);
        XMLUtil.CreateAttribute(ATTRIBUTE_ISRED, isRed.ToString(), ref elem);
    }
}
