using UnityEngine;
using UnityEngine.UI;

public class TextScreenNotification : MonoBehaviour
{
    [SerializeField] private SetTextScreen textScreen;

    [Header("UI")]
    [Space(9)]
    [SerializeField] private Image imageRef;
    [SerializeField] private Sprite defaltSprite;
    [SerializeField] private Sprite onNewPageSprite;


    private void Awake()
    {
        if (textScreen == null)
            return;

        textScreen.OnTextScreenOpen += TextScreem_OnTextScreenOpen;
        textScreen.OnNewPageAdded += TextScreen_OnNewPageAdded;
    }

    private void TextScreen_OnNewPageAdded()
    {
        imageRef.sprite = onNewPageSprite;
    }

    private void TextScreem_OnTextScreenOpen()
    {
        imageRef.sprite = defaltSprite;
    }
}
