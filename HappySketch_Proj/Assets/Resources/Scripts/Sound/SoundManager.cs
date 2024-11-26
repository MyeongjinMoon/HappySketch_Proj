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

        private AudioClip currentBackgroundClip;        // 현재 재생 중인 배경음악
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
                    BackgroundSoundPlay(backgroundlist[i]);
            }
        }
        public void SFXPlay(string sfxName, AudioClip clip)
        {
            GameObject go = new GameObject(sfxName + "Sound");
            AudioSource audiosource = go.AddComponent<AudioSource>();
            audiosource.clip = clip;
            audiosource.Play();

            Destroy(go, clip.length);
        }

        public void BackgroundSoundPlay(AudioClip clip)
        {
            if (currentBackgroundClip == clip)
                return;

            backgroundSound.clip = clip;
            backgroundSound.loop = true;
            backgroundSound.volume = 0.1f;
            backgroundSound.Play();

            currentBackgroundClip = clip;       // 현재 배경음악 업데이트
        }
    }
}
