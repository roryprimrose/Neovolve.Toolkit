namespace Neovolve.Toolkit.Workflow.UnitTests
{
    using System;
    using System.Configuration;
    using System.Reflection;
    using Microsoft.Practices.Unity.Configuration;

    /// <summary>
    /// The <see cref="TestHelper"/>
    ///   class is used to provide common test helper methods.
    /// </summary>
    internal static class TestHelper
    {
        /// <summary>
        /// Creates the unity section.
        /// </summary>
        /// <param name="containerName">
        /// Name of the container.
        /// </param>
        /// <returns>
        /// A <see cref="UnityConfigurationSection"/> instance.
        /// </returns>
        public static UnityConfigurationSection CreateUnitySection(String containerName)
        {
            ContainerElement container = new ContainerElement
                                         {
                                             Name = containerName
                                         };

            return CreateUnitySection(container);
        }

        /// <summary>
        /// Creates the unity section.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        /// <returns>
        /// A <see cref="UnityConfigurationSection"/> instance.
        /// </returns>
        public static UnityConfigurationSection CreateUnitySection(ContainerElement container)
        {
            UnityConfigurationSection section = new UnityConfigurationSection();

            if (container != null)
            {
                const BindingFlags Flags =
                    BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.FlattenHierarchy |
                    BindingFlags.ExactBinding;
                Type[] types = new[]
                               {
                                   typeof(ConfigurationElement)
                               };
                MethodInfo method = section.Containers.GetType().GetMethod("BaseAdd", Flags, null, types, null);
                Object[] parameters = new Object[]
                                      {
                                          container
                                      };

                method.Invoke(section.Containers, parameters);
            }

            return section;
        }
    }
}