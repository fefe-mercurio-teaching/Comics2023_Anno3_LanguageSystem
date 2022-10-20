using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance { get; private set; }
    
    [SerializeField] private Language language;

    private void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public string GetString(string key)
    {
        if (language.strings.ContainsKey(key)) return language.strings[key];

        return $"??{key}??"; // ??NEW_GAME??
    }
}
