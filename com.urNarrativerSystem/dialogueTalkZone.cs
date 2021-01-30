using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dialogueTalkZone : MonoBehaviour
{
    private DialogueTalk dialogueTalk;
    private void Awake()
    {
        dialogueTalk = GetComponent<DialogueTalk>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            dialogueTalk.StartDialogue();
        }
    }
}
