﻿using System;
using System.IO;
using QuantoAgent.Database;

namespace QuantoAgent {
    public static class Configuration {
        public static string SyslogServer { get; private set; }
        public static string SyslogFacility { get; private set; }
        public static string PrivateKeyFolder { get; private set; }
        public static string KeyPrefix { get; private set; }
        public static string SKSServer { get; private set; }
        public static int HttpPort { get; private set; }
        public static int MaxKeyRingCache { get; private set; }
        public static string MasterGPGKeyPath { get; private set; }
        public static string MasterGPGKeyPassword { get; private set; }

        static Configuration() {
            SyslogServer = ConfigurationManager.Get("SYSLOG_IP", "127.0.0.1");
            SyslogFacility = ConfigurationManager.Get("SYSLOG_FACILITY", "LOG_USER");
            PrivateKeyFolder = ConfigurationManager.Get("PRIVATE_KEY_FOLDER", "keys");
            SKSServer = ConfigurationManager.Get("SKS_SERVER", "http://pgp.mit.edu/");
            KeyPrefix = ConfigurationManager.Get("KEY_PREFIX", "");

            var mkrc = ConfigurationManager.Get("MAX_KEYRING_CACHE_SIZE", "1000");
            MaxKeyRingCache = int.Parse(mkrc);

            var hp = ConfigurationManager.Get("HTTP_PORT", "5100");
            HttpPort = int.Parse(hp);

            MasterGPGKeyPath = ConfigurationManager.Get("MASTER_GPG_KEY_PATH", "master.key");
            MasterGPGKeyPassword = ConfigurationManager.Get("MASTER_GPG_KEY_PASSWORD", "quanto-agent");

#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
            try { Directory.CreateDirectory(PrivateKeyFolder); } catch (Exception) { }
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
        }
    }
}
