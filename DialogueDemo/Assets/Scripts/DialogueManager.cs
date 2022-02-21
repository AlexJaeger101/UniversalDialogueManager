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

    [Header("UI Data")]
    public Text mUIDialougeText;
    public float mTextScrollSpeed = 0.015f;
    
    private void Start()
    {
        mDialogueQueue = new Queue<string>();
        StartDialogue(test);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        //Read the dialouge from the text file
        ReadTxtFile(dialogue);
        if(dialogue.mDialougeLines.Count < 1)
        {
            return;
        }

        //Add lines to the queue
        mDialogueQueue.Clear();
        foreach(string line in dialogue.mDialougeLines)
        {
            mDialogueQueue.Enqueue(line);
        }

        DisplayNexLine();
    }

    public void DisplayNexLine()
    {
        if (mDialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        string newLine = mDialogueQueue.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeLine(newLine));
    }

    public void EndDialogue()
    {
        mUIDialougeText.text = string.Empty;
    }

    IEnumerator TypeLine(string line)
    {
        mUIDialougeText.text = string.Empty;
        foreach(char letter in line.ToCharArray())
        {
            mUIDialougeText.text += letter;
            yield return new WaitForSeconds(mTextScrollSpeed);
        }
    }


    void ReadTxtFile(Dialogue dialogue)
    {
        string filePath = mTxtFilePath + dialogue.mTextFileName;
        try
        {
            if (!File.Exists(filePath))
            {
                Debug.LogError("ERROR: Missing file at this location! Is your filename and/or path correct?");
                return;
            }

            //Read the text from directly from the txt file
            using (StreamReader txtReader = new StreamReader(filePath))
            {
                while (txtReader.Peek() >= 0)
                {
                    string currentLine = txtReader.ReadLine();
                    if (currentLine != "")
                    {
                        dialogue.mDialougeLines.Add(currentLine);
                    }
                }
            }
        }
        catch(Exception ex)
        {
            Debug.LogError("ERROR: Failed to read file " + ex.ToString());
        }
    }
}
