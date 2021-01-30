using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CustomTools 
{
    [MenuItem("Custom Tools/Dialogue/Save Narrative to CSV")]
    public static void SaveToCSV()
    {
        SaveCSV saveCSV = new SaveCSV();
        saveCSV.Save();

        Debug.Log("Создаю единный массив всех Нарративов:");

        EditorApplication.Beep();
        Debug.Log("<color=green> Нарративный массив успешно сохранен как CSV! </color>");

    }

    [MenuItem("Custom Tools/Dialogue/Update Dialogue Languages")]
    public static void UpdateDialogueLanguage()
    {
        UpdateLanguageType updateLanguageType = new UpdateLanguageType();
        updateLanguageType.UpdateLanguage();

        EditorApplication.Beep();
        Debug.Log("<color=green> Обновление языков успешно завершено! </color>");
    }

}
