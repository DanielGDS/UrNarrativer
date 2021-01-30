using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    // private DialogueContainer dialogue;


    [Header("[Text Settings]")]
    public bool isWtritting;
    [Space]

    [Tooltip(" 0.01 = Быстрее / Медленее = 0.2")]
    [Range(0.01f, 0.2f)] 
    public float m_characterInterval;
    private float m_tempInterval;

    private float m_cumalativeDeltaTime;
    private string m_text;

    [SerializeField] private GameObject dialogueUI;
    [Header("[Text]")]
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI dialogueChar;
    [Header("[Image]")]
    [SerializeField] private Image leftImage;
    [SerializeField] private GameObject leftImageGO;
    [SerializeField] private Image rightImage;
    [SerializeField] private GameObject rightImageGO;
    [Header("[Background Picture]")]
    [SerializeField] private Image Background;
    [Space]
    [Header("[Buttons]")]
    [SerializeField] private GameObject buttonsContainer;
    [SerializeField] private Button button01;
    [SerializeField] private TextMeshProUGUI buttonText01;
    [Space]
    [SerializeField] private Button button02;
    [SerializeField] private TextMeshProUGUI buttonText02;
    [Space]
    [SerializeField] private Button button03;
    [SerializeField] private TextMeshProUGUI buttonText03;
    [Space]
    [SerializeField] private Button button04;
    [SerializeField] private TextMeshProUGUI buttonText04;

    // private Button choicePrefab;
    // private Transform buttonContainer;


    //public GameObject buttonsContainer;

    private List<Button> buttons = new List<Button>();
    private List<TextMeshProUGUI> buttonsTexts = new List<TextMeshProUGUI>();

    private void Awake()
    {
        ShowDialogueUI(true);

        buttons.Add(button01);
        buttons.Add(button02);
        buttons.Add(button03);
        buttons.Add(button04);

        buttonsTexts.Add(buttonText01);
        buttonsTexts.Add(buttonText02);
        buttonsTexts.Add(buttonText03);
        buttonsTexts.Add(buttonText04);

    }

    private void Start()
    {
        m_tempInterval = m_characterInterval;
    }


    private void FixedUpdate()
    {
        //По клику ускоряет печать текста, для нетерпеливых, или если уже видели данный текст
        if (Input.GetMouseButton(0))
        {
            m_characterInterval = (float)0.001f;

        }
    }

    /*
    private void Start()
    {
        var narrativeData = dialogue.NodeLinks.First();
        ProceedToNarrative(narrativeData.TargetNodeGuid);
    }
    */

    /*
    private void ProceedToNarrative(string narrativeDataGUID)
    {
        var choices = dialogue.NodeLinks.Where(x => x.BaseNodeGuid == narrativeDataGUID);

        var buttons = buttonContainer.GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++)
        {
            Destroy(buttons[i].gameObject);
        }

        foreach (var choice in choices)
        {
            var button = Instantiate(choicePrefab, buttonContainer);
            button.GetComponentInChildren<TextMeshProUGUI>().text = ProcessProperties(choice.BaseNodeGuid);
            button.onClick.AddListener(() => ProceedToNarrative(choice.TargetNodeGuid));
        }
    }

    private string ProcessProperties(string text)
    {
        return text;
    }
    */

    public void ShowDialogueUI(bool _show)
    {
        dialogueUI.SetActive(_show);
    }

    public void SetText (string _name, string _textBox)
    {
        buttonsContainer.SetActive(false);
        dialogueChar.text = _name;
        m_text = _textBox;
        StartCoroutine(WritterEffect(m_text));
    }

    IEnumerator WritterEffect(string tempText)
    {
        // Эффект побуквенной записи текста в диагловое окно
        // In char effect to text writting in dialogue window 
        isWtritting = true;

        // Восстанавливает изначальную скорость печати текста
        m_characterInterval = m_tempInterval;
        dialogueText.text = "";
        foreach (char letter in tempText.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(m_characterInterval);
        }
        isWtritting = false;
        // Дополнительная необходимая задержка перед появлением кнопок
        yield return new WaitForSeconds(0.2f);
        buttonsContainer.SetActive(true);


    }

    public void SetImage(Sprite _image, DialogueFaceImageType dialogueFaceImageType)
    {
        leftImageGO.SetActive(false);
        rightImageGO.SetActive(false);

        if (_image != null)
        {
            if (dialogueFaceImageType == DialogueFaceImageType.Left)
            { leftImage.sprite = _image; leftImageGO.SetActive(true); }
            else { rightImage.sprite = _image; rightImageGO.SetActive(true); }
        }
    }

    public void SetBackground(Sprite _image)
    {
        if (_image != null && Background.sprite != _image)
        {
            Background.sprite = _image;
        }
    }

    public void SetButtons(List<string> _texts, List<UnityAction> unityActions)
    {
        buttons.ForEach(button => button.gameObject.SetActive(false));

        for(int i = 0; i < _texts.Count; i++)
        {
            buttonsTexts[i].text = _texts[i];
            buttons[i].gameObject.SetActive(true);
            buttons[i].onClick = new Button.ButtonClickedEvent();
            buttons[i].onClick.AddListener(unityActions[i]);
        }
    }
}
