using System;
using System.Collections.Generic;
using UnityEngine;

public class SetTextScreen : MonoBehaviour
{
    [SerializeField] private List<TextPage> pages = new List<TextPage>();
    [SerializeField] private TextScreen textScreen;

    [Header("Save")]
    [SerializeField] private string fileName = "TextPages.dr";
    [SerializeField] private string textPageFolder;
    [SerializeField] private bool loadOnStart=true;

    public Action OnNewPageAdded;
    public Action OnTextScreenOpen;

    private void Awake()
    {
        if (textScreen == null)
            textScreen = FindObjectOfType<TextScreen>();
    }

    private void Start()
    {
        if (loadOnStart)
            Load();
    }

    public void SetPage()
    {
        textScreen.StartShowPages(pages.ToArray());

        OnTextScreenOpen?.Invoke();
    }

    public void AddNewPage(TextPage page, bool notifyEvent)
    {
        if (PageAlreadyExists(page))
            return;

        pages.Add(page);

        if(notifyEvent)
            OnNewPageAdded?.Invoke();
    }



    private bool PageAlreadyExists(TextPage page)
    {
        foreach(TextPage t in pages)
        {
            if (t == page)
                return true;
        }

        return false;
    }

    public void Save()
    {
        SaveTextScreen.SaveTextPages(pages.ToArray(), fileName);
    }

    public void Load()
    {
        List<TextPage> temp = SaveTextScreen.LoadTextPages(fileName, textPageFolder);

        if (temp == null)
            return;

        foreach(TextPage t in temp)
        {
            AddNewPage(t,false);
        }
    }
}
