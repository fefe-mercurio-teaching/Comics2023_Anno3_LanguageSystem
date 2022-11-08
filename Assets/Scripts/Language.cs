using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Language.asset", menuName = "Language")]
public class Language : ScriptableObject
{
    [Serializable]
    public class ExportedLanguage
    {
        public string languageName;
        public StringSerializableDictionary strings;
        
        public ExportedLanguage(Language original)
        {
            languageName = original.languageName;
            strings = original.strings;
        }
    }
    
    public string languageName;
    public StringSerializableDictionary strings = new();

    public ExportedLanguage GetExportedLanguage()
    {
        return new ExportedLanguage(this);
    }

    public void ImportLanguage(ExportedLanguage exportedLanguage)
    {
        languageName = exportedLanguage.languageName;
        strings = exportedLanguage.strings;
    }
}
