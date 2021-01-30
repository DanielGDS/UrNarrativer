using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class StartNode : BaseNode
{
    private List<DialogueNodeID<string>> dialogueID;
    public List<DialogueNodeID<string>> DialogueID { get => dialogueID; set => dialogueID = value; }

    private TextField id_Field;

    public StartNode()
    {

    }

    public StartNode(Vector2 _position, DialogueGraph _editorWindow, DialogueGraphView _graphView)
    {
        editorWindow = _editorWindow;
        graphView = _graphView;

        /*

        //Dialogue ID
        id_Field = new TextField("");
        id_Field.RegisterValueChangedCallback(value =>
        {
            DialogueID.Find(id => id.dialogueIDs == DialogueIDName.ID).DialogueGenericID = value.newValue;
        });
        id_Field.SetValueWithoutNotify(DialogueID.Find(id => id.dialogueIDs == DialogueIDName.ID).DialogueGenericID);

        id_Field.AddToClassList("TextName");
        mainContainer.Add(id_Field);

        */
      


        title = "Start";
        SetPosition(new Rect(_position, defaultNodeSize));
        nodeGuid = Guid.NewGuid().ToString();

        AddOutputPort("Output", Port.Capacity.Single);

        RefreshExpandedState();
        RefreshPorts();
    }


}
