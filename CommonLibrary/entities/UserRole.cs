using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace CommonLibrary.entities
{
    public class UserRole : INotifyPropertyChanged
    {
        private static int GlobalId { get; set; } = 0;

        public event PropertyChangedEventHandler PropertyChanged;

        // Не стал делать enum, потому что по идее каждый модуль сам знает какие в нем есть действия
        // Значит не стоит их задавать тут
        // И модуль должен уметь сказать какие у него есть возможные права для 
        // Заполнения колонок таблицы редактирования прав
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

        public UserRole() 
        {
            actionAccess = new List<string>();
            Id = "role_" + GlobalId++;
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

        public void SetAccess(string module, string action, bool access)
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
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
