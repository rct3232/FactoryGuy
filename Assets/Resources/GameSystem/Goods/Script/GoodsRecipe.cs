using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsRecipe : MonoBehaviour
{
    public class Recipe
    {
        public Recipe() {}
        public int ObjectID;
        public GameObject GoodsObject;
        public string OutputName;
        public string[] InputName;
        public string RequiredProcessor;
        public Attractiveness Attractiveness;
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
    GameObject[] GoodsArr;
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
        string resultType = "None";
        List<int> SlotObjectID = new List<int>();
        List<string> OriginalType = new List<string>();
        string CaseString = null;

        foreach(var Recipe in RecipeArray)
        {
            foreach(var ObjectName in SelectedObject)
            {
                if(Recipe.OutputName == ObjectName)
                {
                    OriginalType.Add(Recipe.GoodsObject.name);
                }
            }
        }

        foreach(var Recipe in RecipeArray)
        {
            foreach(var Type in OriginalType)
            {
                if(Recipe.OutputName == Type)
                {
                    SlotObjectID.Add(Recipe.ObjectID);
                    break;
                }
            }
        }

        SlotObjectID.Sort();

        foreach(var tmp in SlotObjectID)
        {
            foreach(var _tmp in RecipeArray)
            {
                if(tmp == _tmp.ObjectID)
                {
                    CaseString += _tmp.GoodsObject.name + ".";
                    break;
                }
            }
        }

        // caseOfObject = int.Parse(tmpString);
        switch(CaseString)
        {
            case "Wafer.Plastic Panel." : resultType = "Transister"; break;
            case "Mother Board.Transister." : resultType = "Calculating Circuit"; break;
            case "Plastic Panel.Calculating Circuit." : resultType = "Calculator"; break;
            case "Box.Calculator." : resultType = "Packaged Calculator"; break;
        }

        return resultType;
    }

    public Attractiveness CalculateAttractiveness(string[] SelectedObject, string requiredProcessor)
    {
        GoodsRecipe.Attractiveness attractiveness = new GoodsRecipe.Attractiveness();
        if(requiredProcessor != null)
        {
            string ProcessorType = requiredProcessor.Split('?')[0];
            string ProcessorName = requiredProcessor.Split('?')[1];
            switch(ProcessorName)
            {
                case "Dummy" :
                    attractiveness.TechPoint = 1f;
                    break;
            }
            if(ProcessorType == "Assembler") attractiveness.TechPoint += 2;
        }

        float sumMaterialPoints = 0f;
        bool isPackaged = false;

        foreach(var ObjectName in SelectedObject)
        {
            foreach(var ExistRecipe in RecipeArray)
            {
                if(ExistRecipe.OutputName == ObjectName)
                {
                    if(ExistRecipe.GoodsObject.name != "Box")
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

    public void AddRecipeArray(string objectName, string Name, string[] inputName, string requiredProcessor, Attractiveness attractiveness)
    {
        Recipe newRecipe = new Recipe();

        foreach(var tmp in GoodsArr)
        {
            if(tmp.name == objectName)
            {
                newRecipe.GoodsObject = tmp;
            }
        }
        if(newRecipe.GoodsObject == null)
        {
            Debug.Log("Fail to Find " + objectName + " in Prefab List");
            newRecipe.GoodsObject = GoodsArr[0];
        }

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

    public void MakeCustomRecipe(string Type, string Name, string[] Input, Attractiveness attractiveness, string CompanyName)
    {
        Recipe newRecipe = new Recipe();

        foreach(var Recipe in RecipeArray)
        {
            if(Recipe.OutputName == Recipe.GoodsObject.name)
            {
                if(Recipe.OutputName == Type)
                {
                    newRecipe.GoodsObject = Recipe.GoodsObject;
                    newRecipe.OutputName = Name;
                    newRecipe.InputName = Input;
                    newRecipe.RequiredProcessor = Recipe.RequiredProcessor;
                    if(attractiveness != null)
                        newRecipe.Attractiveness = attractiveness;
                    else
                        newRecipe.Attractiveness = Recipe.Attractiveness;
                    newRecipe.ObjectID = CurrentID;

                    RecipeArray.Add(newRecipe);
                    CurrentID++;

                    break;
                }
            }
        }
        
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
                Result = Recipe.GoodsObject.name;
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
        Object[] ObjectArr = Resources.LoadAll<Object>("GameSystem/Goods/Object");
        int CurArr = 0;
        GoodsArr = new GameObject[ObjectArr.Length];
        foreach (Object tmp in ObjectArr)
        {
            GoodsArr[CurArr] = tmp as GameObject;
            CurArr++;
        }

        RecipeArray = new List<Recipe>();

        string ObjectName;
        string outputName;
        string[] inputName;
        string requiredProcessor;
        Attractiveness attractiveness;
        
        ObjectName = "Silicon Mass";
        outputName = "Silicon Mass";
        inputName = null;
        requiredProcessor = null;
        attractiveness = new Attractiveness();
        attractiveness.MaterialPoint = 0.5f;
        attractiveness.TechPoint = 0f;
        attractiveness.LookPoint = 0f;
        attractiveness.TotalPoint = attractiveness.MaterialPoint * attractiveness.TechPoint + attractiveness.LookPoint;
        AddRecipeArray(ObjectName, outputName, inputName, requiredProcessor, attractiveness);

        ObjectName = "Plastic Mass";
        outputName = "Plastic Mass";
        inputName = null;
        requiredProcessor = null;
        attractiveness = new Attractiveness();
        attractiveness.MaterialPoint = 0.5f;
        attractiveness.TechPoint = 0f;
        attractiveness.LookPoint = 0.5f;
        attractiveness.TotalPoint = attractiveness.MaterialPoint * attractiveness.TechPoint + attractiveness.LookPoint;
        AddRecipeArray(ObjectName, outputName, inputName, requiredProcessor, attractiveness);

        ObjectName = "Paper Roll";
        outputName = "Paper Roll";
        inputName = null;
        requiredProcessor = null;
        attractiveness = new Attractiveness();
        attractiveness.MaterialPoint = 0.5f;
        attractiveness.TechPoint = 0f;
        attractiveness.LookPoint = 0f;
        attractiveness.TotalPoint = attractiveness.MaterialPoint * attractiveness.TechPoint + attractiveness.LookPoint;
        AddRecipeArray(ObjectName, outputName, inputName, requiredProcessor, attractiveness);

        ObjectName = "Box";
        outputName = "Box";
        inputName = new string[1];
        inputName[0] = "Paper Roll";
        requiredProcessor = "Processor?Dummy";
        attractiveness = CalculateAttractiveness(inputName, requiredProcessor);
        AddRecipeArray(ObjectName, outputName, inputName, requiredProcessor, attractiveness);

        ObjectName = "Wafer";
        outputName = "Wafer";
        inputName = new string[1];
        inputName[0] = "Silicon Mass";
        requiredProcessor = "Processor?Dummy";
        attractiveness = CalculateAttractiveness(inputName, requiredProcessor);
        AddRecipeArray(ObjectName, outputName, inputName, requiredProcessor, attractiveness);

        ObjectName = "Plastic Panel";
        outputName = "Plastic Panel";
        inputName = new string[1];
        inputName[0] = "Plastic Mass";
        requiredProcessor = "Processor?Dummy";
        attractiveness = CalculateAttractiveness(inputName, requiredProcessor);
        AddRecipeArray(ObjectName, outputName, inputName, requiredProcessor, attractiveness);

        ObjectName = "Mother Board";
        outputName = "Mother Board";
        inputName = new string[1];
        inputName[0] = "Plastic Mass";
        requiredProcessor = "Processor?Dummy";
        attractiveness = CalculateAttractiveness(inputName, requiredProcessor);
        AddRecipeArray(ObjectName, outputName, inputName, requiredProcessor, attractiveness);

        ObjectName = "Transister";
        outputName = "Transister";
        inputName = new string[2];
        inputName[0] = "Wafer";
        inputName[1] = "Plastic Panel";
        requiredProcessor = "Assembler?Dummy";
        attractiveness = CalculateAttractiveness(inputName, requiredProcessor);
        AddRecipeArray(ObjectName, outputName, inputName, requiredProcessor, attractiveness);

        ObjectName = "Calculating Circuit";
        outputName = "Calculating Circuit";
        inputName = new string[2];
        inputName[0] = "Transister";
        inputName[1] = "Mother Board";
        requiredProcessor = "Assembler?Dummy";
        attractiveness = CalculateAttractiveness(inputName, requiredProcessor);
        AddRecipeArray(ObjectName, outputName, inputName, requiredProcessor, attractiveness);

        ObjectName = "Calculator";
        outputName = "Calculator";
        inputName = new string[2];
        inputName[0] = "Calculating Circuit";
        inputName[1] = "Plastic Panel";
        requiredProcessor = "Assembler?Dummy";
        attractiveness = CalculateAttractiveness(inputName, requiredProcessor);
        AddRecipeArray(ObjectName, outputName, inputName, requiredProcessor, attractiveness);

        ObjectName = "Packaged Calculator";
        outputName = "Packaged Calculator";
        inputName = new string[2];
        inputName[0] = "Box";
        inputName[1] = "Calculator";
        requiredProcessor = "Assembler?Dummy";
        attractiveness = CalculateAttractiveness(inputName, requiredProcessor);
        AddRecipeArray(ObjectName, outputName, inputName, requiredProcessor, attractiveness);
    }
}
