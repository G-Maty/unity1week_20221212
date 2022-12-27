using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    private TimeScoreManager TSManager;
    // Start is called before the first frame update
    void Start()
    {
        //タイムとスコアを初期化
        TSManager = GameObject.FindGameObjectWithTag("TSManager").GetComponent<TimeScoreManager>();
        TSManager.stopTime = 60f;
        TSManager.score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartTutrial()
    {
        FadeManager.Instance.LoadScene("Tutrial", 0.5f);
    }

    public void StartGame()
    {
        FadeManager.Instance.LoadScene("FirstStep", 0.5f);
    }

}
