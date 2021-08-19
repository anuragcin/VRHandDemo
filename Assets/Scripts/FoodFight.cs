using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FoodFight : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerDisplay;
    [SerializeField] private TextMeshProUGUI scoreDisplay;
    [SerializeField] private AudioSource audioSource;

    private float countDown = 10f;
    private int score = 0;

    private void OnTriggerEnter(Collider other)
    {
        // display.text = other.tag;
        if (other.CompareTag("Food"))
        {
            audioSource.Play();
            score += 1;
            scoreDisplay.text = "Score: "+ score;

            float x = Random.Range(1,2);
            float y = Random.Range(0, 1);
            float z = Random.Range(0, 2);

            transform.position = new Vector3(x,y,z);
        }
    }

    private void Update()
    {
       
        if (countDown > 0)
        {
            timerDisplay.text = "Time :" + countDown.ToString("F1");
            countDown -= Time.deltaTime;
        }
        else
        {
            timerDisplay.text = "Time : 0:00";
        }
        
    }
}
