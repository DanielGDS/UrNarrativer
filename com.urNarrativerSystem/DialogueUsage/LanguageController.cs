﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageController : MonoBehaviour
{
    [SerializeField] private LanguageType language;
    [SerializeField] public static LanguageController Instance { get; private set; }
    [SerializeField] public LanguageType Language { get => language; set => language = value; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
}
