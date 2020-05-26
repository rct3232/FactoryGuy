using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandValue : MonoBehaviour
{
    class LandInfo
    {
        public LandInfo() {}
        public string LandStatus;
        public int LandValue;
    }
    List<LandInfo> LandArray = new List<LandInfo>();
    EconomyValue EconomyValueCall;
    TimeManager TimeManagerCall;
    CompanyManager CompanyManagerCall;
    CompanyValue CompanyValueCall;
    public int ActivatedLandCount = 0;
    public int FactoryAffectLandIndex = -1;
    public bool AllLandBought = false;
    public int ValueModificationRef = 2;
    int LandValueStandard = 4000;

    void Awake()
    {
        for(int i = 0; i < (int)Mathf.Pow(TopValue.TopValueSingleton.MapSize, 2); i++)
        {
            LandInfo newInfo = new LandInfo();
            newInfo.LandStatus = "GrassLand";
            newInfo.LandValue = LandValueStandard;            

            LandArray.Add(newInfo);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        CompanyManagerCall = GameObject.Find("CompanyManager").GetComponent<CompanyManager>();
        CompanyValueCall = transform.parent.gameObject.GetComponent<CompanyValue>();
        EconomyValueCall = CompanyValueCall.GetEconomyValue().GetComponent<EconomyValue>();
        TimeManagerCall = GameObject.Find("TimeManager").GetComponent<TimeManager>();

        EconomyValueCall.AddPersistHistory(TimeManagerCall.GetNextMonth(0), TimeManagerCall.Month, "Real Estate", "Land Tax", "Land Tax", 0);
        // EconomyValueCall.AddHistory(TimeManagerCall.TimeValue, "Real Estate", "Land Tax", "Land Tax", 0, true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // if(TimeManagerCall.TimeValue < TimeManagerCall.PlaySpeed)
        // {
        //     LandValueChainReaction();
        //     ManageLandStatus();
        // }

        if(TimeManagerCall.TimeValue % TimeManagerCall.Day < TimeManagerCall.PlaySpeed)
        {
            LandValueChainReaction();
            ManageLandStatus();
        }
    }

    void ManageLandStatus()
    {
        for(int i = 0; i < LandArray.Count; i++)
        {
            if(LandArray[i].LandStatus == "Factory")
                continue;
            
            if(LandArray[i].LandValue >= LandValueStandard +3000)
            {
                if(LandArray[i].LandStatus != "City")
                {
                    LandArray[i].LandStatus = "City";
                    if(CompanyManagerCall.PlayerCompanyName == CompanyValueCall.CompanyName)
                    {
                        GameObject.Find("BaseSystem").GetComponent<GroupActivation>().ChangeGroupStruct((int)(i / TopValue.TopValueSingleton.MapSize), i % TopValue.TopValueSingleton.MapSize,
                            "City" + Random.Range(1,3).ToString());
                    }
                }
            }
            else if(LandArray[i].LandValue >= LandValueStandard +1000)
            {
                if(LandArray[i].LandStatus != "Town")
                {
                    LandArray[i].LandStatus = "Town";
                    if(CompanyManagerCall.PlayerCompanyName == CompanyValueCall.CompanyName)
                    {
                        GameObject.Find("BaseSystem").GetComponent<GroupActivation>().ChangeGroupStruct((int)(i / TopValue.TopValueSingleton.MapSize), i % TopValue.TopValueSingleton.MapSize,
                            "Town" + Random.Range(1,4).ToString());
                    }
                }
            }
            else if(LandArray[i].LandValue >= LandValueStandard +200)
            {
                if(LandArray[i].LandStatus != "Country")
                {
                    LandArray[i].LandStatus = "Country";
                    if(CompanyManagerCall.PlayerCompanyName == CompanyValueCall.CompanyName)
                    {
                        GameObject.Find("BaseSystem").GetComponent<GroupActivation>().ChangeGroupStruct((int)(i / TopValue.TopValueSingleton.MapSize), i % TopValue.TopValueSingleton.MapSize,
                            "Country" + Random.Range(1,4).ToString());
                    }
                }
            }
            else if(LandArray[i].LandValue >= LandValueStandard)
            {
                if(LandArray[i].LandStatus != "GrassLand")
                {
                    LandArray[i].LandStatus = "GrassLand";
                    if(CompanyManagerCall.PlayerCompanyName == CompanyValueCall.CompanyName)
                    {
                        GameObject.Find("BaseSystem").GetComponent<GroupActivation>().ChangeGroupStruct(i / TopValue.TopValueSingleton.MapSize, i % TopValue.TopValueSingleton.MapSize,
                            "GrassLand" + Random.Range(1,6).ToString());
                    }
                }
            }
        }
    }

    public string GetLandStatus(int x, int y)
    {
        return LandArray[(x * TopValue.TopValueSingleton.MapSize) + y].LandStatus;
    }

    public string GetLandStatus(int Index)
    {
        return LandArray[Index].LandStatus;
    }

    public int GetLandValue(int x, int y)
    {
        return LandArray[(x * TopValue.TopValueSingleton.MapSize) + y].LandValue;
    }

    public int GetLandValue(int Index)
    {
        return LandArray[Index].LandValue;
    }

    void LandValueChainReaction()
    {
        for(int i = 0; i < LandArray.Count; i++)
        {
            if(LandArray[i].LandStatus != "Factory")
            {
                for(int j = -1; j < 2; j++)
                {
                    for(int k = -1; j < 2; j++)
                    {
                        if(j != 0 || k != 0)
                        {
                            if((float)LandArray[i].LandValue / (float)LandValueStandard > 1f)
                            {
                                float IncreaseRatio = (float)LandArray[i].LandValue / (float)LandValueStandard;
                                int TargetIndex = i + (j * TopValue.TopValueSingleton.MapSize) + k;
                                if(TargetIndex >= 0 && TargetIndex < LandArray.Count)
                                {
                                    if(LandArray[TargetIndex].LandStatus != "Factory")
                                    {
                                        if(LandArray[TargetIndex].LandValue <= LandArray[i].LandValue)
                                        {
                                            LandArray[TargetIndex].LandValue += Mathf.FloorToInt((float)ValueModificationRef * IncreaseRatio);
                                        }
                                        else
                                        {
                                            LandArray[TargetIndex].LandValue += Mathf.FloorToInt((1f / (float)ValueModificationRef) * IncreaseRatio);
                                        }

                                        // Debug.Log(LandArray[TargetIndex].LandValue);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void ChangeLandStatus(int x, int y, string Status, int ValueDifference)
    {
        LandArray[(x * TopValue.TopValueSingleton.MapSize) + y].LandStatus = Status;
        LandArray[(x * TopValue.TopValueSingleton.MapSize) + y].LandValue += ValueDifference;
    }

    public void ChangeLandValue(int x, int y, int ValueDifference)
    {

    }

    public void ChangeLandValue(int ValueDifference)
    {
        LandArray[FactoryAffectLandIndex].LandValue += ValueDifference;

        if(Random.Range(0, 21) == 0)
        {
            FactoryAffectLandIndex = GetRandomIndexNotFactory(FactoryAffectLandIndex);
        }
    }

    public bool BuyLand(int x, int y)
    {
        if(EconomyValueCall.Balance < LandArray[(x * TopValue.TopValueSingleton.MapSize) + y].LandValue)
        {
            if(CompanyManagerCall.PlayerCompanyName == CompanyValueCall.CompanyName)
            {
                // GameObject.Find("NotificationManager").GetComponent<NotificationManager>().SetNote("Lack of money", true, new Color(1f,0f,0f));
            }

            return false;
        }
        
        EconomyValueCall.AddHistory(TimeManagerCall.TimeValue, "Real Estate", "Buy a " + LandArray[(x * TopValue.TopValueSingleton.MapSize) + y].LandStatus, 
            x + "," + y + " " + LandArray[(x * TopValue.TopValueSingleton.MapSize) + y].LandStatus, - LandArray[(x * TopValue.TopValueSingleton.MapSize) + y].LandValue);
        EconomyValueCall.ModifyPersistHistory("Land Tax", - LandArray[(x * TopValue.TopValueSingleton.MapSize) + y].LandValue / 100);

        ChangeLandStatus(x, y, "Factory", -1000);

        ActivatedLandCount++;

        if(ActivatedLandCount == 1)
        {
            GetFactoryAffectLandIndex(x, y);
        }

        return true;
    }

    int GetRandomIndexNotFactory(int Index)
    {
        int FactoryCount = 0;
        foreach(var Land in LandArray)
        {
            if(Land.LandStatus != "Factory")
                break;
            
            FactoryCount++;
        }

        if(FactoryCount == TopValue.TopValueSingleton.MapSize - 1)
        {
            return Index;
        }
        else if(FactoryCount == TopValue.TopValueSingleton.MapSize)
        {
            AllLandBought = true;
            return -1;
        }

        int Result = (Random.Range(0, TopValue.TopValueSingleton.MapSize) * TopValue.TopValueSingleton.MapSize) + Random.Range(0, TopValue.TopValueSingleton.MapSize);

        if(LandArray[Result].LandStatus != "Factory" && Result != Index)
            return Result;
        else
            return GetRandomIndexNotFactory(Index);
    }

    int GetRandomIndexNotFactory()
    {
        int FactoryCount = 0;
        foreach(var Land in LandArray)
        {
            if(Land.LandStatus != "Factory")
                break;
            
            FactoryCount++;
        }

        if(FactoryCount == TopValue.TopValueSingleton.MapSize)
        {
            AllLandBought = true;
            return -1;
        }

        int Result = (Random.Range(0, TopValue.TopValueSingleton.MapSize) * TopValue.TopValueSingleton.MapSize) + Random.Range(0, TopValue.TopValueSingleton.MapSize);

        if(LandArray[Result].LandStatus != "Factory")
            return Result;
        else
            return GetRandomIndexNotFactory();
    }

    void GetFactoryAffectLandIndex(int x, int y)
    {
        int Index = -1;

        List<int> IndexArray = new List<int>();
        for(int i = 0; i < LandArray.Count; i++)
        {
            IndexArray.Add(i);
        }

        if(TopValue.TopValueSingleton.MapSize > 1)
        {
            for(int i = 0; i < LandArray.Count; i++)
            {
                int IndexArrayIndex = Random.Range(0, IndexArray.Count);
                int IndexX = IndexArray[IndexArrayIndex] / TopValue.TopValueSingleton.MapSize;
                int IndexY = IndexArray[IndexArrayIndex] % TopValue.TopValueSingleton.MapSize;

                IndexArray.RemoveAt(IndexArrayIndex);
                
                if(IndexX == x && IndexY == y)
                    continue;

                if((int)Mathf.Abs(x - IndexX) > (TopValue.TopValueSingleton.MapSize / 2 - 1) || (int)Mathf.Abs(y - IndexY) > (TopValue.TopValueSingleton.MapSize / 2 - 1))
                {
                    Index = (IndexX * TopValue.TopValueSingleton.MapSize) + IndexY;
                    break;
                }
            }
        }

        FactoryAffectLandIndex = Index;
    }
}
