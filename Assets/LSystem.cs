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
        public Vector3 RotationIncrement;
        public Vector3 rotationValue;
    }
    DrawingState ds;
    Stack<DrawingState> saveStack;

	// Use this for initialization
	void Start () {
        rules = new Dictionary<char, string>();
        rules.Add('F', "F");
        rules.Add('0', "F[+F0][-F0][+F0][-F0]");
        rules.Add('+', ")+");

        root = this.gameObject;


        saveStack = new Stack<DrawingState>();

        // StartCoroutine(cycle());


        grow(3);
        build();
    
    }
    IEnumerator cycle()
    {
        while (true)
        {
            grow();
            clearScene();
            build();
            yield return new WaitForSeconds(1f);
        }
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
        ds.RotationIncrement = new Vector3(20,0,20);
        ds.rotationValue = new Vector3(20,0,20);
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
                    ds.transform.position = go.transform.position + go.transform.up * go.transform.localScale.y;

                    break;
                case '[':
                    saveStack.Push(ds);
                    break;
                case ']':
                    ds = saveStack.Pop();
                    break;
                case '+':
                    ds.transform.rotation.eulerAngles += ds.rotationValue;
                    break;
                case '-':
                    ds.transform.rotation.eulerAngles -= ds.rotationValue;
                    break;
                case '(':
                    ds.rotationValue += ds.RotationIncrement;
                    break;
                case ')':
                    ds.rotationValue -= ds.RotationIncrement;
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
            build();
        }
	}
}
