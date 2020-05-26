using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewsPanelController : MonoBehaviour
{
    public PanelController CallPanelController;
    public GameObject NewsScrollPanel;
    public GameObject NewsCarrier;
    public GameObject FunctionItem;
    public GameObject NewsItem;
    TimeManager CallTimeManager;
    NotificationManager CallNotificationManager;
    List<NotificationManager.News> NewsList = new List<NotificationManager.News>();
    GameObject TimeFunctionButtonObject, TimeTextCarrier, TimeDateTextObject, TimeHourTextObject, TimeSpeedButtonCarrier, TimePauseButtonObject, TimePlayButtonObject, TimeMultiplyButtonObject, TimeMoreMultiplyButtonObject,
    LatestNewsTypeIconImageCarrier, LatestNewsTextCarrier, LatestNewsTextObject, NewsExpandButtonObject;
    public bool isExpanded;
    int NewsDisplayTimeLimit;

    void Awake()
    {
        CallTimeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        CallNotificationManager = GameObject.Find("NotificationManager").GetComponent<NotificationManager>();

        TimeFunctionButtonObject = FunctionItem.transform.GetChild(0).gameObject;
        TimeTextCarrier = TimeFunctionButtonObject.transform.GetChild(0).gameObject;
        TimeDateTextObject = TimeTextCarrier.transform.GetChild(0).gameObject;
        TimeHourTextObject = TimeTextCarrier.transform.GetChild(1).gameObject;
        TimeSpeedButtonCarrier = TimeFunctionButtonObject.transform.GetChild(1).gameObject;
        TimePauseButtonObject = TimeSpeedButtonCarrier.transform.GetChild(0).gameObject;
        TimePlayButtonObject = TimeSpeedButtonCarrier.transform.GetChild(1).gameObject;
        TimeMultiplyButtonObject = TimeSpeedButtonCarrier.transform.GetChild(2).gameObject;
        TimeMoreMultiplyButtonObject = TimeSpeedButtonCarrier.transform.GetChild(3).gameObject;
        LatestNewsTypeIconImageCarrier = FunctionItem.transform.GetChild(1).gameObject;
        LatestNewsTextCarrier = FunctionItem.transform.GetChild(2).gameObject;
        LatestNewsTextObject = FunctionItem.transform.GetChild(2).GetChild(0).gameObject;
        NewsExpandButtonObject = FunctionItem.transform.GetChild(3).GetChild(0).gameObject;

        isExpanded = false;
        NewsDisplayTimeLimit = 0;
    }

    public void Scaling()
    {
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 15.4f, CallPanelController.CurrentUIsize);
        transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentEdgePadding);

        NewsScrollPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 15.4f, CallPanelController.CurrentUIsize);
        NewsScrollPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        NewsScrollPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(- CallPanelController.CurrentEdgePadding, 0);
        NewsScrollPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentEdgePadding, 0);

        NewsCarrier.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 15f, 0);

        NewsCarrier.transform.GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 3f, CallPanelController.CurrentUIsize);
        NewsCarrier.transform.GetChild(0).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize, CallPanelController.CurrentUIsize);
        NewsCarrier.transform.GetChild(0).GetChild(3).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize, CallPanelController.CurrentUIsize);

        NewsCarrier.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize);
        NewsCarrier.transform.GetChild(1).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize, 0);
        NewsCarrier.transform.GetChild(1).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 10f, 0);
        NewsCarrier.transform.GetChild(1).GetChild(2).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize, 0);

        NewsCarrier.GetComponent<VerticalLayoutGroup>().spacing = CallPanelController.CurrentEdgePadding * 0.5f;

        CallPanelController.FontScaling(gameObject);
    }

    void FixedUpdate()
    {
        if(NewsDisplayTimeLimit > 1) NewsDisplayTimeLimit--;
        else if(NewsDisplayTimeLimit == 1) HideLatestNews();
    }

    public void TimeFunctionButtonSelect()
    {
        if(TimeTextCarrier.activeSelf)
        {
            TimeTextCarrier.SetActive(false);
            TimeSpeedButtonCarrier.SetActive(true);
        }
        else
        {
            TimeTextCarrier.SetActive(true);
            TimeSpeedButtonCarrier.SetActive(false);
        }
    }

    public void TimeSpeedSelect(GameObject SelectedObject)
    {
        switch(SelectedObject.transform.GetSiblingIndex())
        {
            case 0 :
                CallTimeManager.Pause();
                break;
            case 1 :
                CallTimeManager.MultiplyTimeSpeed(1);
                break;
            case 2 :
                CallTimeManager.MultiplyTimeSpeed(2);
                break;
            case 3 :
                CallTimeManager.MultiplyTimeSpeed(4);
                break;
        }

        TimeFunctionButtonSelect();
    }

    public void UpdateTimeText()
    {
        TimeDateTextObject.GetComponent<Text>().text = CallTimeManager.GetTimeString(CallTimeManager.TimeValue, "Long");
        TimeHourTextObject.GetComponent<Text>().text = CallTimeManager.GetHourTypeString();
    }

    public void UpdateNews()
    {
        NewsList.Clear();

        NewsList.AddRange(CallNotificationManager.NewsList);

        if(isExpanded) DisplayExpandedNewsPanel();
        else DisplayLatestNews();
    }

    void DisplayLatestNews()
    {
        if(NewsList.Count == 0) return;

        Sprite TypeIcon = null;
        switch(NewsList[NewsList.Count - 1].Type)
        {
            case "Attention" :
                TypeIcon = Resources.Load<Sprite>("GameSystem/InGameUI/Sprite/AttentionIcon");
                break;
            case "Info" :
                TypeIcon = Resources.Load<Sprite>("GameSystem/InGameUI/Sprite/InfoIcon");
                break;
            case "Award" :
                TypeIcon = Resources.Load<Sprite>("GameSystem/InGameUI/Sprite/AwardIcon");
                break;
        }

        LatestNewsTypeIconImageCarrier.SetActive(true);
        LatestNewsTypeIconImageCarrier.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = TypeIcon;
        LatestNewsTextObject.GetComponent<Text>().text = NewsList[NewsList.Count - 1].Content;

        float TextPanelSize = LatestNewsTextObject.GetComponent<Text>().text.Length * 9 * CallPanelController.UIscale;
        if(TextPanelSize > CallPanelController.CurrentUIsize * 10f) TextPanelSize = CallPanelController.CurrentUIsize * 10f;

        LatestNewsTextCarrier.GetComponent<RectTransform>().sizeDelta = new Vector2(TextPanelSize, CallPanelController.CurrentUIsize);

        Canvas.ForceUpdateCanvases();
        CallPanelController.ContentSizeFitterReseter(FunctionItem);
        CallPanelController.ContentSizeFitterReseter(NewsCarrier);

        NewsDisplayTimeLimit = 300;

        // CallPanelController.ContentSizeFitterReseter(LatestNewsTextCarrier);
        // Canvas.ForceUpdateCanvases();
    }

    void HideLatestNews()
    {
        LatestNewsTypeIconImageCarrier.SetActive(false);
        LatestNewsTextObject.GetComponent<Text>().text = "";

        LatestNewsTextCarrier.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize);

        NewsDisplayTimeLimit = 0;

        Canvas.ForceUpdateCanvases();
        CallPanelController.ContentSizeFitterReseter(FunctionItem);
        CallPanelController.ContentSizeFitterReseter(NewsCarrier);
    }

    public void NewsExpandSelect()
    {
        if(NewsList.Count > 0 && !isExpanded)
        {
            if(NewsDisplayTimeLimit > 1) HideLatestNews();

            DisplayExpandedNewsPanel();

            isExpanded = true;
        }
        else
        {
            for(int i = NewsCarrier.transform.childCount - 1; i >= 1; i--)
            {
                NewsCarrier.transform.GetChild(i).gameObject.SetActive(false);
            }
            
            isExpanded = false;

            UpdateNewsPanelSize();
        }
    }

    void DisplayExpandedNewsPanel()
    {
        if(NewsList.Count == 0)
        {
            NewsExpandSelect();
            return;
        }

        if(NewsCarrier.transform.childCount > NewsList.Count + 1)
        {
            for(int i = NewsCarrier.transform.childCount - 1; i >= NewsList.Count + 1; i--)
            {
                Destroy(NewsCarrier.transform.GetChild(i).gameObject);
            }
        }
        else
        {
            for(int i = NewsCarrier.transform.childCount; i < NewsList.Count + 1; i++)
            {
                GameObject.Instantiate(NewsCarrier.transform.GetChild(1).gameObject, NewsCarrier.transform);
            }
        }

        for(int i = 1; i < NewsCarrier.transform.childCount; i++) NewsCarrier.transform.GetChild(i).gameObject.SetActive(false);

        for(int i = 1; i < NewsList.Count + 1; i++)
        {
            NewsCarrier.transform.GetChild(i).gameObject.SetActive(true);
            Sprite TypeIcon = null;
            switch(NewsList[NewsList.Count - i].Type)
            {
                case "Attention" :
                    TypeIcon = Resources.Load<Sprite>("GameSystem/InGameUI/Sprite/AttentionIcon");
                    break;
                case "Info" :
                    TypeIcon = Resources.Load<Sprite>("GameSystem/InGameUI/Sprite/InfoIcon");
                    break;
                case "Award" :
                    TypeIcon = Resources.Load<Sprite>("GameSystem/InGameUI/Sprite/AwardIcon");
                    break;
            }
            NewsCarrier.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = TypeIcon;
            NewsCarrier.transform.GetChild(i).GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = NewsList[NewsList.Count - i].Content;
        }

        UpdateNewsPanelSize();
    }

    public void RemoveNews(GameObject TargetObject)
    {
        CallNotificationManager.RemoveNews(NewsCarrier.transform.childCount - (TargetObject.transform.GetSiblingIndex() + 1));
    }

    void UpdateNewsPanelSize()
    {
        RectTransform NewsScrollRect = NewsScrollPanel.GetComponent<RectTransform>();

        Canvas.ForceUpdateCanvases();
        CallPanelController.ContentSizeFitterReseter(NewsCarrier);

        float NewsPanelSize = NewsCarrier.GetComponent<RectTransform>().rect.height;
        if(NewsPanelSize > Screen.height - CallPanelController.CurrentUIsize - CallPanelController.CurrentEdgePadding) NewsPanelSize = Screen.height - CallPanelController.CurrentUIsize - CallPanelController.CurrentEdgePadding;

        NewsScrollRect.sizeDelta = new Vector2(NewsScrollRect.rect.width, NewsPanelSize);
    }
}