using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class ScriptNode : BaseNode
{
    private List<DialogueNodePort> dialogueNodePorts = new List<DialogueNodePort>();
    private List<ScriptNodePort> scriptNodePorts = new List<ScriptNodePort>();
    private ScriptType ScriptType;
    private ReqScriptType ReqScriptType;

    private List<string> scriptText = new List<string>();
    private List<string> resultText = new List<string>();

    public ScriptType scriptType { get => ScriptType; set => ScriptType = value; }
    public List<DialogueNodePort> DialogueNodePorts { get => dialogueNodePorts; set => dialogueNodePorts = value; }
    public ReqScriptType reqScriptType { get => ReqScriptType; set => ReqScriptType = value; }


    public List<ScriptNodePort> ScriptNodePorts { get => scriptNodePorts; set => scriptNodePorts = value; }
    public List<string> ScriptText { get => scriptText; set => scriptText = value; }
    public List<string> ResultText { get => resultText; set => resultText = value; }

    //public List<ScriptGeneric<string>> ScriptText { get => scriptText; set => scriptText = value; }
    //public List<ScriptGeneric<string>> ResultText { get => resultText; set => resultText = value; }

    private TextField scriptTextField;
    private TextField resultTextField;
    private EnumField scriptType_Field;
    private EnumField reqScriptType_Field;



    public ScriptNode()
    {

    }

    public ScriptNode(Vector2 _position, DialogueGraph _editorWindow, DialogueGraphView _graphView)
    {
        editorWindow = _editorWindow;
        graphView = _graphView;

        title = "Script";
        SetPosition(new Rect(_position, defaultNodeSize));
        nodeGuid = Guid.NewGuid().ToString();

        AddInputPort("Input", Port.Capacity.Multi);

        Button button = new Button() { text = "Add Req" };
        button.AddToClassList("button");
        button.clicked += () => { AddRequirement(this); };

        titleButtonContainer.Add(button);

        /* 
        Button button1 = new Button() { text = "/If" };
        button1.AddToClassList("button");
        button1.clicked += () => { AddRequirement(this); };

        titleButtonContainer.Add(button1);

        Button button2 = new Button() { text = "/Get" };
        button2.AddToClassList("button");
        button2.clicked += () => { AddRequirement(this); };

        titleButtonContainer.Add(button2);

        Button button3 = new Button() { text = "/Set" };
        button3.AddToClassList("button");
        button3.clicked += () => { AddRequirement(this); };

        titleButtonContainer.Add(button3);

        */

        //
    }

    public Port AddRequirement(BaseNode _baseNode, ScriptNodePort _dialogueNodePort = null) 
    {
        Port port = GetPortInstance(Direction.Output);

        int outputReqCount = _baseNode.outputContainer.Query("req").ToList().Count();
        string outputReqName = $"{outputReqCount + 1}";

        ScriptNodePort scriptNodePort = new ScriptNodePort();
        scriptNodePort.PortGuid = Guid.NewGuid().ToString();

        if (_dialogueNodePort != null)
        {
            scriptNodePort.InputGuid = _dialogueNodePort.InputGuid;
            scriptNodePort.OutputGuid = _dialogueNodePort.OutputGuid;
            scriptNodePort.PortGuid = _dialogueNodePort.PortGuid;
        }

        // Script Node Req
        scriptType_Field = new EnumField() { value = scriptType };
        scriptType_Field.Init(scriptType);
        scriptType_Field.RegisterValueChangedCallback(value => scriptType = (ScriptType)value.newValue);
        scriptType_Field.AddToClassList("IfElseOrAndScript");
        port.contentContainer.Add(scriptType_Field);

        // Script Node Req
        reqScriptType_Field = new EnumField() { value = reqScriptType };
        reqScriptType_Field.Init(reqScriptType);
        reqScriptType_Field.RegisterValueChangedCallback(value => reqScriptType = (ReqScriptType)value.newValue);
        reqScriptType_Field.AddToClassList("equalLessMoreNorEqual");
        port.contentContainer.Add(reqScriptType_Field);

        /*
        // Script textfield
        string scriptText = "";

        scriptTextField = new TextField("Requirement: = ");
        scriptTextField.RegisterValueChangedCallback(evt => { scriptText = evt.newValue; });
        scriptTextField.SetValueWithoutNotify(scriptText);
        scriptTextField.multiline = false;
        scriptTextField.AddToClassList("TextBox");
        _baseNode.contentContainer.Add(scriptTextField);
        */


        scriptTextField = new TextField();
        scriptTextField.RegisterValueChangedCallback(value => { scriptText = new List<string>(); });


        scriptTextField.SetValueWithoutNotify(scriptText.Find(scr => scr == scriptText.ToString()));
        //scriptNodePort.TextField.multiline = false;
        scriptTextField.AddToClassList("ScriptField");
        port.contentContainer.Add(scriptTextField);


        resultTextField = new TextField("");
        resultTextField.RegisterValueChangedCallback(value => { resultText = new List<string>(); });
        resultTextField.SetValueWithoutNotify(resultText.Find(scr => scr == scriptText.ToString()));
        //scriptNodePort.TextField.multiline = false;
        resultTextField.AddToClassList("ResultField");
        port.contentContainer.Add(resultTextField);



        // Delete button
        Button deleteButton = new Button(() => DeletePort(_baseNode, port)) { text = "X" };
        port.contentContainer.Add(deleteButton);

        scriptNodePort.MyPort = port;
        port.portName = "";

        scriptNodePorts.Add(scriptNodePort);
        _baseNode.outputContainer.Add(port);

        port.AddToClassList("");

        // Refresh all
        _baseNode.RefreshPorts();
        _baseNode.RefreshExpandedState();

        return port;
    }

    private void DeletePort(BaseNode _node, Port _port)
    {
        DialogueNodePort tmp = dialogueNodePorts.Find(port => port.MyPort == _port);
        dialogueNodePorts.Remove(tmp);

        IEnumerable<Edge> portEdge = graphView.edges.ToList().Where(edge => edge.output == _port);

        if (portEdge.Any())
        {
            Edge edge = portEdge.First();
            edge.input.Disconnect(edge);
            edge.output.Disconnect(edge);
            graphView.RemoveElement(edge);
        }

        // Refresh
        _node.outputContainer.Remove(_port);
        _node.RefreshPorts();
        _node.RefreshExpandedState();


    }

    public override void LoadValueInToField()
    {
        scriptTextField.SetValueWithoutNotify(scriptText.Find(scr => scr == scriptText.ToString()));
        resultTextField.SetValueWithoutNotify(resultText.Find(scr => scr == scriptText.ToString()));
    }

}
