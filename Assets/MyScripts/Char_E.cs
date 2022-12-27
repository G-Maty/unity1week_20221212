using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char_E : MonoBehaviour
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

    public void click_E()
    {
        gameManager.click_E();
    }
}
