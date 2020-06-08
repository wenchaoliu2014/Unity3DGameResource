using System.Collections.Generic;
using UnityEngine;

namespace GameResource.abAPI
{
    //AbRequest
    public class AbRequest : MonoBehaviour
    {
        public delegate void ErrorCallbackDelegate(string path, string error); //error callback

        public delegate void FinishCallbackDelegate(Dictionary<string, AssetBundle> res); //finish callback

        private const int MaxLoaderNum = 20; //max loader num
        private ErrorCallbackDelegate _mDelErrorCallback; //error callback

        private FinishCallbackDelegate _mDelFinishCallback; //finish callback
        private int _mICompleteNum; //loading complete num
        private int _mILoadIndex; //index of the load list
        private readonly List<AssetBundleLoader> _mLstLoader = new List<AssetBundleLoader>(); //list loader
        private readonly List<string> _mLstPath = new List<string>(); //the path of the path

        private readonly Dictionary<string, AssetBundle>
            _mMapRes = new Dictionary<string, AssetBundle>(); //the resource map

        public float Progress
        {
            get
            {
                float sum = 0;
                foreach (var abl in _mLstLoader)
                {
                    sum += abl.Progress;
                }

                return sum / _mLstLoader.Count;
            }
        }

        //create request
        public static AbRequest Create(FinishCallbackDelegate finishCallback = null,
            ErrorCallbackDelegate errorCallback = null)
        {
            var go = new GameObject("AbRequest");
            var req = go.AddComponent<AbRequest>();
            req._mDelFinishCallback = finishCallback;
            req._mDelErrorCallback = errorCallback;
            req._mILoadIndex = 0;
            req._mICompleteNum = 0;
            req._mLstPath.Clear();
            req._mLstLoader.Clear();
            req._mMapRes.Clear();
            return req;
        }

        //request
        public void Request(string path)
        {
            if (_mMapRes.ContainsKey(path)) return;
            _mLstPath.Add(path);
            _mMapRes.Add(path, null);
        }

        //disport
        public void Disport()
        {
            _mLstLoader.Clear();
            foreach (var item in _mMapRes) item.Value.Unload(false);

            _mMapRes.Clear();
            DestroyImmediate(gameObject);
        }

        //get asset bundle
        public AssetBundle GetAssetBundle(string keyName)
        {
            return _mMapRes.ContainsKey(keyName) ? _mMapRes[keyName] : null;
        }

        //error callback
        private void ErrorCallback(string path, string error)
        {
            _mDelErrorCallback?.Invoke(path, error);
        }

        //finish callback
        private void FinishCallback(string path, AssetBundle obj)
        {
            _mMapRes[path] = obj;
            _mICompleteNum++;
            if (_mICompleteNum != _mLstPath.Count) return;
            if (_mDelFinishCallback == null) Debug.LogError("The finish callback is null.");
            else _mDelFinishCallback(_mMapRes);
        }

        //update
        private void Update()
        {
            if (_mICompleteNum != _mILoadIndex) return;
            if (_mILoadIndex >= _mLstPath.Count) return;
            for (var i = 0; i < MaxLoaderNum && _mILoadIndex + i < _mLstPath.Count; i++)
            {
                var loader = AssetBundleLoader.LoadWww(_mLstPath[_mILoadIndex + i],
                    FinishCallback, ErrorCallback);
                _mLstLoader.Add(loader);
            }

            _mILoadIndex += MaxLoaderNum;
            if (_mILoadIndex > _mLstPath.Count)
                _mILoadIndex = _mLstPath.Count;
        }
    }
}