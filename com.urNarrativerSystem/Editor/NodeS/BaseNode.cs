using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class BaseNode : Node
{
    protected string nodeGuid;
    protected DialogueGraphView graphView;
    protected DialogueGraph editorWindow;
    protected Vector2 defaultNodeSize = new Vector2(350, 450);

    public string NodeGuid { get => nodeGuid; set => nodeGuid = value; }


    public BaseNode()
    {
        StyleSheet styleSheets = Resources.Load<StyleSheet>("Node");
    }

    public void AddOutputPort(string name, Port.Capacity capacity = Port.Capacity.Single)
    {
        Port outputPort = GetPortInstance(Direction.Output, capacity);
        outputPort.portName = name;
        outputContainer.Add(outputPort);
    }

    public void AddInputPort(string name, Port.Capacity capacity = Port.Capacity.Multi)
    {
        Port inputPort = GetPortInstance(Direction.Input, capacity);
        inputPort.portName = name;
        inputContainer.Add(inputPort);
    }

    public Port GetPortInstance(Direction nodeDirection, Port.Capacity capacity = Port.Capacity.Single)
    {
        return InstantiatePort(Orientation.Horizontal, nodeDirection, capacity, typeof(float));
    }

    public virtual void LoadValueInToField()
    {

    }

}
