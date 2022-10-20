using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Language.asset", menuName = "Language")]
public class Language : ScriptableObject
{
    public string languageName;
    public StringSerializableDictionary strings = new();
}
