using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class GraphSaveAndLoadUtility 
{
    private List<Edge> edges => graphView.edges.ToList();
    private List<BaseNode> nodes => graphView.nodes.ToList().Where(node => node is BaseNode).Cast<BaseNode>().ToList();
    private DialogueGraphView graphView;

    public GraphSaveAndLoadUtility(DialogueGraphView _graphView)
    {
        graphView = _graphView;
    }

    public void Save(DialogueContainerSO _dialogueContainerSO)
    {
        SaveEdges(_dialogueContainerSO);
        SaveNodes(_dialogueContainerSO);

        EditorUtility.SetDirty(_dialogueContainerSO);
        AssetDatabase.SaveAssets();

    }

    public void Load(DialogueContainerSO _dialogueContainerSO)
    {
        ClearGraph();
        GenerateNodes(_dialogueContainerSO);
        ConnectNodes(_dialogueContainerSO);

    }

    private void SaveEdges(DialogueContainerSO _dialogueContainerSO)
    {
        _dialogueContainerSO.NodeLinkData.Clear();

        Edge[] connectedEdges = edges.Where(edge => edge.input != null).ToArray();
        for (int i = 0; i < connectedEdges.Count(); i++)
        {
            BaseNode outputNode = (BaseNode)connectedEdges[i].output.node;
            BaseNode inputNode = connectedEdges[i].input.node as BaseNode;

            _dialogueContainerSO.NodeLinkData.Add(new NodeLinkData { BaseNodeGuid = outputNode.NodeGuid, TargetNodeGuid = inputNode.NodeGuid });
        }
    }

    private void SaveNodes(DialogueContainerSO _dialogueContainerSO)
    {
        _dialogueContainerSO.StartNodeData.Clear();
        _dialogueContainerSO.DialogueNodeData.Clear();
        _dialogueContainerSO.EventNodeData.Clear();
        _dialogueContainerSO.EndNodeData.Clear();


        nodes.ForEach(node =>
        {
            switch (node)
            {
                case DialogueNode dialogueNode:
                    _dialogueContainerSO.DialogueNodeData.Add(SaveNodeData(dialogueNode));
                    break;
                case StartNode startNode:
                    _dialogueContainerSO.StartNodeData.Add(SaveNodeData(startNode));
                    break;
                case EndNode endNode:
                    _dialogueContainerSO.EndNodeData.Add(SaveNodeData(endNode));
                    break;
                case EventNode EventNode:
                    _dialogueContainerSO.EventNodeData.Add(SaveNodeData(EventNode));
                    break;
                case ScriptNode ScriptNode:
                    _dialogueContainerSO.ScriptNodeData.Add(SaveNodeData(ScriptNode));
                    break;
                default:
                    break;
            }

        });

    }

    private DialogueNodeData SaveNodeData(DialogueNode _node)
    {
        DialogueNodeData dialogueNodeData = new DialogueNodeData
        {
            NodeGuid = _node.NodeGuid,
            Position = _node.GetPosition().position,
            TextType = _node.Texts,
            Name = _node.Name,
            AudioClips = _node.AudioClips,
            DialogueFaceImageType = _node.FaceImageType,
            Sprite = _node.FaceImage,
            Background = _node.BackGround,
            DialogueNodePorts = new List<DialogueNodePort>(_node.DialogueNodePorts)
        };

        foreach (DialogueNodePort nodePort in dialogueNodeData.DialogueNodePorts)
        {
            nodePort.OutputGuid = string.Empty;
            nodePort.InputGuid = string.Empty;
            foreach(Edge edge in edges)
            {
                if (edge.output == nodePort.MyPort)
                {
                    nodePort.OutputGuid = (edge.output.node as BaseNode).NodeGuid;
                    nodePort.InputGuid = (edge.input.node as BaseNode).NodeGuid;
                }
            }
        }
        return dialogueNodeData;
    }

    private StartNodeData SaveNodeData(StartNode _node)
    {
        StartNodeData nodeData = new StartNodeData()
        {
            dialogueID = _node.DialogueID,
            NodeGuid = _node.NodeGuid,
            Position = _node.GetPosition().position,
        };
        return nodeData;
    }

    private EndNodeData SaveNodeData(EndNode _node)
    {
        EndNodeData nodeData = new EndNodeData()
        {
            NodeGuid = _node.NodeGuid,
            Position = _node.GetPosition().position,
            EndNodeType = _node.EndNodeType
        };
        return nodeData;
    }

    private EventNodeData SaveNodeData(EventNode _node)
    {
        EventNodeData nodeData = new EventNodeData()
        {
            NodeGuid = _node.NodeGuid,
            Position = _node.GetPosition().position,
            DialogueEventSO = _node.DialogueEvent,
        };
        return nodeData;
    }

    private ScriptNodeData SaveNodeData(ScriptNode _node)
    {
        ScriptNodeData scriptNodeData = new ScriptNodeData()
        {
            NodeGuid = _node.NodeGuid,
            Position = _node.GetPosition().position,
            scriptNodeType = _node.scriptType,
            reqNodeType = _node.reqScriptType,
            scriptNodePorts = new List<ScriptNodePort>(_node.ScriptNodePorts)
        };

        foreach (ScriptNodePort nodePort in scriptNodeData.scriptNodePorts)
        {
            nodePort.OutputGuid = string.Empty;
            nodePort.InputGuid = string.Empty;
            foreach (Edge edge in edges)
            {
                if (edge.output == nodePort.MyPort)
                {
                    // In Developing progress
                    /*
                    nodePort.ResultName = (edge.output.node as BaseNode).ResultName;
                    nodePort.ScriptName = (edge.output.node as BaseNode).ScriptName;
                    */
                    // Not save Scripts and Result text now

                    nodePort.OutputGuid = (edge.output.node as BaseNode).NodeGuid;
                    nodePort.InputGuid = (edge.input.node as BaseNode).NodeGuid;
                }
            }
        }
        return scriptNodeData;
    }



    private void ClearGraph()
    {
        edges.ForEach(edge => graphView.RemoveElement(edge));
        foreach(BaseNode node in nodes)
        {
            graphView.RemoveElement(node);
        }
    }

    private void GenerateNodes(DialogueContainerSO _dialogueContainer)
    {

        // Start Node
        foreach(StartNodeData node in _dialogueContainer.StartNodeData)
        {
            StartNode tempNode = graphView.CreateStartNode(node.Position);
            tempNode.NodeGuid = node.NodeGuid;
            tempNode.DialogueID = node.dialogueID;

            graphView.AddElement(tempNode);
        }

        // End Node
        foreach (EndNodeData node in _dialogueContainer.EndNodeData)
        {
            EndNode tempNode = graphView.CreateEndNode(node.Position);
            tempNode.NodeGuid = node.NodeGuid;
            tempNode.EndNodeType = node.EndNodeType;

            graphView.AddElement(tempNode);
        }

        // Event Node
        foreach (EventNodeData node in _dialogueContainer.EventNodeData)
        {
            EventNode tempNode = graphView.CreateEventNode(node.Position);
            tempNode.NodeGuid = node.NodeGuid;
            tempNode.DialogueEvent = node.DialogueEventSO;

            graphView.AddElement(tempNode);

        }

        // Dialogue Node
        foreach (DialogueNodeData node in _dialogueContainer.DialogueNodeData)
        {
            DialogueNode tempNode = graphView.CreateDialogueNode(node.Position);
            tempNode.NodeGuid = node.NodeGuid;
            tempNode.Name = node.Name;
            //tempNode.Texts = node.TextType;
            tempNode.FaceImage = node.Sprite;
            tempNode.BackGround = node.Background;
            tempNode.FaceImageType = node.DialogueFaceImageType;
            //tempNode.AudioClips = node.AudioClips;

            foreach (LanguageGeneric<string> languageGeneric in node.TextType)
            {
                tempNode.Texts.Find(language => language.LanguageType == languageGeneric.LanguageType).LanguageGenericType = languageGeneric.LanguageGenericType;
            }

            foreach (LanguageGeneric<AudioClip> languageGeneric in node.AudioClips)
            {
                tempNode.AudioClips.Find(language => language.LanguageType == languageGeneric.LanguageType).LanguageGenericType = languageGeneric.LanguageGenericType;
            }

            foreach (DialogueNodePort nodePort in node.DialogueNodePorts)
            {
                tempNode.AddChoicePort(tempNode, nodePort);
            }

            tempNode.LoadValueInToField();
            graphView.AddElement(tempNode);
        }


        // Script Node
        foreach (ScriptNodeData node in _dialogueContainer.ScriptNodeData)
        {
            ScriptNode tempNode = graphView.CreateScriptNode(node.Position);
            tempNode.NodeGuid = node.NodeGuid;
            tempNode.scriptType = node.scriptNodeType;
            tempNode.reqScriptType = node.reqNodeType;

            foreach (ScriptNodePort scriptNodePort in node.scriptNodePorts)
            {
                tempNode.AddRequirement(tempNode, scriptNodePort);
                tempNode.ScriptText = node.scriptTexts;
                tempNode.ResultText = node.resultTexts;
            }

            tempNode.LoadValueInToField();
            graphView.AddElement(tempNode);
        }
    }

    private void ConnectNodes(DialogueContainerSO _dialogueContainer)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            List<NodeLinkData> connections = _dialogueContainer.NodeLinkData.Where(edge => edge.BaseNodeGuid == nodes[i].NodeGuid).ToList();

            for (int j = 0; j < connections.Count; j++)
            {
                var targetNodeGuid = connections[j].TargetNodeGuid;
                BaseNode targetNode = nodes.First(node => node.NodeGuid == targetNodeGuid);

                if (nodes[i] is DialogueNode == false)
                {
                    Debug.Log("Подключаю Ноды");
                    LinkNodesTogether(nodes[i].outputContainer[j].Q<Port>(), (Port)targetNode.inputContainer[0]);
                }

            }
        }

        List<DialogueNode> dialogueNodes = nodes.FindAll(node => node is DialogueNode).Cast<DialogueNode>().ToList();

        foreach (DialogueNode dialogueNode in dialogueNodes)
        {
            foreach(DialogueNodePort nodePort in dialogueNode.DialogueNodePorts)
            {
                if (nodePort.InputGuid != string.Empty)
                {
                    BaseNode targetNode = nodes.First(Node => Node.NodeGuid == nodePort.InputGuid);
                    LinkNodesTogether(nodePort.MyPort, (Port)targetNode.inputContainer[0]);
                };

            }
        }

    }

    private void LinkNodesTogether(Port output, Port input)
    {
        Edge tempEdge = new Edge
        {
            output = output,
            input = input,
        };

        tempEdge.input.Connect(tempEdge);
        tempEdge.output.Connect(tempEdge);

        graphView.Add(tempEdge);
    }
}
