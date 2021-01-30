using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class Helper 
{
    public static List<T> FindAllObjectsFromResources<T>()
    {
        Debug.Log("Проверка файлов:");
        List<T> tmp = new List<T>();
        string ResourcesPath = Application.dataPath + "/Resources";
        string[] directories = Directory.GetDirectories(ResourcesPath,"*", SearchOption.AllDirectories);

        foreach(string directory in directories)
        {
            string directionalPath = directory.Substring(ResourcesPath.Length + 1);
            T[] result = Resources.LoadAll(directionalPath, typeof(T)).Cast<T>().ToArray();

            foreach(T item in result)
            {
                if (!tmp.Contains(item))
                {
                    Debug.Log("Найден файл ----[ " + item + " ]");
                    tmp.Add(item);
                }
            }
        }
        return tmp;
    }

}
