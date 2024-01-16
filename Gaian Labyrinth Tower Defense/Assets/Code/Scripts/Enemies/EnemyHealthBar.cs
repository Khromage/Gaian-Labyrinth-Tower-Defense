using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField]
    private Image HealthImage;
    [SerializeField]
    private float DefaultSpeed = 1f;
    [SerializeField]
    private UnityEvent<float> OnHealthUpdate;
    [SerializeField]
    private UnityEvent onCompleted;

    private Coroutine AnimationCoroutine;

    private void Start()
    {
        if (HealthImage.type != Image.Type.Filled)
        {
            Debug.Log("HealthImage is not the right type. Health Bar is being disabled");
            this.enabled = false;
        }
    }

    // Default Health Bar update without speed parameter, uses default speed of 1f
    public void SetHealth(float newHealth)
    {
        SetHealth(newHealth, DefaultSpeed);
    }
    // Overloaded Health Bar update with speed parameter
    public void SetHealth(float Health, float Speed)
    {
        if(/*Health < 0 || */Health > 1)
        {
            Debug.LogWarning("Health passed was not between 0 and 1. Clamping");
            Health = Mathf.Clamp01(Health);
        }
        // Check if updated health value is same as current. If so, don't run extra instructions
        if(Health != HealthImage.fillAmount)
        {
            if(AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }

            AnimationCoroutine = StartCoroutine(AnimateHealth(Health, Speed));
        }
    }

    private IEnumerator AnimateHealth(float Health, float Speed)
    {
        float time = 0f;
        float initialHealth = HealthImage.fillAmount;

        float deltaHealth = initialHealth - Health;

        if (Health <= 0 && Speed < 10f)
        {
            Speed = 10f;
        }

        while (time < 1)
        {
            //added this line, commented out the below line
            HealthImage.fillAmount -= deltaHealth * Time.deltaTime * Speed;
            //HealthImage.fillAmount = Mathf.Lerp(initialHealth, Health, time);
            time += Time.deltaTime * Speed;

            OnHealthUpdate?.Invoke(HealthImage.fillAmount);
            yield return null;
        }

        //HealthImage.fillAmount = Health;
        OnHealthUpdate?.Invoke(Health);
        onCompleted?.Invoke();
    }
}
