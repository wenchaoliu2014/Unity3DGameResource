using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameResource;


public class example_test1 : MonoBehaviour
{
    AbRequest req = null;
    List<string> paths = new List<string>();

    void Start()
    {
        this.req = AbRequest.Create(finish_callback, error_callback);

        string path = "file:///" + Application.dataPath + "/example/bundle_ios/item";
        this.paths.Add(path);
        req.Request(path);

        path = "file:///" + Application.dataPath + "/example/bundle_ios/terrain";
        this.paths.Add(path);
        req.Request(path);

        path = "file:///" + Application.dataPath + "/example/bundle_ios/button";
        this.paths.Add(path);
        req.Request(path);
    }

    // Use this for initialization

    private void finish_callback(Dictionary<string, AssetBundle> res)
    {
        for (int i = 0; i < this.paths.Count; i++)
        {
//            GameObject.Instantiate(res[paths[i]]);
            GameObject _Prefab = res[paths[i]].LoadAsset<GameObject>(paths[i]);
            GameObject obj = Instantiate(_Prefab, transform, true);
        }

//        this.req.Disport();
    }

    private void error_callback(string path, string error)
    {
        Debug.LogError(" path :" + path + "\nerror :" + error);
    }

    // Update is called once per frame
    void Update()
    {
        //
    }

    void OnGUI()
    {
        if (this.req != null)
        {
            GUI.Label(new Rect(0, 0, 100, 40), "Progress " + this.req.Progress);
        }
    }
}