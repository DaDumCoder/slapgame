using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public CharacterSc player;
    public FinishSc finish;
    public CamFollower cam;

    public Animator camAnim, uiAnim;

    public CharacterSc[] players;
    public TextMeshProUGUI levelTxt;
    public GameObject gameOverPanel;

    private void Awake()
    {
		
		
        if (PlayerPrefs.GetInt("FirstTime") == 0)
        {
            PlayerPrefs.SetInt("FirstTime", 1);
            PlayerPrefs.SetInt("Level", 1);
        }
    }

    private void Start()
    {
        Implementation.Instance.ShawBanner();

        levelTxt.text = "LVL " + PlayerPrefs.GetInt("Level").ToString("00");
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }


    public void CamZoom(int zoom,float time)
    {
        Camera.main.DOKill();
        Camera.main.DOFieldOfView(zoom, time);
    }


    public void StartPlaying()
    {
        camAnim.SetBool("Start", true);
        uiAnim.SetBool("Start", true);
        foreach (CharacterSc character in players)
        {
            character.move = true;
        }
    }

    public void Lose()
    {
        //Debug.Log("LOSE");
        player.move = false;
        cam.enabled = false;
        cam.transform.DOLookAt(FindObjectOfType<FinishSc>().bulb.transform.position, 2f);
        uiAnim.SetBool("Lose", true);
        gameOverPanel.SetActive(true);
        gameOverPanel.transform.GetChild(0).GetChild(3).gameObject.GetComponent<Button>().interactable = false;
        Implementation.Instance.ShowInterstitial();
    }
    public void Won()
    {
        //Debug.Log("WON");
        finish.wonFx.Play();
        uiAnim.SetBool("Won", true);
        camAnim.SetBool("Won", true);
        CamZoom(60, 1);
        Implementation.Instance.ShowInterstitial();
        gameOverPanel.SetActive(true);
        gameOverPanel.transform.GetChild(0).GetChild(2).gameObject.GetComponent<Button>().interactable = true;
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
    }

    public void Replay()
    {
        SceneManager.LoadScene(0);
    }
}
