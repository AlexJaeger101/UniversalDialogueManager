using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class DialogueManager : MonoBehaviour
{
    [Header("Text Data")]
    public string mTxtFilePath = "Assets/Resources/txtFiles/";
    public string mTxtFileName = "demoTextFile.txt";
    public List<string> mTextList;

    [Header("UI Data")]
    public Text UIDialougeText;
    
    

    private void Start()
    {
        mTextList = new List<string>();
        ReadTxtFile(mTxtFileName);
    }

    private void Update()
    {

    }

    //For future plans: Add a way write text to a file dynamically when the game is coming
    //This is a stretch goal for now
    void WriteToTxtFile(string newText, string txtFile)
    {
        string filePath = mTxtFilePath + txtFile;
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
            TextAsset txtAsset = (TextAsset)Resources.Load(mTxtFileName);
        }
        catch(Exception ex)
        {
            Debug.LogError("Error: Failed to write to file " + ex.ToString());
        }
    }


    void ReadTxtFile(string txtFile)
    {
        string filePath = mTxtFilePath + txtFile;
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
                        mTextList.Add(currentLine);
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
