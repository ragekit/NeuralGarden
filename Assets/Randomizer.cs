using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;
using UnityEditor;

public class Randomizer : MonoBehaviour
{

    // Use this for initialization
    LSystem system;

    public TextAsset data;

    JSONNode dataText;


    void Start()
    {
        system = GetComponent<LSystem>();
        dataText = JSON.Parse(data.text);
        if(dataText == null){
            dataText = new JSONObject();
        }
        StartCoroutine(Testing());
    }

    void Randomize()
    {

        system.iterations = Random.Range(2, 5);
        
        do
        {
            GenerateRules();
        } while (!validate());

    }

    void GenerateRules()
    {
        system.rules.Clear();
        int nbOfRules = Random.Range(1, LSystem.variables.Length);

        List<Module> variables = new List<Module>((Module[])LSystem.variables.Clone());

        Module[] precursors = new Module[nbOfRules];
        for (int i = 0; i < nbOfRules; i++)
        {
            Module precursor = variables.Random();
            variables.Remove(precursor);

            precursors[i] = precursor;
            int ruleLength = Random.Range(1, 10);
            List<Module> output = new List<Module>();

          
            //TODO make rule take into account input parameters ?  

            for (int j = 0; j < ruleLength; j++)
            {
                Module randomModule = new Module(LSystem.variables.Random());
                for (int k = 0; k < randomModule.parameters.Length; k++)
                {
                    randomModule.parameters[k] = Random.Range(0f,360f);
                }
                output.Add(randomModule);
            }

            system.rules.Add(precursor, output.ToArray());
        }

        system.axiom = precursors.Random();

    }

    bool validate(){
        
        foreach (var item in system.rules)
        {
            foreach (var module in item.Value)
            {
                //just check if the rules output at least get an F
                if(module.symbol == 'F'){
                    return true;
                }
            }
        }

        return false;
    }

    IEnumerator Testing()
    {
        while (true)
        {
            yield return null;
            Randomize();
            system.Regenerate();
            Camera.main.GetComponent<FitToScreen>().Fit();
            Debug.Log(system);
            while (!Input.GetKeyDown(KeyCode.Y) && !Input.GetKeyDown(KeyCode.N))
            {
                yield return null;
            }
            writeData(Input.GetKeyDown(KeyCode.Y) ? true : false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void writeData(bool accept)
    {

        JSONObject plant = new JSONObject();
        JSONObject drawingData = new JSONObject();

        plant.Add("iteration", system.iterations);
        plant.Add("output", system.Output);
        plant.Add("accept", accept ? 1 : 0);

        dataText["data"].Add(plant);
        File.WriteAllText(AssetDatabase.GetAssetPath(data), dataText.ToString(5));

        Debug.Log(dataText["data"].Count);
    }
}
