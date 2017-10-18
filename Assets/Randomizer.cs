using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;
using UnityEditor;

public class Randomizer : MonoBehaviour {

	// Use this for initialization
	LSystem system;

	public TextAsset data;

	JSONNode dataText;

	private DrawingState startingPointDs;

	void Start () {
		system = GetComponent<LSystem>();
		dataText = JSON.Parse(data.text);
		StartCoroutine(Testing());
	}

	void Randomize(){
		        system.ds.Reset();

		system.ds.thau = Random.Range(-180f,180f);
		system.ds.phi = Random.Range(0f,360f);
		system.ds.phiIncrement = Random.Range(-180f,180f);
		system.ds.thauIncrement = Random.Range(-180f,180f);
		system.iterations = Random.Range(1,5);


		LSystem.variables.Random();

		GenerateRules();

		startingPointDs = system.ds;

	}

	void GenerateRules(){
		int nbOfRules = Random.Range(1,LSystem.variables.Length);
		system.rules.Clear();
		List<char> variables = new List<char>(LSystem.variables);
		char[] precursors = new char[nbOfRules];
		for (int i = 0; i < nbOfRules; i++)
		{
			char precursor = variables.Random();
			variables.Remove(precursor);

			precursors[i] = precursor;
			int ruleLength = Random.Range(1,10);
			string rule="";

			for (int j = 0; j < ruleLength; j++)
			{
				rule += LSystem.variables.Random();
			}

			system.rules.Add(precursor,rule);
		}

		system.axiom = precursors.Random();

	}

	IEnumerator Testing(){
		while(true){
			yield return null;
			Randomize();
        	system.Regenerate();
			Camera.main.GetComponent<FitToScreen>().Fit();
			 	while (!Input.GetKeyDown(KeyCode.Y) && !Input.GetKeyDown(KeyCode.N))
				{
					yield return null;
				}
				writeData(Input.GetKeyDown(KeyCode.Y) ? true : false);
				
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void writeData(bool accept){
			
			JSONObject plant = new JSONObject();
			JSONObject drawingData = new JSONObject();


			drawingData.Add("thau",startingPointDs.thau);
			drawingData.Add("phi",startingPointDs.phi);
			drawingData.Add("thauIncrement",startingPointDs.thauIncrement);
			drawingData.Add("phiIncrement",startingPointDs.phiIncrement);
			plant.Add("iteration",system.iterations);
			plant.Add("output",system.state);
			plant.Add("drawingData",drawingData);
			plant.Add("accept",accept ? 1 : 0);

			dataText["data"].Add(plant);
			File.WriteAllText(AssetDatabase.GetAssetPath(data),dataText.ToString(5));

			Debug.Log(dataText["data"].Count);
	}
}
