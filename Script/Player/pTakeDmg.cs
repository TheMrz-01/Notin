using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class pTakeDmg : MonoBehaviour
{
    public float pHealth,maxHealth = 100;
    public Slider slider;

    private void Start()
    {
        slider.maxValue = maxHealth;
        pHealth = maxHealth;
        slider.value = pHealth;
    }
    public void pTakeDamg(float dmg)
    {
        pHealth -= dmg;
        slider.value = pHealth;
        if (0 >= pHealth){
            Destroy(gameObject);
        }
    }
}
