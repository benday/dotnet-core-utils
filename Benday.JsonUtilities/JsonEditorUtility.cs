﻿using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Benday.JsonUtilities
{
    public static class JsonEditorUtility
    {
        public static string GetValue(string pathToFile, string node1)
        {
            if (string.IsNullOrEmpty(pathToFile))
                throw new ArgumentException($"{nameof(pathToFile)} is null or empty.", nameof(pathToFile));
            if (string.IsNullOrEmpty(node1))
                throw new ArgumentException($"{nameof(node1)} is null or empty.", nameof(node1));

            var query = string.Format("{0}", node1);

            return GetValueUsingQuery(pathToFile, query);
        }

        public static string GetValue(string pathToFile, string node1, string node2)
        {
            if (string.IsNullOrEmpty(pathToFile))
                throw new ArgumentException($"{nameof(pathToFile)} is null or empty.", nameof(pathToFile));
            if (string.IsNullOrEmpty(node1))
                throw new ArgumentException($"{nameof(node1)} is null or empty.", nameof(node1));
            if (string.IsNullOrEmpty(node2))
                throw new ArgumentException($"{nameof(node2)} is null or empty.", nameof(node2));

            var query = string.Format("{0}.{1}", node1, node2);

            return GetValueUsingQuery(pathToFile, query);
        }

        public static string GetValue(string pathToFile,
            string node1, string node2, string node3)
        {
            if (string.IsNullOrEmpty(pathToFile))
                throw new ArgumentException($"{nameof(pathToFile)} is null or empty.", nameof(pathToFile));
            if (string.IsNullOrEmpty(node1))
                throw new ArgumentException($"{nameof(node1)} is null or empty.", nameof(node1));
            if (string.IsNullOrEmpty(node2))
                throw new ArgumentException($"{nameof(node2)} is null or empty.", nameof(node2));
            if (string.IsNullOrEmpty(node3))
                throw new ArgumentException($"{nameof(node3)} is null or empty.", nameof(node3));
            var query = string.Format("{0}.{1}.{2}", node1, node2, node3);

            return GetValueUsingQuery(pathToFile, query);
        }

        public static void SetValue(
            string pathToFile,
            string node1, string value)
        {
            if (string.IsNullOrEmpty(pathToFile))
                throw new ArgumentException($"{nameof(pathToFile)} is null or empty.", nameof(pathToFile));
            if (string.IsNullOrEmpty(node1))
                throw new ArgumentException($"{nameof(node1)} is null or empty.", nameof(node1));
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException($"{nameof(value)} is null or empty.", nameof(value));

            SetValueUsingQuery(pathToFile, node1, value);
        }

        public static void SetValue(
            string pathToFile,
            string node1, string node2, string value)
        {
            if (string.IsNullOrEmpty(pathToFile))
                throw new ArgumentException($"{nameof(pathToFile)} is null or empty.", nameof(pathToFile));
            if (string.IsNullOrEmpty(node1))
                throw new ArgumentException($"{nameof(node1)} is null or empty.", nameof(node1));
            if (string.IsNullOrEmpty(node2))
                throw new ArgumentException($"{nameof(node2)} is null or empty.", nameof(node2));
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException($"{nameof(value)} is null or empty.", nameof(value));

            var query = string.Format("{0}.{1}", node1, node2);

            SetValueUsingQuery(pathToFile, query, value);
        }

        public static void SetValue(
            string pathToFile,
            string node1, string node2, string node3, string value)
        {
            if (string.IsNullOrEmpty(pathToFile))
                throw new ArgumentException($"{nameof(pathToFile)} is null or empty.", nameof(pathToFile));
            if (string.IsNullOrEmpty(node1))
                throw new ArgumentException($"{nameof(node1)} is null or empty.", nameof(node1));
            if (string.IsNullOrEmpty(node2))
                throw new ArgumentException($"{nameof(node2)} is null or empty.", nameof(node2));
            if (string.IsNullOrEmpty(node3))
                throw new ArgumentException($"{nameof(node3)} is null or empty.", nameof(node3));
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException($"{nameof(value)} is null or empty.", nameof(value));

            var query = string.Format("{0}.{1}.{2}",
                node1, node2, node3);

            SetValueUsingQuery(pathToFile, query, value);
        }

        private static void WriteJsonFile(string pathToFile, JObject json)
        {
            File.WriteAllText(
                pathToFile,
                JsonConvert.SerializeObject(json));
        }

        private static void SetValueUsingQuery(
            string pathToFile,
            string query, string value)
        {
            AssertFileExists(pathToFile);

            var json = LoadJsonFile(pathToFile);

            var match = GetJToken(json, query);

            if (match != null)
            {
                match.Replace(new JValue(value));
            }
            else
            {
                throw new InvalidOperationException("No matching node.");
            }


            WriteJsonFile(pathToFile, json);
        }

        private static string GetValueUsingQuery(string pathToFile, string query)
        {
            var tempJObject = LoadJsonFile(pathToFile);

            var match = GetJToken(
                tempJObject,
                query);

            if (match == null)
            {
                return null;
            }
            else
            {
                return match.Value<string>();
            }
        }

        public static JToken GetJToken(JObject json, string query)
        {
            var match = json.SelectToken(query);

            return match;
        }

        public static JObject LoadJsonFile(string pathToFile)
        {
            AssertFileExists(pathToFile);

            var jsonText = File.ReadAllText(pathToFile);

            var json = JObject.Parse(jsonText);

            return json;
        }

        private static void AssertFileExists(string pathToFile)
        {
            if (File.Exists(pathToFile) == false)
            {
                throw new FileNotFoundException("File not found.", pathToFile);
            }
        }

        public static string BeautifyJson(string input)
        {
            dynamic parsedJson = JsonConvert.DeserializeObject(input);
            return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
        }
    }
}
