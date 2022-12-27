using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

/*
 * �e�������Ƃ̃N���b�N���̏���
 * �^�C�}�[
 * ����
 * �A�j���[�V�����ASE
 * 
 */

public class GameManager : MonoBehaviour
{
    private TimeScoreManager TSManager; //�^�C�}�[ & �X�R�A�Ǘ��p
    private Subject<string> click_char = new Subject<string>();

    public bool isR = true;
    private string ansChar;
    private bool isSuccess = false;

    //SE�֘A
    private AudioSource audio;
    [SerializeField] private AudioClip timerSE;
    [SerializeField] private AudioClip wrongAnsSE;
    [SerializeField] private AudioClip timeBonusSE;

    //�^�C�}�[�֘A
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI addTimeText;
    [SerializeField] private TextMeshProUGUI subTimeText;

    private float totalTime = 60;

    //�X�R�A�֘A
    [SerializeField] private TextMeshProUGUI scoreText;
    private int myScore = 0; //�V�[���J�n���_�ł̂���܂ł̃X�R�A

    //�I�u�W�F�N�g�v���n�u
    [SerializeField] private GameObject A_prefab;
    [SerializeField] private GameObject R_prefab;
    [SerializeField] private GameObject P_prefab;
    [SerializeField] private GameObject F_prefab;
    [SerializeField] private GameObject E_prefab;

    //�I�u�W�F�N�g������
    public int max_A;
    public int max_R;
    public int max_P;
    public int max_F;
    public int max_E;

    //�����͈�
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
        timerText.text = totalTime.ToString(); //�^�C���\��
        myScore = TSManager.score;
        scoreText.text = myScore.ToString(); //�X�R�A��\��

        instance_charObj();
        this.click_char.Subscribe(myChar =>
        {
            //�����Ȃ玟�̖��ցA�s�����Ȃ�^�C���k��
            if(myChar == ansChar)
            {
                isSuccess = true;
                audio.PlayOneShot(timeBonusSE);
                GameObject.FindGameObjectWithTag("Char_R").GetComponent<Collider2D>().enabled = false; //�A�ő΍�
                //�A�j���[�V����
                AddTimeAnim();
                TSManager.score++; //�N���A���{1
                DestroyCharObj();
            }
            else
            {
                audio.PlayOneShot(wrongAnsSE);
                totalTime -= 5;
                SubTimeAnim();
            }
        });
        StartCoroutine(CountTime()); //�^�C�}�[�X�^�[�g
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator CountTime()
    {
        timerText.text = totalTime.ToString(); //�^�C���\��

        while (!isSuccess){
            yield return new WaitForSeconds(1f);
            totalTime -= 1;
            if (totalTime > 0)
            {
                timerText.text = totalTime.ToString(); //�^�C����\��
                audio.PlayOneShot(timerSE);
            }
            else
            {
                //�Q�[���I�[�o�[����
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
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(myScore); //�����L���O
    }

    //�I�u�W�F�N�g�̐���
    private void instance_charObj()
    {
        for(int i=0; i < max_A; i++)
        {
            float x = Random.Range(instance_minX, instance_maxX);
            float y = Random.Range(instance_minY, instance_maxY);
            Vector3 pos = new Vector3(x, y, 0);

            Instantiate(A_prefab, pos, Quaternion.identity);
        }

        //Target�͘g���ɓ���悤�ɍl��
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
            FadeManager.Instance.LoadScene("Level_8", 0.5f); //����������������
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
            if(totalTime + 4 > 100) //���ԏ��100�b
            {
                timerText.DOCounter((int)totalTime, 100, 0.5f).SetLink(gameObject)
                .OnComplete(() =>
                {
                    totalTime = 100; //�����{�[�i�X(���ԏ��100�b)
                    TSManager.stopTime = totalTime; //�^�C�������p��
                    //���̃V�[���J��
                    myLoadScene();
                    //�f�o�b�O�p
                    //FadeManager.Instance.LoadScene("Level_10-2", 0.5f);
                });
            }
            else
            {
                timerText.DOCounter((int)totalTime, (int)(totalTime + 4), 0.5f).SetLink(gameObject)
                .OnComplete(() =>
                {
                    totalTime += 4; //�����{�[�i�X
                    TSManager.stopTime = totalTime; //�^�C�������p��
                    //���̃V�[���J��
                    myLoadScene();
                    //�f�o�b�O�p
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
