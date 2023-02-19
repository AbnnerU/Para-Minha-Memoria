
using UnityEngine;

public class SlidePuzzleChangeActiveState : InteractionAction
{
    [SerializeField] private SlidePuzzleManager SlidePuzzleManager;
    [SerializeField] private bool setActive;

    public override void ExecuteAction()
    {
        SlidePuzzleManager.SetActive(setActive);
    }
}
