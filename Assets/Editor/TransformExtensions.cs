using UnityEditor;
using UnityEngine;
/// <summary>
/// 1.8，1.8
/// </summary>
public class TransformExtensions : Editor
{
    static Vector3 sourcePos;
    static Vector3 targetPos;
    [MenuItem("CONTEXT/Transform/拷贝世界坐标", false, 22)]
    static void CopyTransformPosition()
    {
        sourcePos = Selection.activeGameObject.transform.position;
    }
    [MenuItem("CONTEXT/Transform/粘贴世界坐标", false, 22)]
    static void PasteTransformPosition()
    {
        Selection.activeGameObject.transform.position = sourcePos;
    }
}
