using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultSceneManager : MonoBehaviour
{
    public GameObject topImage;
    public Text[] scoreText;
    
    public float limitTime = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        int count, wrong;
        float average, aveTime;
        
        count = Gamemanager.GetCount();
        wrong = Gamemanager.GetWrong();
        average = (count * 100.0f) / (count + wrong);
        aveTime = 20.0f / (wrong + count);
        scoreText[0].text = count.ToString();
        scoreText[1].text = wrong.ToString();
        scoreText[2].text = average.ToString("f1") + "%";
        scoreText[3].text = "平均回答時間 : " + aveTime.ToString("f2") + "s";
    }

    // Update is called once per frame
    void Update()
    {
        if (limitTime <= 0)
        {
            Destroy(topImage);
        }
        else
        {
            limitTime -= Time.deltaTime;
        }
    }

    public void PuhuRetryButton()
    {
        SceneManager.LoadScene("StartScene");
    }

}
