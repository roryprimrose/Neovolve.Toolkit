namespace Neovolve.Toolkit.Unity
{
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.IO;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Configuration;
    using Neovolve.Toolkit.Storage;
    using Neovolve.Toolkit.Unity.Properties;

    /// <summary>
    /// The <see cref="UnityContainerResolver"/>
    ///   class is used to resolve a <see cref="IUnityContainer"/> instance from configuration.
    /// </summary>
    public static class UnityContainerResolver
    {
        /// <summary>
        /// Resolves the container from configuration using the default configuration options.
        /// </summary>
        /// <returns>
        /// A <see cref="IUnityContainer"/> instance.
        /// </returns>
        /// <exception cref="ConfigurationErrorsException">
        /// No container has been configured.
        /// </exception>
        public static IUnityContainer Resolve()
        {
            Contract.Ensures(Contract.Result<IUnityContainer>() != null);

            return Resolve(null, String.Empty, String.Empty);
        }

        /// <summary>
        /// Resolves the container from configuration using the specified configuration values.
        /// </summary>
        /// <param name="store">
        /// The configuration store.
        /// </param>
        /// <param name="unitySectionName">
        /// Name of the unity section.
        /// </param>
        /// <param name="containerName">
        /// Name of the container.
        /// </param>
        /// <returns>
        /// A <see cref="IUnityContainer"/> instance.
        /// </returns>
        /// <exception cref="ConfigurationErrorsException">
        /// No container has been configured.
        /// </exception>
        public static IUnityContainer Resolve(IConfigurationStore store, String unitySectionName, String containerName)
        {
            Contract.Ensures(Contract.Result<IUnityContainer>() != null);

            IUnityContainer container;
            String failureMessage;

            if (TryResolve(out container, out failureMessage, store, unitySectionName, containerName))
            {
                Contract.Assume(container != null);

                return container;
            }

            throw new ConfigurationErrorsException(failureMessage);
        }

        /// <summary>
        /// Tries to resolve a container.
        /// </summary>
        /// <param name="container">
        /// The resolved container.
        /// </param>
        /// <returns>
        /// <c>true</c> if the container could be resolved; otherwise <c>false</c>.
        /// </returns>
        public static Boolean TryResolve(out IUnityContainer container)
        {
            return TryResolve(out container, null, String.Empty, String.Empty);
        }

        /// <summary>
        /// Tries the resolve.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        /// <param name="store">
        /// The configuration store.
        /// </param>
        /// <param name="unitySectionName">
        /// Name of the unity section.
        /// </param>
        /// <param name="containerName">
        /// Name of the container.
        /// </param>
        /// <returns>
        /// <c>true</c> if the container could be resolved; otherwise <c>false</c>.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", 
            Justification = "Out parameter is required for the TryX member pattern.")]
        public static Boolean TryResolve(out IUnityContainer container, IConfigurationStore store, String unitySectionName, String containerName)
        {
            String failureMessage;

            return TryResolve(out container, out failureMessage, store, unitySectionName, containerName);
        }

        /// <summary>
        /// Configures the container.
        /// </summary>
        /// <param name="failureMessage">
        /// The failure message.
        /// </param>
        /// <param name="container">
        /// The container.
        /// </param>
        /// <param name="store">
        /// The store.
        /// </param>
        /// <param name="unitySectionName">
        /// Name of the unity section.
        /// </param>
        /// <param name="containerName">
        /// Name of the container.
        /// </param>
        /// <returns>
        /// A <see cref="Boolean"/> instance.
        /// </returns>
        private static Boolean ConfigureContainer(
            out String failureMessage, out IUnityContainer container, IConfigurationStore store, String unitySectionName, String containerName)
        {
            Contract.Requires<ArgumentNullException>(store != null);
            Contract.Requires<ArgumentNullException>(String.IsNullOrWhiteSpace(unitySectionName) == false);
            Contract.Ensures(Contract.Result<Boolean>() == false || Contract.ValueAtReturn(out container) != null);

            UnityConfigurationSection config = store.GetSection<UnityConfigurationSection>(unitySectionName);

            container = null;

            if (config == null)
            {
                failureMessage = Resources.UnityContainerResolver_ConfigurationSectionNotFound;

                return false;
            }

            Debug.Assert(config.Containers != null, "No containers found");

            if (config.Containers.Count == 0)
            {
                failureMessage = Resources.UnityContainerResolver_NoContainerConfigured;

                return false;
            }

            ContainerElement containerConfig;

            if (String.IsNullOrEmpty(containerName))
            {
                containerConfig = config.Containers.Default;

                if (containerConfig == null)
                {
                    failureMessage = Resources.UnityContainerResolver_DefaultContainerNotFound;

                    return false;
                }
            }
            else
            {
                containerConfig = config.Containers[containerName];

                if (containerConfig == null)
                {
                    failureMessage = String.Format(
                        CultureInfo.InvariantCulture, Resources.UnityContainerResolver_NamedContainerNotFound, containerName);

                    return false;
                }
            }

            container = new UnityContainer();

            config.Configure(container, containerName);

            failureMessage = String.Empty;

            return true;
        }

        /// <summary>
        /// Tries to resolve the container.
        /// </summary>
        /// <param name="container">
        /// The container to resolve.
        /// </param>
        /// <param name="failureMessage">
        /// The failure message.
        /// </param>
        /// <param name="store">
        /// The configuration store.
        /// </param>
        /// <param name="unitySectionName">
        /// Name of the unity section.
        /// </param>
        /// <param name="containerName">
        /// Name of the container.
        /// </param>
        /// <returns>
        /// <c>true</c> if the container could be resolved; otherwise <c>false</c>.
        /// </returns>
        private static Boolean TryResolve(
            out IUnityContainer container, out String failureMessage, IConfigurationStore store, String unitySectionName, String containerName)
        {
            Contract.Ensures(Contract.Result<Boolean>() == false || Contract.ValueAtReturn(out container) != null);

            if (store == null)
            {
                store = ConfigurationStoreFactory.Create();
            }

            if (String.IsNullOrEmpty(unitySectionName))
            {
                unitySectionName = UnityConfigurationSection.SectionName;
            }

            try
            {
                if (ConfigureContainer(out failureMessage, out container, store, unitySectionName, containerName))
                {
                    return true;
                }

                return false;
            }
            catch (IOException ex)
            {
                // This exception will be thrown when the Microsoft.Practices.Unity.Configuration.dll assembly is not referenced
                failureMessage = ex.ToString();
                container = null;

                return false;
            }
        }
    }
}