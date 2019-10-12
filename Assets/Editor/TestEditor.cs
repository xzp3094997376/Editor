using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TestEditor : EditorWindow
{
    [MenuItem("Assets/Prefab/create")]
    public static void Create()
    {
        EditorWindow.GetWindow(typeof(TestEditor)).Show(true);
    }
    string text = "fsfs";
    private void OnGUI()
    {
        GUILayout.Label("预制体名字");
        text = EditorGUILayout.TextField(text);

        EditorGUILayout.BeginScrollView(Input.mousePosition);

        if (GUILayout.Button("create"))
        {
            CreateObj();
        }
        if (GUILayout.Button("souceCopy"))//源预制copy
        {
            PrefabCopy();
        }
        if (GUILayout.Button("FindAsset"))
        {
            FindAsset();
        }
        if (GUILayout.Button("OpenScript"))
        {
            //FindAsset();
            OpenScript();
        }
        if (GUILayout.Button("copyAsset"))
        {
            //FindAsset();
            CopyAsset();
        }

        if (GUILayout.Button("deleteAsset"))
        {
            //FindAsset();
            DeleteAsset();
        }

        if (GUILayout.Button("exportPack"))
        {
            ExportPackage();
        }

        if (GUILayout.Button("CreateUGUIPrefab"))
        {
            CreateUGUIPrefab();
        }

        EditorGUILayout.EndScrollView();
    }
    /// <summary>
    /// 将物体创建成预制体
    /// </summary>
    void CreateObj()
    {
        string path = "Assets/prefabs";//路径错误 StreamingAssets只可以读
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        path += "/" + text + ".prefab";
        GameObject go = new GameObject(text);
        PrefabUtility.CreatePrefab(path, go);
        AssetDatabase.Refresh();
    }
    /// <summary>
    /// 将预制体属性赋值到场景GameObject属性上  对预制体拷贝
    /// </summary>
    void PrefabCopy()
    {
        GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/prefabs/abc.prefab");
        PrefabUtility.ConnectGameObjectToPrefab(new GameObject("fsd"), go);
        AssetDatabase.Refresh();
    }
    /// <summary>
    /// 查找指定类型的资源并且实例化出来// 可以用词打assetBundle包体
    /// </summary>
    void FindAsset()
    {
        string[] guids = AssetDatabase.FindAssets("t:prefab");
        foreach (string item in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(item);
            Debug.Log(path);
            GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            Instantiate(go);
            AssetDatabase.Refresh();
        }
    }
    /// <summary>
    /// 打开特定脚本
    /// </summary>
    void OpenScript()
    {
        AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath("Assets/Scripts/OpenScript.cs", typeof(object)));
        //AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath("Assets/prefabs/p.png", typeof(object)));
    }
    /// <summary>
    /// 资源拷贝到新的目录
    /// </summary>
    void CopyAsset()
    {
        AssetDatabase.CopyAsset("Assets/prefabs/abc.prefab", Application.streamingAssetsPath + "/prefabs/1.prefab");
    }
    /// <summary>
    /// 删除资源
    /// </summary>
    void DeleteAsset()
    {
        bool delete = AssetDatabase.DeleteAsset("Assets/prefabs/123.prefab");
        Debug.Log(delete);
        AssetDatabase.Refresh();
    }
    /// <summary>
    /// 删除资源
    /// </summary>
    void ExportPackage()
    {
        Object[] objs = EditorUtility.CollectDependencies(Selection.objects);
        Debug.Log(objs.Length);
        List<string> pathList = new List<string>();
        for (int i = 0; i < objs.Length; i++)
        {
            int lid;
            string guid;
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(objs[i], out guid, out lid);//得到文件夹的guid
            string path = AssetDatabase.GUIDToAssetPath(guid);
            pathList.Add(path);
        }
        AssetDatabase.ExportPackage(pathList.ToArray(), "Assets/unityPack/1.unitypackage", ExportPackageOptions.IncludeDependencies);
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/createUGUIPrefab")]
    static void CreateUGUIPrefab()
    {
        Object[] objs = Selection.GetFiltered(typeof(Object), SelectionMode.Deep);
        Debug.Log(objs.Length);
        for (int i = 0; i < objs.Length; i++)
        {
            int lid;
            string guid;
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(objs[i], out guid, out lid);//得到文件夹的guid
            string path = AssetDatabase.GUIDToAssetPath(guid);

            string[] paths = path.Split('.');
            paths[0] += ".prefab";
            GameObject go = PrefabUtility.CreatePrefab(paths[0], new GameObject("prefab")) as GameObject;
            Image img = go.AddComponent<Image>();
            Sprite sp = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            img.sprite = sp;
        }
    }

    [MenuItem("Assets/buildAB")]
    static void BuildAB()
    {

        string abFilePath = "Assets/ABfile";
        if (!Directory.Exists(abFilePath))
        {
            DirectorySecurity ds = new DirectorySecurity("ab", AccessControlSections.Access);
            Directory.CreateDirectory(abFilePath, ds);
        }
        abFilePath += "/abfile.txt";
        if (!File.Exists(abFilePath))
        {
            using (FileStream fs = File.Create(abFilePath))
            {
                fs.Flush();
                fs.Close();
            }
        }
        AssetDatabase.Refresh();


        Object[] objs = Selection.GetFiltered(typeof(object), SelectionMode.DeepAssets);
        Debug.Log(objs.Length);
        for (int i = 0; i < objs.Length; i++)
        {
            int lid;
            string guid;
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(objs[i], out guid, out lid);//得到文件夹的guid
            string path = AssetDatabase.GUIDToAssetPath(guid);

            Debug.Log(path);


            AssetBundleBuild[] buildMap = new AssetBundleBuild[1];
            buildMap[0].assetBundleName = objs[i].name;
            buildMap[0].assetNames = new string[] { path };

            string abPath = "Assets/AB";


            File.WriteAllLines(abFilePath, new string[] { abPath + "/" + objs[i].name + "|" + path });

            BuildPipeline.BuildAssetBundles(abPath, buildMap,
                BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);

            AssetDatabase.Refresh();
        }
    }
    [MenuItem("Assets/loadAsset")]
    static void LoadAsset()
    {
        AssetBundle.UnloadAllAssetBundles(true);

        string filePath = "Assets/ABfile/abfile.txt";
        string[] abPaths = File.ReadAllLines(filePath);
        for (int i = 0; i < abPaths.Length; i++)
        {
            float t = Time.realtimeSinceStartup;
            string[] path_ab = abPaths[i].Split('|');
            AssetBundle ast = AssetBundle.LoadFromFile(path_ab[0]);
            Debug.Log(ast.name);
            //GameObject go = ast.LoadAsset<GameObject>(path_ab[1]);
            //Instantiate<GameObject>(go);
            //TextAsset tx = ast.LoadAsset<TextAsset>(path_ab[1]);
            float cur = Time.realtimeSinceStartup;
            Debug.Log(cur - t);
        }
    }
    [MenuItem("Assets/loadTextAsset")]
    static void LoadTextAsset()
    {
        AssetBundle.UnloadAllAssetBundles(true);

        string filePath = "Assets/ABfile/abfile.txt";
        string[] abPaths = File.ReadAllLines(filePath);
        for (int i = 0; i < abPaths.Length; i++)
        {
            float t = Time.realtimeSinceStartup;
            string[] path_ab = abPaths[i].Split('|');
            AssetBundle ast = AssetBundle.LoadFromFile(path_ab[0]);
            Debug.Log(ast.name);
            //GameObject go = ast.LoadAsset<GameObject>(path_ab[1]);
            //Instantiate<GameObject>(go);
            TextAsset tx = ast.LoadAsset<TextAsset>(path_ab[1]);
            string luaPath = path_ab[1].Split('.')[0] + ".lua";
            Debug.Log(luaPath);
            if (!File.Exists(luaPath))
            {
                using (FileStream fs = File.OpenWrite(luaPath))
                {
                    byte[] luaBytes = Encoding.UTF8.GetBytes(tx.text);
                    fs.Write(luaBytes, 0, luaBytes.Length);
                    fs.Flush();
                    fs.Close();
                }
            }
            AssetDatabase.Refresh();
            float cur = Time.realtimeSinceStartup;
            Debug.Log(cur - t);
        }
    }
}
