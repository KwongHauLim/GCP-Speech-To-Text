using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Language.V1;
using Google.Cloud.Speech.V1;
using Grpc.Auth;
using Grpc.Core;

namespace HaLi.GoogleSpeech
{
    public sealed class GCP
    {
        private readonly FileInfo JsonFile = new FileInfo("GoogleCredential.json");
        private static GCP _ptr = null;
        public static GCP Share => _ptr = _ptr ?? new GCP();

        public GoogleCredential Credential { get; set; }

        public Channel Channel { get; private set; }
        public SpeechClient Client { get; private set; }
        
        private GCP()
        {
            Init();
        }

        public void Init()
        {
            if (Client == null && JsonFile.Exists)
            {
                Credential = GoogleCredential.FromFile(JsonFile.FullName).CreateScoped(LanguageServiceClient.DefaultScopes);
                Channel = new Channel(SpeechClient.DefaultEndpoint.Host, Credential.ToChannelCredentials());
                Client = SpeechClient.Create(Channel);
            }
        }
    }
}
