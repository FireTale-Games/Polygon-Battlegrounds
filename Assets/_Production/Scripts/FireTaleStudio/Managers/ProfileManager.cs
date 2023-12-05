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

        //private void ProfileSlot(IProfile profile)
        //{
        //    if (_currentProfiles.TryGetValue(profile.Name, out _))
        //    {
        //        FindAndAssignProfile(profile);
        //        return;
        //    }
//
        //    _activeProfile = null;
        //}
        
        private void ProfileSlot(IProfile profile) => 
            _activeProfile = profile.Name;

        private void FindAndAssignProfile(IProfile profile)
        {
            if (!_currentProfiles.TryGetValue(profile.Name, out Dictionary<int, object> foundProfile)) 
                return;
            
            profile.SetValue(foundProfile[profile.Name]);
            _activeProfile = _currentProfiles.Keys.FirstOrDefault(i => i == profile.Name);
        }
        
        public void CreateNewProfile(IProfile profile, string profileName)
        {
            Dictionary<int, object> newPlayer = new() { [profile.Name] = profileName };
            _currentProfiles.Add(profile.Name, newPlayer);
            _activeProfile = profile.Name;
            profile.SetValue(profileName);
            new DataSaver<Dictionary<int, object>, object>(profile.Name.ToString()).SaveData(newPlayer);
        }

        private object LoadValue(IProfile profile)
        {
            DataLoader<Dictionary<int, object>, object> loadedObject = new(profile.Name.ToString());
            Dictionary<int, object> profileLoad = loadedObject.LoadData();
            
            if (profileLoad == null) return null;

            _currentProfiles[profile.Name] = profileLoad;
            profileLoad.TryGetValue(profile.Name, out object savedValue);
            return savedValue;
        }
        
        public void SetInitialValues(IEnumerable<IProfile> profiles)
        {
            Action<IProfile> OnProfileSelected = ProfileSlot;
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