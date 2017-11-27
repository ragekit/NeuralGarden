using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
public class LineReader : MonoBehaviour {
	public TextAsset input;
	// Use this for initialization
	string[] generationRules;

	bool dirty;
	LSystem system;
	void Start () {
		generationRules = Regex.Split ( input.text, "\n|\r|\r\n" );
	//	Debug.Log(randomRule);
		dirty = true;
		system = GetComponent<LSystem>();
	}

	void build(){
		Randomize();
		system.BuildFromState(generationRules.Random());

	}

	void Randomize(){
		
		//system.iterations = Random.Range(1,5);


		//LSystem.variables.Random();

		//GenerateRules();

		//startingPointDs = system.ds;

	}
	
	// Update is called once per frame
	void Update () {
		if(dirty){
			build();
			dirty = false;
		}

		if(Input.GetKeyDown(KeyCode.Space)){
			dirty = true;
		}
	}
}
