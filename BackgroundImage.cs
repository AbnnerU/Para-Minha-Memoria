
using UnityEngine;

public class BackgroundImage : MonoBehaviour
{
    [SerializeField] private SpriteRenderer image01;
    [SerializeField] private SpriteRenderer image02;
    [SerializeField] private Animator anim;

    private void Awake()
    {
        if (anim == null)
            anim = GetComponent<Animator>();
    }

    public void SetSprites(Sprite sprite01, Sprite sprite02)
    {
        image01.sprite = sprite01;
        image02.sprite = sprite02;
    }

    public void PlayAnimation(string animationName)
    {
        anim?.Play(animationName,0,0);
    }
}
