
using UnityEngine;

public class ChangeSpriteAction : InteractionAction
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite sprite;

    public override void ExecuteAction()
    {
        if (spriteRenderer == null)
            return;

        spriteRenderer.sprite = sprite;
    }

   
}
