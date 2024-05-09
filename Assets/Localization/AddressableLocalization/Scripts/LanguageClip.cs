using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Localization.Addressable
{
    [RequireComponent(typeof(AudioSource))]
    public class LanguageClip : LanguageObject<AssetReferenceT<AudioClip>>
    {
        private AudioSource audioSource;
        private Coroutine playCoroutine;
        private Coroutine waitCoroutine;
        private AudioClip currentClip;
        private AsyncOperationHandle<AudioClip> handle;
        public UnityEvent clipFinished;
        public UnityEvent onEnable;
        public UnityEvent onDisable;
        
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        protected override void OnLanguageSwitch()
        {
            PlayClip(LocalizationManager.LangName);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            onEnable.Invoke();
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            Release();
            onDisable.Invoke();
        }

        private void PlayClip(LangName langName)
        {
            Release();
            playCoroutine = StartCoroutine(PlayClipForLanguageCoroutine(langName));
        }

        private IEnumerator PlayClipForLanguageCoroutine(LangName langName)
        {
            var reference = Value[langName];
            
            if (reference == null)
            {
                Debug.LogError($"Missing clip on {name}:{nameof(LanguageClip)}", this);
                yield break;
            }
            
            handle = reference.LoadAssetAsync();
            yield return handle;
            
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                currentClip = handle.Result;
                Debug.Log($"Load clip {currentClip.name}");
                audioSource.PlayOneShot(currentClip);
                waitCoroutine = StartCoroutine(WaitReleaseClipCoroutine());
            }
            else
            {
                Debug.LogError($"Cant play clip! {name}:{nameof(LanguageClip)}", this);
                Addressables.Release(handle);
            }

            playCoroutine = null;
        }

        private IEnumerator WaitReleaseClipCoroutine()
        {
            while (audioSource.isPlaying)
            {
                yield return null;
            }
            ReleaseCurrentClip();
            waitCoroutine = null;
            clipFinished.Invoke();
        }

        // полное освобождение памяти
        private void Release()
        {
            audioSource.Stop();
            ReleaseCurrentClip();
            
            if (playCoroutine != null)
            {
                StopCoroutine(playCoroutine);
                playCoroutine = null;
            }

            if (waitCoroutine != null)
            {
                StopCoroutine(waitCoroutine);
                waitCoroutine = null;
            }
        }
        
        private void ReleaseCurrentClip()
        {
            if (currentClip == null)
                return;
            Debug.Log($"Release clip {currentClip.name}");
            Addressables.Release(handle);
            currentClip = null;
            audioSource.clip = null;
        }
    }
}
