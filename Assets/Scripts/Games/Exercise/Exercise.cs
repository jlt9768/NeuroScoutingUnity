﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Exercise : GameBase{

    const string INSTRUCTIONS = "<color=black>Press <color=cyan>Spacebar</color> as soon as you see the square but don't press space if the square is <color=red>red</color></color>";
    const string FINISHED = "FINISHED!";
    const string RESPONSE_GUESS = "No Guessing!";
    const string RESPONSE_CORRECT = "Good!";
    const string RESPONSE_TIMEOUT = "Missed it!";
    const string RESPONSE_SLOW = "Too Slow!";
    const string RESPONSE_RED = "Stimulus was red!";
    Color RESPONSE_COLOR_GOOD = Color.green;
    Color RESPONSE_COLOR_BAD = Color.red;
    //// Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}
    /// <summary>
	/// A reference to the UI canvas so we can instantiate the feedback text.
	/// </summary>
	public GameObject uiCanvas;
    /// <summary>
    /// The object that will be displayed briefly to the player.
    /// </summary>
    public GameObject stimulus;
    /// <summary>
    /// A prefab for an animated text label that appears when a trial fails/succeeds.
    /// </summary>
    public GameObject feedbackTextPrefab;
    /// <summary>
    /// The instructions text label.
    /// </summary>
    public Text instructionsText;


    /// <summary>
    /// Called when the game session has started.
    /// </summary>
    public override GameBase StartSession(TextAsset sessionFile)
    {
        base.StartSession(sessionFile);

        instructionsText.text = INSTRUCTIONS;
        StartCoroutine(RunTrials(SessionData));

        return this;
    }


    /// <summary>
    /// Iterates through all the trials, and calls the appropriate Start/End/Finished events.
    /// </summary>
    protected virtual IEnumerator RunTrials(SessionData data)
    {
        foreach (Trial t in data.trials)
        {
            StartTrial(t);
            yield return StartCoroutine(DisplayStimulus(t));
            EndTrial(t);
        }
        FinishedSession();
        yield break;
    }


    /// <summary>
    /// Displays the Stimulus for a specified duration.
    /// During that duration the player needs to respond as quickly as possible.
    /// </summary>
    protected virtual IEnumerator DisplayStimulus(Trial t)
    {
        GameObject stim = stimulus;
        stim.SetActive(false);
        
        yield return new WaitForSeconds(t.delay);

        ExerciseTrial eT = (ExerciseTrial)t;

        StartInput();

        if (eT.IsRed)
        {
            stim.GetComponent<Image>().color = Color.red;
        }
        else
        {
            stim.GetComponent<Image>().color = Color.white;
        }
        stim.SetActive(true);

        
        //Checks to see if the trial is random if so generates a random value within the screen space. If it is not random
        //Uses a predefined position from the session file
        if (eT.IsRandom)
        {
            eT.X = Random.Range(eT.MinX, eT.MaxX);
            eT.Y = Random.Range(eT.MinY, eT.MaxY);
            stim.GetComponent<RectTransform>().localPosition = new Vector3(eT.X, eT.Y, 0);
        }
        else
        {
            stim.GetComponent<RectTransform>().localPosition  = new Vector3(eT.X, eT.Y, 0);
        }


        yield return new WaitForSeconds(((ExerciseTrial)t).duration);
        stim.SetActive(false);
        EndInput();

        yield break;
    }


    /// <summary>
    /// Called when the game session is finished.
    /// e.g. All session trials have been completed.
    /// </summary>
    protected override void FinishedSession()
    {
        base.FinishedSession();
        instructionsText.text = FINISHED;
    }


    /// <summary>
    /// Called when the player makes a response during a Trial.
    /// StartInput needs to be called for this to execute, or override the function.
    /// </summary>
    public override void PlayerResponded(KeyCode key, float time)
    {
        if (!listenForInput)
        {
            return;
        }
        base.PlayerResponded(key, time);
        if (key == KeyCode.Space)
        {
            EndInput();
            AddResult(CurrentTrial, time);
        }
    }


    /// <summary>
    /// Adds a result to the SessionData for the given trial.
    /// </summary>
    protected override void AddResult(Trial t, float time)
    {
        TrialResult r = new TrialResult(t);
        r.responseTime = time;
        ExerciseTrial eT = (ExerciseTrial)t;
        if (!eT.IsRed)
        {
            if (time == 0)
            {
                // No response.
                DisplayFeedback(RESPONSE_TIMEOUT, RESPONSE_COLOR_BAD);
                GUILog.Log("Fail! No response!");
            }
            else
            {
                if (IsGuessResponse(time))
                {
                    // Responded before the guess limit, aka guessed.
                    DisplayFeedback(RESPONSE_GUESS, RESPONSE_COLOR_BAD);
                    GUILog.Log("Fail! Guess response! responseTime = {0}", time);
                }
                else if (IsValidResponse(time))
                {
                    // Responded correctly.
                    DisplayFeedback(RESPONSE_CORRECT, RESPONSE_COLOR_GOOD);
                    r.success = true;
                    r.accuracy = GetAccuracy(t, time);
                    GUILog.Log("Success! responseTime = {0}", time);
                }
                else
                {
                    // Responded too slow.
                    DisplayFeedback(RESPONSE_SLOW, RESPONSE_COLOR_BAD);
                    GUILog.Log("Fail! Slow response! responseTime = {0}", time);
                }
            }
        }
        else
        {
            if (time == 0)
            {
                // No response.
                DisplayFeedback(RESPONSE_CORRECT, RESPONSE_COLOR_GOOD);
                r.success = true;
                GUILog.Log("Success! stimulus was red and no button was pressed");
            }
            else
            {
                if (IsGuessResponse(time))
                {
                    // Responded before the guess limit, aka guessed.
                    DisplayFeedback(RESPONSE_GUESS, RESPONSE_COLOR_BAD);
                    GUILog.Log("Fail! Guess response!", time);
                }
                else if (IsValidResponse(time))
                {
                    // Responded correctly but stimulus was red.
                    DisplayFeedback(RESPONSE_RED, RESPONSE_COLOR_BAD);
                    GUILog.Log("Fail! Stimulus was red.", time);
                }
            }
        }       
        sessionData.results.Add(r);
    }


    /// <summary>
    /// Display visual feedback on whether the trial has been responded to correctly or incorrectly.
    /// </summary>
    private void DisplayFeedback(string text, Color color)
    {
        GameObject g = Instantiate(feedbackTextPrefab);
        g.transform.SetParent(uiCanvas.transform);
        g.transform.localPosition = feedbackTextPrefab.transform.localPosition;
        Text t = g.GetComponent<Text>();
        t.text = text;
        t.color = color;
    }


    /// <summary>
    /// Returns the players response accuracy.
    /// The perfect accuracy would be 1, most inaccuracy is 0.
    /// </summary>
    protected float GetAccuracy(Trial t, float time)
    {
        ExerciseData data = sessionData.gameData as ExerciseData;
        bool hasResponseTimeLimit = data.ResponseTimeLimit > 0;

        float rTime = time - data.GuessTimeLimit;
        float totalTimeWindow = hasResponseTimeLimit ?
            data.ResponseTimeLimit : (t as ExerciseTrial).duration;

        return 1f - (rTime / (totalTimeWindow - data.GuessTimeLimit));
    }


    /// <summary>
    /// Returns True if the given response time is considered a guess.
    /// </summary>
    protected bool IsGuessResponse(float time)
    {
        ExerciseData data = sessionData.gameData as ExerciseData;
        return data.GuessTimeLimit > 0 && time < data.GuessTimeLimit;
    }


    /// <summary>
    /// Returns True if the given response time is considered valid.
    /// </summary>
    protected bool IsValidResponse(float time)
    {
        ExerciseData data = sessionData.gameData as ExerciseData;
        return data.ResponseTimeLimit <= 0 || time < data.ResponseTimeLimit;
    }
}
