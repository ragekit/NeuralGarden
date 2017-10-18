using UnityEngine;

using System.Collections;
using System.Collections.Generic; 

 public struct SimpleTransform{
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

    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
}

public struct DrawingState{
    public SimpleTransform transform;
    public float thau;
    public float phi;
    public float thauIncrement;
    public float phiIncrement;

    public void Reset(){
        transform.rotation = Quaternion.identity;
        thau = phi = thauIncrement = phiIncrement = 0;
        transform.scale = Vector3.one;
        transform.position = Vector3.zero;
    }
}
public class LSystem : MonoBehaviour {

    //public string alphabet;
    public Dictionary<char,string> rules;

    [HideInInspector]
    public string state = "0";
    GameObject root;
    public GameObject branch;
    public GameObject leaf;
    public static char[] variables = new char[]{'F','0','[',']','t','T','p','P','/'};
    [HideInInspector]
    public int iterations;
    public char axiom;
    [HideInInspector]
    public DrawingState ds;
    Stack<DrawingState> saveStack;


	// Use this for initialization

    //F forward
    //0 end
    //[ ] push//pop
    //t T increase thau - + by thauincrement
    //p P increase phi - + by phiincrement
    // * reset rotation (not imp)
    // / apply rotation



	void Start () {
        iterations = 2;
        rules = new Dictionary<char, string>();
        root = this.gameObject;
        saveStack = new Stack<DrawingState>();
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

    private void build()
    {
       saveStack.Clear();
        for (int i = 0; i < state.Length; i++)
        {
            GameObject go;

            switch (state[i])
            {
                case '0':
                    go = makeModule(leaf);
                    ds.transform.AssignToGameObject(go);
                    ds.transform.position = go.transform.position;
                    break;
                case 'F':
                    go = makeModule(branch);
                    ds.transform.AssignToGameObject(go);
                   // Debug.Log("///" + ds.transform.position);
                    ds.transform.position = go.transform.position + go.transform.up * 1;
                   // Debug.Log(ds.transform.position);
                    break;
                case '[':
                    saveStack.Push(ds);
                    break;
                case ']':
                    if(saveStack.Count>0){
                        ds = saveStack.Pop();
                    }
                    break;
                case 'T':
                    ds.thau += ds.thauIncrement;
                    break;
                case 't':
                    ds.thau -= ds.thauIncrement;
                    break;
                case 'P':
                    ds.phi += ds.phiIncrement;
                    break;
                case 'p':
                    ds.phi -= ds.phiIncrement;
                    break;
                case '/':
                    
                    //Vector3 toward = MathUtil.ComputeCartesian(1,ds.thau,ds.phi);
                    Vector3 localUp = ds.transform.rotation * Vector3.up;
                    Vector3 localRight = ds.transform.rotation * Vector3.right;
                    ds.transform.rotation *= Quaternion.AngleAxis(ds.phi,localUp) *Quaternion.AngleAxis(ds.thau,localRight);

                break;
            }

        }
    }

    GameObject makeModule(GameObject prefab) 
    {
        
        GameObject go = Instantiate(prefab);

        go.name = "test";
        go.transform.localScale = ds.transform.scale;
        go.transform.parent = root.transform;
        return go;

    }

    void grow(int level = 0)
    {
        state = "" + axiom;
        for (int j = 0; j < level; j++)
        {
            string temp = "";

            for (int i = 0; i < state.Length; i++)
            {
                bool rulefound = false;
                foreach (var item in rules)
                {
                    if (state[i] == item.Key)
                    {
                        rulefound = true;
                        temp += item.Value;
                    }
                }
                if (!rulefound)
                {
                    temp += state[i];
                }
            }
            state = temp;
        }
       Debug.Log(state);
    }

    public void Regenerate(){
        clearScene();
        grow(iterations);
        build();
    }
    
	// Update is called once per frame
	void Update () {
      
	}

    public override string ToString(){

        string ret = "rules : \n";
        foreach (var item in rules)
        {
            ret += item.Key + " ::: " + item.Value +"\n";
        }

        ret += "Axiom : " + axiom;
        ret += "\nIteration " + iterations;


        return ret;
    }
}
