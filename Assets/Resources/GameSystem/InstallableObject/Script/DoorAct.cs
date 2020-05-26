using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAct : MonoBehaviour
{
    public string TargetGoodsName;
    public GameObject Mover;
    public GameObject DummyBelt;
    public BeltAct DummyBeltActCall;
    public GameObject Goods;
    public string DoorMode;
    GoodsValue GoodsValueCall;
    CompanyManager CompanyManagerCall;
    SalesValue SalesValueCall;
    InstallableObjectAct ObjectActCall;
    GameObject MoverDetector;
    GameObject WarehouseDetector;
    int DoorDirection = -1;
    public bool isInitialized = false;

    // Start is called before the first frame update
    void Start()
    {
        CompanyManagerCall = GameObject.Find("CompanyManager").GetComponent<CompanyManager>();
        GoodsValueCall = CompanyManagerCall.GetPlayerCompanyValue().GetGoodsValue().GetComponent<GoodsValue>();
        SalesValueCall = GameObject.Find("SalesManager").GetComponent<SalesValue>();
        ObjectActCall = gameObject.GetComponent<InstallableObjectAct>();
        Goods = GameObject.Find("Goods");
        DummyBeltActCall = DummyBelt.GetComponent<BeltAct>();

        MoverDetector = transform.GetChild(2).GetChild(0).GetChild(0).gameObject;
        WarehouseDetector = transform.GetChild(2).GetChild(1).GetChild(0).gameObject;

        TargetGoodsName = "None";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ObjectActCall.isInstall)
        {
            isInitialized = true;

            if(DoorMode == "Ejector")
            {
                if(TargetGoodsName == "None")
                {
                    isInitialized = false;
                }
            }

            if(isInitialized)
            {
                if(DoorMode == "Ejector") CreateGoods();
                else if(DoorMode == "Loader") LoadToWarehouse();

                ObjectActCall.IsWorking = true;
            }
            else
            {
                ObjectActCall.IsWorking = false;
            }
        }
    }

    void CheckDirection()
    {
        if (gameObject.transform.eulerAngles.y == 0)
        {
            DoorDirection = 0;
        }
        else if (gameObject.transform.eulerAngles.y == 90)
        {
            DoorDirection = 1;
        }
        else if (gameObject.transform.eulerAngles.y == 180)
        {
            DoorDirection = 2;
        }
        else if (gameObject.transform.eulerAngles.y == 270)
        {
            DoorDirection = 3;
        }
    }

    public void Initializing()
    {
        if(DoorMode == "Ejector")
        {
            DummyBelt.transform.rotation = Quaternion.Euler(0, 0, 0);
            Mover.GetComponent<BeltAct>().PrevBelt.GetComponent<BeltAct>().ChangeNextBelt(null);
            Mover.GetComponent<BeltAct>().ChangePrevBelt(DummyBelt);
            DummyBelt.GetComponent<BeltAct>().ChangeNextBelt(Mover);
        }
        else
        {
            DummyBelt.transform.rotation = Quaternion.Euler(0, 180, 0);
            Mover.GetComponent<BeltAct>().NextBelt.GetComponent<BeltAct>().ChangePrevBelt(null);
            Mover.GetComponent<BeltAct>().ChangeNextBelt(DummyBelt);
            DummyBelt.GetComponent<BeltAct>().ChangePrevBelt(Mover);
        }

        Mover.GetComponent<BeltAct>().ModuleObject = gameObject;

        if(Mover != null)
        {
            ObjectActCall.Installation();
        }
    }

    public void SetTargetGoods(string name)
    {
        TargetGoodsName = name;
    }
    
    void CreateGoods()
    {
        if(GoodsValueCall.GetGoodsCount(TargetGoodsName, true) <= 0)
        {
            ObjectActCall.IsWorking = false;

            return;
        }
        
        if(Mover.GetComponent<BeltAct>().GoodsOnBelt == null && DummyBeltActCall.GoodsOnBelt == null)
        {
            ObjectActCall.IsWorking = true;

            GameObject newGoods = Goods.GetComponent<GoodsInstantiater>().CreateGoods(TargetGoodsName);
            newGoods.transform.position = new Vector3(DummyBelt.transform.position.x, DummyBelt.transform.position.y + 0.325f, DummyBelt.transform.position.z);
            DummyBeltActCall.GoodsOnBelt = newGoods;
        }
        else
        {
            ObjectActCall.IsWorking = false;
        }
    }

    void LoadToWarehouse()
    {
        if(DummyBeltActCall.GoodsOnBelt != null)
        {
            ObjectActCall.IsWorking = true;

            if (GoodsValueCall.CheckMovingState(int.Parse(DummyBeltActCall.GoodsOnBelt.name)) == 0)
            {
                SalesValueCall.UpdateItemCount(CompanyManagerCall.PlayerCompanyName, GoodsValueCall.FindGoodsName(DummyBeltActCall.GoodsOnBelt));

                GoodsValueCall.ChangeInMapState(int.Parse(DummyBeltActCall.GoodsOnBelt.name), false, null);
                Destroy(DummyBeltActCall.GoodsOnBelt);
                DummyBeltActCall.GoodsOnBelt = null;
            }
        }
        else
        {
            ObjectActCall.IsWorking = false;
        }
    }

    public bool DeleteObject()
    {
        if(ObjectActCall.isInstall)
        {   
            if(DoorMode == "Ejector")
            {
                Mover.GetComponent<BeltAct>().ChangePrevBelt(null);
            }
            else
            {
                Mover.GetComponent<BeltAct>().ChangeNextBelt(null);
            }

            Mover.GetComponent<BeltAct>().ModuleObject = null;
        }

        return true;
    }
}
