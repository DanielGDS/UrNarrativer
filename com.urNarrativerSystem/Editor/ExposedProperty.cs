using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExposedProperty
{
    public static ExposedProperty CreateInstance()
    {
        return new ExposedProperty();
    }

    public string PropertyName = "New String";
    public string PropertyValue = "New Value";
}
