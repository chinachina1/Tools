using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace readlogfile
{
    class Program
    {
        static List<string> output = new List<string>();
        static void Main(string[] args)
        {
            string filepath = args[0];
            string[] lines = File.ReadAllLines(filepath);
            Enter(lines, 0);
            File.WriteAllLines("1.txt", output.ToArray());
            //Console.WriteLine("Hello World!");
        }
        static string[] matchs = { @"WARNING", @"^((?!load avatar failed).)*$", @"^((?!ThinkingSDK).)*$", @"^((?!handler).)*$", @"^((?!Read/Write).)*$", @"^((?!BoxColliders).)*$", @"^((?!opentalk).)*$" };

        static bool IsMatch(string line)
        {
            bool ma = true;
            foreach (var match in matchs)
            {
                ma = ma && Regex.IsMatch(line, match);
            }
            return ma;
        }

        static void Enter(string[] lines, int idx)
        {
            if (idx >= lines.Length)
                return;
            string line = lines[idx];
            if (string.IsNullOrEmpty(line))
            {
                EnterEmpty(lines, idx);
            }
            else if (IsMatch(line))
            {
                EnterHandle(lines, idx);
            }
            else
            {
                EnterNoHandle(lines, idx);
            }
        }

        static void EnterEmpty(string[] lines, int idx)
        {
            Enter(lines, idx + 1);
        }

        static void EnterNoHandle(string[] lines, int idx)
        {
            for (int i = idx + 1; i < lines.Length; i++)
            {
                string line = lines[i];
                if (string.IsNullOrEmpty(line))
                {
                    Enter(lines, i);
                    return;
                }
            }
            Enter(lines, lines.Length + 1);
        }

        static void EnterHandle(string[] lines, int idx)
        {
            List<string> res = new List<string>();
            for (int i = idx; i < lines.Length; i++)
            {
                string line = lines[i];
                if (string.IsNullOrEmpty(line))
                {
                    if (IsLinesMatch(res))
                    {
                        output.Add("\r\n");
                        output.AddRange(res);
                    }
                    Enter(lines, i);
                    return;
                }
                else
                {
                    res.Add(line);
                }
            }
            if (IsLinesMatch(res))
            {
                output.Add("\r\n");
                output.AddRange(res);
            }
            Enter(lines, lines.Length + 1);
        }

        static string[] linematchs = { @"Fish" };

        static bool IsLinesMatch(List<string> lines)
        {
            foreach(string line in lines)
            {
                if (IsLineMatch(line))
                    return false;
            }
            return true;
        }
        static bool IsLineMatch(string line)
        {
            foreach (var match in linematchs)
            {
                if (Regex.IsMatch(line, match))
                    return true;
            }
            return false;
        }
    }
}