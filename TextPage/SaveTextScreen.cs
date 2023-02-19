
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;

public static class SaveTextScreen 
{
    public static void SaveTextPages(TextPage[] textPages)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/TextPages.dr";

        FileStream stream = new FileStream(path, FileMode.Create);

        TextPagesName data = new TextPagesName();

        data.pages = new List<string>();

        for(int i =0; i< textPages.Length; i++)
        {
            data.pages.Add(textPages[i].name);
        }

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void SaveTextPages(TextPage[] textPages, string file)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/"+file;

        Debug.Log("Saved On: " + Application.persistentDataPath + "/" + file);

        FileStream stream = new FileStream(path, FileMode.Create);

        TextPagesName data = new TextPagesName();

        data.pages = new List<string>();

        for (int i = 0; i < textPages.Length; i++)
        {
            data.pages.Add(textPages[i].name);
        }

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static List<TextPage> LoadTextPages(string file,string folder)
    {
        string path = Application.persistentDataPath + "/" + file;

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            TextPagesName data = formatter.Deserialize(stream) as TextPagesName;

            List<TextPage> pages = new List<TextPage>();

            for(int i =0; i < data.pages.Count; i++)
            {
                TextPage textPage = Resources.Load<TextPage>(folder + "/" + data.pages[i]);

                pages.Add(textPage);
            }
           
            return pages;
        }
        else
        {
            Debug.Log("NUNO FILE");
            return null;
        }
    }

    public static void DeleteFile(string file)
    {
        string path = Application.persistentDataPath + "/" + file;

        if (File.Exists(path))
            File.Delete(path);
    }
}


[System.Serializable]
public class TextPagesName
{
    public List<string> pages;
}