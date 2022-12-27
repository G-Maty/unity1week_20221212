using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

/*
 * 各文字ごとのクリック時の処理
 * タイマー
 * 問題提示
 * アニメーション、SE
 * 
 */

public class GameManager : MonoBehaviour
{
    private TimeScoreManager TSManager; //タイマー & スコア管理用
    private Subject<string> click_char = new Subject<string>();

    public bool isR = true;
    private string ansChar;
    private bool isSuccess = false;

    //SE関連
    private AudioSource audio;
    [SerializeField] private AudioClip timerSE;
    [SerializeField] private AudioClip wrongAnsSE;
    [SerializeField] private AudioClip timeBonusSE;

    //タイマー関連
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI addTimeText;
    [SerializeField] private TextMeshProUGUI subTimeText;

    private float totalTime = 60;

    //スコア関連
    [SerializeField] private TextMeshProUGUI scoreText;
    private int myScore = 0; //シーン開始時点でのこれまでのスコア

    //オブジェクトプレハブ
    [SerializeField] private GameObject A_prefab;
    [SerializeField] private GameObject R_prefab;
    [SerializeField] private GameObject P_prefab;
    [SerializeField] private GameObject F_prefab;
    [SerializeField] private GameObject E_prefab;

    //オブジェクト生成数
    public int max_A;
    public int max_R;
    public int max_P;
    public int max_F;
    public int max_E;

    //生成範囲
    [Range(-6, 6)] public float instance_minX;
    [Range(-6, 6)] public float instance_maxX;
    [Range(-4, 3)] public float instance_minY;
    [Range(-4, 3)] public float instance_maxY;

    [Range(-5.5f, 5.5f)] public float instanceR_minX;
    [Range(-5.5f, 5.5f)] public float instanceR_maxX;
    [Range(-3.5f, 2.5f)] public float instanceR_minY;
    [Range(-3.5f, 2.5f)] public float instanceR_maxY;



    private void Awake()
    {
        if (isR)
        {
            ansChar = "R";
        }
        else
        {
            ansChar = "E";
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        TSManager = GameObject.FindGameObjectWithTag("TSManager").GetComponent<TimeScoreManager>();
        totalTime = TSManager.stopTime;
        timerText.text = totalTime.ToString(); //タイム表示
        myScore = TSManager.score;
        scoreText.text = myScore.ToString(); //スコアを表示

        instance_charObj();
        this.click_char.Subscribe(myChar =>
        {
            //正解なら次の問題へ、不正解ならタイム縮小
            if(myChar == ansChar)
            {
                isSuccess = true;
                audio.PlayOneShot(timeBonusSE);
                GameObject.FindGameObjectWithTag("Char_R").GetComponent<Collider2D>().enabled = false; //連打対策
                //アニメーション
                AddTimeAnim();
                TSManager.score++; //クリア数＋1
                DestroyCharObj();
            }
            else
            {
                audio.PlayOneShot(wrongAnsSE);
                totalTime -= 5;
                SubTimeAnim();
            }
        });
        StartCoroutine(CountTime()); //タイマースタート
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator CountTime()
    {
        timerText.text = totalTime.ToString(); //タイム表示

        while (!isSuccess){
            yield return new WaitForSeconds(1f);
            totalTime -= 1;
            if (totalTime > 0)
            {
                timerText.text = totalTime.ToString(); //タイムを表示
                audio.PlayOneShot(timerSE);
            }
            else
            {
                //ゲームオーバー処理
                timerText.text = "0";
                audio.PlayOneShot(timerSE);
                DestroyCharObj();
                GameObject.FindGameObjectWithTag("Char_R").GetComponent<Collider2D>().enabled = false;
                StartCoroutine(ShowRanking());
                yield break;
            }
        }
    }

    private IEnumerator ShowRanking()
    {
        yield return new WaitForSeconds(1);
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(myScore); //ランキング
    }

    //オブジェクトの生成
    private void instance_charObj()
    {
        for(int i=0; i < max_A; i++)
        {
            float x = Random.Range(instance_minX, instance_maxX);
            float y = Random.Range(instance_minY, instance_maxY);
            Vector3 pos = new Vector3(x, y, 0);

            Instantiate(A_prefab, pos, Quaternion.identity);
        }

        //Targetは枠内に入るように考慮
        for (int i = 0; i < max_R; i++)
        {
            float x = Random.Range(instanceR_minX, instanceR_maxX);
            float y = Random.Range(instanceR_minY, instanceR_maxY);
            Vector3 pos = new Vector3(x, y, 0);

            Instantiate(R_prefab, pos, Quaternion.identity);
        }

        for (int i = 0; i < max_P; i++)
        {
            float x = Random.Range(instance_minX, instance_maxX);
            float y = Random.Range(instance_minY, instance_maxY);
            Vector3 pos = new Vector3(x, y, 0);

            Instantiate(P_prefab, pos, Quaternion.identity);
        }

        for (int i = 0; i < max_F; i++)
        {
            float x = Random.Range(instance_minX, instance_maxX);
            float y = Random.Range(instance_minY, instance_maxY);
            Vector3 pos = new Vector3(x, y, 0);

            Instantiate(F_prefab, pos, Quaternion.identity);
        }

        for (int i = 0; i < max_E; i++)
        {
            float x = Random.Range(instance_minX, instance_maxX);
            float y = Random.Range(instance_minY, instance_maxY);
            Vector3 pos = new Vector3(x, y, 0);

            Instantiate(E_prefab, pos, Quaternion.identity);
        }
    }

    private void DestroyCharObj()
    {
        GameObject[] A_objects;
        GameObject[] E_objects;
        GameObject[] F_objects;
        GameObject[] P_objects;

        A_objects = GameObject.FindGameObjectsWithTag("Char_A");
        E_objects = GameObject.FindGameObjectsWithTag("Char_E");
        F_objects = GameObject.FindGameObjectsWithTag("Char_F");
        P_objects = GameObject.FindGameObjectsWithTag("Char_P");

        for(int i = 0; i < A_objects.Length; i++)
        {
            Destroy(A_objects[i]);
        }
        for (int i = 0; i < E_objects.Length; i++)
        {
            Destroy(E_objects[i]);
        }
        for (int i = 0; i < F_objects.Length; i++)
        {
            Destroy(F_objects[i]);
        }
        for (int i = 0; i < P_objects.Length; i++)
        {
            Destroy(P_objects[i]);
        }

    }

    private void myLoadScene()
    {
        if(myScore < 5)
        {
            FadeManager.Instance.LoadScene("Level_1", 0.5f);
        }else if (myScore < 10)
        {
            FadeManager.Instance.LoadScene("Level_2", 0.5f);
        }else if(myScore < 15)
        {
            FadeManager.Instance.LoadScene("Level_3", 0.5f);
        }else if(myScore < 20)
        {
            FadeManager.Instance.LoadScene("Level_4", 0.5f);
        }else if(myScore < 25)
        {
            FadeManager.Instance.LoadScene("Level_5", 0.5f);
        }else if(myScore < 30)
        {
            FadeManager.Instance.LoadScene("Level_6", 0.5f);
        }else if(myScore < 35)
        {
            FadeManager.Instance.LoadScene("Level_7", 0.5f);
        }else if(myScore < 40)
        {
            FadeManager.Instance.LoadScene("Level_8", 0.5f); //ここから難しく感じる
        }else if(myScore < 65)
        {
            switch (myScore % 2)
            {
                case 0:
                    FadeManager.Instance.LoadScene("Level_9-2", 0.5f);
                    break;
                case 1:
                    FadeManager.Instance.LoadScene("Level_9", 0.5f);
                    break;
            }
        }
        else if(myScore < 100)
        {
            switch (myScore % 3)
            {
                case 0:
                    FadeManager.Instance.LoadScene("Level_10", 0.5f);
                    break;
                case 1:
                    FadeManager.Instance.LoadScene("Level_10-2", 0.5f);
                    break;
                case 2:
                    FadeManager.Instance.LoadScene("Level_10", 0.5f);
                    break;
            }
        }
        else
        {
            switch (myScore % 3)
            {
                case 0:
                    FadeManager.Instance.LoadScene("Level_max1", 0.5f);
                    break;
                case 1:
                    FadeManager.Instance.LoadScene("Level_max2", 0.5f);
                    break;
                case 2:
                    FadeManager.Instance.LoadScene("Level_max1", 0.5f);
                    break;
            }
        }
    }

    private void AddTimeAnim()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(addTimeText.DOFade(1, 0f));
        sequence.Append(addTimeText.DOFade(0, 1.2f).SetDelay(0.5f));
        sequence.Play().SetLink(gameObject).OnComplete(() =>
        {
            if(totalTime + 4 > 100) //時間上限100秒
            {
                timerText.DOCounter((int)totalTime, 100, 0.5f).SetLink(gameObject)
                .OnComplete(() =>
                {
                    totalTime = 100; //正解ボーナス(時間上限100秒)
                    TSManager.stopTime = totalTime; //タイム引き継ぎ
                    //次のシーン遷移
                    myLoadScene();
                    //デバッグ用
                    //FadeManager.Instance.LoadScene("Level_10-2", 0.5f);
                });
            }
            else
            {
                timerText.DOCounter((int)totalTime, (int)(totalTime + 4), 0.5f).SetLink(gameObject)
                .OnComplete(() =>
                {
                    totalTime += 4; //正解ボーナス
                    TSManager.stopTime = totalTime; //タイム引き継ぎ
                    //次のシーン遷移
                    myLoadScene();
                    //デバッグ用
                    //FadeManager.Instance.LoadScene("Level_10-2", 0.5f);
                });
            }
            
        });
    }

    private void SubTimeAnim()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(subTimeText.DOFade(1, 0f));
        sequence.Append(subTimeText.DOFade(0, 1.2f).SetDelay(0.5f));
        sequence.Play().SetLink(gameObject);
    }



    public void click_A()
    {
        click_char.OnNext("A");
    }

    public void click_E()
    {
        click_char.OnNext("E");
    }

    public void click_F()
    {
        click_char.OnNext("F");
    }

    public void click_P()
    {
        click_char.OnNext("P");
    }

    public void click_R()
    {
        click_char.OnNext("R");
    }
}
