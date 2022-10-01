using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class StatBar : MonoBehaviour
{
    public Color color;
    public Color changeColor;
    public TextMeshProUGUI text;

    public Transform bar, updateBar;
    float lastValue = 0;
    private void Start() {
        bar.GetComponent<Image>().color = color;
    }
    public void UpdateStat(float currentValue, float maxValue){
        Transform instantBar, catchBar;
        if(updateBar)
        {
            if(lastValue > currentValue || lastValue == 0)
            {
                instantBar = bar;
                catchBar = updateBar;
            }
            else{
                instantBar = updateBar;
                catchBar = bar;
            }
        }
        else{
            instantBar = bar;
            catchBar = null;
        }

        instantBar.localScale = new Vector3(Mathf.Clamp(currentValue/(float)maxValue, 0.001f, 1), 1, 1);
        if(catchBar)
        {
            catchBar.DOScale(instantBar.localScale, 1f).SetEase(Ease.InElastic);
        }
        if(text)
        {
            text.text = $"{(int)currentValue}/{(int)maxValue}";
        }
        lastValue = currentValue;
    }
}