
using UnityEngine;

public class AddTextPageAction : InteractionAction
{
    [SerializeField] private SetTextScreen textScreen;

    [SerializeField] private TextPage[] pagesToAdd;

    public override void ExecuteAction()
    {
        if (textScreen == null)
        {
            Debug.LogError("None text screen");
            return;
        }

        foreach(TextPage t in pagesToAdd)
        {
            textScreen.AddNewPage(t,true);
        }
    }
}
