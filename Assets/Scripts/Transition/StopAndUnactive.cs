using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StopAndUnactive : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;
    [SerializeField] private GameObject Transi;
 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(WaitAndPlayCine());
    }

    IEnumerator WaitAndPlayCine()
    {
        m_Animator.Play("heliceReversed");
        yield return new WaitForSeconds(1f);
        Transi.SetActive(false);
    }
}
