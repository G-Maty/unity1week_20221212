using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char_R : MonoBehaviour
{
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void click_R()
    {
        gameManager.click_R();
    }
}
