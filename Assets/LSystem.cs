using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class LSystem : MonoBehaviour {

    //public string alphabet;
    public Dictionary<char,string> rules;

    string state = "0";
    GameObject root;
    public GameObject branch;
    public GameObject leaf;
    struct SimpleTransform{

        

        SimpleTransform(Transform t)
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

    struct DrawingState{
        public SimpleTransform transform;
        public float thau;
        public float phi;
        public float thauIncrement;
        public float phiIncrement;
    }
    DrawingState ds;
    Stack<DrawingState> saveStack;

	// Use this for initialization

    //F forward
    //0 end
    //[ ] push//pop
    //t T increase thau - + by thauincrement
    //p P increase phi - + by phiincrement
    // / apply rotation

	void Start () {
        rules = new Dictionary<char, string>();
        rules.Add('0', "F/0");
      //  rules.Add('F', "");
      //  rules.Add('0', "F[F/tp0]");

        root = this.gameObject;


        saveStack = new Stack<DrawingState>();

        // StartCoroutine(cycle());

    


    }
    private void clearScene()
    {
        Destroy(root);
        root = new GameObject();
        root.name = "root";
    }

    private void build()
    {
        ds.transform = new SimpleTransform();
        ds.transform.scale = new Vector3(1,1,1);
        ds.thau = 2;
        ds.phi = 0;

        ds.thauIncrement = 10;
        ds.phiIncrement = 10;
        saveStack.Clear();



    
        for (int i = 0; i < state.Length; i++)
        {
            GameObject go;

            switch (state[i])
            {
                case '0':
                    go = makeModule(leaf);
                    ds.transform.AssignToGameObject(go);
                    ds.transform.position = go.transform.position + go.transform.up * go.transform.localScale.y;
                    break;
                case 'F':
                    go = makeModule(branch);
                    ds.transform.AssignToGameObject(go);
                    Debug.Log("///" + ds.transform.position);
                    ds.transform.position = go.transform.position + go.transform.up * go.transform.localScale.y;
                    Debug.Log(ds.transform.position);
                    break;
                case '[':
                    saveStack.Push(ds);
                    break;
                case ']':
                    ds = saveStack.Pop();
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
                    
                    Vector3 toward = MathUtil.ComputeCartesian(1,ds.thau,ds.phi);
                    Debug.Log(ds.transform.position + MathUtil.ComputeCartesian(1,ds.thau,ds.phi));
                   // ds.transform.rotation.SetLookRotation(ds.transform.position+Vector3.forward,ds.transform.position+toward);
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
       
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            state="0";
            clearScene();
            grow(5);
            Debug.Log(state);
            
            build();
        }
	}
}
