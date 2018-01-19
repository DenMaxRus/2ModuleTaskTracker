using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.ModuleFramework {
	public class ModuleManager {
		private static readonly Lazy<ModuleManager> instance = new Lazy<ModuleManager>();

		public static ModuleManager Instance => instance.Value;

		public Dictionary<String, Module> Modules { get; } = new Dictionary<string, Module>();

		public void Register(Module module) {
			Modules.Add(module.Name, module);
		}
	}
}
