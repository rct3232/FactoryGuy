using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanyManager : MonoBehaviour
{
    class CompanyInfo
    {
        public CompanyInfo() {}
        public string Name;
        public GameObject Object;
    }
    public string PlayerCompanyName;
    List<CompanyInfo> CompanyList = new List<CompanyInfo>();

    public void AddCompany(string Name, GameObject Object)
    {
        CompanyInfo newCompany = new CompanyInfo();

        newCompany.Name = Name;
        newCompany.Object = Object;
        newCompany.Object.GetComponent<CompanyValue>().CompanyName = Name;

        CompanyList.Add(newCompany);
    }

    public GameObject GetCompanyObject(string Name)
    {
        GameObject result = null;

        foreach(var Info in CompanyList)
        {
            if(Info.Name == Name)
            {
                result = Info.Object;
                break;
            }
        }
        
        return result;
    }

    public CompanyValue GetCompanyValue(string Name)
    {
        CompanyValue result = null;

        foreach(var Info in CompanyList)
        {
            if(Info.Name == Name)
            {
                result = Info.Object.GetComponent<CompanyValue>();
                break;
            }
        }
        
        return result;
    }

    public List<GameObject> GetAllCompanyObject()
    {
        List<GameObject> ObjectList = new List<GameObject>();

        foreach(var Info in CompanyList)
        {
            ObjectList.Add(Info.Object);
        }

        return ObjectList;
    }

    public GameObject GetPlayerCompanyObject()
    {
        GameObject result = null;

        foreach(var Info in CompanyList)
        {
            if(Info.Name == PlayerCompanyName)
            {
                result = Info.Object;
                break;
            }
        }
        
        return result;
    }

    public CompanyValue GetPlayerCompanyValue()
    {
        CompanyValue result = null;

        foreach(var Info in CompanyList)
        {
            if(Info.Name == PlayerCompanyName)
            {
                result = Info.Object.GetComponent<CompanyValue>();
                break;
            }
        }
        
        return result;
    }
}
