using System;
using System.Collections.Generic;
using System.Linq;
using FTS.Data;
using FTS.UI.Profiles;
using UnityEngine;

namespace FTS.Managers
{
    internal sealed class ProfileManager : BaseManager
    {
        private readonly Dictionary<int, Dictionary<int, object>> _currentProfiles = new();
        [SerializeField] private int _activeProfile;

        private void ProfileSlot(IProfile profile)
        {
            if (_currentProfiles.TryGetValue(profile.Name, out _))
                FindAndAssignProfile(profile);
            
            CreateNewProfile(profile);
        }

        private void FindAndAssignProfile(IProfile profile)
        {
            if (!_currentProfiles.TryGetValue(profile.Name, out Dictionary<int, object> foundProfile)) 
                return;
            
            profile.SetValue(foundProfile[profile.Name]);
            _activeProfile = _currentProfiles.Keys.FirstOrDefault(i => i == profile.Name);
        }
        
        private void CreateNewProfile(IProfile profile)
        {
            //int profileName = Enumerable.Range(0, 9).Aggregate(0, (current, _) => current * 50 + Random.Range(0, 50));
            Dictionary<int, object> newPlayer = new() { [profile.Name] = profile.Name };
            _currentProfiles.Add(profile.Name, newPlayer);
            _activeProfile = profile.Name;
            profile.SetValue(profile.Name);
            new DataSaver<Dictionary<int, object>, object>(profile.Name.ToString()).SaveData(newPlayer);
        }
        
        public void SetInitialValues(IEnumerable<IProfile> profiles)
        {
            Action<IProfile> OnProfileSelected = ProfileSlot;
            foreach (IProfile profile in profiles)
            {
                DataLoader<Dictionary<int, object>, object> loadedObject = new(profile.Name.ToString());
                Dictionary<int, object> profileLoad = loadedObject.LoadData();

                object savedValue = null;
                if (profileLoad != null)
                {
                    _currentProfiles[profile.Name] = profileLoad;
                    profileLoad.TryGetValue(profile.Name, out savedValue);
                }

                profile.Initialize(OnProfileSelected, savedValue);
            }
        }
    }
}