using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class TextScreenIPlayerMovement : MonoBehaviour
{
    [SerializeField] private TextScreen textScreen;

    [SerializeField] private bool disableMovementOnOpen = true;

    [SerializeField] private bool enableMovementOnClose = true;

    private List<IMovementGeneral> allMovementScripts = new List<IMovementGeneral>();

    private bool movementsEnabled = true;

    private void Awake()
    {
        if (textScreen == null)
            textScreen = FindObjectOfType<TextScreen>();

        var reference = FindObjectsOfType<MonoBehaviour>().OfType<IMovementGeneral>();

        foreach (IMovementGeneral mg in reference)
        {
            allMovementScripts.Add(mg);
        }

        textScreen.OnOpenTextScrenEvent += TextScreen_OnOpenTextScreenEvent;
        textScreen.OnCloseTextScreenEvent += TextScreen_OnCloseTextScreenEvent;
    }

    private void TextScreen_OnOpenTextScreenEvent()
    {
        if (movementsEnabled && disableMovementOnOpen)
        {
            foreach (IMovementGeneral mg in allMovementScripts)
            {
                mg.Disable();
            }

            movementsEnabled = false;

        }

    }

    private void TextScreen_OnCloseTextScreenEvent()
    {
        if (enableMovementOnClose)
        {
            foreach (IMovementGeneral mg in allMovementScripts)
            {
                mg.Enable();
            }

            movementsEnabled = true;
        }
    }

}
