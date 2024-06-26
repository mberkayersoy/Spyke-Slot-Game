#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class ShowResultsWindow : EditorWindow
{
    private ResultData[] _spinResults;
    private Vector2 scrollPosition;
    private int _currentSpin;
    private IDataService _dataService = new JsonDataService();
    [MenuItem("Slot Tools/Show Results")]
    public static void Init()
    {
        GetWindow<ShowResultsWindow>("Show Results");
    }

    private void OnEnable()
    {
        _spinResults = _dataService.LoadData<ResultData[]>(GameConstantData.ALL_SPIN_RESULTS_FILE_PATH, false);
        _currentSpin = _dataService.LoadData<int>(GameConstantData.CURRENT_SPIN_FILE_PATH, false);
    }

    private void OnGUI()
    {
        if (_spinResults == null)
        {
            EditorGUILayout.LabelField("Slot results not found!");
            return;
        }
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        EditorGUILayout.LabelField("Result Indices", EditorStyles.boldLabel);
        foreach (var pair in GetKeyIndices(_spinResults))
        {
            EditorGUILayout.LabelField($"{pair.Key}: {string.Join("-", pair.Value)}");
        }
        EditorGUILayout.Space(20);

        EditorGUILayout.LabelField($"Current Spin: {_currentSpin}", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Results List", EditorStyles.boldLabel);
        for (int i = 0; i < _spinResults.Length; i++)
        {
            EditorGUILayout.BeginHorizontal();
            if (i == _currentSpin)
            {
                EditorGUILayout.LabelField($"Spin {i}:", EditorStyles.boldLabel, GUILayout.Width(50));
                EditorGUILayout.LabelField(_spinResults[i].ResultName, EditorStyles.boldLabel);
            }
            else
            {

                EditorGUILayout.LabelField($"Spin {i}:", GUILayout.Width(50));
                EditorGUILayout.LabelField(_spinResults[i].ResultName);
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();
    }

    private Dictionary<string, List<int>> GetKeyIndices(ResultData[] results)
    {
        Dictionary<string, List<int>> keyIndices = new Dictionary<string, List<int>>();

        for (int i = 0; i < results.Length; i++)
        {
            string key = results[i].ResultName;
            if (!keyIndices.ContainsKey(key))
            {
                keyIndices[key] = new List<int>();
            }
            keyIndices[key].Add(i);
        }

        return keyIndices;
    }
}
#endif