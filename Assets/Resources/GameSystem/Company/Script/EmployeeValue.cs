using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeValue : MonoBehaviour
{
    class LaborInfo
    {
        public LaborInfo() {}
        public FacilityValue.FacilityInfo Info;
    }
    public class DayRoomInfo
    {
        public DayRoomInfo() {}
        public FacilityValue.FacilityInfo Info;
        public DayRoomAct DayRoomActCall;
    }
    public class EmployeeInfo
    {
        public EmployeeInfo() {}
        public int Index;
        public WorkerSelector.Worker BaseInfo;
        public float FatigueValue;
        public float CurrentLaborForce;
        public float Happiness;
        public float CurrentSincerityRate;
        public float Experience;
        public bool isWorking;
        public bool isResting;
        public DayRoomAct CurrentDayRoom;
    }
    InGameValue ValueCall;
    TimeManager TimeManagerCall;
    CompanyManager CompanyManagerCall;
    CompanyValue CompanyValueCall;
    EconomyValue EconomyValueCall;
    PanelController PanelControllerCall;
    List<LaborInfo> LaborInfoList = new List<LaborInfo>();
    List<DayRoomInfo> DayRoomList = new List<DayRoomInfo>();
    public List<EmployeeInfo> EmployeeList = new List<EmployeeInfo>();
    int CurrentIndex = 0;
    public float TotalLabor;
    public float RequiredLabor;
    public float ActivatedRequiredLabor;
    public float ActivatedLabor;
    public int RotateTimeLimit;
    int RotateTimer;
    // Start is called before the first frame update
    void Start()
    {
        ValueCall = GameObject.Find("BaseSystem").GetComponent<InGameValue>();
        TimeManagerCall = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        CompanyManagerCall = transform.parent.parent.gameObject.GetComponent<CompanyManager>();
        CompanyValueCall = transform.parent.gameObject.GetComponent<CompanyValue>();
        EconomyValueCall  = CompanyValueCall.GetEconomyValue().GetComponent<EconomyValue>();
        PanelControllerCall = GameObject.Find("Canvas").GetComponent<PanelController>();
        ActivatedLabor = 0;
        ActivatedRequiredLabor = 0;
        RotateTimeLimit = 200;
        RotateTimer = RotateTimeLimit;

        if(CompanyValueCall.CompanyName == CompanyManagerCall.PlayerCompanyName) PanelControllerCall.UpdateFactoryInfo("Employee", TotalLabor, RequiredLabor);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RotateTimer += TimeManagerCall.PlaySpeed;

        if(RotateTimer >= RotateTimeLimit)
        {
            RotatingEmployee();
        }

        UpdateLaborForce();

        if(TimeManagerCall.TimeValue % TimeManagerCall.Day < TimeManagerCall.PlaySpeed)
        {
            ManageEmployeeExperience();
        }
    }

    public void HireEmployee(WorkerSelector.Worker newWorker)
    {
        EmployeeInfo newEmployee = new EmployeeInfo();
        newEmployee.Index = CurrentIndex++;
        newEmployee.BaseInfo = newWorker;
        newEmployee.FatigueValue = newWorker.LaborForce * 0.5f;
        newEmployee.CurrentLaborForce = newWorker.LaborForce;
        newEmployee.CurrentSincerityRate = newWorker.SincerityRate;
        newEmployee.Happiness = 0.45f;
        newEmployee.isResting = false;
        newEmployee.isWorking = false;
        newEmployee.CurrentDayRoom = null;

        EmployeeList.Add(newEmployee);
        TotalLabor += newWorker.LaborForce;

        if(EmployeeList.Count != 1)
        {
            EconomyValueCall.AddPersistHistory(TimeManagerCall.TimeValue - (TimeManagerCall.TimeValue % TimeManagerCall.Month), TimeManagerCall.Month, "Employee Pay", "#" + newEmployee.Index + " " + newEmployee.BaseInfo.Name, "Employee #" + newEmployee.Index + " Pay", - newEmployee.BaseInfo.Salary);
            CompanyValueCall.GetLandValue().GetComponent<LandValue>().ChangeLandValue(200);

            if(CompanyValueCall.CompanyName == CompanyManagerCall.PlayerCompanyName) 
            {
                GameObject.Find("NotificationManager").GetComponent<NotificationManager>().AddNews("Info", newWorker.Name + " is new partner.");
                // Debug.Log(newEmployee.BaseInfo.Name + " " + newEmployee.BaseInfo.LaborForce + " " + newEmployee.BaseInfo.Salary);

                PanelControllerCall.UpdateFactoryInfo("Employee", TotalLabor, RequiredLabor);
                if(PanelControllerCall.CurrentSidePanel != null)
                {
                    if(PanelControllerCall.CurrentSidePanel.name == "WorkerPanel")
                    {
                        WorkerPanelController PanelComponent = PanelControllerCall.CurrentSidePanel.GetComponent<WorkerPanelController>();

                        PanelComponent.DisplayWorkerList();
                    }
                }
            }
        }
    }

    public void FireEmployee(int Index)
    {
        DeleteEmployee(EmployeeList[Index]);
    }

    void DeleteEmployee(EmployeeInfo Employee)
    {
        TotalLabor -= Employee.BaseInfo.LaborForce;

        int TargetIndex = GetEmployeeIndex(Employee);

        EconomyValueCall.DeletePersistHistory("Employee #" + Employee.Index + " Pay");
        EmployeeList.RemoveAt(TargetIndex);
        Employee.BaseInfo.LeaveFactoryList.Add(CompanyValueCall.CompanyName);
        GameObject.Find("WorkerManager").GetComponent<WorkerSelector>().UnHiredList.Add(Employee.BaseInfo);

        if(CompanyValueCall.CompanyName == CompanyManagerCall.PlayerCompanyName) 
        {
            PanelControllerCall.UpdateFactoryInfo("Employee", TotalLabor, RequiredLabor);
            if(PanelControllerCall.CurrentSidePanel != null)
            {
                if(PanelControllerCall.CurrentSidePanel.name == "WorkerPanel")
                {
                    WorkerPanelController PanelComponent = PanelControllerCall.CurrentSidePanel.GetComponent<WorkerPanelController>();

                    PanelComponent.DisplayWorkerList();
                    if(PanelComponent.CurrentWorkerIndex == TargetIndex) PanelComponent.ClearInfoPanel();
                }
            }
        }
    }

    public int GetEmployeeIndex(EmployeeInfo Target)
    {
        int result = -1;

        for(int i = 0; i < EmployeeList.Count; i++)
        {
            if(EmployeeList[i] == Target)
            {
                result = i;

                break;
            }
        }

        return result;
    }

    public int GetEmployeeIndex(int Target)
    {
        int result = -1;

        for(int i = 0; i < EmployeeList.Count; i++)
        {
            if(EmployeeList[i].Index == Target)
            {
                result = i;

                break;
            }
        }

        return result;
    }

    public void AddLaborInfo(FacilityValue.FacilityInfo Info)
    {
        if(Info.ObjectActCall.Info.Type == "DayRoom")
        {
            DayRoomInfo newInfo = new DayRoomInfo();

            newInfo.Info = Info;
            newInfo.DayRoomActCall = Info.Object.GetComponent<DayRoomAct>();

            DayRoomList.Add(newInfo);
        }
        else
        {
            LaborInfo newInfo = new LaborInfo();

            newInfo.Info = Info;

            LaborInfoList.Add(newInfo);

            if(Info.ObjectActCall.Info.LaborRequirement > 0)
            {
                RequiredLabor += Info.ObjectActCall.Info.LaborRequirement;
                RotatingEmployee();
            }
        }

        if(CompanyValueCall.CompanyName == CompanyManagerCall.PlayerCompanyName) PanelControllerCall.UpdateFactoryInfo("Employee", TotalLabor, RequiredLabor);
    }

    public void DeleteLaborInfo(FacilityValue.FacilityInfo Info)
    {
        if(Info.ObjectActCall.Info.Type == "DayRoom")
        {
            DayRoomInfo Target = null;

            foreach(var Supply in DayRoomList)
            {
                if(Supply.Info == Info)
                {
                    Target = Supply;
                    break;
                }
            }

            if(Target != null)
            {
                DayRoomList.Remove(Target);
            }
        }
        else
        {
            LaborInfo Target = null;

            foreach(var Usage in LaborInfoList)
            {
                if(Usage.Info == Info)
                {
                    Target = Usage;
                    break;
                }
            }

            if(Target != null)
            {
                LaborInfoList.Remove(Target);
            }

            if(Target.Info.ObjectActCall.Info.LaborRequirement != 0)
                RotatingEmployee();
        }

        if(CompanyValueCall.CompanyName == CompanyManagerCall.PlayerCompanyName) PanelControllerCall.UpdateFactoryInfo("Employee", TotalLabor, RequiredLabor);
    }

    void ManageEmployeeExperience()
    {
        for(int i = 1; i < EmployeeList.Count; i++)
        {
            if(EmployeeList[i].Experience < EmployeeList[i].BaseInfo.LaborForce * 10 && EmployeeList[i].isWorking)
            {
                EmployeeList[i].Experience++;

                if(CompanyValueCall.CompanyName == CompanyManagerCall.PlayerCompanyName) 
                {
                    if(PanelControllerCall.CurrentSidePanel != null)
                    {
                        if(PanelControllerCall.CurrentSidePanel.name == "WorkerPanel")
                        {
                            WorkerPanelController PanelComponent = PanelControllerCall.CurrentSidePanel.GetComponent<WorkerPanelController>();
                            
                            if(PanelComponent.CurrentWorkerIndex == i) PanelComponent.UpdateInfoPanel();
                        }
                    }
                }
            }
        }
    }

    public void PromoteEmployee(int Index)
    {
        int UpgradeValue = Random.Range(2, Random.Range(6, Random.Range(8, 11)));
        EmployeeList[Index].BaseInfo.LaborForce += UpgradeValue;
        TotalLabor += UpgradeValue;

        int PayRaiseValue = Mathf.CeilToInt((float)EmployeeList[Index].BaseInfo.LaborForce / 2f * (float)UpgradeValue * Random.Range(1.5f, Random.Range(2f, Random.Range(3.5f, 5f))));
        EmployeeList[Index].BaseInfo.Salary += PayRaiseValue;
        EconomyValueCall.ModifyPersistHistory("Employee #" + EmployeeList[Index].Index + " Pay", EmployeeList[Index].BaseInfo.Salary);

        EmployeeList[Index].Experience = 0;

        if(CompanyValueCall.CompanyName == CompanyManagerCall.PlayerCompanyName) 
        {
            PanelControllerCall.UpdateFactoryInfo("Employee", TotalLabor, RequiredLabor);
            if(PanelControllerCall.CurrentSidePanel != null)
            {
                if(PanelControllerCall.CurrentSidePanel.name == "WorkerPanel")
                {
                    WorkerPanelController PanelComponent = PanelControllerCall.CurrentSidePanel.GetComponent<WorkerPanelController>();

                    PanelComponent.DisplayWorkerList();
                }
            }
        }
    }

    void RotatingEmployee()
    {
        foreach(var Facility in LaborInfoList)
        {
            Facility.Info.SuppliedLabor = 0f;
        }

        RotateTimer = 0;
        ActivatedRequiredLabor = 0;
        ActivatedLabor = 0;

        List<EmployeeInfo> tempEmployeeList = new List<EmployeeInfo>();
        List<LaborInfo> WorkingFacilityLaborInfoList = new List<LaborInfo>();

        foreach(var Employee in EmployeeList)
        {
            Employee.isWorking = false;
            if(!Employee.isResting)
            {
                tempEmployeeList.Add(Employee);      
            }
        }

        foreach(var LaborInfo in LaborInfoList)
        {
            if(LaborInfo.Info.ObjectActCall.IsWorking)
            {
                WorkingFacilityLaborInfoList.Add(LaborInfo);
                ActivatedRequiredLabor += LaborInfo.Info.ObjectActCall.Info.LaborRequirement;
            }
        }

        LaborInfo LastFacility = null;
        int EmployeeLimit = tempEmployeeList.Count;
        for(int i = 0; i < EmployeeLimit; i++)
        {
            if(WorkingFacilityLaborInfoList.Count == 0)
            {
                break;
            }

            EmployeeInfo WorkerTarget = tempEmployeeList[Random.Range(0, tempEmployeeList.Count)];
            float ReaminLaborForce = WorkerTarget.CurrentLaborForce;
            LaborInfo FacilityTarget = LastFacility;

            tempEmployeeList.Remove(WorkerTarget);

            if(LastFacility != null)
            {                    
                if(LastFacility.Info.SuppliedLabor < LastFacility.Info.ObjectActCall.Info.LaborRequirement)
                {
                    float Requirement = LastFacility.Info.ObjectActCall.Info.LaborRequirement - LastFacility.Info.SuppliedLabor;
                    float Difference = ReaminLaborForce - Requirement;

                    if(LastFacility.Info.ObjectActCall.Info.LaborRequirement > 0)
                        WorkerTarget.isWorking = true;

                    if(Difference >= 0)
                    {
                        LastFacility.Info.SuppliedLabor += Requirement;
                        ActivatedLabor += Requirement;
                        ReaminLaborForce -= Requirement;
                        LastFacility = null;
                        if(Difference == 0)
                        {
                            WorkingFacilityLaborInfoList.Remove(LastFacility);
                            continue;
                        }
                    }
                    else
                    {
                        LastFacility.Info.SuppliedLabor += ReaminLaborForce;
                        ActivatedLabor +=ReaminLaborForce;
                        WorkingFacilityLaborInfoList.Remove(LastFacility);
                        continue;
                    }
                }
            }

            int FacilityLimit = WorkingFacilityLaborInfoList.Count;
            for(int j = 0; j < FacilityLimit; j++)
            {
                FacilityTarget = WorkingFacilityLaborInfoList[Random.Range(0, WorkingFacilityLaborInfoList.Count)];
                float Requirement = FacilityTarget.Info.ObjectActCall.Info.LaborRequirement - FacilityTarget.Info.SuppliedLabor;
                float Difference = ReaminLaborForce - Requirement;

                if(FacilityTarget.Info.ObjectActCall.Info.LaborRequirement > 0)
                    WorkerTarget.isWorking = true;
                    

                if(Difference >= 0)
                {
                    FacilityTarget.Info.SuppliedLabor += Requirement;
                    ReaminLaborForce -= Requirement;
                    LastFacility = null;
                    WorkingFacilityLaborInfoList.Remove(FacilityTarget);
                    if(Difference == 0)
                    {
                        break;
                    }
                }
                else
                {
                    FacilityTarget.Info.SuppliedLabor += ReaminLaborForce;
                    LastFacility = FacilityTarget;
                    break;
                }
            }
        }
    }

    void UpdateLaborForce()
    {
        List<EmployeeInfo> DeleteList = new List<EmployeeInfo>();

        foreach(var Employee in EmployeeList)
        {
            if(Employee.BaseInfo.LaborForceLossRate == 0)
                continue;

            if(Employee.Happiness <= 0)
            {
                DeleteList.Add(Employee);
                continue;
            }

            if(Employee.isWorking)
            {
                if(Employee.FatigueValue < Employee.BaseInfo.LaborForce) Employee.FatigueValue += Employee.BaseInfo.LaborForce * Employee.BaseInfo.LaborForceLossRate / 500;
                else Employee.FatigueValue = Employee.BaseInfo.LaborForce;
            }
            else if(Employee.isResting)
            {
                if(Employee.FatigueValue > 0)
                {
                    Employee.FatigueValue -= (Employee.BaseInfo.LaborForce * (Employee.CurrentSincerityRate / 500)) * Employee.CurrentDayRoom.CurrentPerformance;
                }

                if(Employee.FatigueValue < 0)
                {
                    Employee.FatigueValue = 0;
                }
            }

            if(Employee.FatigueValue > 0.3f && !Employee.isResting)
            {
                List<DayRoomInfo> AvaliableDayRoomList = new List<DayRoomInfo>();
                foreach(var DayRoom in DayRoomList)
                {
                    if(DayRoom.Info.ObjectActCall.WorkSpeed > 0f) AvaliableDayRoomList.Add(DayRoom);
                }

                for(int i = AvaliableDayRoomList.Count - 1; i >= 0; i--)
                {
                    DayRoomInfo Target = AvaliableDayRoomList[Random.Range(0, AvaliableDayRoomList.Count)];
                    if(Target.DayRoomActCall.EmployeeCapacity > Target.DayRoomActCall.RestingEmployeeList.Count)
                    {
                        Target.DayRoomActCall.EmployeeEnroll(Employee);
                        // Debug.Log(Employee.BaseInfo.Name + " is going to rest " + Employee.FatigueValue);
                        break;
                    }
                    AvaliableDayRoomList.Remove(Target);
                }
            }

            if(TimeManagerCall.TimeValue % TimeManagerCall.Hour < TimeManagerCall.PlaySpeed)
            {
                if(Employee.FatigueValue >= 0.5f)
                {
                    if(Employee.CurrentLaborForce > Employee.BaseInfo.LaborForce / 4)
                    {
                        Employee.CurrentLaborForce -= 0.1f;
                    }
                    Employee.Happiness -= 0.05f;       
                }
                else
                {
                    if(Employee.CurrentLaborForce < Employee.BaseInfo.LaborForce)
                    {
                        Employee.CurrentLaborForce += 0.5f;
                    }

                    if(Employee.Happiness < 1f) Employee.Happiness += 0.005f;
                    else Employee.Happiness = 1f;
                }

                if(CompanyValueCall.CompanyName == CompanyManagerCall.PlayerCompanyName)
                {
                    if(PanelControllerCall.CurrentSidePanel != null)
                    {
                        if(PanelControllerCall.CurrentSidePanel.name == "WorkerPanel")
                        {
                            WorkerPanelController PanelComponent = PanelControllerCall.CurrentSidePanel.GetComponent<WorkerPanelController>();
                            if(PanelComponent.CurrentWorkerIndex == GetEmployeeIndex(Employee)) PanelComponent.UpdateInfoPanel();
                        }
                    }
                }
            }
        }

        if(DeleteList.Count > 0)
        {
            string LeaveNameList = "";
            for(int i = 0; i < DeleteList.Count; i++)
            {
                LeaveNameList += DeleteList[i].BaseInfo.Name + " ";
                DeleteEmployee(DeleteList[i]);
            }

            if(CompanyValueCall.CompanyName == GameObject.Find("CompanyManager").GetComponent<CompanyManager>().PlayerCompanyName)
            {
                if(DeleteList.Count > 1)
                    LeaveNameList += "are";
                else
                    LeaveNameList += "is";
                    
                GameObject.Find("NotificationManager").GetComponent<NotificationManager>().AddNews("Info", LeaveNameList + " Leave your factory.");
                // Debug.Log(LeaveNameList + " Leave your factory.");
            }

            DeleteList.Clear();
        }
    }
}
