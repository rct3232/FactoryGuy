using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public int TimeValue = 0;
    public int Year;// = 864000 Real 4 Hours 48 Minutes
    public int Month;// = 72000 Real 24 Minutes
    public int Day;// = 2400 Real 48 Seconds
    public int Hour;// = 100 Real 2 Seconds
    public int PlaySpeed = 1;
    List<Vector3> GoodsSpeedArray;
    PanelController PanelControllerCall;
    NewsPanelController NewsPanelControllerCall;

    // Start is called before the first frame update
    void Start()
    {
        PanelControllerCall = GameObject.Find("Canvas").GetComponent<PanelController>();
        NewsPanelControllerCall = PanelControllerCall.NewsPanel.GetComponent<NewsPanelController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TimeTicker();
    }

    void TimeTicker()
    {
        TimeValue += PlaySpeed;

        if(TimeValue % Hour < PlaySpeed)
        {
            NewsPanelControllerCall.UpdateTimeText();
        }
    }

    public int GetYearInt(int Time)
    {
        return Mathf.FloorToInt(Time / Year) + 1;
    }

    public int GetMonthInt(int Time)
    {
        return Mathf.FloorToInt((Time % Year) / Month) + 1;
    }


    public int GetDayInt(int Time)
    {
        return Mathf.FloorToInt((Time % Month) / Day) + 1;
    }

    public int GetHourInt(int Time)
    {
        return Mathf.FloorToInt((Time % Day) / Hour);
    }

    public int GetNextDay(int Time, int Multiple=1)
    {
        if(Time == 0)
        {
            Time = TimeValue;
        }

        return (Time % Day) + (Day * Multiple);
    }

    public int GetNextMonth(int Time, int Multiple=1)
    {
        if(Time == 0)
        {
            Time = TimeValue;
        }
        
        return (Time % Month) + (Month * Multiple);
    }

    public int GetNextYear(int Time, int Multiple=1)
    {
        if(Time == 0)
        {
            Time = TimeValue;
        }
        
        return (Time % Year) + (Year * Multiple);
    }

    public string ConvertMonthToString(int Month)
    {
        string result;
        switch(Month)
        {
            case 1 : result = "Jan"; break; case 2 : result = "Feb"; break;
            case 3 : result = "Mar"; break; case 4 : result = "Apr"; break;
            case 5 : result = "May"; break; case 6 : result = "Jun"; break;
            case 7 : result = "Jul"; break; case 8 : result = "Aug"; break;
            case 9 : result = "Sep"; break; case 10 : result = "Oct"; break;
            case 11 : result = "Nov"; break; case 12 : result = "Dec"; break;
            default : result = "Error"; break;
        }

        return result;
    }

    public string ConvertMonthToString()
    {
        string result;
        switch(GetMonthInt(TimeValue))
        {
            case 1 : result = "Jan"; break; case 2 : result = "Feb"; break;
            case 3 : result = "Mar"; break; case 4 : result = "Apr"; break;
            case 5 : result = "May"; break; case 6 : result = "Jun"; break;
            case 7 : result = "Jul"; break; case 8 : result = "Aug"; break;
            case 9 : result = "Sep"; break; case 10 : result = "Oct"; break;
            case 11 : result = "Nov"; break; case 12 : result = "Dec"; break;
            default : result = "Error"; break;
        }

        return result;
    }

    public string GetHourTypeString(int Hour)
    {
        string result;
        if(Hour < 12)
        {
            result = Hour.ToString() + " AM";
        }
        else if(Hour == 12)
        {
            result = Hour.ToString() + " PM";
        }
        else
        {
            result = (Hour - 12).ToString() + " PM";
        }

        return result;
    }

    public string GetHourTypeString()
    {
        int HourValue = GetHourInt(TimeValue);
        string result;
        if(HourValue < 12)
        {
            result = HourValue.ToString() + " AM";
        }
        else if(HourValue == 12)
        {
            result = HourValue.ToString() + " PM";
        }
        else
        {
            result = (HourValue - 12).ToString() + " PM";
        }

        return result;
    }

    public string GetPeriodString(int Time, string Type)
    {
        string result = "";
        string TimeString = "";

        if(GetYearInt(Time) > 0)
        {
            if(Type == "Short") TimeString = " Y ";
            else TimeString = " Year ";
            result += GetYearInt(Time).ToString() + TimeString;
        }
        if(GetMonthInt(Time) - 1 > 0)
        {
            if(Type == "Short") TimeString = " M ";
            else TimeString = " Month ";
            result += (GetMonthInt(Time) - 1).ToString() + TimeString;
        }

        if(Type == "Short") TimeString = " D";
        else TimeString = " Day";
        result += GetDayInt(Time).ToString() + TimeString;

        return result;
    }

    public string GetTimeString(int Time, string Type)
    {
        string result = "";

        if(Type != "Short") result += "Year " + GetYearInt(Time).ToString() + ", ";
        else result +=  GetYearInt(Time).ToString() + ".";
        

        if(Type != "Short") result += ConvertMonthToString(GetMonthInt(Time)) + " ";
        else result += GetMonthInt(Time).ToString() + ".";

        result += GetDayInt(Time).ToString();

        return result;
    }

    public void Pause()
    {
        PlaySpeed = 0;
        StopAllMovement();
    }

    public void MultiplyTimeSpeed(int MultiplyValue)
    {
        if(PlaySpeed == 0) ResumeAllMovement();
        PlaySpeed = MultiplyValue;
    }

    void StopAllMovement()
    {
        GoodsSpeedArray = new List<Vector3>();
        GameObject GoodsCarrier = GameObject.Find("Goods");

        for(int i = 0; i < GoodsCarrier.transform.childCount; i++)
        {
            GoodsSpeedArray.Add(GoodsCarrier.transform.GetChild(i).gameObject.GetComponent<Rigidbody>().velocity);
            GoodsCarrier.transform.GetChild(i).gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }
    }

    void ResumeAllMovement()
    {
        GameObject GoodsCarrier = GameObject.Find("Goods");

        for(int i = 0; i < GoodsCarrier.transform.childCount; i++)
        {
            GoodsCarrier.transform.GetChild(i).gameObject.GetComponent<Rigidbody>().velocity = GoodsSpeedArray[i];
        }

        GoodsSpeedArray = null;
    }
}
