using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueNode : BaseNode
{
    private List<LanguageGeneric<string>> texts = new List<LanguageGeneric<string>>();
    private List<LanguageGeneric<AudioClip>> audioClips = new List<LanguageGeneric<AudioClip>>();
    private List<LanguageGeneric<string>> name = new List<LanguageGeneric<string>>();
    private Sprite faceImage;
    private DialogueFaceImageType faceImageType;
    private Sprite backGround;

    private List<DialogueNodePort> dialogueNodePorts = new List<DialogueNodePort>();

    public List<LanguageGeneric<string>> Texts { get => texts; set => texts = value; }
    public List<LanguageGeneric<AudioClip>> AudioClips { get => audioClips; set => audioClips = value; }
    public List<LanguageGeneric<string>> Name { get => name; set => name = value; }
    public Sprite FaceImage { get => faceImage; set => faceImage = value; }
    public DialogueFaceImageType FaceImageType { get => faceImageType; set => faceImageType = value; }
    public List<DialogueNodePort> DialogueNodePorts { get => dialogueNodePorts; set => dialogueNodePorts = value; }
    public Sprite BackGround { get => backGround; set => backGround = value; }

    private TextField texts_Field;
    private ObjectField audioClips_Field;
    private ObjectField faceImage_Field;
    private ObjectField background_Field;
    private TextField name_Field;
    private EnumField faceImageType_Field;

    public DialogueNode()
    {

    }

    public DialogueNode(Vector2 _position, DialogueGraph _editorWindow, DialogueGraphView _graphView)
    {
        editorWindow = _editorWindow;
        graphView = _graphView;

        title = "Dialogue";
        SetPosition(new Rect(_position, defaultNodeSize));
        nodeGuid = Guid.NewGuid().ToString();

        AddInputPort("Input", Port.Capacity.Multi);

        foreach (LanguageType language in (LanguageType[])Enum.GetValues(typeof(LanguageType)))
        {
            Name.Add(new LanguageGeneric<string> { LanguageType = language, LanguageGenericType = "" });
            texts.Add(new LanguageGeneric<string> { LanguageType = language, LanguageGenericType = "" });
            audioClips.Add(new LanguageGeneric<AudioClip> { LanguageType = language, LanguageGenericType = null });
        }


        // Character Sprite
        faceImage_Field = new ObjectField { objectType = typeof(Sprite), allowSceneObjects = false, value = faceImage };
        faceImage_Field.RegisterValueChangedCallback(value => { faceImage = value.newValue as Sprite; });
        mainContainer.Add(faceImage_Field);

        // Character Sprite Side
        faceImageType_Field = new EnumField(){ value = faceImageType};
        faceImageType_Field.Init(faceImageType);
        faceImageType_Field.RegisterValueChangedCallback(value => faceImageType = (DialogueFaceImageType)value.newValue);
        mainContainer.Add(faceImageType_Field);

        // Audio Clips
        audioClips_Field = new ObjectField()
        {
            objectType = typeof(AudioClip),
            allowSceneObjects = false,
            value = audioClips.Find(audioClip => audioClip.LanguageType == editorWindow.LanguageType).LanguageGenericType,
        };
        audioClips_Field.RegisterValueChangedCallback(value => { audioClips.Find(audioClip => audioClip.LanguageType == editorWindow.LanguageType).LanguageGenericType = value.newValue as AudioClip; });
        audioClips_Field.SetValueWithoutNotify(audioClips.Find(audioClip => audioClip.LanguageType == editorWindow.LanguageType).LanguageGenericType);
        mainContainer.Add(audioClips_Field);

        //Text Name
        Label label_name = new Label("Name");
        label_name.AddToClassList("label_name");
        label_name.AddToClassList("Label");
        mainContainer.Add(label_name);

        name_Field = new TextField("");
        name_Field.RegisterValueChangedCallback(value => { name.Find(name => name.LanguageType == editorWindow.LanguageType).LanguageGenericType = value.newValue; });
        name_Field.SetValueWithoutNotify(name.Find(name => name.LanguageType == editorWindow.LanguageType).LanguageGenericType);


        name_Field.AddToClassList("TextName");
        mainContainer.Add(name_Field);
        //


        // Text Box
        Label label_texts = new Label("Text Box");
        label_texts.AddToClassList("label_texts");
        label_texts.AddToClassList("Label");
        mainContainer.Add(label_texts);

        texts_Field = new TextField("");
        texts_Field.RegisterValueChangedCallback(value =>{ texts.Find(text => text.LanguageType == editorWindow.LanguageType).LanguageGenericType = value.newValue; });
        texts_Field.SetValueWithoutNotify(texts.Find(text => text.LanguageType == editorWindow.LanguageType).LanguageGenericType);
        texts_Field.multiline = true;

        texts_Field.AddToClassList("TextBox");
        mainContainer.Add(texts_Field);

        Button button = new Button() { text = "Add Choice" };
        button.AddToClassList("button");
        button.clicked += () => { AddChoicePort(this); };

        titleButtonContainer.Add(button);
        //


        // Background Sprite
        Label label_bg = new Label("Background");
        label_bg.AddToClassList("label_texts");
        label_bg.AddToClassList("Label");
        mainContainer.Add(label_bg);

        background_Field = new ObjectField { objectType = typeof(Sprite), allowSceneObjects = false, value = backGround };
        background_Field.RegisterValueChangedCallback(value => { backGround = value.newValue as Sprite; });
        mainContainer.Add(background_Field);

    }

    public void ReloadLanguage()
    {
        // Reload TextFields
        texts_Field.RegisterValueChangedCallback(value => { texts.Find(text => text.LanguageType == editorWindow.LanguageType).LanguageGenericType = value.newValue; });
        texts_Field.SetValueWithoutNotify(texts.Find(text => text.LanguageType == editorWindow.LanguageType).LanguageGenericType);

        // Reload AudioClips
        audioClips_Field.RegisterValueChangedCallback(value => { audioClips.Find(audioClip => audioClip.LanguageType == editorWindow.LanguageType).LanguageGenericType = value.newValue as AudioClip; });
        audioClips_Field.SetValueWithoutNotify(audioClips.Find(audioClip => audioClip.LanguageType == editorWindow.LanguageType).LanguageGenericType);

        // Reload Names
        

        name_Field.RegisterValueChangedCallback(value => { name.Find(name => name.LanguageType == editorWindow.LanguageType).LanguageGenericType = value.newValue; });
        name_Field.SetValueWithoutNotify(Name.Find(name => name.LanguageType == editorWindow.LanguageType).LanguageGenericType);


        foreach (DialogueNodePort nodePort in dialogueNodePorts)
        {
            nodePort.TextField.RegisterValueChangedCallback(value => { nodePort.TextLanguages.Find(language => language.LanguageType == editorWindow.LanguageType).LanguageGenericType = value.newValue; });
            nodePort.TextField.SetValueWithoutNotify(nodePort.TextLanguages.Find(language => language.LanguageType == editorWindow.LanguageType).LanguageGenericType);
        }
    }

    public override void LoadValueInToField()
    {
        texts_Field.SetValueWithoutNotify(texts.Find(language => language.LanguageType == editorWindow.LanguageType).LanguageGenericType);
        audioClips_Field.SetValueWithoutNotify(audioClips.Find(language => language.LanguageType == editorWindow.LanguageType).LanguageGenericType);
        faceImage_Field.SetValueWithoutNotify(faceImage);
        faceImageType_Field.SetValueWithoutNotify(faceImageType);
        name_Field.SetValueWithoutNotify(name.Find(language => language.LanguageType == editorWindow.LanguageType).LanguageGenericType);
    }

    public Port AddChoicePort(BaseNode _baseNode, DialogueNodePort _dialogueNodePort = null)
    {
        Port port = GetPortInstance(Direction.Output);

        int outputPortCount = _baseNode.outputContainer.Query("connector").ToList().Count();
        string outputPortName = $"{outputPortCount + 1}:      ";

        DialogueNodePort dialogueNodePort = new DialogueNodePort();
        dialogueNodePort.PortGuid = Guid.NewGuid().ToString();

        foreach (LanguageType language in (LanguageType[])Enum.GetValues(typeof(LanguageType)))
        {
            dialogueNodePort.TextLanguages.Add(new LanguageGeneric<string>()
            {
                LanguageType = language,
                LanguageGenericType = outputPortName
            });
        }

        if (_dialogueNodePort != null)
        {
            dialogueNodePort.InputGuid = _dialogueNodePort.InputGuid;
            dialogueNodePort.OutputGuid= _dialogueNodePort.OutputGuid;
            dialogueNodePort.PortGuid = _dialogueNodePort.PortGuid;

            foreach (LanguageGeneric<string> LanguageGeneric in _dialogueNodePort.TextLanguages)
            {
                dialogueNodePort.TextLanguages.Find(language => language.LanguageType == LanguageGeneric.LanguageType).LanguageGenericType = LanguageGeneric.LanguageGenericType;
            }
        }

        // Text for the port
        dialogueNodePort.TextField = new TextField();
        dialogueNodePort.TextField.RegisterValueChangedCallback(value =>
        {
            dialogueNodePort.TextLanguages.Find(language => language.LanguageType == editorWindow.LanguageType).LanguageGenericType = value.newValue;
        });
        dialogueNodePort.TextField.SetValueWithoutNotify(dialogueNodePort.TextLanguages.Find(language => language.LanguageType == editorWindow.LanguageType).LanguageGenericType);
        port.contentContainer.Add(dialogueNodePort.TextField);

        // Delete choice button
        Button deleteButton = new Button(() => DeletePort(_baseNode,port)) { text = "X" };
        port.contentContainer.Add(deleteButton);

        dialogueNodePort.MyPort = port;
        port.portName = "";

        dialogueNodePorts.Add(dialogueNodePort);
        _baseNode.outputContainer.Add(port);

        // Refresh
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
}
