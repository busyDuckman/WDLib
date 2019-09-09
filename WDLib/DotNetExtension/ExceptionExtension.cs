/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using WD_toolbox;

namespace WD_toolbox//.AplicationFramework
{
    public static class ExceptionExtension
    {
        public static string GetADecentExplination(this Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            Exception current = ex;
            int traceDepth = 100;
            string indent = "";

            List<string> lines = (ex.StackTrace??"").GetLines(StringExtension.PruneOptions.EmptyOrWhiteSpaceLines, true);
            sb.Append(ex.Message.Trim());
            if (lines.Count > 0)
            {
                sb.AppendLine(" [call to " + getMethodName(lines[0]) + "]");
                lines.RemoveAt(0);
                foreach (string line in lines)
                {
                    string[]tokens = line.Split(new string[] {" in ", ":line"}, 3, StringSplitOptions.None);
                    if (tokens.Length == 3)
                    {
                        string method = getMethodName(tokens[0]);
                        string file = Path.GetFileName(tokens[1].Trim());
                        int lineNumber = tokens[2].ParseAllIntegers().Last();
                        sb.AppendLine(string.Format(": {0} in {1} line {2}", method, file, lineNumber));
                    }
                    else
                    {
                        sb.AppendLine(string.Format(": {0} in {1} line {2}", "?", "?", "?"));
                    }
                }
            }

            return sb.ToString();
        }

        private static string getMethodName(string s)
        {
            string end = s.Trim().Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
            if (end != null)
            {
                string name =  s.TextAfterLast(".").TextBeforeFirst("(");
                if(!string.IsNullOrWhiteSpace(name))
                {
                    int commas = s.IndexOfAll(',').Length;
                    return name + "(" + (new string(',', commas)) + ")";
                }
            }
            return "(n/a)";
        }

        /*
        //technacly much less hacky, but does not work due to exception class being a POS
        public static string GetADecentExplination(this Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            Exception current = ex;
            int traceDepth = 100;
            string indent = "";
            while((current != null) && (traceDepth > 0))
            {
                string desc = current.GetType().Name + ": " +  current.Message;

                sb.Append(indent + desc);
                if(current.Source != null)
                {
                    sb.Append("(" + current.Source + ")");
                }

                if(current.TargetSite != null)
                {
                    string file = Path.GetFileName(current.TargetSite.Module.FullyQualifiedName??"");

                    string desc2 = current.ToString();
                    int lineNum = desc2.ParseAllIntegers().Last();
                   // var res = current.FindValues(36);

                    sb.Append(String.Format(": {0} in {1} line {2}", current.TargetSite.Name, file, lineNum));
                }

                

                if (current.InnerException == null)
                {
                    //sb.AppendLine(current.StackTrace ?? " NO STACK TRACE");
                }

                indent = "  " + indent;
                sb.AppendLine();
                current = current.InnerException;
                traceDepth--;
                if (traceDepth <= 0)
                {
                    sb.AppendLine("TOO MANY INNER EXCEPTION LEVELS. STOPPING TRACE!");
                }
            }

            return sb.ToString();
        }
        */
    }
}
