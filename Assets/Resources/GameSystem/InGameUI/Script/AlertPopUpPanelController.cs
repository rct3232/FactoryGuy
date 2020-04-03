using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertPopUpPanelController : MonoBehaviour
{
    public PanelController CallPanelController;
    public bool isInitialized = false;
    [SerializeField]GameObject MainPanel;
    [SerializeField]GameObject FunctionPanel;
    List<NotificationManager.Alert> AlertList = new List<NotificationManager.Alert>();
    NotificationManager.Alert TargetAlert;
    NotificationManager CallNotificationManager;
    TimeManager CallTimeManager;
    GameObject TextBox, PositiveButtonObject, NegativeButtonObject;
    
    void Awake()
    {
        CallNotificationManager = GameObject.Find("NotificationManager").GetComponent<NotificationManager>();
        CallTimeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();

        TextBox = MainPanel.transform.GetChild(0).gameObject;
        PositiveButtonObject = FunctionPanel.transform.GetChild(0).gameObject;
        NegativeButtonObject = FunctionPanel.transform.GetChild(1).gameObject;
    }
    
    public void Scaling()
    {
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 6f, CallPanelController.CurrentUIsize * 3f);

        MainPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 2f);
        FunctionPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize);

        FunctionPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 2, CallPanelController.CurrentUIsize * 0.6f);
        FunctionPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 2, CallPanelController.CurrentUIsize * 0.6f);

        CallPanelController.FontScaling(gameObject);
    }

    public void UpdateAlert(NotificationManager.Alert newAlert)
    {
        AlertList.Add(newAlert);

        if(!gameObject.activeSelf) Initializing();
        
        DisplayPanel();
    }

    public void Initializing()
    {
        gameObject.SetActive(true);
    }

    void DisplayPanel()
    {
        TargetAlert = AlertList[0];

        TextBox.GetComponent<Text>().text = TargetAlert.Content;

        for(int i = 0; i < FunctionPanel.transform.childCount; i++)
        {
            if(i < TargetAlert.ButtonCount) FunctionPanel.transform.GetChild(i).gameObject.SetActive(true);
            else FunctionPanel.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void PositiveButtonSelect()
    {
        TargetAlert.Answer = true;

        ClosePanel();
    }

    public void NegativeButtonSelect()
    {
        TargetAlert.Answer = false;

        ClosePanel();
    }

    void ClosePanel()
    {
        CallNotificationManager.ClearAlert(TargetAlert);

        AlertList.Remove(TargetAlert);

        if(AlertList.Count == 0) gameObject.SetActive(false);
        else DisplayPanel();
    }
}
