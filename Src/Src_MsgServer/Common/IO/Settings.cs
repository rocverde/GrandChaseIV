﻿using GrandChase.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace GrandChase.IO
{
    public static class Settings
    {
        private const string Comment = "#";

        public static string Path { get; private set; }

        private static Dictionary<string, string> Dictionary;

        public static void Initialize(string path = null)
        {
            if (path == null)
            {
                path = "config_server.ini";
            }

            if (Settings.Dictionary != null)
            {
                Settings.Dictionary.Clear();
            }

            Settings.Path = path;
            Settings.Dictionary = new Dictionary<string, string>();

            string[] array = Settings.Path.Split('\\');
            string name = array[array.Length - 1];

            if (!File.Exists(path))
            {
                throw new FileNotFoundException(string.Format("Não é possível encontrar o arquivo de configuração. '{0}'.", name));
            }
            else
            {
                string line;
                string currentSection = string.Empty;

                using (StreamReader file = new StreamReader(path))
                {
                    while ((line = file.ReadLine()) != null)
                    {
                        if (line.StartsWith(Settings.Comment))
                            continue;

                        if (line.StartsWith("[") && line.EndsWith("]"))
                        {
                            currentSection = line.Trim('[', ']');
                        }
                        else if (line.Contains("="))
                        {
                            Settings.Dictionary.Add(string.Format("{0}{1}{2}",
                                currentSection,
                                (currentSection != string.Empty) ? "/" : string.Empty,
                                line.Split('=')[0]),
                                line.Split('=')[1].Split(';')[0]);
                        }
                    }
                }
            }

            Database.Host = Settings.GetString("Database/Host");
            Database.Schema = Settings.GetString("Database/Schema");
            Database.Username = Settings.GetString("Database/Username");
            Database.Password = Settings.GetString("Database/Password");
        }

        public static int GetInt(string key, params object[] args)
        {
            try
            {
                return int.Parse(Settings.Dictionary[string.Format(key, args)]);
            }
            catch
            {
                throw new SettingReadException(key);
            }
        }

        public static short GetShort(string key, params object[] args)
        {
            try
            {
                return short.Parse(Settings.Dictionary[string.Format(key, args)]);
            }
            catch
            {
                throw new SettingReadException(key);
            }
        }

        public static ushort GetUShort(string key, params object[] args)
        {
            try
            {
                return ushort.Parse(Settings.Dictionary[string.Format(key, args)]);
            }
            catch
            {
                throw new SettingReadException(key);
            }
        }

        public static byte GetByte(string key, params object[] args)
        {
            try
            {
                return byte.Parse(Settings.Dictionary[string.Format(key, args)]);
            }
            catch
            {
                throw new SettingReadException(key);
            }
        }

        public static sbyte GetSByte(string key, params object[] args)
        {
            try
            {
                return sbyte.Parse(Settings.Dictionary[string.Format(key, args)]);
            }
            catch
            {
                throw new SettingReadException(key);
            }
        }

        public static bool GetBool(string key, params object[] args)
        {
            try
            {
                return bool.Parse(Settings.Dictionary[string.Format(key, args)]);
            }
            catch
            {
                throw new SettingReadException(key);
            }
        }

        public static string GetString(string key, params object[] args)
        {
            try
            {
                return Settings.Dictionary[string.Format(key, args)];
            }
            catch
            {
                throw new SettingReadException(key);
            }
        }

        public static IPAddress GetIPAddress(string key, params object[] args)
        {
            try
            {
                if (Settings.Dictionary[string.Format(key, args)] == "localhost")
                {
                    return IPAddress.Loopback;
                }
                else
                {
                    return IPAddress.Parse(Settings.Dictionary[string.Format(key, args)]);
                }
            }
            catch
            {
                throw new SettingReadException(key);
            }
        }

        public static T GetEnum<T>(string key, params object[] args)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), Settings.Dictionary[string.Format(key, args)]);
            }
            catch
            {
                throw new SettingReadException(key);
            }
        }
    }
}
