using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Localization.Addressable
{
    public class LanguageObjectPreloader: LanguageObject<AssetReference>
    {
        private AsyncOperationHandle handle;
        
        protected override void OnEnable()
        {
            Load();
        }

        protected override void OnDisable()
        {
            Release();
        }
        
        protected override void OnLanguageSwitch()
        {
            Release();
            Load();
        }

        private void Load()
        {
            Debug.Log($"Preload {CurrentValue}");
            handle = CurrentValue.LoadAssetAsync<Object>();
        }

        private void Release()
        {
            if (CurrentValue == null)
                return;
            Debug.Log($"Release {CurrentValue}");
            Addressables.Release(handle);
        }
    }
}