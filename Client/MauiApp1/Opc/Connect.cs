using Opc.Ua;
using Opc.Ua.Configuration;
using static System.Net.Mime.MediaTypeNames;

namespace OPCClientTest
{
    public class Connect    
    {
        private readonly string url = "opc.tcp://10.0.2.2:4840";
        private readonly string nodeLocation = "ns=2;i=2";

        public async Task<bool> connectPrint()
        {
            var applicationName = "ConsoleReferenceClient";
            var configSectionName = "Quickstarts.ReferenceClient";

            ApplicationInstance application = new ApplicationInstance
            {
                ApplicationName = applicationName,
                ApplicationType = ApplicationType.Client,
                ConfigSectionName = configSectionName,
            };

            //
            ApplicationConfiguration configuration = new ApplicationConfiguration();

            configuration.ApplicationName = "Quickstart Console Reference Client";
            configuration.ApplicationType = ApplicationType.Client;
            configuration.ApplicationUri = $"urn:{Utils.GetHostName()}:Quickstarts:Console ReferenceClient";
            configuration.TransportConfigurations = new TransportConfigurationCollection();
            configuration.TransportQuotas = new TransportQuotas { OperationTimeout = 15000 };
            configuration.ClientConfiguration = new ClientConfiguration { DefaultSessionTimeout = 60000 };

            configuration.SecurityConfiguration = new SecurityConfiguration
            {
                ApplicationCertificate = new CertificateIdentifier
                {
                    StoreType = CertificateStoreType.Directory,
                    StorePath = @"%CommonApplicationData%\OPC Foundation\pki\own",
                    SubjectName = "Quickstart Console Reference Client"
                },
                TrustedPeerCertificates = new CertificateTrustList
                {
                    StoreType = CertificateStoreType.Directory,
                    StorePath = @"%CommonApplicationData%\OPC Foundation\pki\trusted",
                },
                TrustedIssuerCertificates = new CertificateTrustList
                {
                    StoreType = CertificateStoreType.Directory,
                    StorePath = @"%CommonApplicationData%\OPC Foundation\pki\issuer",
                },
                RejectedCertificateStore = new CertificateTrustList
                {
                    StoreType = CertificateStoreType.Directory,
                    StorePath = @"%CommonApplicationData%\OPC Foundation\pki\rejected",
                },
                AutoAcceptUntrustedCertificates = true
            };

            configuration.TraceConfiguration = new TraceConfiguration()
            {
                OutputFilePath = @"%CommonApplicationData%\OPC Foundation\Logs\Quickstarts.ConsoleReferenceClient.log.txt",
                TraceMasks = 1
            };

            configuration.Validate(ApplicationType.Client).Wait();
            //

            application.ApplicationConfiguration = configuration;

            Uri serverUrl = new Uri(this.url);
            UAClient uaClient = new UAClient(application.ApplicationConfiguration);

            var connected = await uaClient.ConnectAsync(serverUrl.ToString());

            if (connected)
            {
                var isAnomaly = uaClient.Session.ReadNode(new NodeId(this.nodeLocation));

                ReadValueIdCollection nodesToRead = new ReadValueIdCollection()
                {
                    new ReadValueId() { NodeId = isAnomaly.NodeId, AttributeId = Attributes.Value },
                };

                uaClient.Session.Read(
                    requestHeader: null,
                    maxAge: 0,
                    TimestampsToReturn.Both,
                    nodesToRead,
                    out DataValueCollection resultsValues,
                    out DiagnosticInfoCollection diagnosticInfos);

                foreach (DataValue result in resultsValues)
                {
                    return Convert.ToBoolean(result.Value);
                }
            }

            return false;
        }
    }
}
