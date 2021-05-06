using Krypton.CompilationData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;

namespace Krypton.Analysis
{
    internal sealed class DiagnosticsMessageProvider
    {
        public DiagnosticsMessageProvider()
        {
            messages = new Lazy<Dictionary<DiagnosticsCode, string>>(InitLazy);

            static Dictionary<DiagnosticsCode, string> InitLazy()
            {
                byte[] bytes = Properties.Resources.DiagnosticsMessages;
                string json = BitConverter.ToString(bytes);

                var dictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

                Debug.Assert(dictionary != null);

                return dictionary.ToDictionary(kvp => ParseDiagnosticsCode(kvp.Key), kvp => kvp.Value);

                static DiagnosticsCode ParseDiagnosticsCode(string name)
                {
                    if (Enum.TryParse(name, out DiagnosticsCode diagnosticsCode))
                    {
                        return diagnosticsCode;
                    }
                    else
                    {
                        return default;
                    }
                }
            }
        }

        private readonly Lazy<Dictionary<DiagnosticsCode, string>> messages;

        public string this[DiagnosticsCode key]
            => messages.Value[key];

        public bool ContainsMessage(DiagnosticsCode key)
            => messages.Value.ContainsKey(key);

        public bool TryGetValue(DiagnosticsCode key, [MaybeNullWhen(false)] out string value)
            => messages.Value.TryGetValue(key, out value);
    }
}
