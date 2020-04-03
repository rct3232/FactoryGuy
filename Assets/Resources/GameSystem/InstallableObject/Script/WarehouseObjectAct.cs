using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseObjectAct : MonoBehaviour
{
    InstallableObjectAct ObjectActCall;
    CompanyValue CompanyValueCall;
    public int StorageType;
    public int Capacity;
    bool isInitialized = false;
    
    // Start is called before the first frame update
    void Start()
    {
        ObjectActCall = gameObject.GetComponent<InstallableObjectAct>();
        CompanyValueCall = GameObject.Find("CompanyManager").GetComponent<CompanyManager>().GetPlayerCompanyValue();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(ObjectActCall.isInstall && !isInitialized)
        {
            isInitialized = true;
            ObjectActCall.IsWorking = true;
            CompanyValueCall.GetGoodsValue().GetComponent<GoodsValue>().AddStorage(gameObject);
        }
    }

    public bool DeleteObject()
    {
        CompanyValueCall.GetGoodsValue().GetComponent<GoodsValue>().DeleteStorage(gameObject);

        return true;
    }
}