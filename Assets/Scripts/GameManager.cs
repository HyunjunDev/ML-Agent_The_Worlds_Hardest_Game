using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    public int death = 0;

    public List<string> sceneList = new List<string>();

    private int curLevel = 0;

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
        DontDestroyOnLoad(this.gameObject);
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            sceneList.Add(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)));
        }
    }

    public void NextScene()
    {
        curLevel++;
        int nextLevel = curLevel;
        if(nextLevel >= sceneList.Count)
        {
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene(sceneList[nextLevel]);
        }
    }
}
