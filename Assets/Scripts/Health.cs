using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float health;

    public UnityEvent OnHealthChanged;

    public void Damage(float Amount)
    {
        health -= Amount;
        OnHealthChanged.Invoke();
    }
}
