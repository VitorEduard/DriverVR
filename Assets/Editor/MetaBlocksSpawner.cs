using UnityEditor;
using UnityEngine;
using System.IO;

public class MetaBlocksSpawner : EditorWindow
{
    private Vector2 scrollPosition;
    private GameObject[] blockPrefabs;

    [MenuItem("Window/Meta Blocks Spawner")]
    public static void ShowWindow()
    {
        GetWindow<MetaBlocksSpawner>("Meta Blocks");
    }

    private void OnEnable()
    {
        LoadBlocks();
    }

    private void LoadBlocks()
    {
        string blocksPath = "Packages/com.meta.xr.sdk.interaction.ovr/Editor/Blocks"; // Caminho dos blocks
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { blocksPath });

        blockPrefabs = new GameObject[guids.Length];
        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            blockPrefabs[i] = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
        }
    }

    private void OnGUI()
    {
        if (blockPrefabs == null || blockPrefabs.Length == 0)
        {
            GUILayout.Label("Nenhum block encontrado!", EditorStyles.boldLabel);
            return;
        }

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        foreach (var prefab in blockPrefabs)
        {
            if (prefab == null) continue;

            if (GUILayout.Button(prefab.name, GUILayout.Height(30)))
            {
                InstantiatePrefabInScene(prefab);
            }
        }

        GUILayout.EndScrollView();
    }

    private void InstantiatePrefabInScene(GameObject prefab)
    {
        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        if (instance != null)
        {
            Undo.RegisterCreatedObjectUndo(instance, "Spawn Meta Block");
            Selection.activeObject = instance;
        }
    }
}
