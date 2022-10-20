using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LanguageEditor : EditorWindow
{
    [MenuItem("Tools/Language Editor")]
    public static void OpenWindow()
    {
        GetWindow<LanguageEditor>();
    }

    private Vector2 _scrollView;
    private int _selectedLanguageIndex;

    private void OnGUI()
    {
        titleContent = new("Language Editor");
        
        GUILayout.Label("Language Editor", EditorStyles.whiteLargeLabel);

        string[] languageAssets = AssetDatabase.FindAssets("t:Language");
        string[] languageNames = new string[languageAssets.Length];
        Language[] languages = new Language[languageAssets.Length];

        for (int i = 0; i < languageAssets.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(languageAssets[i]);
            Language asset = AssetDatabase.LoadAssetAtPath<Language>(assetPath);

            languages[i] = asset;
            languageNames[i] = asset.languageName;
        }
        
        _selectedLanguageIndex = EditorGUILayout.Popup("Language", 
            _selectedLanguageIndex,
            languageNames);

        Language selectedLanguage = languages[_selectedLanguageIndex];

        selectedLanguage.languageName = EditorGUILayout.TextField("Name", 
            selectedLanguage.languageName);

        EditorGUILayout.BeginHorizontal();
        
        GUILayout.Label($"Strings (Count: {selectedLanguage.strings.Count})");
        GUILayout.Button("+", GUILayout.Width(30f));
        
        EditorGUILayout.EndHorizontal();

        _scrollView = EditorGUILayout.BeginScrollView(_scrollView);

        foreach (string key in selectedLanguage.strings.Keys)
        {
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.LabelField(key, selectedLanguage.strings[key]);
            
            GUILayout.Button("E", GUILayout.Width(30f));
            GUILayout.Button("-",GUILayout.Width(30f));
            
            EditorGUILayout.EndHorizontal();
        }

        // for (int i = 0; i < 100; i++)
        // {
        //     EditorGUILayout.BeginHorizontal();
        //     
        //     EditorGUILayout.LabelField("KEY_" + i, "Valore");
        //     
        //     GUILayout.Button("E", GUILayout.Width(30f));
        //     GUILayout.Button("-",GUILayout.Width(30f));
        //     
        //     EditorGUILayout.EndHorizontal();
        // }
        
        EditorGUILayout.EndScrollView();
        
    }
}
