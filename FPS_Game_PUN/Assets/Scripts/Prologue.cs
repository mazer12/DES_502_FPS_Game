using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Prologue : MonoBehaviour
{
    public GameObject[] prologues;
    public GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(7f);
        prologues[1].SetActive(true);
        yield return new WaitForSeconds(7f);
        prologues[2].SetActive(true);
        yield return new WaitForSeconds(7f);
        prologues[3].SetActive(true);
        yield return new WaitForSeconds(7f);
        Destroy(canvas);
        
    }
}
