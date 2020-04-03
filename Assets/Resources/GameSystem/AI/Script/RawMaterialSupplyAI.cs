using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RawMaterialSupplyAI : MonoBehaviour
{
    public string Name;
    CompanyValue CompanyValueCall;
    GoodsValue GoodsValueCall;
    TimeManager TimaManagerCall;
    SalesValue SalesValueCall;

    // Start is called before the first frame update
    void Start()
    {
        CompanyValueCall = transform.parent.gameObject.GetComponent<CompanyValue>();
        GoodsValueCall = CompanyValueCall.GetGoodsValue().GetComponent<GoodsValue>();    
        TimaManagerCall = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        SalesValueCall = GameObject.Find("SalesManager").GetComponent<SalesValue>();

        List<string> ItemList = new List<string>();
        List<int> PriceList = new List<int>();

        CompanyValueCall.GetGoodsValue().GetComponent<GoodsValue>().TotalCapacity = 9999999;

        switch(Name)
        {
            case "General Industry Co." :
                ItemList.Add("Paper Roll");
                PriceList.Add(1);
                Initializing(ItemList, PriceList);
                break;
            case "Federal Agency of Industry" :
                ItemList.Add("Plastic Mass"); ItemList.Add("Silicon Mass");
                PriceList.Add(2); PriceList.Add(3);
                Initializing(ItemList, PriceList);
                break;
            case "Sasio" :
                ItemList.Add("Packaged Calculator"); ItemList.Add("Calculator"); ItemList.Add("Calculating Circuit");
                List<string> NameList = new List<string>();
                NameList.Add("Packaged Cal-100"); NameList.Add("Cal-100"); NameList.Add("CM100");
                PriceList.Add(18); PriceList.Add(13); PriceList.Add(10);
                GoodsRecipe GoodsRecipeCall = GameObject.Find("BaseSystem").GetComponent<GoodsRecipe>();
                TechValue TechValueCall = CompanyValueCall.GetTechValue().GetComponent<TechValue>();
                for(int i = 0; i < ItemList.Count; i++)
                {
                    for(int j = 0; j < GoodsRecipeCall.RecipeArray.Count; j++)
                    {
                        if(GoodsRecipeCall.RecipeArray[j].OutputName == ItemList[i])
                        {
                            GoodsRecipeCall.MakeCustomRecipe(ItemList[i], NameList[i], GoodsRecipeCall.RecipeArray[j].InputName, GoodsRecipeCall.RecipeArray[j].Attractiveness, Name);
                            SalesValueCall.AddSales(NameList[i], Name, PriceList[i]);
                        }
                    }
                }
                break;
        }

        StockManage();
    }

    // Update is called once per frame
    void Update()
    {
        if(TimaManagerCall.TimeValue % TimaManagerCall.Hour < TimaManagerCall.PlaySpeed)
            StockManage();
    }

    void Initializing(List<string> ItemList, List<int> PriceList)
    {
        GoodsRecipe GoodsRecipeCall = GameObject.Find("BaseSystem").GetComponent<GoodsRecipe>();
        TechValue TechValueCall = CompanyValueCall.GetTechValue().GetComponent<TechValue>();
        for(int i = 0; i < ItemList.Count; i++)
        {
            foreach(var Recipe in GoodsRecipeCall.RecipeArray)
            {
                if(Recipe.OutputName == ItemList[i])
                {
                    TechValueCall.AddRecipe(Recipe, null);
                }
            }
            // GoodsRecipeCall.MakeCustomRecipe(ItemList[i], Name + " " + ItemList[i], null, Name);
            SalesValueCall.AddSales(ItemList[i], Name, PriceList[i]);
        }
    }

    void StockManage()
    {
        int Difference = 0;
        switch(Name)
        {
            case "General Industry Co." :
                Difference = 999 - GoodsValueCall.GetGoodsCount("Paper Roll" , true);
                if(Difference > 0)
                {
                    for(int i = 0; i < Difference; i++)
                    {
                        GoodsValueCall.AddGoodsArray("Paper Roll", Random.Range(0.8f, 1f));
                    }
                }
                break;
            case "Federal Agency of Industry" :
                Difference = 999 - GoodsValueCall.GetGoodsCount("Plastic Mass", true);
                if(Difference > 0)
                {
                    for(int i = 0; i < Difference; i++)
                    {
                        GoodsValueCall.AddGoodsArray("Plastic Mass", Random.Range(0.5f, 1f));
                    }
                }

                Difference = 999 - GoodsValueCall.GetGoodsCount("Silicon Mass", true);
                if(Difference > 0)
                {
                    for(int i = 0; i < Difference; i++)
                    {
                        GoodsValueCall.AddGoodsArray("Silicon Mass", Random.Range(0.7f, 1f));
                    }
                }
                break;
            case "Sasio" :
                Difference = 999 - GoodsValueCall.GetGoodsCount("Packaged Cal-100", true);
                if(Difference > 0)
                {
                    for(int i = 0; i < Difference; i++)
                    {
                        GoodsValueCall.AddGoodsArray("Packaged Cal-100", Random.Range(0.6f, 0.8f));
                    }
                }
                Difference = 999 - GoodsValueCall.GetGoodsCount("Cal-100", true);
                if(Difference > 0)
                {
                    for(int i = 0; i < Difference; i++)
                    {
                        GoodsValueCall.AddGoodsArray("Cal-100", Random.Range(0.6f, 0.8f));
                    }
                }
                Difference = 999 - GoodsValueCall.GetGoodsCount("CM100", true);
                if(Difference > 0)
                {
                    for(int i = 0; i < Difference; i++)
                    {
                        GoodsValueCall.AddGoodsArray("CM100", Random.Range(0.6f, 0.8f));
                    }
                }
                break;
        }

        SalesValueCall.UpdateItemCount(Name);
    }
}
