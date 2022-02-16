using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DialogueManager : MonoBehaviour
{
    public string mTxtFilePath = "Assets/Resources/txtFiles/";
    public string mTxtFileName = "demoTextFile.txt";
    public List<string> mTextList;

    private void Start()
    {
        mTextList = new List<string>();
        WriteToTxtFile("Hello, I am new here!", mTxtFileName);
        ReadTxtFile(mTxtFileName);
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
                Debug.LogError("ERROR: Missing file at this location!");
                return;
            }

            //Write some text to the txt file
            StreamWriter writer = new StreamWriter(filePath, true);
            writer.WriteLine(newText);
            writer.Close();

            //Re-import the file
            AssetDatabase.ImportAsset(filePath);
            TextAsset txtAsset = (TextAsset)Resources.Load(mTxtFileName);

            //Print the new text from the file
            Debug.Log(txtAsset.text);
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
                Debug.LogError("ERROR: Missing file at this location!");
                return;
            }

            //Read the text from directly from the txt file
            using (StreamReader txtReader = new StreamReader(filePath))
            {
                while (txtReader.Peek() >= 0)
                {
                    mTextList.Add(txtReader.ReadLine());
                    Debug.Log(txtReader.ReadLine());
                }
            }
        }
        catch(Exception ex)
        {
            Debug.LogError("Error: Failed to read file " + ex.ToString());
        }
    }
}
