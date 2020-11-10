using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsRecipe : MonoBehaviour
{
    public class Recipe
    {
        public Recipe() {}
        public int ObjectID;
        public string Type;
        public string OutputName;
        public string[] InputName;
        public string RequiredProcessor;
        public Attractiveness Attractiveness;
    }

    public class BaseRecipe
    {
        public BaseRecipe() {}
        public int ObjectID;
        public string Name;
        public List<string[]> InputName;
        public string RequiredProcessor;
    }

    public class Attractiveness
    {
        public Attractiveness() {}
        public float TotalPoint;
        public float MaterialPoint;
        public float TechPoint;
        public float LookPoint;
        public bool isPackaged = false;
        public float PerfectionPoint = 0;
    }

    public List<Recipe> RecipeArray;
    public List<BaseRecipe> BaseRecipeList;
    int CurrentID;
    CompanyManager CompanyManagerCall;

    void Awake()
    {
        InitializingArray();
    }

    void Start()
    {
        CompanyManagerCall = GameObject.Find("CompanyManager").GetComponent<CompanyManager>();
        CurrentID = 0;
    }

    public string getNewGoodsType(List<string> SelectedObject)
    {        
        if (SelectedObject.Count == 0) return "";

        List<string> InputType = new List<string>();

        foreach(var name in SelectedObject)
        {
            foreach(var Recipe in RecipeArray)
            {
                if(Recipe.OutputName == name)
                {
                    foreach(var Base in BaseRecipeList)
                    {
                        if(Base.Name == Recipe.Type)
                        {
                            InputType.Add(Base.Name);
                            break;
                        }
                    }
                    break;
                }
            }
        }

        InputType.Sort();

        foreach(var Base in BaseRecipeList)
        {
            foreach(var Inputs in Base.InputName)
            {
                if (Inputs[0] == "None") continue;

                List<string> InputList = new List<string>();
                InputList.AddRange(Inputs);

                bool Equal = true;
                for(int i = 0; i < InputType.Count; i++)
                {
                    if (i >= InputList.Count) break;

                    if (InputType[i] != InputList[i])
                    {
                        Equal = false;
                        break;
                    }
                }

                if (Equal) return Base.Name;
            }
        }

        return "";
    }

    public Attractiveness CalculateAttractiveness(string[] SelectedObject, string requiredProcessor)
    {
        TechRecipe CallTechRecipe = GameObject.Find("BaseSystem").GetComponent<TechRecipe>();

        GoodsRecipe.Attractiveness attractiveness = new GoodsRecipe.Attractiveness();
        if(requiredProcessor != null)
        {
            string ProcessorName = requiredProcessor.Split('?')[0];
            string ActorName = requiredProcessor.Split('?')[1];
            foreach(var Actor in CallTechRecipe.ActorList)
            {
                if(Actor.Name == ActorName)
                {
                    attractiveness.TechPoint = Actor.TechPoint;
                    break;
                }
            }
            foreach(var Processor in CallTechRecipe.ProcessorList)
            {
                if(Processor.Name == ProcessorName)
                {
                    attractiveness.TechPoint += Processor.PerformanceQuality;
                }
            }
        }

        float sumMaterialPoints = 0f;
        bool isPackaged = false;

        foreach(var ObjectName in SelectedObject)
        {
            foreach(var ExistRecipe in RecipeArray)
            {
                if(ExistRecipe.OutputName == ObjectName)
                {
                    if(ExistRecipe.Type != "Box")
                        sumMaterialPoints += ((ExistRecipe.Attractiveness.MaterialPoint + ExistRecipe.Attractiveness.TechPoint) / 2f);
                    else
                        isPackaged = true;
                    attractiveness.LookPoint += ExistRecipe.Attractiveness.LookPoint;
                    break;
                }
            }
        }

        if(isPackaged)
        {
            attractiveness.MaterialPoint = sumMaterialPoints / (SelectedObject.Length - 1);
            attractiveness.isPackaged = true;
        }
        else
        {
            attractiveness.MaterialPoint = sumMaterialPoints / SelectedObject.Length;    
        }

        attractiveness.TotalPoint = attractiveness.MaterialPoint * attractiveness.TechPoint + attractiveness.LookPoint;
        attractiveness.PerfectionPoint = 0;

        return attractiveness;
    }

    public void AddRecipeArray(string Type, string Name, string[] inputName, string requiredProcessor, Attractiveness attractiveness)
    {
        Recipe newRecipe = new Recipe();
        
        newRecipe.Type = Type;

        newRecipe.OutputName = Name;
        if(inputName == null)
        {
            newRecipe.InputName = null;
        }
        else
        {
            newRecipe.InputName = new string[inputName.Length];
            for(int i = 0; i < inputName.Length; i++)
            {
                newRecipe.InputName[i] = inputName[i];
            }
        }
        newRecipe.RequiredProcessor = requiredProcessor;
        newRecipe.Attractiveness = attractiveness;
        newRecipe.ObjectID = CurrentID;

        RecipeArray.Add(newRecipe);
        CurrentID++;
    }

    public void MakeCustomRecipe(LabatoryAct.DevelopingProduct ResultObject, string CompanyName)
    {
        Recipe newRecipe = new Recipe();
        
        newRecipe.Type = ResultObject.ObjectInfo.Type;
        newRecipe.OutputName = ResultObject.Name;
        newRecipe.InputName = ResultObject.ObjectInfo.Input.ToArray();
        newRecipe.RequiredProcessor = ResultObject.ObjectInfo.RequiredProcessor;
        newRecipe.Attractiveness = ResultObject.ObjectInfo.Attractiveness;

        newRecipe.ObjectID = CurrentID;

        RecipeArray.Add(newRecipe);
        CurrentID++;
        
        if(newRecipe != null)
        {
            CompanyManagerCall.GetCompanyValue(CompanyName).GetTechValue().GetComponent<TechValue>().AddRecipe(newRecipe, CompanyName);
        }
    }

    public string getObjectType(string OutputName)
    {
        string Result = "None";

        foreach(var Recipe in RecipeArray)
        {
            if(Recipe.OutputName == OutputName)
            {
                Result = Recipe.Type;
                break;
            }
        }

        return Result;
    }

    public Recipe GetRecipe(string OutputName)
    {
        Recipe Result = null;

        foreach(var Recipe in RecipeArray)
        {
            if(Recipe.OutputName == OutputName)
            {
                Result = Recipe;
                break;
            }
        }

        return Result;
    }

    void InitializingArray()
    {
        string[] FieldName;
        List<string[]> DataList;

        BaseRecipeList = new List<BaseRecipe>();
        FieldName = new string[] {"Type", "Input", "Processor"};
        DataList = new List<string[]>();

        int CurArr = 0;

        xmlReader.xmlReaderAccess.ReadXml("Data/Tech/XML/Recipeinfo", "RecipeInfo/Recipe", FieldName, DataList);

        foreach(string[] Data in DataList)
        {
            BaseRecipe newRecipe = new BaseRecipe();
            newRecipe.ObjectID = CurArr++;
            newRecipe.Name = Data[0];
            newRecipe.InputName = new List<string[]>();
            string[] InputStringArray = Data[1].Split(';');
            foreach(var Inputs in InputStringArray)
            {
                List<string> InputList = new List<string>();
                InputList.AddRange(Inputs.Split(','));
                InputList.Sort();
                newRecipe.InputName.Add(InputList.ToArray());
            }
            newRecipe.RequiredProcessor = Data[2];

            BaseRecipeList.Add(newRecipe);
        }

        RecipeArray = new List<Recipe>();

        string ObjectName;
        string outputName;
        string[] inputName;
        string requiredProcessor;
        Attractiveness attractiveness;
        
        ObjectName = "Silicon mass";
        outputName = "Silicon mass";
        inputName = null;
        requiredProcessor = null;
        attractiveness = new Attractiveness();
        attractiveness.MaterialPoint = 0.5f;
        attractiveness.TechPoint = 0f;
        attractiveness.LookPoint = 0f;
        attractiveness.TotalPoint = attractiveness.MaterialPoint * attractiveness.TechPoint + attractiveness.LookPoint;
        AddRecipeArray(ObjectName, outputName, inputName, requiredProcessor, attractiveness);

        ObjectName = "Plastic mass";
        outputName = "Plastic mass";
        inputName = null;
        requiredProcessor = null;
        attractiveness = new Attractiveness();
        attractiveness.MaterialPoint = 0.5f;
        attractiveness.TechPoint = 0f;
        attractiveness.LookPoint = 0.5f;
        attractiveness.TotalPoint = attractiveness.MaterialPoint * attractiveness.TechPoint + attractiveness.LookPoint;
        AddRecipeArray(ObjectName, outputName, inputName, requiredProcessor, attractiveness);

        ObjectName = "Steel mass";
        outputName = "Steel mass";
        inputName = null;
        requiredProcessor = null;
        attractiveness = new Attractiveness();
        attractiveness.MaterialPoint = 0.5f;
        attractiveness.TechPoint = 0f;
        attractiveness.LookPoint = 0f;
        attractiveness.TotalPoint = attractiveness.MaterialPoint * attractiveness.TechPoint + attractiveness.LookPoint;
        AddRecipeArray(ObjectName, outputName, inputName, requiredProcessor, attractiveness);

        ObjectName = "Copper mass";
        outputName = "Copper mass";
        inputName = null;
        requiredProcessor = null;
        attractiveness = new Attractiveness();
        attractiveness.MaterialPoint = 0.5f;
        attractiveness.TechPoint = 0f;
        attractiveness.LookPoint = 0f;
        attractiveness.TotalPoint = attractiveness.MaterialPoint * attractiveness.TechPoint + attractiveness.LookPoint;
        AddRecipeArray(ObjectName, outputName, inputName, requiredProcessor, attractiveness);

        ObjectName = "Paper roll";
        outputName = "Paper roll";
        inputName = null;
        requiredProcessor = null;
        attractiveness = new Attractiveness();
        attractiveness.MaterialPoint = 0.5f;
        attractiveness.TechPoint = 0f;
        attractiveness.LookPoint = 0f;
        attractiveness.TotalPoint = attractiveness.MaterialPoint * attractiveness.TechPoint + attractiveness.LookPoint;
        AddRecipeArray(ObjectName, outputName, inputName, requiredProcessor, attractiveness);

        ObjectName = "Chemical mix";
        outputName = "Chemical mix";
        inputName = null;
        requiredProcessor = null;
        attractiveness = new Attractiveness();
        attractiveness.MaterialPoint = 0.5f;
        attractiveness.TechPoint = 0f;
        attractiveness.LookPoint = 0f;
        attractiveness.TotalPoint = attractiveness.MaterialPoint * attractiveness.TechPoint + attractiveness.LookPoint;
        AddRecipeArray(ObjectName, outputName, inputName, requiredProcessor, attractiveness);
    }
}
