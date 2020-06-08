using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using GameResource;
using GameResource.abAPI;


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
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < this.paths.Count; i++)
        {
//            GameObject.Instantiate(res[paths[i]].mainAsset);
            sb.Clear();
            var index = paths[i].LastIndexOf("/", StringComparison.Ordinal);
            sb.Append(paths[i].Substring(index + 1));
            GameObject prefab = res[paths[i]].LoadAsset<GameObject>(sb.ToString());
            Instantiate(prefab);
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

        if (GUI.Button(new Rect(30, 50, 50, 50), "clear"))
        {
            this.req.Disport();
        }
    }
}