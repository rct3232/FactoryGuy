using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityValue : MonoBehaviour
{
    class UsageInfo
    {
        public UsageInfo() {}
        public FacilityValue.FacilityInfo FacilityInfo;
    }
    class StoreObject
    {
        public StoreObject() {}
        public FacilityValue.FacilityInfo FacilityInfo;
        public EnergyStorageAct EnergyStoreActCall;
    }
    public float TotalUsage;
    public float TotalStoredAmount;
    public float TotalStorableAmount;
    public float AvailableElectricityAmount;
    public float ElectricityInputValue;
    public float ElectricityInputRatio;
    public float ElectricFee;
    public int BlackOutTimeLimit;
    public int StoreObjectCount;
    public bool Lack;
    int BlackOutTimer;
    [SerializeField] float UsedElectricityAmount = 0f;
    List<UsageInfo> UsageList = new List<UsageInfo>();
    List<StoreObject> StoreList = new List<StoreObject>();
    TimeManager TimeManagerCall;
    CompanyManager CompanyManagerCall;
    CompanyValue CompanyValueCall;
    FacilityValue FaciliyValueCall;
    PanelController PanelControllerCall;

    // Start is called before the first frame update
    void Start()
    {
        TimeManagerCall = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        CompanyManagerCall = transform.parent.parent.gameObject.GetComponent<CompanyManager>();
        CompanyValueCall = transform.parent.gameObject.GetComponent<CompanyValue>();
        FaciliyValueCall = CompanyValueCall.GetFacilityValue().GetComponent<FacilityValue>();
        PanelControllerCall = GameObject.Find("Canvas").GetComponent<PanelController>();

        TotalStorableAmount = 0f;
        AvailableElectricityAmount = 0f;
        ElectricityInputValue = 20f;
        ElectricityInputRatio = 1f;
        ElectricFee = 0.001f;
        BlackOutTimeLimit = 200;
        BlackOutTimer = BlackOutTimeLimit;
        UsedElectricityAmount = 0f;
        StoreObjectCount = 0;
        Lack = false;

        CompanyValueCall.GetEconomyValue().GetComponent<EconomyValue>().AddPersistHistory(TimeManagerCall.GetNextMonth(0), TimeManagerCall.Month, "UpKeep", 
            "Electric Fee", "Electric Fee", -Mathf.CeilToInt(UsedElectricityAmount * ElectricFee));
        // CompanyValueCall.GetEconomyValue().GetComponent<EconomyValue>().AddHistory(TimeManagerCall.TimeValue, "UpKeep", 
        //     "Electric Fee", "Electric Fee", -Mathf.CeilToInt(UsedElectricityAmount * ElectricFee), true);

        if(CompanyValueCall.CompanyName == CompanyManagerCall.PlayerCompanyName) PanelControllerCall.UpdateFactoryInfo("Electricity", TotalUsage, AvailableElectricityAmount);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(TimeManagerCall.TimeValue % TimeManagerCall.Month < TimeManagerCall.PlaySpeed)
        {
            UsedElectricityAmount = 0f;
        }

        CalculateAvailableElectricity();
        BlackOutTimer += TimeManagerCall.PlaySpeed;
        DistributeElectricity();

        CalculateUsedElectricityPerTic();
        if(TimeManagerCall.TimeValue % TimeManagerCall.Day < TimeManagerCall.PlaySpeed)
        {
            CompanyValueCall.GetEconomyValue().GetComponent<EconomyValue>().ModifyPersistHistory("Electric Fee", -Mathf.CeilToInt(UsedElectricityAmount * ElectricFee));
        }

        if(TimeManagerCall.TimeValue % TimeManagerCall.Day < TimeManagerCall.PlaySpeed)
        {
            if(CompanyValueCall.CompanyName == CompanyManagerCall.PlayerCompanyName) 
            {
                PanelControllerCall.UpdateFactoryInfo("Electricity", TotalUsage, AvailableElectricityAmount);
            }
        }
    }

    void CalculateAvailableElectricity()
    {
        TotalUsage = 0;
        TotalStoredAmount = 0;

        foreach(var Usage in UsageList)
        {
            if(Usage.FacilityInfo.isActive)
            {
                TotalUsage += Usage.FacilityInfo.ObjectActCall.Info.ElectricConsum;
            }
        }

        foreach(var Supply in StoreList)
        {
            if(Supply.FacilityInfo.isActive)
            {
                TotalStoredAmount += Supply.EnergyStoreActCall.StoredElectricity;
            }
        }

        AvailableElectricityAmount = ElectricityInputValue * ElectricityInputRatio + TotalStoredAmount;
    }

    void CalculateUsedElectricityPerTic()
    {
        foreach(var Facility in UsageList)
        {
            UsedElectricityAmount += Facility.FacilityInfo.SuppliedElectricity;
        }
        foreach(var Facility in StoreList)
        {
            UsedElectricityAmount += Facility.FacilityInfo.SuppliedElectricity;
        }
    }

    public void AddObject(FacilityValue.FacilityInfo Info)
    {
        if(Info.ObjectActCall.Info.Type == "EnergyStorage")
        {
            StoreObject newObject = new StoreObject();

            newObject.FacilityInfo = Info;
            newObject.EnergyStoreActCall = Info.Object.GetComponent<EnergyStorageAct>();

            StoreList.Add(newObject);

            TotalStorableAmount += newObject.EnergyStoreActCall.StorableElectricity;
            StoreObjectCount++;
        }
        else
        {
            UsageInfo newUsage = new UsageInfo();
        
            newUsage.FacilityInfo = Info;

            UsageList.Add(newUsage);
        }

        BlackOutTimer = BlackOutTimeLimit;

        if(CompanyValueCall.CompanyName == CompanyManagerCall.PlayerCompanyName) 
        {
            PanelControllerCall.UpdateFactoryInfo("Electricity", TotalUsage, AvailableElectricityAmount);
        }
    }

    public void DeleteObject(FacilityValue.FacilityInfo Info)
    {
        if(Info.ObjectActCall.Info.Type == "EnergyStorage")
        {
            StoreObject Target = null;

            foreach(var Supply in StoreList)
            {
                if(Supply.FacilityInfo == Info)
                {
                    Target = Supply;
                    break;
                }
            }

            if(Target != null)
            {
                StoreList.Remove(Target);
            }
            else
            {
                Debug.Log("Cannot Find " + Info.Object.name);
            }
        }
        else
        {
            UsageInfo Target = null;

            foreach(var Usage in UsageList)
            {
                if(Usage.FacilityInfo == Info)
                {
                    Target = Usage;
                    break;
                }
            }

            if(Target != null)
            {
                UsageList.Remove(Target);
            }
            else
            {
                Debug.Log("Cannot Find " + Info.Object.name);
            }
        }

        BlackOutTimer = BlackOutTimeLimit;

        if(CompanyValueCall.CompanyName == CompanyManagerCall.PlayerCompanyName) 
        {
            PanelControllerCall.UpdateFactoryInfo("Electricity", TotalUsage, AvailableElectricityAmount);
        }
    }

    public void AddElectricityInput(GameObject Object)
    {
        ElectricityInputValue += Object.GetComponent<EnergySupplierAct>().InputCapacity;
        BlackOutTimer = BlackOutTimeLimit;
    }

    public void DeleteElectricityInput(GameObject Object)
    {
        ElectricityInputValue -= Object.GetComponent<EnergySupplierAct>().InputCapacity;
        BlackOutTimer = BlackOutTimeLimit;
    }

    public void ChangeElectrictiyInputRatio(float Value)
    {
        ElectricityInputRatio = Value;
        BlackOutTimer = BlackOutTimeLimit;

        if(CompanyValueCall.CompanyName == CompanyManagerCall.PlayerCompanyName) 
        {
            PanelControllerCall.UpdateFactoryInfo("Electricity", TotalUsage, AvailableElectricityAmount);
        }
    }

    void DistributeElectricity()
    {
        List<UsageInfo> TargetList = new List<UsageInfo>();
        List<UsageInfo> ExcludedList = new List<UsageInfo>();
        float CurrentUsage = TotalUsage;

        TargetList.AddRange(UsageList);
        for(int i = TargetList.Count - 1; i >= 0; i--)
        {
            if(!TargetList[i].FacilityInfo.ObjectActCall.IsWorking)
            {
                CurrentUsage -= TargetList[i].FacilityInfo.ObjectActCall.Info.ElectricConsum;
                TargetList[i].FacilityInfo.SuppliedElectricity = 0;

                TargetList.RemoveAt(i);
            }
        }

        if(BlackOutTimer >= BlackOutTimeLimit)
        {
            if(AvailableElectricityAmount - TotalStoredAmount < CurrentUsage)
            {
                Lack = true;
            }
            else
            {
                Lack = false;
            }

            foreach(var Usage in TargetList)
            {
                Usage.FacilityInfo.SuppliedElectricity = Usage.FacilityInfo.ObjectActCall.Info.ElectricConsum;
            }

            while(CurrentUsage > AvailableElectricityAmount)
            {
                int TargetIndex = Random.Range(0, TargetList.Count);
                if(TargetList[TargetIndex].FacilityInfo.ObjectActCall.IsWorking)
                {
                    ExcludedList.Add(TargetList[TargetIndex]);

                    CurrentUsage -= TargetList[TargetIndex].FacilityInfo.ObjectActCall.Info.ElectricConsum;
                    TargetList[TargetIndex].FacilityInfo.SuppliedElectricity = 0f;

                    TargetList.RemoveAt(TargetIndex);
                }
            }

            if(AvailableElectricityAmount - CurrentUsage > 0)
            {
                if(ExcludedList.Count > 0)
                {
                    int RandomIndex = Random.Range(0, ExcludedList.Count - 1);

                    ExcludedList[RandomIndex].FacilityInfo.SuppliedElectricity = AvailableElectricityAmount - CurrentUsage;
                }
                else
                {
                    if(CurrentUsage < AvailableElectricityAmount - TotalStoredAmount)
                    {
                        float RemainElectricity = AvailableElectricityAmount - TotalStoredAmount - CurrentUsage;

                        foreach(var Store in StoreList)
                        {
                            Store.FacilityInfo.SuppliedElectricity = RemainElectricity / (float)StoreList.Count;
                        }
                    }
                }
            }

            BlackOutTimer = 0;
        }
    }
}
