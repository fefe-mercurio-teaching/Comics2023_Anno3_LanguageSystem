using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TranslatedText : MonoBehaviour
{
    [SerializeField] private string key;

    private TextMeshProUGUI _textMesh;
    private string _lastKey;

    private void Start()
    {
        _textMesh = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (key != _lastKey)
        {
            _lastKey = key;
            _textMesh.text = LanguageManager.Instance.GetString(key);
        }
    }
}
