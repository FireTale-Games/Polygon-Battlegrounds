using System.Collections.Generic;
using FTS.Data;
using FTS.Tools.ExtensionMethods;
using FTS.Tools.ScriptableEvents;
using FTS.UI.Profiles;
using UnityEngine;

namespace FTS.Managers
{
    internal sealed class ProfileManager : MonoBehaviour
    {
        private EventObserver<IProfile> OnProfileSlot => _onProfileSlot ??= ExtensionMethods.LoadEventObject<IProfile>(nameof(OnProfileSlot));
        private EventObserver<IProfile> _onProfileSlot;
        private readonly Dictionary<int, Dictionary<int, object>> _currentProfiles = new();
        private Dictionary<int, object> _activeProfile;

        private void ProfileSlot(IProfile profile)
        {
            if (_currentProfiles.TryGetValue(profile.Name, out _) == false)
            {
                Dictionary<int, object> newPlayer = new();
                newPlayer[profile.Name] = 12345;
                _currentProfiles.Add(profile.Name, newPlayer);
                _activeProfile = _currentProfiles[profile.Name];
                profile.SetValue(_currentProfiles[profile.Name][profile.Name]);
                IDataSaver<Dictionary<int, object>> saver = new DataSaver<Dictionary<int, object>, object>(profile.Name.ToString());
                saver.SaveData(_activeProfile);
            }
            
            Debug.Log("Hello");
        }
        
        private void Awake()
        {
            OnProfileSlot.Null()?.AddObserver(ProfileSlot);
        }

        private void OnDestroy() => 
            OnProfileSlot.Null()?.RemoveObserver(ProfileSlot);

        public void SetInitialValues(IProfile[] profiles)
        {
            EventInvoker<IProfile> OnSettingInvoker = ExtensionMethods.LoadEventObject<IProfile>(nameof(OnProfileSlot));
            for (int i = 0; i < profiles.Length; i++)
            {
                DataLoader<Dictionary<int, object>, object> loadedObject = new(profiles[i].Name.ToString());
                Dictionary<int, object> profileLoad = loadedObject.LoadData();
                if (profileLoad == null)
                {
                    profiles[i].Initialize(OnSettingInvoker, null);
                    continue;
                }

                _currentProfiles[i] = profileLoad;
                profiles[i].Initialize(OnSettingInvoker, _currentProfiles[i].TryGetValue(profiles[i].Name, out object savedValue) ? savedValue : null);
            }
        }
    }
}