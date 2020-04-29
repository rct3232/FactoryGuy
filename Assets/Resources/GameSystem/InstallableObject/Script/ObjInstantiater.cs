using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjInstantiater : MonoBehaviour
{
    CompanyManager CompanyManagerCall;
    CompanyValue CompanyValueCall;
    TechValue TechValueCall;
    InGameValue ValueCall;
    GameObject newObject;

    // Start is called before the first frame update
    void Awake()
    {
        CompanyManagerCall = GameObject.Find("CompanyManager").GetComponent<CompanyManager>();
        CompanyValueCall = CompanyManagerCall.GetPlayerCompanyValue();
        TechValueCall = CompanyValueCall.GetTechValue().GetComponent<TechValue>();
        ValueCall = GameObject.Find("BaseSystem").GetComponent<InGameValue>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject InstantiateNewObject(string name)
    {
        if (ValueCall.AttachedOnMouse != null)
        {
            Destroy(ValueCall.AttachedOnMouse);
        }
        foreach(var Facility in TechValueCall.FacilityList)
        {
            if (Facility.Name == name)
            {
                newObject = GameObject.Instantiate(Facility.Object, transform);
                ValueCall.AttachedOnMouse = newObject;
                newObject.transform.name = "New " + Facility.Type;
                newObject.transform.parent = this.transform;
                newObject.GetComponent<InstallableObjectAct>().Info = Facility;

                return newObject;
            }
        }

        return null;
    }
}
