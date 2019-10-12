using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
public class Test : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        StartCoroutine(LoadAsset());
    }
    IEnumerator LoadAsset()
    {
        UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle("http://192.168.3.6:8998/Assetbundle/p");
        yield return uwr.SendWebRequest();
        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(uwr);
        var loadAsset = bundle.LoadAssetAsync<GameObject>("Assets/Image/p.prefab");
        yield return loadAsset;
        Debug.Log(loadAsset.asset.name);
        Instantiate(loadAsset.asset);

    }
}
