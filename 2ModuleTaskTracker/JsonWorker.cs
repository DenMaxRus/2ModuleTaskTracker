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

        public List<Responsible> LoadResponsiblesFromJson(string path)
        {
            return new List<Responsible>() { new Responsible(0, "Test1"), new Responsible(1, "Test2") };
            //return JsonConvert.DeserializeObject<List<Responsible>>(File.ReadAllText(path));
        }

        public void SaveTaskInformation(List<Task> tasks, List<Responsible> responsibles, string path)
        {
            Dictionary<int, double> result = new Dictionary<int, double>();
            foreach (Responsible r in responsibles)
            {
                result.Add(r.ID, 0);
                foreach(Task t in tasks)
                {
                    if(r.ID == t.Responsible.ID)
                    {
                        result[r.ID] += t.Duration;
                    }
                }
            }
            File.WriteAllText(path, JsonConvert.SerializeObject(result));
        }
    }
}
