using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEditor.Callbacks;

public partial class DialogueGraph : EditorWindow
{
    private DialogueContainerSO currentdialogueContainerSO;
    private DialogueGraphView _graphView;
    private ToolbarMenu toolbarMenu;
    private Label nameOfDialogueContainer;
    private GraphSaveAndLoadUtility saveAndLoadUtility;
    private LanguageType languageType = LanguageType.English;
    public LanguageType LanguageType { get => languageType; set => languageType = value; }

    [OnOpenAsset(1)]
    public static bool ShowWindow(int _instanceID, int line)
    {
        UnityEngine.Object item = EditorUtility.InstanceIDToObject(_instanceID);

        if (item is DialogueContainerSO)
        {
            DialogueGraph window = (DialogueGraph)GetWindow(typeof(DialogueGraph));
            window.titleContent = new GUIContent("Dialogue Editor");
            window.currentdialogueContainerSO = item as DialogueContainerSO;
            window.minSize = new Vector2(500, 250);
            window.RequestDataOperation(false);
        }

        return false;
    }

    [MenuItem("Graph/Dialogue Graph")]
    public static void OpenDialogueGraphWindow()
    {
        var window = GetWindow<DialogueGraph>();
        window.titleContent = new GUIContent (text: "Narrative Graph");

    }

    private void OnEnable()
    {
        languageType = LanguageType.English;

        ConstructGraphView();
        GenerateToolbar();
        GenerateMinimap();
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(_graphView);
    }

    private void GenerateMinimap()
    {
        var miniMap = new MiniMap { anchored = true };
        var cords = _graphView.contentViewContainer.WorldToLocal(new Vector2(maxSize.x - 10, 30));
        miniMap.SetPosition(new Rect(cords.x, cords.y, width: 200, height: 140));
        _graphView.Add(miniMap);
    }

    private void ConstructGraphView()
    {
        _graphView = new DialogueGraphView(this)
        {
            name = "Dialogue Graph"
        };

        _graphView.StretchToParentSize();
        rootVisualElement.Add(_graphView);

        saveAndLoadUtility = new GraphSaveAndLoadUtility(_graphView);

    }

    private void GenerateToolbar()
    {
        var toolbar = new Toolbar();


        // <summary>
        // Save and Load button
        toolbar.Add(child: new Button(clickEvent: () => RequestDataOperation(save: true)) { text = "Save Data" });
        toolbar.Add(child: new Button(clickEvent: () => RequestDataOperation(save: false)) { text = "Load Data" });
        //

        // 27.01.2021
        //Dropdown menu for languages switch
        toolbarMenu = new ToolbarMenu();
        foreach (LanguageType language in (LanguageType[])Enum.GetValues(typeof(LanguageType)))
        {
            toolbarMenu.menu.AppendAction(language.ToString(), new Action<DropdownMenuAction>(x => Language(language,toolbarMenu)));
        }
        toolbar.Add(toolbarMenu);
        //


        /*
        //Title of current Narrative Dialogue file
        var fileNameTextField = new TextField(label: "File Name");
        fileNameTextField.SetValueWithoutNotify(_fileName);
        fileNameTextField.MarkDirtyRepaint();
        fileNameTextField.RegisterValueChangedCallback(evt => _fileName = evt.newValue);
        toolbar.Add(fileNameTextField);
        //Will destroy in future
        */


        //Name for current Dialogue Container of we open
        nameOfDialogueContainer = new Label("");
        toolbar.Add(nameOfDialogueContainer);





        rootVisualElement.Add(toolbar);
    }

    private void RequestDataOperation(bool save)
    {
        /*
        if (string.IsNullOrEmpty(_fileName))
        {
            EditorUtility.DisplayDialog("Invalid file name!", message: "Please enter a valid file name.", ok: "OK");
            return;
        }
        */

        if (save && currentdialogueContainerSO != null)
            saveAndLoadUtility.Save(currentdialogueContainerSO);
        else
        {
            if (currentdialogueContainerSO != null)
            {
                nameOfDialogueContainer.text = "Name:    " + currentdialogueContainerSO.name;
                saveAndLoadUtility.Load(currentdialogueContainerSO);
            }
            ;
        }

    }

    private void Language(LanguageType _language, ToolbarMenu _toolbarMenu)
    {
        toolbarMenu.text = "Language: " + _language.ToString();
        languageType = _language;
        _graphView.LanguageReload();
    }
}
