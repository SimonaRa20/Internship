using System.Reflection;

namespace Savanna.Plugins
{
    public class PluginLoader
    {
        public static List<IAnimal> Plugins { get; set; }

        public void LoadPlugins()
        {
            Plugins = new List<IAnimal>();

            if (Directory.Exists(Constants.FolderName))
            {
                string[] files = Directory.GetFiles(Constants.FolderName);
                foreach (string file in files)
                {
                    if (file.EndsWith(".dll"))
                    {
                        Assembly.LoadFile(Path.GetFullPath(file));
                    }
                }
            }

            Type interfaceType = typeof(IAnimal);
            Type[] types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(p => interfaceType.IsAssignableFrom(p) && p.IsClass)
                .ToArray();
            foreach (Type type in types)
            {
                Plugins.Add((IAnimal)Activator.CreateInstance(type));
            }
        }
    }
}