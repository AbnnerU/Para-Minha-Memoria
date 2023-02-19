using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class DisableMovementAction : InteractionAction
{
    private List<IMovementGeneral> allMovementScripts = new List<IMovementGeneral>();
  
    private void Awake()
    {
        var reference = FindObjectsOfType<MonoBehaviour>().OfType<IMovementGeneral>();

        foreach (IMovementGeneral mg in reference)
        {
            allMovementScripts.Add(mg);
        }

    }

    public override void ExecuteAction()
    {
        foreach (IMovementGeneral mg in allMovementScripts)
        {
            mg.Disable();
        }
    }



}
