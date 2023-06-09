using Opc.Ua;
using Opc.Ua.Client;

namespace OPCClientTest
{
    public class UAClient
    {
        public Session Session => session;

        private ApplicationConfiguration appConfiguration;
        private Session session;

        public UAClient(ApplicationConfiguration applicationConfiguration)
        {
            this.appConfiguration = applicationConfiguration;
        }

        public async Task<bool> ConnectAsync(string serverUrl)
        {
            try
            {
                EndpointDescription endpointDescription = new EndpointDescription(serverUrl);
                EndpointConfiguration endpointConfiguration = EndpointConfiguration.Create(this.appConfiguration);
                ConfiguredEndpoint configuredEndpoint = new ConfiguredEndpoint(null, endpointDescription, endpointConfiguration);

                var session = await Opc.Ua.Client.Session.Create(
                    appConfiguration,
                    configuredEndpoint,
                    updateBeforeConnect: true,
                    sessionName: "Test",
                    sessionTimeout: 60000,
                    identity: new UserIdentity(),
                    preferredLocales: null).ConfigureAwait(false);

                if (session != null && session.Connected)
                {
                    Console.WriteLine("Conexion exitosa");
                    this.session = session;

                    return true;
                }

                Console.WriteLine("Conexion fallida");

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");

                return false;
            }
        }


        public void Browse()
        {
            Opc.Ua.Client.Browser browser = new Opc.Ua.Client.Browser(this.session);

            browser.BrowseDirection = BrowseDirection.Forward;
            browser.NodeClassMask = (int)NodeClass.Object | (int)NodeClass.Variable;
            browser.ReferenceTypeId = ReferenceTypeIds.HierarchicalReferences;
            browser.IncludeSubtypes = true;

            NodeId nodeToBrowse = ObjectIds.Server;

            ReferenceDescriptionCollection browseResults = browser.Browse(nodeToBrowse);

            foreach (ReferenceDescription result in browseResults)
            {
                Console.WriteLine($"{result.DisplayName.Text} : {result.NodeId} : {result.BrowseName.NamespaceIndex}");
            }
        }
    }
}
