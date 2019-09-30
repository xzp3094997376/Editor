using System.IO;
using UnityEditor;
using UnityEngine;

public class TestEditor : EditorWindow
{
    [MenuItem("Assets/Prefab/create")]
    public static void Create()
    {
        EditorWindow.GetWindow(typeof(TestEditor));
    }
    string text = "fsfs";
    private void OnGUI()
    {
        GUILayout.Label("预制体名字");
        text = EditorGUILayout.TextField(text);
        if (GUILayout.Button("create"))
        {
            Init();
        }
        if (GUILayout.Button("souceCopy"))//源预制copy
        {
            Copy();
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
    }
    void Init()
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
    /// 复制预制体
    /// </summary>
    void Copy()
    {
        GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/prefabs/abc.prefab");
        PrefabUtility.ConnectGameObjectToPrefab(new GameObject("fsd"), go);
        AssetDatabase.Refresh();
    }
    void FindAsset()
    {
        string[] guids = AssetDatabase.FindAssets("t:Script");
        foreach (string item in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(item);
            Debug.Log(path);
            GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            Instantiate(go);
            AssetDatabase.Refresh();
        }
    }
    void OpenScript()
    {
        //Debug.Log("xzp");        
        AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath("Assets/Scripts/OpenScript.cs", typeof(object)));
    }
}
