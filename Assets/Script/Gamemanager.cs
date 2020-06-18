using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{

    public GameObject[] drop = new GameObject[5];/* メインで使うオブジェクト */
    public GameObject[] prefab;/* プレファブ */

    /* positionの管理のための配列 */
    public Transform[] tr = new Transform[5];
    public Vector3[] pos = new Vector3[5];
    /* 乱数生成を記録 */
    public int num;

    /* 親オブジェクト */
    public GameObject ImageBack;

    /* 傘 */
    public GameObject umbrella;
    public Transform umbrellaTr;
    public Vector3 umbrellaPos;

    /* 変数 */
    public static int count = 0, wrong = 0;
    public float limitTime = 20.0f;
    //public Text timeText; STRATと表示させるときに使用

    /* 画像の透明度 */
    float colorClear;
    public GameObject alert;
    // Start is called before the first frame update
    void Start()
    {
        /* 初期配置 */
        count = 0;// 正解の回数
        wrong = 0;// 失敗の回数
        colorClear = 1.0f;
        alert.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);// 最初は見えないようになっている
        float y = 420.0f;

        for(int i = 0; i < 5; i++)//ランダムにオブジェクトを5個生成
        {
            drop[i] = (GameObject)Instantiate(prefab[Random.Range(0, prefab.Length)]);
            drop[i].transform.SetParent(ImageBack.transform,false);
            tr[i] = drop[i].transform;
            pos[i] = tr[i].localPosition;
            pos[i].y = y;
            pos[i].x += Random.Range(0, 2) * 200.0f;
            tr[i].localPosition = pos[i];
            y -= 180.0f;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (count != 0)
        {
            //時間をカウントダウンする
            limitTime -= Time.deltaTime;
            if (limitTime <= 0)
            {
                SceneManager.LoadScene("ResultScene");
            }
            else if(limitTime <= 5)
            {
                //Destroy(timeText);
                /* 5秒以下の時アラート発生 */
                alert.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, colorClear);
                if (colorClear <= 0)// 色の濃さで表現
                {
                    colorClear = 1.0f;
                }
                else
                {
                    colorClear -= 0.01f;
                }
            }
        }
    }

    public void PushButton(int x)
    {
        /* 傘の移動 */
        umbrellaTr = umbrella.transform;
        umbrellaPos = umbrellaTr.localPosition;
        umbrellaPos.x = x;
        umbrellaTr.localPosition = umbrellaPos;

        if (Judge(x)) count++;
        else
        {
            wrong++;
            if (SystemInfo.supportsVibration)
            {

                Handheld.Vibrate();

            }
            else
            {
                print("cannot vibrate.");
            }
            return;
        }

        Destroy(drop[4]);

        for(int i = 4; i > 0; i--)/* 1段ずつずらす */
        {
            drop[i] = drop[i - 1];
            pos[i] = pos[i - 1];
            tr[i] = tr[i - 1];
            pos[i].y -= 180.0f;
            tr[i].localPosition = pos[i];
        }
        /* 新しいオブジェクトの作成 */
        num = Random.Range(0, prefab.Length);
        drop[0] = (GameObject)Instantiate(prefab[num]);
        drop[0].transform.SetParent(ImageBack.transform, false);
        tr[0] = drop[0].transform;
        pos[0] = tr[0].localPosition;
        pos[0].x += Random.Range(0, 2) * 200.0f;
        tr[0].localPosition = pos[0];

        //Debug.Log(drop[0].transform.tag);
    }

    public bool Judge(int direction)
    {
        if(drop[4].transform.tag == "Rain")
        {
            if(pos[4].x == direction)
            {
                return true;
            }
            else
            {
                return false;
            }
        }else if(drop[4].transform.tag == "Thunder")
        {
            if (pos[4].x != direction)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }
    public static int GetCount()
    {
        return count;
    }
    public static int GetWrong()
    {
        return wrong;
    }
}
