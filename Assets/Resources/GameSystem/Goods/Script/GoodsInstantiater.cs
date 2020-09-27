using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsInstantiater : MonoBehaviour
{
    public GameObject BaseSystem;
    public GameObject GoodsPrefab;
    InGameValue ValueCall;
    GoodsValue GoodsValueCall;
    GoodsRecipe GoodsRecipeCall;

    // Start is called before the first frame update
    void Start()
    {
        BaseSystem = GameObject.Find("BaseSystem");
        ValueCall = BaseSystem.GetComponent<InGameValue>();
        GoodsValueCall = GameObject.Find("CompanyManager").GetComponent<CompanyManager>().GetPlayerCompanyValue().GetGoodsValue().GetComponent<GoodsValue>();
        GoodsRecipeCall = BaseSystem.GetComponent<GoodsRecipe>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject CreateGoods(string name)
    {
        int ID = 0;
        if((ID = GoodsValueCall.FindGoodsID(name, false)) == -1)
        {
            Debug.Log("There is no " + name + " in Warehouse");
            return null;
        }

        foreach(var tmp in GoodsRecipeCall.RecipeArray)
        {
            GameObject newGoods = GameObject.Instantiate(GoodsPrefab);
            GameObject GoodsStruct = GameObject.Instantiate(Resources.Load<GameObject>("GameSystem/Goods/Object/" + name), newGoods.transform);
            GoodsStruct.name = name;
            newGoods.transform.SetParent(transform);
            newGoods.transform.name = ID.ToString();
            GoodsValueCall.ChangeInMapState(ID, true, newGoods);
            return newGoods;
        }
        Debug.Log("There is no " + name);
        return null;
    }

    public GameObject ChangeGoods(GameObject CurGoods, string name)
    {
        string ObjectType = GoodsRecipeCall.getObjectType(name);

        if (ObjectType == "None")
        {
            Debug.Log("There is no " + name);
            return null;
        }
        else
        {
            Destroy(CurGoods.transform.GetChild(0).gameObject);
            GameObject GoodsStruct = GameObject.Instantiate(Resources.Load<GameObject>("GameSystem/Goods/Object/" + ObjectType), CurGoods.transform);
            GoodsStruct.name = name;
            GoodsValueCall.ChangeGoodsInfo(int.Parse(CurGoods.name), name);
            return CurGoods;
        }
    }
}
