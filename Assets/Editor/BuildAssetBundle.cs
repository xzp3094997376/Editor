using UnityEditor;

public class BuildAssetBundle : EditorWindow
{
    [MenuItem("Frame/BuildAB")]
    static void Init()
    {
        BuildAssetBundle abw = (BuildAssetBundle)EditorWindow.GetWindow(typeof(BuildAssetBundle));
        abw.Show();
    }

    void buiild()
    {

    }
}
