using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateLanguageType
{
   public void UpdateLanguage()
    {

        List<DialogueContainerSO> dialogueContainers = Helper.FindAllObjectsFromResources<DialogueContainerSO>();
        Debug.Log("Найдено " + dialogueContainers.Count + " массивов диалогов!" + "                              " +
            "[Если было найдено 0 диалогов означает, что они уже обновлены, или находятся напрямую в папке *yourProject/Resources ]");

        foreach(DialogueContainerSO DialogueContainer in dialogueContainers)
        {
            foreach(DialogueNodeData NodeData in DialogueContainer.DialogueNodeData)
            {
                NodeData.TextType = UpdateLanguageGeneric(NodeData.TextType);
                NodeData.AudioClips = UpdateLanguageGeneric(NodeData.AudioClips);

                foreach(DialogueNodePort nodePort in NodeData.DialogueNodePorts)
                {
                    nodePort.TextLanguages = UpdateLanguageGeneric(nodePort.TextLanguages);
                }
            }
        }
    }
    private List<LanguageGeneric<T>> UpdateLanguageGeneric<T>(List<LanguageGeneric<T>> languageGenerics)
    {
        List<LanguageGeneric<T>> tmp = new List<LanguageGeneric<T>>();

        foreach (LanguageType languages in (LanguageType[])Enum.GetValues(typeof(LanguageType)))
        {
            tmp.Add(new LanguageGeneric<T>
            {
                LanguageType = languages
            });
        }

        foreach(LanguageGeneric<T> languageGeneric in languageGenerics)
        {
            if (tmp.Find(language => language.LanguageType == languageGeneric.LanguageType) != null)
            {
                tmp.Find(language => language.LanguageType 
                == languageGeneric.LanguageType).LanguageGenericType = languageGeneric.LanguageGenericType;
            }
        }

        return tmp;
    }
}
