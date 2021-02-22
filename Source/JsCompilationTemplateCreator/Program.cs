using DouglasCrockford.JsMin;
using System;
using System.IO;

if (args.Length <= 0 || !File.Exists(args[0]))
{
    Console.WriteLine("Error: no file specified");
    Console.ReadKey();
    return;
}

string jsCode = File.ReadAllText(args[0]);
string processedCode = ProcessJsCode();
File.WriteAllText("template.js", processedCode);

Console.WriteLine("Saved template to template.js");
Console.ReadKey();

string ProcessJsCode()
{
    int cutHereIndex = FindCutHere();

    int FindCutHere()
    {
        const string CutHere = "CUT HERE";

        for (int i = 0; i < jsCode.Length - CutHere.Length - 1; i++)
        {
            if (jsCode[i..].StartsWith(CutHere))
            {
                return i;
            }
        }

        return -1;
    }

    string cutCode = jsCode[..cutHereIndex];

    JsMinifier minifier = new();
    string minifiedCode = minifier.Minify(cutCode);

    minifiedCode = minifiedCode.Replace("\n", "");

    return minifiedCode;
}