using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public struct SimpleTransform
{
    public SimpleTransform(Transform t)
    {
        position = t.position;
        rotation = t.rotation;
        scale = t.localScale;
    }

    public void AssignToGameObject(GameObject go)
    {
        go.transform.position = position;
        go.transform.rotation = rotation;
        go.transform.localScale = scale;
    }

    private SimpleTransform(Vector3 pos, Quaternion rot, Vector3 sc)
    {
        position = pos;
        rotation = rot;
        scale = sc;
    }

    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;


    public static SimpleTransform Identity
    {
        get
        {
            return new SimpleTransform(Vector3.zero, Quaternion.identity, Vector3.one);
        }
    }
}

public class Module
{
    public char symbol;
    public float[] parameters;
    public Module(char symbol, int parameterLength)
    {
        this.symbol = symbol;
        parameters = new float[parameterLength];
    }

    public Module(Module m){
        symbol = m.symbol;
        parameters = new float[m.parameters.Length];
        for (int i = 0; i < parameters.Length; i++)
        {
            parameters[i] = m.parameters[i];
        }
    }

    override public string ToString()
    {
        string ret = "";
        ret += symbol;
        for (int i = 0; i < parameters.Length; i++)
        {
            ret += parameters[i];
            if (i < parameters.Length - 1)
            {
                ret += ",";
            }
        }
        return ret;
    }
}


public class LSystem : MonoBehaviour
{

    //public string alphabet;
    public Dictionary<Module, Module[]> rules;

    [HideInInspector]
    private List<Module> state;
    public string Output
    {
        get
        {
            string ret = "";
            for (int i = 0; i < state.Count; i++)
            {
                ret += state[i];
            }
            return ret;
        }
    }
    GameObject root;
    public GameObject branch;
    public GameObject leaf;
    public static Module[] variables = new Module[] {
        new Module('F',0),
        new Module('N',0),
        new Module('[',0),
        new Module(']',0),
        new Module('R',2)};

    [HideInInspector]
    public int iterations;
    public Module axiom;
    [HideInInspector]
    public SimpleTransform DrawingTransform;
    Stack<SimpleTransform> saveStack;




    // Use this for initialization

    //F forward
    //E end
    //[ ] push//pop
    // RAB rotation of A thau B phi

    //t T increase thau - + by thauincrement DEPRECATED
    //p P increase phi - + by phiincrement DEPRECATED
    // * reset rotation (not imp) DEPRECATED
    // / apply rotation DEPRECATED



    void Start()
    {
        iterations = 2;
        rules = new Dictionary<Module, Module[]>();
        state = new List<Module>();
        root = this.gameObject;
        saveStack = new Stack<SimpleTransform>();
    }

    public void Reset(){
      //  rules.Clear();
       // state.Clear();
    }

       public void Regenerate()
    {
        clearScene();
        grow(iterations);
        build();
    }
    private void clearScene()
    {
        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
        }
        //root = new GameObject();
        // root.name = "root";
    }

    public void BuildFromState(string st)
    {
        // clearScene();
        // state =
        // build();

        //TODO parse string to module;
    }


    private void build()
    {
        saveStack.Clear();

        DrawingTransform = SimpleTransform.Identity;


        for (int i = 0; i < state.Count; i++)
        {
            GameObject go;

            switch (state[i].symbol)
            {
                case 'N':
                    go = makeModule(leaf);
                    DrawingTransform.AssignToGameObject(go);
                    DrawingTransform.position = go.transform.position;
                    break;
                case 'F':
                    go = makeModule(branch);
                    DrawingTransform.AssignToGameObject(go);
                    DrawingTransform.position = go.transform.position + go.transform.up * 1;
                    break;
                case '[':
                    saveStack.Push(DrawingTransform);
                    break;
                case ']':
                    if (saveStack.Count > 0)
                    {
                        DrawingTransform = saveStack.Pop();
                    }
                    break;
                case 'R':
                    Vector3 localUp = DrawingTransform.rotation * Vector3.up;
                    Vector3 localRight = DrawingTransform.rotation * Vector3.right;
                    if (state[i].parameters.Length == 2)
                    {
                        DrawingTransform.rotation *= Quaternion.AngleAxis(state[i].parameters[0], localUp) * Quaternion.AngleAxis(state[i].parameters[1], localRight);
                    }
                    break;
            }

        }
    }

    GameObject makeModule(GameObject prefab)
    {
        GameObject go = Instantiate(prefab);
        go.name = "test";
        go.transform.parent = root.transform;
        return go;
    }

    void grow(int level = 0)
    {
        state.Clear();
        state.Add(axiom);
        List<Module> temp = new List<Module>();
        for (int j = 0; j < level; j++)
        {
            temp.Clear();
            for (int i = 0; i < state.Count; i++)
            {
                bool rulefound = false;
                foreach (var item in rules)
                {
                    if (state[i].symbol == item.Key.symbol)
                    {
                        rulefound = true;

                        temp.AddRange(item.Value);

                    }
                }
                if (!rulefound)
                {
                    temp.Add(state[i]);
                }
            }
            state.AddRange(temp);
        }
    }

 

    // Update is called once per frame
    void Update()
    {

    }



    public override string ToString()
    {

        string ret = "rules : \n";
        foreach (var item in rules)
        {
            ret += item.Key + " ::: ";

            foreach (var ruleoutput in item.Value)
            {
                ret += ruleoutput;
            }
            ret += "\n";
        }

        ret += "Axiom : " + axiom;
        ret += "\nIteration " + iterations;
        ret += "\nOutput : " + Output;
        return ret;
    }
}
