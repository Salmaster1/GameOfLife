using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    GridManager gridManager;
    Camera mainCamera;


    [SerializeField] Slider tickRateSlider, aliveChanceSlider;

    [SerializeField] TextMeshProUGUI tickRateText, pauseButtonText, aliveChanceText, startSizeText, mousePositionText;

    [SerializeField] GameObject startupUI, activeUI;

    int width, height;

    public bool MouseIsOverUI { get; private set; }

    private void Start()
    {
        mainCamera = Camera.main;
        gridManager = GridManager.Instance;
        gridManager.TickRate = tickRateSlider.value;
        gridManager.AliveChance = aliveChanceSlider.value / 100;
        startupUI.SetActive(true);
        activeUI.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Pause();
        }

        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePositionText.text = $"({Mathf.RoundToInt(mousePos.x)}, {Mathf.RoundToInt(mousePos.y)})";
    }

    public void SetTickRate(float value)
    {
        gridManager.TickRate = value;
        tickRateText.text = $"Tickrate: {value:0.00}";
    }

    public void Pause()
    {
        gridManager.Pause = !gridManager.Pause;
        if(gridManager.Pause)
        {
            pauseButtonText.text = "Unpause";
        }
        else
        {
            pauseButtonText.text = "Pause";
        }
    }

    public void SetAlivePercent(float value)
    {
        gridManager.AliveChance = aliveChanceSlider.value / 100;
        aliveChanceText.text = $"Alive Percentage: {value}%";
    }

    public void RegenerateField()
    {
        if (height < 1 || width < 1)
        {
            gridManager.RegenerateField();
            return;
        }
        gridManager.Generate(width, height);

    }

    public void SetNewWidth(string value)
    {
        try
        {
            width = Convert.ToInt32(value);
            if (width < 0)
            {
                width = 0;
            }
        }
        catch
        {
            width = 0;
        }
    }

    public void SetNewHeight(string value)
    {
        try
        {
            height = Convert.ToInt32(value);
            if (height < 0)
            {
                height = 0;
            }
        }
        catch
        {
            height = 0;
        }
    }

    public void SetStartWidth(string value)
    {
        try
        {
            width = Convert.ToInt32(value);
            startSizeText.text = $"Cell Count: {width * height}\nDimentions: {width} x {height}";
            if (width < 0)
            {
                startSizeText.text = $"INVALID INPUT: {value}";
                width = 0;
            }
        }
        catch
        {
            startSizeText.text = $"INVALID INPUT: {value}";
            width = 0;
        }
    }

    public void SetStartHeight(string value)
    {
        try
        {
            height = Convert.ToInt32(value);
            startSizeText.text = $"Cell Count: {width * height}\nDimentions: {width} x {height}";
            if (height < 0)
            {
                startSizeText.text = $"INVALID INPUT: {value}";
                height = 0 ;
            }
        }
        catch
        {
            startSizeText.text = $"INVALID INPUT: {value}";
            height= 0;
        }
    }

    public void Generate()
    {
        //gridManager.Width= width;
        //gridManager.Height= height;
        gridManager.Generate(width, height);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MouseIsOverUI = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MouseIsOverUI = false;
    }
}
