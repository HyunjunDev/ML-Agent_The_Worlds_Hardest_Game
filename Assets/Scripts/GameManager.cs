using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    public int death = 0;

    public Text deathText;

    public static GameManager Instance
    {
        get
        {
            if(instance==null)
            {
                GameObject obj = new GameObject("GameManager");
                obj.AddComponent<GameManager>();
                instance = obj.GetComponent<GameManager>();
            }
            return instance;
        }
        private set { instance = value; }
    }

    private void Awake()
    {
        instance = this;
        SetDeathText();
        DontDestroyOnLoad(this.gameObject);
    }

    public void SetDeathText()
    {
        deathText.text = "Death : " + death;
    }

    public void SceneLoad()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
