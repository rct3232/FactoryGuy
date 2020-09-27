using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumerAI : MonoBehaviour
{
    class Consumer
    {
        public string Type;
        public float PriceFactor;
        public float AttractivenessFactor;
        public float QualityFactor;
        public int BuyDate = 0;
        public int BuyTerm;
    }

    TimeManager TimeManagerCall;
    SalesValue SalesValueCall;
    GoodsRecipe GoodsRecipeCall;
    List<Consumer> ConsumerList = new List<Consumer>();
    Consumer CostEffective = new Consumer();
    Consumer PriceFirst = new Consumer();
    Consumer AttractivenessFirst = new Consumer();
    Consumer QualityFirst = new Consumer();
    // Start is called before the first frame update
    void Start()
    {
        TimeManagerCall = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        SalesValueCall = GameObject.Find("SalesManager").GetComponent<SalesValue>();
        GoodsRecipeCall = GameObject.Find("BaseSystem").GetComponent<GoodsRecipe>();
        Initializinig();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach(var Consumer in ConsumerList)
        {
            if(TimeManagerCall.TimeValue - Consumer.BuyDate > Consumer.BuyTerm)
            {
                BuyItem(Consumer);
            }
        }
    }

    void BuyItem(Consumer Subject)
    {
        Subject.BuyDate = TimeManagerCall.TimeValue;
        List<string> TypeList = GetGoodsType();
        if(TypeList.Count == 0)
        {
            return;
        }
        int TypeIndex = Random.Range(0, TypeList.Count);

        List<SalesValue.SalesInfo> InfoList = GetSalesInfo(TypeList[TypeIndex]);

        if(InfoList.Count == 0)
        {
            Debug.Log(Subject.Type + " Nothing to Buy");
            return;
        }
        float[] PointList = new float[InfoList.Count];

        for(int i = 0; i < InfoList.Count; i++)
        {
            PointList[i] = (-(float)InfoList[i].Price * Subject.PriceFactor) 
                + (InfoList[i].RecipeInfo.Recipe.Attractiveness.TotalPoint * Subject.AttractivenessFactor)
                + (InfoList[i].QualityEvaluation * Subject.QualityFactor);
        }

        float MaxPoint = Mathf.Max(PointList);
        int SelectedInfoIndex = 0;
        for(int i = 0; i < PointList.Length; i++)
        {
            if(MaxPoint == PointList[i])
            {
                SelectedInfoIndex = i;
            }
        }

        SalesValueCall.BuyItem(InfoList[SelectedInfoIndex].RecipeInfo.Recipe.OutputName, "Consumer", 1);
        // if(SalesValueCall.BuyItem(InfoList[SelectedInfoIndex].RecipeInfo.Recipe.OutputName, "Consumer", 1)[0] == -2f)
        // {
        //     if(Random.Range(0,3) != 0)
        //     {
        //         BuyItem(Subject);
        //     }
        // }
    }

    void AddConsumer(string Type)
    {
        Consumer NewConsumer = new Consumer();
        switch(Type)
        {
            case "CostEffective":
                NewConsumer = CostEffective;
                NewConsumer.BuyTerm = Random.Range(1, 5) * TimeManagerCall.Hour;
                break;
            case "PriceFirst":
                NewConsumer = PriceFirst;
                NewConsumer.BuyTerm = Random.Range(1, 5) * TimeManagerCall.Hour;
                break;
            case "AttractivenessFirst":
                NewConsumer = AttractivenessFirst;
                NewConsumer.BuyTerm = Random.Range(1, 5) * TimeManagerCall.Hour;
                break;
            case "QualityFirst":
                NewConsumer = QualityFirst;
                NewConsumer.BuyTerm = Random.Range(1, 5) * TimeManagerCall.Hour;
                break;
            default:
                return;
        }
        ConsumerList.Add(NewConsumer);
    }

    List<string> GetGoodsType()
    {
        List<string> Result = new List<string>();

        foreach(var Sale in SalesValueCall.SalesItemArray)
        {
            if(Sale.RecipeInfo.Recipe.Attractiveness.isPackaged)
            {
                bool Exist = false;
                foreach(var Type in Result)
                {
                    if(Type == Sale.RecipeInfo.Recipe.Type)
                    {
                        Exist = true;
                        break;
                    }
                }

                if(!Exist)
                {
                    Result.Add(Sale.RecipeInfo.Recipe.Type);
                }
            }
        }

        return Result;
    }

    List<SalesValue.SalesInfo> GetSalesInfo(string Type)
    {
        List<SalesValue.SalesInfo> InfoList = new List<SalesValue.SalesInfo>();

        foreach(var Sale in SalesValueCall.SalesItemArray)
        {
            if(Sale.RecipeInfo.Recipe.Type == Type)
            {
                InfoList.Add(Sale);
            }
        }

        return InfoList;
    }

    void Initializinig()
    {
        CostEffective.Type = "CostEffective";
        CostEffective.PriceFactor = 0.4f;
        CostEffective.AttractivenessFactor = 0.4f;
        CostEffective.QualityFactor = 0.2f;

        PriceFirst.Type = "PriceFirst";
        PriceFirst.PriceFactor = 0.8f;
        PriceFirst.AttractivenessFactor = 0.1f;
        PriceFirst.QualityFactor = 0.1f;

        AttractivenessFirst.Type = "AttractivenessFirst";
        AttractivenessFirst.PriceFactor = 0.1f;
        AttractivenessFirst.AttractivenessFactor = 0.6f;
        AttractivenessFirst.QualityFactor = 0.3f;

        QualityFirst.Type = "QualityFirst";
        QualityFirst.PriceFactor = 0.1f;
        QualityFirst.AttractivenessFactor = 0.3f;
        QualityFirst.QualityFactor = 0.6f;

        AddConsumer("CostEffective");
        AddConsumer("PriceFirst");
        AddConsumer("AttractivenessFirst");
        AddConsumer("QualityFirst");
    }
}
