using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class UIManager : MonoBehaviour
{
    public TMP_Text currentScoreText;
    public TMP_Text bestScoreText;

    public Slider slider;
    public TMP_Text actualLevel;
    public TMP_Text nextLevel;
    public Transform  topTransform;
    public Transform  goalTransform;
    public Transform  ball;


    void Update()
    {
        currentScoreText.text = "Score: " + GameManager.singleton.currentScore;
        bestScoreText.text = "Best: " + GameManager.singleton.bestScore;        
    }

    public void ChangeSliderLevelAndProgress()
    {
        actualLevel.text = "" + (GameManager.singleton.currentLevel + 1);
        nextLevel.text = "" + (GameManager.singleton.currentLevel + 2);

        float totalDistance = (topTransform.position.y - goalTransform.position.y);
        float distanceLeft = totalDistance -(ball.position.y - goalTransform.position.y);
        float value = (distanceLeft / totalDistance);

        slider.value = Mathf.Lerp(slider.value, value, 5);
    }


}
