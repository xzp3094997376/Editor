using UnityEngine;
using System.Collections;
using System.IO;

[ExecuteInEditMode] 
public class LoadAssetTest : MonoBehaviour
{
    public bool isExecute = false;
    [TextArea]
    public string path="";
    void Start()
    {
        
    }

    void Update()
    {
        if (unLoadAll)
        {
            AssetBundle.UnloadAllAssetBundles(true);
        }
        if (isExecute)
        {
            path = Application.dataPath + "/AB/sphere";
            isExecute = false;
            //StopAllCoroutines();        
           bool hasLoad= LoadedAssets();
            if (!hasLoad)
            {
                StartCoroutine(LoadFromMemoryAsync(path));
            }        
        }
        if (isLoadFromFile) { isLoadFromFile = false;   LoadFromFile(); }

        if (isLoadFromCache)
        {
            isLoadFromCache = false;
            StartCoroutine(LoadFromCacheOrDownload());
        }
    }

    public bool hasLoaded = false;
    public bool unLoadAll = false;
    bool LoadedAssets()
    {
        hasLoaded = false;
        foreach (var item in AssetBundle.GetAllLoadedAssetBundles())
        {
            Debug.Log(item.name);
            var prefab = item.LoadAsset<GameObject>(item.name);
            Instantiate(prefab).name="laoded";
            hasLoaded = true;
        }
        return hasLoaded;
    }
    IEnumerator LoadFromMemoryAsync(string path)
    {
        Debug.Log(path);
   
        AssetBundleCreateRequest createRequest = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(path));
        yield return createRequest;
        AssetBundle bundle = createRequest.assetBundle;
        var prefab = bundle.LoadAsset<GameObject>("sphere");
        Instantiate(prefab);
        Debug.Log(prefab.name);        
    }

    public bool isLoadFromFile = false;
    void LoadFromFile()
    {
        AssetBundle myLoadedAssetBundle = AssetBundle.LoadFromFile(/*Path.Combine(Application.streamingAssetsPath, "myassetBundle")*/Application.dataPath + "/AB/sphere");
        if (myLoadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return;
        }
        var prefab = myLoadedAssetBundle.LoadAsset<GameObject>("sphere");
        Instantiate(prefab).name= "LoadFromFile";
    }


    public bool isLoadFromCache = false;
    IEnumerator LoadFromCacheOrDownload()
    {
        while (!Caching.ready)
            yield return null;

        var www = WWW.LoadFromCacheOrDownload("http://localhost:8090/sphere.assetbundle", 0);
        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log(www.error);
            yield return null;
        }
        var myLoadedAssetBundle = www.assetBundle;

        var asset = myLoadedAssetBundle.LoadAssetAsync(myLoadedAssetBundle.name);
        Debug.Log(asset.allAssets.Length + " _" + asset.asset);
        while (!asset.isDone)
        {
            Debug.Log(asset.progress);
           yield return null;
        }
      
       
        GameObject.Instantiate(asset.asset).name="123";
    }


    /// <summary>
    /// /
    /// </summary>
    void LoadDepend()
    {

    }


}