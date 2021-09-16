namespace PSModule.AlmLabMgmtClient.SDK.Util
{
    public class Args
    {
        public string Username { get; internal set; }
        public string Password { get; internal set; }
        public string ServerUrl { get; internal set; }
        public string ClientType { get; internal set; }
        public string RunType { get; internal set; }
        public string EntityId { get; internal set; }
        public string Domain { get; internal set; }
        public string Project { get; internal set; }
        public string Duration { get; internal set; }
        public string EnvironmentConfigurationId { get; internal set; }
        public string Description { get; internal set; }
    }
}