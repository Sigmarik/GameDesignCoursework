using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class BBEG : MonoBehaviour
{
    public GameObject head;
    private Rigidbody m_headRB;
    public GameObject eyesOpen;
    public GameObject eyesClosed;

    public GameObject healthDisplay;

    public int health = 7;
    public int m_startingHealth;

    void Start()
    {
        m_headRB = head.GetComponent<Rigidbody>();
        healthDisplay.GetComponent<TextMeshPro>().text = health.ToString();
        m_startingHealth = health;
    }

    public void ResetHealth()
    {
        health = m_startingHealth;
        healthDisplay.GetComponent<TextMeshPro>().text = health.ToString();
    }

    public void Hit(int damage)
    {
        Vector3 randomDirection = UnityEngine.Random.onUnitSphere * 0.3f + Vector3.up;
        float strength = damage * (damage > 0 ? 0.2f : 0.1f);
        strength = Mathf.Min(strength, 2.0f);

        if (damage >= health) strength *= 10.0f;
        m_headRB.AddForce(randomDirection * strength, ForceMode.Impulse);

        health -= damage;
        health = Math.Max(health, 0);
        healthDisplay.GetComponent<TextMeshPro>().text = health.ToString();

        if (damage > 0)
            StartCoroutine(Blink());
    }

    private IEnumerator Blink()
    {
        eyesOpen.SetActive(false);
        eyesClosed.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        eyesOpen.SetActive(true);
        eyesClosed.SetActive(false);
    }

    public void StartTweakingAnimation(int count = 3)
    {
        StartCoroutine(Tweaking(count));
    }

    private IEnumerator Tweaking(int count)
    {
        for (int id = 0; id < count; id++)
        {
            yield return new WaitForSeconds(0.3f);
            Vector3 randomDirection = UnityEngine.Random.onUnitSphere * 0.3f + Vector3.up;
            m_headRB.AddForce(randomDirection * -0.2f, ForceMode.Impulse);
        }
    }
}
