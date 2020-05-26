using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanyValue : MonoBehaviour
{
    public string CompanyName;
    public bool isAI;
    public bool Unevaluable = false;
    public bool isCorrupt = false;
    public float TotalValue;
    public List<float> CompanyValueHistory = new List<float>();
    public float ContractReliability = 0.5f;
    TimeManager TimeManagerCall;
    CompanyManager CompanyManagerCall;
    PanelController PanelControllerCall;

    void Start()
    {
        TimeManagerCall = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        CompanyManagerCall = transform.parent.gameObject.GetComponent<CompanyManager>();
        PanelControllerCall = GameObject.Find("Canvas").GetComponent<PanelController>();
        if(!Unevaluable) CalculateCompanyValue();
    }

    void FixedUpdate()
    {
        if(!Unevaluable)
        {
            if(TimeManagerCall.TimeValue % TimeManagerCall.Day < TimeManagerCall.PlaySpeed) CalculateCompanyValue();

            if(TimeManagerCall.TimeValue % TimeManagerCall.Month < TimeManagerCall.PlaySpeed) CompanyValueHistory.Add(TotalValue);
        }
    }

    void CalculateCompanyValue()
    {
        float result = 0f;
        float difference = 0f;

        foreach(var Facility in GetFacilityValue().GetComponent<FacilityValue>().FacilityList)
        {
            result += Facility.ObjectActCall.Info.Price / 100;
        }

        LandValue LandValueCall = GetLandValue().GetComponent<LandValue>();
        for(int i = 0; i < TopValue.TopValueSingleton.MapSize * TopValue.TopValueSingleton.MapSize; i++)
        {
            if(LandValueCall.GetLandStatus(i) == "Factory")
                result += LandValueCall.GetLandValue(i) / 1000;
        }

        result += GetEmployeeValue().GetComponent<EmployeeValue>().EmployeeList.Count;

        if(GameObject.Find("SalesManager").GetComponent<SalesValue>().GetMarketPower(CompanyName) > 0)
        {
            float MarketPower = GameObject.Find("SalesManager").GetComponent<SalesValue>().GetMarketPower(CompanyName);
            result += MarketPower / MarketPower / 10f;
        }

        result += ContractReliability * 10f;

        if(TimeManagerCall.TimeValue > TimeManagerCall.Month * 4 && TimeManagerCall.TimeValue % TimeManagerCall.Month * 4 < TimeManagerCall.PlaySpeed)
        {
            List<EconomyValue.History> HistoryList = new List<EconomyValue.History>();
            for(int i = 0; i < 4; i++)
            {
                int TargetIndex = Mathf.FloorToInt(TimeManagerCall.TimeValue / TimeManagerCall.Month) - i;
                result += GetEconomyValue().GetComponent<EconomyValue>().GetHistorySub(TargetIndex);
            }
        }

        // Add Technology point

        difference = TotalValue - result;
        TotalValue = result;
        
        if(CompanyName == CompanyManagerCall.PlayerCompanyName)
        {
            PanelControllerCall.UpdateFactoryInfo("CompanyValue", result, TotalValue + difference);
            if(PanelControllerCall.CurrentSidePanel != null)
            {
                if(PanelControllerCall.CurrentSidePanel.name == "EconomyPanel") PanelControllerCall.CurrentSidePanel.GetComponent<EconomyPanelController>().UpdateBankList();
            }
        }

    }

    public GameObject GetGoodsValue()
    {
        return transform.GetChild(0).gameObject;
    }

    public GameObject GetEconomyValue()
    {
        return transform.GetChild(1).gameObject;
    }

    public GameObject GetTechValue()
    {
        return transform.GetChild(2).gameObject;
    }
    
    public GameObject GetElectricityValue()
    {
        return transform.GetChild(3).gameObject;
    }

    public GameObject GetFacilityValue()
    {
        return transform.GetChild(4).gameObject;
    }

    public GameObject GetEmployeeValue()
    {
        return transform.GetChild(5).gameObject;
    }

    public GameObject GetEventValue()
    {
        return transform.GetChild(6).gameObject;
    }
    
    public GameObject GetLandValue()
    {
        return transform.GetChild(7).gameObject;
    }
}
