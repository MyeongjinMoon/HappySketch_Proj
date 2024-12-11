using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Jaehoon
{
    public class SoundManager : MonoBehaviour
    {
        public AudioSource backgroundSound;
        public AudioClip[] backgroundlist;
        public static SoundManager instance;

        private AudioClip currentBackgroundClip;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(instance);
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            for (int i = 0; i < backgroundlist.Length; i++)
            {
                if (arg0.name == backgroundlist[i].name)
                    BackgroundMusicPlay(backgroundlist[i]);
            }
        }

        public void SFXPlay(string sfxName, float delay = 0f)
        {
            StartCoroutine(PlaySFXWithDelay(sfxName, delay));
        }

        private IEnumerator PlaySFXWithDelay(string sfxName, float delay)
        {
            if (delay > 0f)
            {
                yield return new WaitForSeconds(delay);
            }

            AudioClip clip = Resources.Load<AudioClip>(sfxName);

            if (clip == null)
            {
                yield break;
            }

            GameObject go = new GameObject(sfxName + "Sound");
            AudioSource audiosource = go.AddComponent<AudioSource>();
            audiosource.clip = clip;
            audiosource.Play();

            Destroy(go, clip.length);
        }

        public void BackgroundMusicPlay(AudioClip clip)
        {
            if (currentBackgroundClip == clip)
                return;

            backgroundSound.clip = clip;
            backgroundSound.loop = true;
            backgroundSound.volume = 0.5f;
            backgroundSound.Play();

            currentBackgroundClip = clip;
        }
    }
}