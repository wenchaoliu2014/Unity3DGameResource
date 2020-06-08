using System;
using System.Collections.Generic;
using System.Text;
using GameResource;
using UnityEngine;

namespace example.scripts
{
    public class example_test1 : MonoBehaviour
    {
        private readonly List<string> _paths = new List<string>();
        private AbRequest _req;

        private void Start()
        {
            _req = AbRequest.Create(finish_callback, error_callback);

            var path = "file:///" + Application.dataPath + "/example/bundle_ios/item";
            _paths.Add(path);
            _req.Request(path);

            path = "file:///" + Application.dataPath + "/example/bundle_ios/terrain";
            _paths.Add(path);
            _req.Request(path);

            path = "file:///" + Application.dataPath + "/example/bundle_ios/button";
            _paths.Add(path);
            _req.Request(path);
        }

        // Use this for initialization
        private void finish_callback(Dictionary<string, AssetBundle> res)
        {
            var sb = new StringBuilder();
            foreach (var t in _paths)
            {
                sb.Clear();
                var index = t.LastIndexOf("/", StringComparison.Ordinal);
                sb.Append(t.Substring(index + 1));
                var prefab = res[t].LoadAsset<GameObject>(sb.ToString());
                Instantiate(prefab);
            }
        }

        private static void error_callback(string path, string error)
        {
            Debug.LogError(" path :" + path + "\nerror :" + error);
        }

        // Update is called once per frame
//        private void Update()
//        {
//        }

        private void OnGUI()
        {
            if (_req != null) GUI.Label(new Rect(0, 0, 100, 40), "Progress " + _req.Progress);

            if (GUI.Button(new Rect(30, 50, 50, 50), "clear")) _req.Disport();
        }
    }
}