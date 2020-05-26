using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacilityValue : MonoBehaviour
{
    public class FacilityInfo
    {
        public FacilityInfo() {}
        public GameObject Object;
        public InstallableObjectAct ObjectActCall;
        public bool isActive;
        public float SuppliedElectricity;
        public float SuppliedLabor;
        public int InstallDate;
    }
    public List<FacilityInfo> FacilityList = new List<FacilityInfo>();
    CompanyValue CompanyValueCall;
    TimeManager TimeManagerCall;
    public int InstalledFacilityAmount;
    // Start is called before the first frame update
    void Start()
    {
        CompanyValueCall = transform.parent.gameObject.GetComponent<CompanyValue>();
        TimeManagerCall = GameObject.Find("TimeManager").GetComponent<TimeManager>();

        InstalledFacilityAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public FacilityInfo AddFacilityInfo(GameObject Object)
    {
        FacilityInfo newFacility = new FacilityInfo();

        newFacility.Object = Object;
        newFacility.ObjectActCall = Object.GetComponent<InstallableObjectAct>();
        newFacility.isActive = true;
        newFacility.SuppliedElectricity = 0f;
        newFacility.SuppliedLabor = 0;
        newFacility.InstallDate = TimeManagerCall.TimeValue;

        FacilityList.Add(newFacility);

        InstalledFacilityAmount++;

        CompanyValueCall.GetEconomyValue().GetComponent<EconomyValue>().AddHistory(TimeManagerCall.TimeValue, "Install", 
            newFacility.ObjectActCall.Info.Name, "Install " + Object.name, -newFacility.ObjectActCall.Info.Price);
        CompanyValueCall.GetEconomyValue().GetComponent<EconomyValue>().AddPersistHistory(TimeManagerCall.GetNextMonth(0, newFacility.ObjectActCall.Info.UpkeepMonthTerm) + (TimeManagerCall.TimeValue % TimeManagerCall.Month), 
            newFacility.ObjectActCall.Info.UpkeepMonthTerm * TimeManagerCall.Month, "Upkeep", newFacility.ObjectActCall.Info.Name, 
            Object.name + "'s Upkeep Price", -newFacility.ObjectActCall.Info.UpkeepPrice);

        CompanyValueCall.GetElectricityValue().GetComponent<ElectricityValue>().AddObject(newFacility);

        CompanyValueCall.GetEmployeeValue().GetComponent<EmployeeValue>().AddLaborInfo(newFacility);

        return newFacility;
    }

    public void DeleteFacilityInfo(GameObject Object)
    {
        FacilityInfo Target = null;

        foreach(var Info in FacilityList)
        {
            if(Info.Object == Object)
            {
                Target = Info;
                break;
            }
        }

        if(Target != null)
        {
            InstalledFacilityAmount--;

            CompanyValueCall.GetEconomyValue().GetComponent<EconomyValue>().DeletePersistHistory(Object.name + "'s Upkeep Price");
            CompanyValueCall.GetElectricityValue().GetComponent<ElectricityValue>().DeleteObject(Target);
            CompanyValueCall.GetEmployeeValue().GetComponent<EmployeeValue>().DeleteLaborInfo(Target);

            FacilityList.Remove(Target);
        }
    }
}
