using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "Dialogue/New Narrative")]
[System.Serializable]
public class DialogueContainerSO : ScriptableObject
{
    public List<NodeLinkData> NodeLinkData = new List<NodeLinkData>();

    public List<DialogueNodeData> DialogueNodeData = new List<DialogueNodeData>();
    public List<EndNodeData> EndNodeData = new List<EndNodeData>();
    public List<StartNodeData> StartNodeData = new List<StartNodeData>();
    public List<EventNodeData> EventNodeData = new List<EventNodeData>();
    public List<ScriptNodeData> ScriptNodeData = new List<ScriptNodeData>();

    public List<BaseNodeData> AllNodes
    {
        get
        {
            List<BaseNodeData> tmp = new List<BaseNodeData>();
            tmp.AddRange(DialogueNodeData);
            tmp.AddRange(EndNodeData);
            tmp.AddRange(StartNodeData);
            tmp.AddRange(EventNodeData);

            return tmp;
        }
    }

}

[System.Serializable]
public class NodeLinkData
{
    public string BaseNodeGuid;
    public string TargetNodeGuid;
}

[System.Serializable]
public class BaseNodeData
{
    public string NodeGuid;
    public Vector2 Position;
}

[System.Serializable]
public class DialogueNodeData: BaseNodeData
{
    public List<DialogueNodePort> DialogueNodePorts;
    public Sprite Sprite;
    public Sprite Background;
    public DialogueFaceImageType DialogueFaceImageType;
    public List<LanguageGeneric<AudioClip>> AudioClips;
    public List<LanguageGeneric<string>> Name;
    public List<LanguageGeneric<string>> TextType;
    public List<DialogueNodeID<string>> dialogueNodeID;
}

[System.Serializable]
public class ScriptNodeData: BaseNodeData
{
    public List<ScriptNodePort> scriptNodePorts;
    public List<string> scriptTexts;
    public List<string> resultTexts;
    public ScriptType scriptNodeType;
    public ReqScriptType reqNodeType;

}

[System.Serializable]
public class EndNodeData : BaseNodeData
{
    public EndNodeType EndNodeType;
}

[System.Serializable]
public class StartNodeData : BaseNodeData
{
    public List<DialogueNodeID<string>> dialogueID = new List<DialogueNodeID<string>>();
}

[System.Serializable]
public class EventNodeData : BaseNodeData
{
    public DialogueEventSO DialogueEventSO;
}

[System.Serializable]
public class LanguageGeneric<T>
{
    public LanguageType LanguageType;
    [TextArea(8,25)] public T LanguageGenericType;
}

[System.Serializable]
public class DialogueNodeID<T>
{
    public DialogueIDName dialogueIDs;
    public T DialogueGenericID;
}

[System.Serializable]
public class ScriptGeneric<T>
{
    public ScriptType ScriptType;
    public T scriptGenericType;
}

[System.Serializable]
public class ReqGeneric<T>
{
    public ReqScriptType reqType;
    public T reqGenericType;
}

[System.Serializable]
public class EndNodeType<T>
{
    public EndNodeType endNodeType;
    [TextArea] public T EndNodeGenericType;
}


[System.Serializable]
public class DialogueNodePort
{
    public string PortGuid;
    public string InputGuid;
    public string OutputGuid;
    public Port MyPort;
    public TextField TextField;
    public List<LanguageGeneric<string>> NameLanguages = new List<LanguageGeneric<string>>();
    public List<LanguageGeneric<string>> TextLanguages = new List<LanguageGeneric<string>>();
}

[System.Serializable]
public class ScriptNodePort
{
    public string PortGuid;
    public string InputGuid;
    public string OutputGuid;
    public Port MyPort;
    public TextField TextField;
    public List<string> ScriptName = new List<string>();
    public List<string> ResultName = new List<string>();
}
