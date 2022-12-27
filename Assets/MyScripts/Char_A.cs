using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UniRx;
using UnityEngine;

public class Char_A : MonoBehaviour
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

    public void click_A()
    {
        gameManager.click_A();
    }
}
