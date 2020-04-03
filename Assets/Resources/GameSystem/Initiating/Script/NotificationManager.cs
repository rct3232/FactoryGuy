using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    public class News
    {
        public News() {}
        public string Content;
        public string Type; // Attention, Info, Award
        public int Time;
    }
    public class Alert
    {
        public Alert() {}
        public string Content;
        public int ButtonCount;
        public string RequestComponentName;
        public bool Answer;
    }
    public class NoteInfo
    {
        public NoteInfo() {}
        public string Content = "";
        public Color NoteColor = new Color(1f,1f,1f,1f);
    }
    public List<News> NewsList = new List<News>();
    public NoteInfo Note = new NoteInfo(); 
    bool NoteExist;
    TimeManager TimeManagerCall;
    PanelController PanelControllerCall;
    
    // Start is called before the first frame update
    void Start()
    {
        TimeManagerCall = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        PanelControllerCall = GameObject.Find("Canvas").GetComponent<PanelController>();

        NoteExist = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(TimeManagerCall.TimeValue % TimeManagerCall.Month < TimeManagerCall.PlaySpeed) ManagerNewsList();

        if(NoteExist)
        {
            if(Note.Content == "") NoteExist = false;
            
            PanelControllerCall.NoteToolTipPanel.GetComponent<NoteToolTipPanelController>().DisplayNote();

            Note.Content = "";
        }
    }

    void ManagerNewsList()
    {
        if(NewsList.Count > 50)
        {
            for(int i = NewsList.Count - 1; i >= 50; i--)
            {
                NewsList.RemoveAt(i);
            }
        }

        for(int i = NewsList.Count - 1; i >= 0; i--)
        {
            if(TimeManagerCall.TimeValue - NewsList[i].Time > TimeManagerCall.Month)
            {
                NewsList.RemoveAt(i);
            }
        }

        PanelControllerCall.NewsPanel.GetComponent<NewsPanelController>().UpdateNews();
    }

    public void AddNews(string type, string content)
    {
        News newNews = new News();
        newNews.Type = type;
        newNews.Content = content;
        newNews.Time = TimeManagerCall.TimeValue;

        NewsList.Add(newNews);

        ManagerNewsList();

        PanelControllerCall.NewsPanel.GetComponent<NewsPanelController>().UpdateNews();
    }

    public void RemoveNews(int Index)
    {
        NewsList.RemoveAt(Index);

        PanelControllerCall.NewsPanel.GetComponent<NewsPanelController>().UpdateNews();
    }

    public void AddAlert(string Content, int ButtonCount, string ComponentName)
    {
        Alert newAlert = new Alert();
        newAlert.Content = Content;
        newAlert.ButtonCount = ButtonCount;
        newAlert.RequestComponentName = ComponentName;
        newAlert.Answer = false;

        PanelControllerCall.AlertPopUpPanel.GetComponent<AlertPopUpPanelController>().UpdateAlert(newAlert);
    }

    public void ClearAlert(Alert TargetAlert)
    {
        if(TargetAlert.ButtonCount != 1)
        {

        }
    }

    public void SetNote(string Content, Color noteColor)
    {
        Note.Content = Content;
        Note.NoteColor = noteColor;

        NoteExist = true;
    }

    public void NotiDebug(int Type)
    {
        AddAlert("TESTTESTTESTTESTTESTTESTTEST", Type, "");
    }
}
