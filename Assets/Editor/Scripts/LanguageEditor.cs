using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
    private string _newKey;
    private string _stringToEdit;
    private string _newValue;

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

        EditorGUI.BeginChangeCheck();
        selectedLanguage.languageName = EditorGUILayout.TextField("Name", 
            selectedLanguage.languageName);
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(selectedLanguage);
        }
        
        GUILayout.Space(30f);

        
        GUILayout.Label($"Strings (Count: {selectedLanguage.strings.Count})", EditorStyles.largeLabel);
        
        
        EditorGUILayout.BeginHorizontal();
        _newKey = EditorGUILayout.TextField("New Key", _newKey);
        
        if (GUILayout.Button("+", GUILayout.Width(30f)))
        {
            // 1) Key non può essere vuota
            // 2) Key non può contenere spazi, solo lettere maiuscole, numeri e _
            // 3) Key deve essere univoca

            string newKey = _newKey.Trim(); // "    PROVA   " -> "PROVA"
            newKey = newKey.Replace(' ', '_'); // "PROVA 1" -> "PROVA_1"
            newKey = newKey.ToUpper(); // "prova" -> "PROVA"
            
            // [A-Z0-9_]+

            if (newKey.Length == 0)
            {
                EditorUtility.DisplayDialog("Errore", 
                    "La chiave non può essere vuota", "Ok");
            }
            else if (!Regex.IsMatch(newKey, "^[A-Z0-9_]+$"))
            {
                EditorUtility.DisplayDialog("Errore",
                    "La chiave deve essere composta solo da numeri, lettere maiuscole e _", "Ok");
            }
            else if (selectedLanguage.strings.ContainsKey(newKey))
            {
                EditorUtility.DisplayDialog("Errore",
                    $"Chiave {newKey} già esistente", "Ok");
            }
            else
            {
                // string.Empty == "" <--- Equivalenti
                selectedLanguage.strings.Add(newKey, string.Empty);
                EditorUtility.SetDirty(selectedLanguage);
            }
        }
        
        EditorGUILayout.EndHorizontal();

        if (selectedLanguage.strings.Count == 0)
        {
            EditorGUILayout.HelpBox("Non ci sono stringhe in questa lingua", 
                MessageType.Info);

            return;
        }

        _scrollView = EditorGUILayout.BeginScrollView(_scrollView);

        string stringToRemove = "";

        foreach (string key in selectedLanguage.strings.Keys)
        {
            EditorGUILayout.BeginHorizontal();

            if (_stringToEdit == key)
            {
                _newValue = EditorGUILayout.TextField(key, _newValue);

                if (GUILayout.Button("Ok", GUILayout.Width(60f)))
                {
                    selectedLanguage.strings[key] = _newValue;
                    _stringToEdit = String.Empty;
                    
                    EditorUtility.SetDirty(selectedLanguage);
                    
                    EditorGUILayout.EndHorizontal();
                    break;
                }
            }
            else
            {
                EditorGUILayout.LabelField(key, selectedLanguage.strings[key]);
                
                if (GUILayout.Button("E", GUILayout.Width(30f)))
                {
                    _stringToEdit = key;
                    _newValue = selectedLanguage.strings[key];
                }
            
                if (GUILayout.Button("-", GUILayout.Width(30f)) && 
                    EditorUtility.DisplayDialog("Conferma", $"Vuoi rimuovere la stringa {key}?", "Sì", "No"))
                {
                    stringToRemove = key;
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        if (stringToRemove.Length > 0)
        {
            selectedLanguage.strings.Remove(stringToRemove);
            EditorUtility.SetDirty(selectedLanguage);
        }

        EditorGUILayout.EndScrollView();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Export"))
        {
            string path = EditorUtility.SaveFilePanel("Export", String.Empty, 
                selectedLanguage.languageName + ".json", "json");

            if (path.Length > 0)
            {
                string json = JsonUtility.ToJson(selectedLanguage.GetExportedLanguage());

                Debug.Log(json);
                
                File.WriteAllText(path, json);
            }
        }

        if (GUILayout.Button("Import"))
        {
            string path = EditorUtility.OpenFilePanel("Import", String.Empty, "json");

            if (path.Length > 0)
            {
                string json = File.ReadAllText(path);

                Language.ExportedLanguage importedLanguage = JsonUtility.FromJson<Language.ExportedLanguage>(json);

                string languagePath = EditorUtility.SaveFilePanelInProject("Import",
                    importedLanguage.languageName + ".asset", "asset", "Dove vuoi importare il nuovo linguaggio?");

                if (languagePath.Length > 0)
                {
                    Language newLanguage = ScriptableObject.CreateInstance<Language>();
                    newLanguage.ImportLanguage(importedLanguage);
                    
                    AssetDatabase.CreateAsset(newLanguage, languagePath);
                    EditorUtility.SetDirty(newLanguage);
                }

                //Debug.Log(importedLanguage.languageName);

                // Language importedLanguage = ScriptableObject.CreateInstance<Language>();
                //
                // importedLanguage = JsonUtility.FromJson<Language>(json);
                //
                // Debug.Log(importedLanguage.languageName);
            }
        }
        
        EditorGUILayout.EndHorizontal();
    }
}
