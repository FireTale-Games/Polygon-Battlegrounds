using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private int _activeProfile;

        private void ProfileSlot(IProfile profile)
        {
            if (FindAndAssignProfile(profile))
                return;

            CreateNewProfile(profile);
        }

        private void Awake() => 
            OnProfileSlot.Null()?.AddObserver(ProfileSlot);

        private void OnDestroy() => 
            OnProfileSlot.Null()?.RemoveObserver(ProfileSlot);

        private bool FindAndAssignProfile(IProfile profile)
        {
            if (!_currentProfiles.TryGetValue(profile.Name, out Dictionary<int, object> foundProfile)) 
                return false;
            
            profile.SetValue(foundProfile[profile.Name]);
            _activeProfile = _currentProfiles.Keys.FirstOrDefault(i => i == profile.Name);
            return true;
        }
        
        private void CreateNewProfile(IProfile profile)
        {
            Dictionary<int, object> newPlayer = new();
            int profileName = 0;
            for (int i = 0; i < 9; i++)
                profileName = profileName * 50 + Random.Range(0, 50);

            newPlayer[profile.Name] = profileName;
            _currentProfiles.Add(profile.Name, newPlayer);
            _activeProfile = _currentProfiles.Keys.FirstOrDefault(i => i == profile.Name);
            profile.SetValue(_currentProfiles[profile.Name][profile.Name]);
            IDataSaver<Dictionary<int, object>> saver = new DataSaver<Dictionary<int, object>, object>(profile.Name.ToString());
            saver.SaveData(_currentProfiles[profile.Name]);
        }
        
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
                _currentProfiles[i].TryGetValue(profiles[i].Name, out object savedValue);
                Debug.Log(savedValue);
                Debug.Log(profiles[i].Name);
                profiles[i].Initialize(OnSettingInvoker, savedValue);
            }
        }
    }
}