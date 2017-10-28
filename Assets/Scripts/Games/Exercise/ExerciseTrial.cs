﻿using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Linq;

public class ExerciseTrial : Trial {

    //// Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}
    /// <summary>
	/// The distance ratio that will be targeted.
	/// </summary>
	/// <summary>
	/// The distance ratio that will be targeted.
	/// </summary>
	public float duration = 0;


    #region ACCESSORS

    public float Duration
    {
        get
        {
            return duration;
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
    }


    /// <summary>
    /// Writes any tracked variables to the given XElement.
    /// </summary>
    public override void WriteOutputData(ref XElement elem)
    {
        base.WriteOutputData(ref elem);
        XMLUtil.CreateAttribute(ExerciseData.ATTRIBUTE_DURATION, duration.ToString(), ref elem);
    }
}