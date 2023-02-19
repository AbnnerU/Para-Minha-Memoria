
using UnityEngine;

public class BackgroundImageAction : InteractionAction
{
    [SerializeField] private BackgroundImage backgroundImage;
    [SerializeField] private Sprite sprite01;
    [SerializeField] private Sprite sprite02;
    [SerializeField] private string animationName;

    private void Awake()
    {
        if (backgroundImage == null)
            backgroundImage = FindObjectOfType<BackgroundImage>();

    }

    public override void ExecuteAction()
    {
        backgroundImage.SetSprites(sprite01, sprite02);
        backgroundImage.PlayAnimation(animationName);
    }

  
}
