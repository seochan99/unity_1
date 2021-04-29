using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int health;
    public PlayerMove player;
    public GameObject[] Stages;

    public Image[] UIhealth;
    public Text UIPoint;
    public Text UIStage;
    public GameObject RestartBtn;

    void Update()
    {
        UIPoint.text = (totalPoint + stagePoint).ToString();    
    }

    public void NextStage()
    {
        // change Stage
        if (stageIndex < Stages.Length - 1)
        {
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
            PlayerReposition();

            UIStage.text = "STAGE" + (stageIndex+1);
        }
        else
        {
            // Game Clear
            Time.timeScale = 0;
            Debug.Log("게임 클리어");
            
            Text btnText = RestartBtn.GetComponentInChildren<Text>();
            btnText.text = "Clear ! ";
            RestartBtn.SetActive(true);
        }

        totalPoint += stagePoint;
        stagePoint = 0;
    }
    public void HealthDown()
    {
        if (health > 1) {
            health--;
            UIhealth[health].color = new Color(1, 0, 0, 0.4f);
        }
            
        else
        {
            UIhealth[0].color = new Color(1, 0, 0, 0.4f);
            player.Ondie();

            //Result UI
            Debug.Log("bye");
            //Retry Button UI
            RestartBtn.SetActive(true);
        }

    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Player Reposition
            if (health > 1)
            {
                PlayerReposition();
            }
            //Health Down
            HealthDown();
        }
    }
   void PlayerReposition()
    {
        player.transform.position = new Vector3(-2, 2, 0);
        player.VelocityZero();
    }
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
            
   }

