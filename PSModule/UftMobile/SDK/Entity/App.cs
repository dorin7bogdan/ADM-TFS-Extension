﻿using Newtonsoft.Json;

namespace PSModule.UftMobile.SDK.Entity
{
    public class App
    {
        private const string ANY = "ANY";
        private const string MC_HOME = "MC.Home";
        private const string HOME = "Home";
        public string Type { get; set; } = ANY;

        public string Id { get; set; } = MC_HOME;

        public string Name { get; set; } = HOME;

        public string Version { get; set; }

        public string FileName { get; set; }

        public string Identifier { get; set; } = MC_HOME;

        public bool Instrumented { get; set; }

        public string AppLocalPath { get; set; }

        [JsonIgnore]
        public string Icon { get; set; }

        [JsonIgnore]
        public bool ApplicationExists { get; set; }

        [JsonIgnore]
        public bool InstrumentedApplicationExists { get; set; }

        public int Counter { get; set; }

        public string AppVersion { get; set; }

        public string AppBuildVersion { get; set; }

        public string Source { get; set; }

        public Workspace[] Workspaces { get; set; }

        public override string ToString()
        {
            return @$"Name: ""{Name}"", Identifier: ""{Identifier}"", Version: ""{Version}"", Type: ""{Type}"", Source: ""{Source}""";
        }

        public App() { }

        public string Json4JobUpdate => @$"{{""type"": ""{Type}"",""identifier"": ""{Identifier}""}}";
    }
}
