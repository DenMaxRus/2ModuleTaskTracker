using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace CommonLibrary.entities
{
    public class UserRole : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        [JsonProperty]
        private List<string> actionAccess;
        private string id;

        public string Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }

        public string Name { get; set; }

        public UserRole() 
        {
            actionAccess = new List<string>();
        }

        private static string ModuleActionToKey(string module, string action)
        {
            return module + "." + action;
        }

        public bool IsHaveAccessTo(string module, string action)
        {
            string key = ModuleActionToKey(module, action);
            return actionAccess.Contains(key);
        }

        public UserRole SetAccess(string module, string action, bool access)
        {
            string key = ModuleActionToKey(module, action);

            if (access)
            {
                if (!actionAccess.Contains(key))
                {
                    actionAccess.Add(key);
                    OnPropertyChanged();
                }
            }
            else
            {
                if (actionAccess.Remove(key))
                    OnPropertyChanged();
            }

			return this;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}