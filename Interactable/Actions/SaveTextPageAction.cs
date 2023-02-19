
using UnityEngine;

public class SaveTextPageAction : InteractionAction
{
    [SerializeField] private SetTextScreen textScreen;

    public override void ExecuteAction()
    {
        if (textScreen == null)
        {
            Debug.LogError("None text screen");
            return;
        }

        textScreen.Save();
    }
}
