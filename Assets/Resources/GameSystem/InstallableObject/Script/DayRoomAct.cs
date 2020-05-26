using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayRoomAct : MonoBehaviour
{
    InstallableObjectAct ObjectActCall;
    CompanyValue CompanyValueCall;
    EmployeeValue EmployeeValueCall;
    public List<EmployeeValue.EmployeeInfo> RestingEmployeeList = new List<EmployeeValue.EmployeeInfo>();
    public float Performance;
    public float CurrentPerformance;
    public float EmployeeCapacity;
    public bool OnUsing = false;

    // Start is called before the first frame update
    void Start()
    {
        ObjectActCall = gameObject.GetComponent<InstallableObjectAct>();
        CompanyValueCall = GameObject.Find("CompanyManager").GetComponent<CompanyManager>().GetPlayerCompanyValue();
        EmployeeValueCall = CompanyValueCall.GetEmployeeValue().GetComponent<EmployeeValue>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(ObjectActCall.isInstall)
        {
            ObjectActCall.IsWorking = true;
            CurrentPerformance = Performance * ObjectActCall.WorkSpeed;
            if(RestingEmployeeList.Count > 0)
            {
                ManageEmployee();
                OnUsing = true;
            }
            else OnUsing = false;
        }
    }

    public void EmployeeEnroll(EmployeeValue.EmployeeInfo Target)
    {
        RestingEmployeeList.Add(Target);
        Target.isResting = true;
        Target.isWorking = false;

        Target.CurrentDayRoom = this;
    }

    void ManageEmployee()
    {
        List<EmployeeValue.EmployeeInfo> TargetList = new List<EmployeeValue.EmployeeInfo>();

        if(ObjectActCall.WorkSpeed > 0)
        {
            foreach(var Employee in RestingEmployeeList)
            {
                if(Employee.FatigueValue <= 0)
                {
                    // Debug.Log(Employee.BaseInfo.Name + " Left dayroom");
                    Employee.isResting = false;
                    TargetList.Add(Employee);
                    Employee.CurrentDayRoom = null;
                }
            }
        }
        else
        {
            TargetList.AddRange(RestingEmployeeList);
        }

        foreach(var Employee in TargetList)
        {
            RestingEmployeeList.Remove(Employee);
        }
    }

    public bool DeleteObject()
    {
        foreach(var Employee in RestingEmployeeList)
        {
            Employee.isResting = false;
            Employee.CurrentDayRoom = null;
            RestingEmployeeList.Remove(Employee);
        }

        return true;
    }
}
