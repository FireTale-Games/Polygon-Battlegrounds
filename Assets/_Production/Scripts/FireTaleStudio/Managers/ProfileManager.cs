using System;
using System.Collections.Generic;
using System.Linq;
using FTS.Data;
using FTS.UI.Profiles;

namespace FTS.Managers
{
    internal sealed class ProfileManager : BaseManager
    {
        private readonly Dictionary<int, Dictionary<int, object>> _currentProfiles = new();
        private int _activeProfile;
        public int? GetProfile => FindAndAssignProfile();
        
        private void ProfileSlot(int profileName) => 
            _activeProfile = profileName;

        private int? FindAndAssignProfile()
        {
            if (!_currentProfiles.TryGetValue(_activeProfile, out Dictionary<int, object> foundProfile)) 
                return null;
            
            return foundProfile.Keys.FirstOrDefault(i => i == _activeProfile);
        }
        
        public void CreateNewProfile(string profileName)
        {
            Dictionary<int, object> newPlayer = new() { [_activeProfile] = profileName };
            _currentProfiles.Add(_activeProfile, newPlayer);
            new DataSaver<Dictionary<int, object>, object>(_activeProfile.ToString()).SaveData(newPlayer);
        }

        private object LoadValue(IProfile profile)
        {
            DataLoader<Dictionary<int, object>, object> loadedObject = new(profile.Name.ToString());
            Dictionary<int, object> profileLoad = loadedObject.LoadData();
            
            if (profileLoad == null) 
                return null;

            _currentProfiles[profile.Name] = profileLoad;
            profileLoad.TryGetValue(profile.Name, out object savedValue);
            return savedValue;
        }
        
        public void SetInitialValues(IEnumerable<IProfile> profiles)
        {
            Action<int> OnProfileSelected = ProfileSlot;
            foreach (IProfile profile in profiles)
            {
                object savedValue = LoadValue(profile);
                profile.Initialize(OnProfileSelected, savedValue);
            }
        }

        public void RefreshValues(IEnumerable<IProfile> profiles)
        {
            foreach (IProfile profile in profiles)
            {
                object savedValue = LoadValue(profile);
                profile.SetValue(savedValue);
            }
        }
    }
}