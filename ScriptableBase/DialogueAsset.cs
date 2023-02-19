using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueAsset", menuName = "Assets/DialogueAsset")]
public class DialogueAsset : ScriptableObject
{
    [SerializeField] private TextAsset textFile;

    [SerializeField] private List<FileLanguageText> languages = new List<FileLanguageText>(); 

    public List<string> allValidyFileLines;

    private string[] allFileLines;

    private string[] mainTags= AllTags.mainTags;

    private string[] eventsTag= AllTags.eventsTag;

    private string[] optionsTag= AllTags.optionsTag;

    public void SplitFile()
    {
        allValidyFileLines.Clear();
        allValidyFileLines.Capacity = 0;

        languages.Clear();
        languages.Capacity = 0;


        allFileLines = textFile.text.Split('\n');

        foreach (string s in allFileLines)
        {
            bool haveMainTag = false;
            foreach (string tags1 in mainTags)
            {
                if (s.Contains(tags1))
                {
                    allValidyFileLines.Add(s);
                    haveMainTag = true;
                    break;
                }
            }

            if (haveMainTag == false)
            {
                foreach (string tags3 in eventsTag)
                {
                    if (s.Contains(tags3))
                    {
                        allValidyFileLines.Add(s);
                        break;
                    }
                }
            }
        }

        SeparateLanguages();
    }

    private void SeparateLanguages()
    {
        int startIndex = 0;
        int languagesAmount = allValidyFileLines.FindAll(x => x.Contains(mainTags[0])).Count;

        for(int i = 0; i < languagesAmount; i++)
        {
            int index = allValidyFileLines.FindIndex(startIndex, x => x.Contains(mainTags[0]));

            string id = allValidyFileLines[index].Substring(mainTags[0].Length);

            int fileEnd = allValidyFileLines.FindIndex(startIndex, x => x.Contains(mainTags[1]));

          
            FileLanguageText fileLenguageText = new FileLanguageText();
            fileLenguageText.languageID = id;
            fileLenguageText.textLines = GetFileContent(startIndex, fileEnd);

            Debug.Log(id);
            Debug.Log(fileEnd);

            languages.Add(fileLenguageText);

            startIndex = fileEnd+1;
        }
    }

    private List<string> GetFileContent(int start, int end)
    {
        List<string> fileText = new List<string>();

        for (int s = start; s < end + 1; s++)
        {
            fileText.Add(allValidyFileLines[s]);
        }

        return fileText;
    }

    public List<FileLanguageText> GetFileLenguageTexts()
    {
        List<FileLanguageText> reference = new List<FileLanguageText>();

        foreach(FileLanguageText f in languages)
        {
            reference.Add(f);
        }

        return reference;
    }

    public void OnValidate()
    {

    }
}

[System.Serializable]
public struct FileLanguageText
{
    public string languageID;
    public List<string> textLines;
}


