using Jaehoon;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterSceneController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private AudioClip entranceSceneSound;
    void Start()
    {
        SoundManager.instance.BackgroundMusicPlay(entranceSceneSound);
        StartCoroutine(TextOnOff());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StopCoroutine(TextOnOff());
            SoundManager.instance.SFXPlay("Sounds/EnterSound");
            StartCoroutine(Wait(3f));
        }
    }
    IEnumerator Wait(float time)
    {
        while (time > 0)
        {
            time-= Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene(1);
    }
    IEnumerator TextOnOff()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.7f);
            titleText.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            yield return new WaitForSeconds(0.35f);
            titleText.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
    }
}
