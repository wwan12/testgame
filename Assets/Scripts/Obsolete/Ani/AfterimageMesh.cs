using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 3d物体移动残影特效
/// </summary>
public class AfterimageMesh : MonoBehaviour
{
    private SkinnedMeshRenderer[] skinMesh;

    private float alphaValue = 150;
    void Start()
    {
        skinMesh = transform.GetComponentsInChildren<SkinnedMeshRenderer>();
    }
    bool IsRunning = false;
    public float timer, rate = 0.3f;
    void Update()
    {

        IsRunning = gameObject.GetComponent<PlayerManage>().v == 0 ? false : true;
        if (IsRunning)
        {
            timer += Time.deltaTime;
            if (timer > rate)
            {
                timer = 0;
                Plays();
            }
        }
        else
        {
            timer = 0.3f;
        }


    }


    private void Plays()
    {
        for (int i = 0; i < skinMesh.Length; i++)
        {
            Mesh mesh = new Mesh();
            skinMesh[i].BakeMesh(mesh);
            GameObject t = ObjPool.Instance.TackOut("SkinMesh");
            MeshFilter mesh_ = null;
            MeshRenderer meshRenderer = null;
            if (t == null)
            {
                t = new GameObject();
                t.hideFlags = HideFlags.HideAndDontSave;
                mesh_ = t.AddComponent<MeshFilter>();
                meshRenderer = t.AddComponent<MeshRenderer>();
                meshRenderer.material = skinMesh[i].material;
                t.AddComponent<DesT>();
            }
            else
            {
                t.SetActive(true);
                mesh_ = t.GetComponent<MeshFilter>();
            }
            mesh_.mesh = mesh;
            t.transform.position = skinMesh[i].transform.position;
            t.transform.rotation = skinMesh[i].transform.rotation;
        }
    }
}

public class DesT : MonoBehaviour
{
    private float alphaValue = 150;
    private void OnEnable()
    {
        GetComponent<MeshRenderer>().material.shader = Shader.Find("First Fantasy/Water/Water Diffuse");
        GetComponent<MeshRenderer>().material.SetFloat("_IsPlay", 1);
        GetComponent<MeshRenderer>().material.SetColor("_MainTexColor", new Color(1, 0, 0, alphaValue / 255));
        GetComponent<MeshRenderer>().material.SetFloat("_MainTexMultiply", 3.21f);
        gameObject.layer = LayerMask.NameToLayer("Temp");
        alphaValue = 150;
        timer = 0;
        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>("Mask"));
    }

    private float timer, rate = 0.50f;
    void Update()
    {
        timer += Time.deltaTime;
        alphaValue -= Time.deltaTime * 300;
        GetComponent<MeshRenderer>().material.SetColor("_MainTexColor", new Color(1, 0, 0, alphaValue / 255));
        if (timer > rate)
        {
            GetComponent<MeshRenderer>().material.SetFloat("_MainTexMultiply", 0);
            GetComponent<MeshRenderer>().material.SetColor("_MainTexColor", new Color(0, 0, 0, 0));
            gameObject.SetActive(false);
            ObjPool.Instance.Add("SkinMesh", this.gameObject);
            return;
        }
    }
}

public class ObjPool
{
    private Dictionary<string, List<GameObject>> pool;
    private static ObjPool instance;
    public static ObjPool Instance
    {
        get
        {
            if (instance == null)
                instance = new ObjPool();
            return instance;
        }
    }
    private ObjPool()
    {
        pool = new Dictionary<string, List<GameObject>>();
    }

    public void Add(string name, GameObject o)
    {
        if (pool.ContainsKey(name))
        {
            pool[name].Add(o);
            return;
        }
        pool.Add(name, new List<GameObject>());
        pool[name].Add(o);
    }

    public GameObject TackOut(string name)
    {
        GameObject o = null;
        if (!pool.ContainsKey(name))
        {
            pool.Add(name, new List<GameObject>());
            return o;
        }
        if (pool[name].Count == 0)
        {
            return o;
        }
        o = pool[name][0];
        pool[name].RemoveAt(0);
        return o;
    }


}
