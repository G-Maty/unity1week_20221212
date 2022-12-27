using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeScoreManager : SingletonMonoBehaviour<TimeScoreManager>
{
    [SerializeField] float maxTime = 60f;
    public float stopTime { get; set; }
    public int score { get; set; }

    private AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        audio = GetComponent<AudioSource>();
        audio.Play();
        stopTime = maxTime;
        score = 0;
    }

    private void Update()
    {
    }
}
