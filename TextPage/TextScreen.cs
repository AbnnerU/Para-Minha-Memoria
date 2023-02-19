using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

[System.Serializable] public class OnOpenTextScreen : UnityEvent { }
[System.Serializable] public class OnCloseTextScreen : UnityEvent { }

public class TextScreen : MonoBehaviour
{
    [SerializeField] private bool active = true;

    [SerializeField] private AnchorTypeConfig anchorType;

    [SerializeField] private GameObject background;

    [SerializeField] private GameObject[] allTextsSlot;

    [SerializeField] private GameObject[] allImagesSlot;

    [Header("Buttons")]
    [SerializeField] private GameObject openButton;
    [SerializeField] private GameObject closeButton;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject backButton;

    [Space(3)]
    public OnOpenTextScreen onOpenTextScreen;

    public OnCloseTextScreen onCloseTextScreen;

    private List<TMP_Text> textComponents;
    private List<Image> imageComponents;

    private RectTransform backgroundTransform;
    private Image backgroundImageComponent;

    private List<RectTransform> textTransform;

    private List<RectTransform> imagesTransform;

    private TextPage[] allPages;

    private TextPage currentPage;

    private int currentNumber=0;

    public Action OnOpenTextScrenEvent;
    public Action OnCloseTextScreenEvent;

    private void Awake()
    {
        background.SetActive(false);

        SetComponents();

    }

    public void DisableTextScreen()
    {
        active = false;

        openButton.SetActive(false);
    }

    public void EnableTextScreen()
    {
        active = true;

        openButton.SetActive(true);
    }

    public void StartShowPages(TextPage[] pages)
    {
        if (!active)
            return;

        onOpenTextScreen?.Invoke();

        OnOpenTextScrenEvent?.Invoke();

        background.SetActive(true);

        //inputController.EnableInventoryInputs();      

        allPages = pages;

        ModifyCanvas(0);

        openButton.SetActive(false);
        closeButton.SetActive(true);

        VerifyNextButton();

        backButton.SetActive(false);
    }

    public void Close()
    {
        if (!active)
            return;

        onCloseTextScreen?.Invoke();

        OnCloseTextScreenEvent?.Invoke();

        background.SetActive(false);
       
        allPages = null;

        currentNumber = 0;

        openButton.SetActive(true);

        closeButton.SetActive(false);

        nextButton.SetActive(false);

        backButton.SetActive(false);
    }

    public void NextPage()
    {
        if (currentNumber < allPages.Length-1)
        {
            currentNumber++;
            ModifyCanvas(currentNumber);

            VerifyNextButton();
            VerifyBackButton();
        }
    }

    public void BackPage()
    {
        if (currentNumber > 0)
        {
            currentNumber--;
            ModifyCanvas(currentNumber);

            VerifyNextButton();
            VerifyBackButton();
        }
    }
 
    private void ModifyCanvas(int pageNumber)
    {

        DisableAllSlots();

        currentNumber = pageNumber;

        currentPage = allPages[pageNumber];

        //background
        ChangeBackground();
        //text
        SetTexts();
        //image
        SetImage();


    }

    public void ChangeBackground()
    {
        SetLeft(backgroundTransform, currentPage.BGLeft);
        SetRight(backgroundTransform, currentPage.BGRight);
        SetTop(backgroundTransform, currentPage.BGTop);
        SetBottom(backgroundTransform, currentPage.BGBottom);


        if (currentPage.BgImage != null)
        {
            backgroundImageComponent.sprite = currentPage.BgImage;

            if (currentPage.setNativeSize)
                backgroundImageComponent.SetNativeSize();
        }

        backgroundImageComponent.color = currentPage.BGColor;
    }

    public void SetTexts()
    {
        int textAmount = 0;
        if(currentPage.texts.Length > allTextsSlot.Length)
        {
            Debug.LogWarning("text slots is less than the amount of texts");
            textAmount = allTextsSlot.Length;
        }
        else
        {
            textAmount = currentPage.texts.Length;
        }

        if (textAmount == 0)
            return;

        Vector2 min = Vector2.zero;
        Vector2 max = Vector2.zero;

        for (int i = 0; i < textAmount; i++)
        {
            allTextsSlot[i].SetActive(true);

            RectTransform rt = textTransform[i];
            TextConfig textConfig = currentPage.texts[i];

            //Anchor
            if (textConfig.anchorStretchConfig.useThis)
            {
                ChooseAnchorStretch(textConfig.anchorStretchConfig.anchortype, out min, out max);

                rt.anchorMax = max;
                rt.anchorMin = min;

                //PosY
                rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, textConfig.anchorStretchConfig.posY);

                //Left-Right
                SetRight(rt, textConfig.anchorStretchConfig.right);
                SetLeft(rt, textConfig.anchorStretchConfig.left);

                //Height
                SetSize(rt, rt.sizeDelta.x, textConfig.anchorStretchConfig.height);
            }
            else
            {
                //Anchor
                ChooseAnchor(textConfig.anchorConfig.anchortype, out min, out max);

                rt.anchorMax = max;
                rt.anchorMin = min;

                //Position
                rt.anchoredPosition = textConfig.anchorConfig.position;

                //Size
                SetSize(rt, textConfig.anchorConfig.width, textConfig.anchorConfig.height);
            }

         
            //Font
            textComponents[i].font = textConfig.font;

            //Text
            textComponents[i].text = textConfig.text;

            //FontSize
            textComponents[i].fontSize = textConfig.fontSize;

            //Alignment
            textComponents[i].alignment = textConfig.alignmentOptions;

            //Font Style
            textComponents[i].fontStyle = textConfig.fontStyle;

            //Font Color
            textComponents[i].color = textConfig.color;

            //Spacing
            textComponents[i].lineSpacing = textConfig.lineSpacing;
            textComponents[i].paragraphSpacing = textConfig.paragraphSpacing;
            textComponents[i].characterSpacing = textConfig.characterSpacing;
            textComponents[i].wordSpacing = textConfig.wordSpacing;

        }

        print("Texto feito");
    }

    private void SetImage()
    {
        int textAmount = 0;
        if (currentPage.images.Length > allImagesSlot.Length)
        {
            Debug.LogWarning("text slots is less than the amount of texts");
            textAmount = allImagesSlot.Length;
        }
        else
        {
            textAmount = currentPage.images.Length;
        }

        if (textAmount == 0)
            return;

        Vector2 min = Vector2.zero;
        Vector2 max = Vector2.zero;

        for (int i = 0; i < textAmount; i++)
        {
            allImagesSlot[i].SetActive(true);

            RectTransform rt = imagesTransform[i];
            ImageConfig imageConfig = currentPage.images[i];
            Image currentImage = imageComponents[i];


            //Anchor
            if (imageConfig.anchorStretchConfig.useThis)
            {
                ChooseAnchorStretch(imageConfig.anchorStretchConfig.anchortype, out min, out max);

                rt.anchorMax = max;
                rt.anchorMin = min;

                //PosY
                rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, imageConfig.anchorStretchConfig.posY);

                //Left-Right
                SetRight(rt, imageConfig.anchorStretchConfig.right);
                SetLeft(rt, imageConfig.anchorStretchConfig.left);

                //Height
                if (imageConfig.setNativeSize)
                {
                    currentImage.SetNativeSize();
                }
                else
                {
                    SetSize(rt, rt.sizeDelta.x, imageConfig.anchorStretchConfig.height);
                }
            }
            else
            {
                ChooseAnchor(imageConfig.anchorConfig.anchortype, out min, out max);

                rt.anchorMax = max;
                rt.anchorMin = min;

                //Position
                rt.anchoredPosition = imageConfig.anchorConfig.position;
             
                //Size
                if (imageConfig.setNativeSize)
                {
                    currentImage.SetNativeSize();
                }
                else
                {
                    SetSize(rt, imageConfig.anchorConfig.width, imageConfig.anchorConfig.height);
                }
            }
         
            //Sprite
            currentImage.sprite = imageConfig.sprite;       
        }
        print("Imagem feito");
    }

    public void ChooseAnchorStretch(AnchorTypeStretch type,out Vector2 minValue,out Vector2 maxValue)
    {       
        int index =anchorType.anchorStretchConfigs.FindIndex(x => x.name == type);
        minValue = anchorType.anchorStretchConfigs[index].anchorMin;
        maxValue = anchorType.anchorStretchConfigs[index].anchorMax;
    }

    public void ChooseAnchor(Anchortype type, out Vector2 minValue, out Vector2 maxValue)
    {
        int index = anchorType.anchorConfigs.FindIndex(x => x.name == type);
        minValue = anchorType.anchorConfigs[index].anchorMin;
        maxValue = anchorType.anchorConfigs[index].anchorMax;
    }

    public void SetLeft( RectTransform rt, float left)
    {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }

    public void SetRight( RectTransform rt, float right)
    {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }

    public void SetTop( RectTransform rt, float top)
    {
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    }

    public void SetBottom( RectTransform rt, float bottom)
    {
        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }

    public void SetSize(RectTransform rt, float width, float height)
    {
        rt.sizeDelta = new Vector2(width ,height);
    }

    private void DisableAllSlots()
    {
        foreach(GameObject obj in allImagesSlot)
        {
            obj.SetActive(false);
        }

        foreach(GameObject obj in allTextsSlot)
        {
            obj.SetActive(false);
        }
    }

    private void SetComponents()
    {
        textComponents = new List<TMP_Text>();
        imageComponents = new List<Image>();

        textTransform = new List<RectTransform>();
        imagesTransform = new List<RectTransform>();  

        foreach (GameObject obj in allTextsSlot)
        {
            textComponents.Add(obj.GetComponent<TMP_Text>());

            textTransform.Add(obj.GetComponent<RectTransform>());

        }

   

        foreach (GameObject obj in allImagesSlot)
        {
            imageComponents.Add(obj.GetComponent<Image>());

            imagesTransform.Add(obj.GetComponent<RectTransform>());
        }


        backgroundTransform = background.GetComponent<RectTransform>();
        backgroundImageComponent = background.GetComponent<Image>();

        openButton.SetActive(true);
        closeButton.SetActive(false);
        nextButton.SetActive(false);
        backButton.SetActive(false);
        
    }

    private void VerifyNextButton()
    {
        if (currentNumber + 1 < allPages.Length)
            nextButton.SetActive(true);
        else
            nextButton.SetActive(false);
    }

    private void VerifyBackButton()
    {
        if (currentNumber - 1 >= 0)
            backButton.SetActive(true);
        else
            backButton.SetActive(false);
    }


}
