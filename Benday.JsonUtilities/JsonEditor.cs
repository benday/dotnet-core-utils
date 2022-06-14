﻿using System;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Benday.JsonUtilities
{
    public class JsonEditor
    {
        private readonly JObject _json;
        private readonly string _pathToFile;

        public JsonEditor(string pathToFile)
        {
            if (string.IsNullOrEmpty(pathToFile))
                throw new ArgumentException($"{nameof(pathToFile)} is null or empty.", nameof(pathToFile));

            _pathToFile = pathToFile;

            AssertFileExists(_pathToFile);

            _json = LoadJsonFromFile(_pathToFile);
        }

        public JsonEditor(string json, bool loadFromString)
        {
            if (loadFromString == false)
            {
                throw new InvalidOperationException("Argument not valid on this constructor.");
            }

            if (string.IsNullOrEmpty(json))
                throw new ArgumentException($"{nameof(json)} is null or empty.", nameof(json));

            _pathToFile = null;

            _json = JObject.Parse(json);
        }

        public JsonEditor(JObject fromObject)
        {
            if (fromObject == null || fromObject.Count == 0)
            {
                throw new ArgumentException($"{nameof(fromObject)} is null or empty.", nameof(fromObject));
            }

            _json = fromObject;
        }

        public string GetValue(params string[] nodes)
        {
            if (nodes == null || nodes.Length == 0)
                throw new ArgumentException(
                $"{nameof(nodes)} is null or empty.", nameof(nodes));
            var query = GetJsonQueryForNodes(nodes);

            return GetValueUsingQuery(query.ToString());
        }

        private string GetJsonQueryForNodes(params string[] nodes)
        {
            var needsPeriod = false;

            var query = new StringBuilder();

            foreach (var node in nodes)
            {
                if (needsPeriod == true)
                {
                    query.Append(".");
                }

                query.Append(node);

                needsPeriod = true;
            }

            return query.ToString();
        }

        private void CreateNodeStructure(string[] nodes)
        {
            if (nodes == null || nodes.Length == 0)
                throw new ArgumentException($"{nameof(nodes)} is null or empty.", nameof(nodes));

            JObject parent = null;

            for (var i = 0; i < nodes.Length; i++)
            {
                var current = GetJToken(_json,
                    GetJsonQueryForNodes(nodes.Take(i + 1).ToArray()));

                if (current == null)
                {
                    if ((nodes.Length - i) > 1)
                    {
                        // node is somewhere in the middle of structure
                        var tempContainer = new JObject();
                        var temp = new JProperty(nodes[i], tempContainer);

                        if (parent == null)
                        {
                            _json.Add(temp);
                        }
                        else
                        {
                            parent.Add(temp);
                        }

                        parent = tempContainer;
                    }
                    else
                    {
                        // end of node structure
                        var temp = new JProperty(nodes[i], string.Empty);

                        if (parent == null)
                        {
                            _json.Add(temp);
                        }
                        else
                        {
                            parent.Add(temp);
                        }
                    }
                }
                else
                {
                    parent = (JObject)current;
                }
            }
        }

        public void SetValue(string nodeValue, params string[] nodes)
        {
            if (string.IsNullOrEmpty(nodeValue))
                throw new ArgumentException($"{nameof(nodeValue)} is null or empty.", nameof(nodeValue));
            if (nodes == null || nodes.Length == 0)
                throw new ArgumentException(
                $"{nameof(nodes)} is null or empty.", nameof(nodes));

            var query = GetJsonQueryForNodes(nodes);

            var match = GetJToken(_json, query);

            if (match != null)
            {
                match.Replace(new JValue(nodeValue));
            }
            else
            {
                CreateNodeStructure(nodes);
                SetValue(nodeValue, nodes);
            }

            WriteJsonFile();
        }

        private void WriteJsonFile()
        {
            if (_pathToFile != null)
            {
                File.WriteAllText(
                    _pathToFile,
                    JsonConvert.SerializeObject(_json, Formatting.Indented));
            }
        }

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(_json, Formatting.Indented);
        }

        private JToken GetJToken(JObject json, string query)
        {
            var match = json.SelectToken(query);

            return match;
        }

        private string GetValueUsingQuery(string query)
        {
            var match = GetJToken(
                _json, query);

            if (match == null)
            {
                return null;
            }
            else
            {
                return match.Value<string>();
            }
        }

        private JObject LoadJsonFromFile(string pathToFile)
        {
            AssertFileExists(pathToFile);

            var jsonText = File.ReadAllText(pathToFile);

            var json = JObject.Parse(jsonText);

            return json;
        }

        private void AssertFileExists(string pathToFile)
        {
            if (File.Exists(pathToFile) == false)
            {
                throw new FileNotFoundException("File not found.", pathToFile);
            }
        }

        public string GetSiblingValue(SiblingValueArguments args)
        {
            var parentNode = FindParentNodeBySiblingValue(args);

            if (parentNode == null)
            {
                return null;
            }
            else
            {
                var match = parentNode[args.DesiredNodeKey];

                if (match == null)
                {
                    return null;
                }
                else
                {
                    return match.Value<string>();
                }
            }
        }

        public void SetSiblingValue(SiblingValueArguments args)
        {
            var parentNode = FindParentNodeBySiblingValue(args);

            if (parentNode == null)
            {
                return;
            }
            else
            {
                parentNode[args.DesiredNodeKey] = args.DesiredNodeValue;
            }
        }

        public JToken GetNode(params string[] nodes)
        {
            if (nodes == null || nodes.Length == 0)
                throw new ArgumentException(
                $"{nameof(nodes)} is null or empty.", nameof(nodes));

            var query = GetJsonQueryForNodes(nodes);

            return GetJToken(_json, query);
        }

        public JToken GetNodeByQuery(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentException($"{nameof(query)} is null or empty.", nameof(query));
            }

            return GetJToken(_json, query);
        }

        private JToken FindParentNodeBySiblingValue(SiblingValueArguments args)
        {
            var collectionMatch = GetJToken(
                _json, GetJsonQueryForNodes(args.PathArguments));

            if (collectionMatch == null)
            {
                return null;
            }

            var matches = collectionMatch.Children().ToList();

            if (matches == null || matches.Count == 0)
            {
                return null;
            }
            else
            {
                foreach (var item in matches)
                {
                    if (item.HasValues == true)
                    {
                        if (item[args.SiblingSearchKey] != null &&
                            item[args.SiblingSearchKey].Value<string>() == args.SiblingSearchValue)
                        {
                            return item;
                        }
                    }
                }

                return null;
            }
        }
    }
}
