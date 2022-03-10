using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Dialogue test;


    [Header("Text Data")]
    public string mTxtFilePath = "Assets/Resources/txtFiles/";
    private Queue<string> mDialogueQueue;
    private Dialogue mCurrentDialogue;
    private float mCurrentTextSpeed;
    public float mPauseTime = 1.0f;
    public float mDefualtTextScroll = 0.015f;
    public float mFastTextScroll = 0.0015f;
    public float mSlowTextScroll = 0.15f;
    private bool mShouldType = false;

    [Header("UI Data")]
    public Text mUIDialougeText;
    public Canvas mDialougeUI;

    [Header("Misc")]
    public AudioManager mAM;
    
    private void Start()
    {
        mDialogueQueue = new Queue<string>();
        mCurrentTextSpeed = mDefualtTextScroll;
        mDialougeUI.enabled = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartDialogue(test);
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        mCurrentDialogue = dialogue;
        mDialougeUI.enabled = true;

        //Read the dialouge from the text file
        List<string> lines = ReadTxtFile(mCurrentDialogue);
        if(lines.Count < 1)
        {
            return;
        }

        //Add lines to the queue
        mDialogueQueue.Clear();
        foreach(string line in lines)
        {
            mDialogueQueue.Enqueue(line);
        }

        DisplayNextLine();
    }


    public void DisplayNextLine()
    {
        if (mDialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        string newLine = mDialogueQueue.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeLine(newLine, mCurrentDialogue.mSoundFontNoises));
    }

    public void EndDialogue()
    {
        mDialougeUI.enabled = false;
        mUIDialougeText.text = string.Empty;
        mCurrentDialogue = null;
    }

    private IEnumerator TypeLine(string line, AudioClip[] clips)
    {
        if (clips.Length > 0)
        {
            mAM.SpeakingOnLoop(clips);
        }

        mUIDialougeText.text = string.Empty;
        foreach(char letter in line.ToCharArray())
        {
            if (letter == '[')
            {
                mShouldType = true;
            }
            else if (letter == ']')
            {
                mShouldType = false;
            }
            else
            {
                DialogueController(letter);
            }

            yield return new WaitForSeconds(mCurrentTextSpeed);
        }

        mAM.mIsPlaying = false;
    }

    private void DialogueController(char letter)
    {
        if (mShouldType)
        {
            mUIDialougeText.text += letter;
            return;
        }

        switch (letter)
        {
            case '_': //Pause

                mCurrentTextSpeed = mPauseTime;
                mAM.mIsPlaying = false;
                break;

            case '-': //Slow

                mCurrentTextSpeed = mSlowTextScroll;
                break;

            case '+': //Fast

                mCurrentTextSpeed = mFastTextScroll;
                break;

            case '=': //defualt speed

                mAM.mIsPlaying = true;
                mCurrentTextSpeed = mDefualtTextScroll;
                break;

            default:

                Debug.Log("Invalid symbol");
                break;
        }
    }


    private List<string> ReadTxtFile(Dialogue dialogue)
    {
        List<string> stringList = new List<string>();
        string filePath = mTxtFilePath + dialogue.mTextFileName;
        try
        {
            if (!File.Exists(filePath))
            {
                Debug.LogError("ERROR: Missing file at this location! Is your filename and/or path correct?");
                return stringList;
            }

            //Read the text from directly from the txt file
            using (StreamReader txtReader = new StreamReader(filePath))
            {
                while (txtReader.Peek() >= 0)
                {
                    string currentLine = txtReader.ReadLine();
                    if (currentLine != "")
                    {
                        stringList.Add(currentLine);
                    }
                }
            }
        }
        catch(Exception ex)
        {
            Debug.LogError("ERROR: Failed to read file " + ex.ToString());
        }

        return stringList;
    }
}
