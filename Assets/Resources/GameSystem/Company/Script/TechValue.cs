using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechValue : MonoBehaviour
{
    public class FacilityState
    {
        public TechRecipe.FacilityRecipe RecipeInfo;
        public bool isCompleted;
        public bool OnResearching;
        public bool PilotFinished;
    }

    public class RecipeInfo
    {
        public GoodsRecipe.Recipe Recipe;
        public string Maker;
        public string Owner;
    }

    ObjInstantiater ObjInstantiaterCall;
    CompanyManager CompanyManagerCall;
    CompanyValue CompanyValueCall;
    TechRecipe TechRecipeCall;
    GoodsRecipe GoodsRecipeCall;
    public List<FacilityState> FacilityArray;
    public List<RecipeInfo> AvailableRecipe;

    void Start()
    {
        ObjInstantiaterCall = GameObject.Find("ObjectInstaller").GetComponent<ObjInstantiater>();
        CompanyManagerCall = GameObject.Find("CompanyManager").GetComponent<CompanyManager>();
        CompanyValueCall = transform.parent.gameObject.GetComponent<CompanyValue>();
        TechRecipeCall = GameObject.Find("BaseSystem").GetComponent<TechRecipe>();
        GoodsRecipeCall = GameObject.Find("BaseSystem").GetComponent<GoodsRecipe>();
        AvailableRecipe = new List<RecipeInfo>();

        Initializing();
    }
    
    void Initializing()
    {
        FacilityArray = new List<FacilityState>();
        FacilityState newTech;

        for(int i = 0; i < TechRecipeCall.FacilityRecipeArray.Count; i++)
        {
            newTech = new FacilityState();
            newTech.RecipeInfo = TechRecipeCall.FacilityRecipeArray[i];
            newTech.isCompleted = false;
            newTech.OnResearching = false;
            newTech.PilotFinished = false;
            if(i == 0)
            {
                newTech.PilotFinished = true;    
            }
            FacilityArray.Add(newTech);
        }

        foreach(var Recipe in GoodsRecipeCall.RecipeArray)
        {
            if(Recipe.InputName == null)
            {
                AddRecipe(Recipe, null);
            }
            else if(Recipe.InputName.Length < 2)
            {
                AddRecipe(Recipe, null);
            }
        }
    }

    public void CompleteResearch(TechRecipe.FacilityRecipe Recipe)
    {
        FacilityState Target = GetTechState(Recipe);

        Target.isCompleted = true;
        Target.OnResearching = false;

        for(int i = 0; i < FacilityArray.Count; i++)
        {
            if(TechRecipeCall.CheckRequiredment(i))
            {
                FacilityArray[i].PilotFinished = true;
            }
        }

        if(CompanyValueCall.CompanyName == CompanyManagerCall.PlayerCompanyName)
        {
            foreach(var tmp in Target.RecipeInfo.UnlockFacility)
            {
                ObjInstantiaterCall.SetUnlockedObject(tmp);
            }
        }
        else
        {
            // Write AI code
        }
    }

    public FacilityState GetTechState(TechRecipe.FacilityRecipe Recipe)
    {
        FacilityState Result = null;

        foreach(var State in FacilityArray)
        {
            if(State.RecipeInfo == Recipe)
            {
                Result = State;
            }
        }

        return Result;
    }

    public void AddRecipe(GoodsRecipe.Recipe Recipe, string CompanyName)
    {
        RecipeInfo newRecipe = new RecipeInfo();

        newRecipe.Recipe = Recipe;
        newRecipe.Maker = CompanyName;
        newRecipe.Owner = CompanyName;

        AvailableRecipe.Add(newRecipe);
    }

    public RecipeInfo GetRecipe(string Name)
    {
        RecipeInfo Result = null;

        foreach(var Recipe in AvailableRecipe)
        {
            if(Recipe.Recipe.OutputName == Name)
            {
                Result = Recipe;
            }
        }
        return Result;
    }
}
