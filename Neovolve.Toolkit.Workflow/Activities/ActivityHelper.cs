namespace Neovolve.Toolkit.Workflow.Activities
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The <see cref="ActivityHelper"/>
    ///   class is used to provide helper methods for activities.
    /// </summary>
    internal static class ActivityHelper
    {
        /// <summary>
        /// Generates the display name.
        /// </summary>
        /// <param name="activityType">
        /// Type of the activity.
        /// </param>
        /// <param name="argumentCount">
        /// The argument count.
        /// </param>
        /// <returns>
        /// A <see cref="String"/> instance.
        /// </returns>
        public static String GenerateDisplayName(Type activityType, GenericArgumentCount argumentCount)
        {
            Contract.Requires<ArgumentNullException>(activityType != null);

            String activityName = activityType.Name;

            if (activityType.IsGenericType == false)
            {
                return activityName;
            }

            // Strip the `x from the name
            Int32 genericMarkerIndex = activityName.IndexOf('`');

            if (genericMarkerIndex > -1)
            {
                activityName = activityName.Substring(0, genericMarkerIndex);
            }

            Type[] genericArguments = activityType.GetGenericArguments();
            String displayName = activityName + "<" + genericArguments[0].Name;
            Int32 maxArguments = (Int32)argumentCount;

            if (maxArguments == 1)
            {
                return displayName + ">";
            }

            for (Int32 index = 1; index < maxArguments; index++)
            {
                displayName += ", " + genericArguments[index].Name;
            }

            return displayName + ">";
        }
    }
}