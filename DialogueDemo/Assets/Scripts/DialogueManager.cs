using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class DialogueManager : MonoBehaviour
{
    public Dialogue test;

    [Header("Text Data")]
    public string mTxtFilePath = "Assets/Resources/txtFiles/";
    private Queue<string> mDialogueQueue;

    [Header("UI Data")]
    public Text UIDialougeText;
    
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
            return;
        }

        string newLine = mDialogueQueue.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeLine(newLine));
    }

    public void EndDialogue()
    {
        UIDialougeText.text = "";
    }

    IEnumerator TypeLine(string line)
    {
        UIDialougeText.text = "";
        foreach(char letter in line.ToCharArray())
        {
            UIDialougeText.text += letter;
            yield return null;
        }
    }

    //For future plans: Add a way write text to a file dynamically when the game is coming
    //This is a stretch goal for now
    void WriteToTxtFile(string newText, Dialogue dialogue)
    {
        string filePath = mTxtFilePath + dialogue.mTextFileName;
        try
        {
            if (!File.Exists(filePath))
            {
                Debug.LogError("ERROR: Missing file at this location! Is your filename and/or path correct?");
                return;
            }

            //Write some text to the txt file
            using (StreamWriter txtWriter = new StreamWriter(filePath))
            {
                txtWriter.WriteLine(newText);
            }

            //Re-import the file
            AssetDatabase.ImportAsset(filePath);
            TextAsset txtAsset = (TextAsset)Resources.Load(dialogue.mTextFileName);
        }
        catch(Exception ex)
        {
            Debug.LogError("Error: Failed to write to file " + ex.ToString());
        }
    }


    void ReadTxtFile(Dialogue dialogue)
    {
        Debug.Log(mTxtFilePath + dialogue.mTextFileName);
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
            Debug.LogError("Error: Failed to read file " + ex.ToString());
        }
    }
}
