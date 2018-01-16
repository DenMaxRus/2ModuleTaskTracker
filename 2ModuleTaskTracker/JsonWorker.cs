using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace _2ModuleTaskTracker
{
    class JsonWorker
    {
        public void SaveTasksToJson(List<Task> tasks, string path)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(tasks));
        }

        public List<Task> LoadTasksFromJson(string path)
        {
            return JsonConvert.DeserializeObject<List<Task>>(File.ReadAllText(path));
        }
    }
}
