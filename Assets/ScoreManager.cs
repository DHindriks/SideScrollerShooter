using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int Score;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    public void AddScore(int ScoreToAdd)
    {
        Score += ScoreToAdd;
    }

}
