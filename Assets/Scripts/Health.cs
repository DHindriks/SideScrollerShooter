using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float health;

    [SerializeField] int KillScore;

    public UnityEvent OnHealthChanged;
    public UnityEvent OnHealthZero;

    public void Damage(float Amount)
    {
        health -= Amount;
        OnHealthChanged.Invoke();
        CheckHealth();
    }

    void CheckHealth()
    {
        if (health <= 0)
        {
            OnHealthZero.Invoke();
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(KillScore);
            }
        }
    }
}
