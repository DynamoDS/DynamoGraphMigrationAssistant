using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dynamo.Extensions;
using Dynamo.Wpf.Extensions;

namespace DynamoGraphMigrationAssistant
{
    public class PythonOverride : IViewExtension
    {
        public void Dispose()
        {
            //
        }

        public void Startup(ViewStartupParams viewStartupParams)
        {
           //
        }

        public void Loaded(ViewLoadedParams viewLoadedParams)
        {
            //
        }

        public void Shutdown()
        {
           //
        }
        private const string EXTENSION_NAME = "Python Migration Override";
        private const string EXTENSION_GUID = "1f8146d0-58b1-4b3c-82b7-34a3fab5ac5d";
        /// <summary>
        /// Extension GUID
        /// </summary>
        public string UniqueId { get { return EXTENSION_GUID; } }

        /// <summary>
        /// Extension Name
        /// </summary>
        public string Name { get { return EXTENSION_NAME; } }
    }

    public class WorkspaceReferencesOverride : IViewExtension
    {
        public void Dispose()
        {
            //
        }

      

        public void Startup(ViewStartupParams viewStartupParams)
        {
           //
        }

        public void Loaded(ViewLoadedParams viewLoadedParams)
        {
            //
        }

        public void Shutdown()
        {
            throw new NotImplementedException();
        }
        private const string EXTENSION_NAME = "Workspace References Override";
        private const string EXTENSION_GUID = "A6706BF5-11C2-458F-B7C8-B745A77EF7FD";
        /// <summary>
        /// Extension GUID
        /// </summary>
        public string UniqueId { get { return EXTENSION_GUID; } }

        /// <summary>
        /// Extension Name
        /// </summary>
        public string Name { get { return EXTENSION_NAME; } }
    }
}
