/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WD_toolbox.Data.DataStructures;
using WD_toolbox;

namespace WD_toolbox.Files
{
    public abstract class WDSettingsFile<T>
    {
        //---------------------------------------------------------------------------------------------------------------
        // Instance Data
        //---------------------------------------------------------------------------------------------------------------
        public List<Tuple<string, string>> Defs;

        public abstract Why parseLine(string line);
        public abstract Why parsePragma(string pragma, string args);
        public abstract Why parseEndOfFile();

        /// <summary> Operator method </summary>
        /// <returns>null on error, "" or other string on sucsess. </returns>
        public delegate string OperatorDelegate(string LHS, string RHS);

        public int FileHash { protected set; get; }
        public int precomplieTag { protected set; get; }

        public class OperatorType
        {
            public string Operator;
            public OperatorDelegate Delegate;
            public bool AllowFloatingPoint;

            public OperatorType(string _operator,
                                OperatorDelegate _delegate,
                                bool _allowFloatingPoint)
            {
                this.Operator = _operator;
                this.Delegate = _delegate;
                this.AllowFloatingPoint = _allowFloatingPoint;
            }
        }
        public List<OperatorType> Operators;

        public T Value { get; protected set; }

        //---------------------------------------------------------------------------------------------------------------
        // Constructors and initilisation
        //---------------------------------------------------------------------------------------------------------------
        public WDSettingsFile()
        {
            Defs = new List<Tuple<string, string>>();
            Operators = new List<OperatorType>();
            Operators.Add(new OperatorType( "...", ListOperator, false));
            precomplieTag = 11111111;
        }

        //---------------------------------------------------------------------------------------------------------------
        // Parsing
        //---------------------------------------------------------------------------------------------------------------
        public Why paseFile(string path)
        {
            precomplieTag = Math.Abs(Path.GetFileName(path).GetHashCode());
            Why r = parseText(File.ReadAllText(path));
            if (!r)
            {
                return Why.FalseBecause("Error loading {0}: {1}", Path.GetFileName(path), r.Reason);
            }
            return r;
        }

        public Why parseText(string text)
        {
            FileHash = Math.Abs(text.GetHashCode());
            string[] lines = text.GetLines(StringExtension.PruneOptions.NoPrune).ToArray();

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                if (isComment(line) || isBlank(line))
                {
                    //skip line
                    continue;
                }
                else if (isDef(line))
                {
                    Why r = parseDef(line);
                    if (!r)
                    {
                        return Why.FalseBecause("#def at line {0} was not parsable ({2}): {1}", i + 1, line, r.Reason);
                    }
                }
                else  if (isPragma(line))
                {
                    Why r = parsePragma(line);
                    if (!r)
                    {
                        return Why.FalseBecause("#pragma at line {0} was not parsable ({2}): {1}", i + 1, line, r.Reason);
                    }
                }
                else
                {
                    line = preProcess(line);
                    Why r = parseLine(line);
                    if (!r)
                    {
                        return Why.FalseBecause("line {0} was not parsable ({2}): {1}", i + 1, line, r.Reason);
                    }
                }

            }
          
            //signal end of file
            return parseEndOfFile();
        }

        private Why parsePragma(string line)
        {
            string[] tokens = line.Trim().Trim("#".ToCharArray()).Split(" \t".ToCharArray(), 2);
            if (tokens.Length == 2)
            {
                string pragma = tokens[0].Trim();
                string details = tokens[1].Trim();
                details = preProcess(details);
                return parsePragma(pragma, details);
            }
            else if (tokens.Length == 1)
            {
                string pragma = line.Trim();
                return parsePragma(pragma, null);
            }

            return false;
        }

        private bool parseDef(string line)
        {
            string[] parts = line.Split("=".ToCharArray(), 2);
            if (parts.Length == 2)
            {
                string def = parts[0];
                string insertText = parts[1];
                if (def.Length > 5)
                {
                    def = parts[0].Substring(5).Trim();
                    insertText = insertText.Trim();
                    insertText = preProcess(insertText);
                    Defs.Add(new Tuple<string, string>(def, insertText));
                    return true;
                }
                else
                {
                    return Why.FalseBecause("mising def before =");
                }
            }
            return Why.FalseBecause("expeting #def<name> = <something>");
        }

        private bool isPragma(string line)
        {
            return line.TrimStart().StartsWith("#");
        }

        private bool isDef(string line)
        {
            return line.TrimStart().StartsWith("#def");
        }

        private bool isBlank(string line)
        {
            return string.IsNullOrWhiteSpace(line);
        }

        private bool isComment(string line)
        {
            return line.TrimStart().StartsWith(@"%");
        }

        protected virtual string preProcess(string line)
        {
            if(line == null)
            {
                return null;
            }

            foreach(var def in Defs)
            {
                line = line.Replace(def.Item1, def.Item2);
            }

            line = applyOperators(line);
            return line;
        }


        //---------------------------------------------------------------------------------------------------------------
        // Operators
        //---------------------------------------------------------------------------------------------------------------
        private string applyOperators(string line)
        {//n  <1...3>  <1*FRAME_RATE>
            string newLine = string.Copy(line);
            foreach (OperatorType myOperator in Operators)
            {
                string LHS, RHS;
                int pos;
                newLine = removeOpertor(line, myOperator, out LHS, out RHS, out pos);
                while (pos >= 0)
                {
                    //apply operator
                    string res = myOperator.Delegate(LHS, RHS);
                    if (res == null)
                    {
                        return line;  //crash out
                    }
                    newLine = newLine.Insert(pos, res);

                    //remove next operator
                    newLine = removeOpertor(line, myOperator, out LHS, out RHS, out pos);
                }
            }
            return newLine;
        }

        private string removeOpertor(string line, OperatorType myOperator, out string LHS, out string RHS, out int pos)
        {
            int tokenPos = line.IndexOf(myOperator.Operator);
            int maxPos = line.Length - myOperator.Operator.Length - 1;

            if ((tokenPos > 0) && (tokenPos <= maxPos)) // > 0, because a liune can not start with an operator 
            {
                string allLeft = line.Substring(0, tokenPos);
                string allRight = line.Substring(tokenPos + myOperator.Operator.Length - 1);

                //remove numerics
                string newLeft = allLeft.TrimEnd(); //remove space after number, but before operator
                newLeft = newLeft.TrimEnd("123456789".ToCharArray()); //remove numbers
                newLeft = newLeft.TrimEnd(".".ToCharArray()); //remove .
                if (myOperator.AllowFloatingPoint)
                {
                    newLeft = newLeft.TrimEnd("123456789".ToCharArray()); //remove numbers
                    newLeft = newLeft.TrimEnd("-".ToCharArray()); //remove negative
                }

                string newRight = allRight.TrimStart(); //remove space after operator, but before number
                newRight = newRight.TrimStart("-".ToCharArray()); //remove negative
                newRight = newRight.TrimStart("123456789".ToCharArray()); //remove numbers
                if (myOperator.AllowFloatingPoint)
                {
                    newLeft = newLeft.TrimEnd(".".ToCharArray()); //remove .
                    newRight = newRight.TrimStart("123456789".ToCharArray()); //remove numbers
                }
                
                //were numbers removed
                int rhsLen = allRight.Length - newRight.Length;
                int lhsLen = allLeft.Length - newLeft.Length;
                if ((lhsLen > 0) && (rhsLen > 0))
                {
                    pos = newLeft.Length;
                    LHS = allLeft.Substring(allLeft.Length - lhsLen);
                    RHS = allRight.Substring(rhsLen);

                    return newLeft + newRight;
                }
            }

            //error
            LHS = RHS = null;
            pos = -1;
            return line;
        }

        private static string ListOperator(string LHS, string RHS)
        {
            try
            {
                int start = int.Parse(LHS);
                int end = int.Parse(RHS);
                int dir = (start < end) ? 1 : -1;

                List<int> nums = new List<int>();
                for (int i = start; i <= end; i += dir)
                {
                    nums.Add(i);
                }

                return string.Join(", ", nums); 
            }
            catch
            {
                return null;
            }
        }

        //---------------------------------------------------------------------------------------------------------------
        // Argument list helper methods
        //---------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// parses arguments
        /// </summary>
        /// <param name="args"></param>
        /// <returns>Empty list on no args,  null on error, otherwise a parsed list. </returns>
        protected static List<KeyValuePair<string, string>> parseArgs(string args)
        {
            string[] sections = (args??"").Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (sections.Length == 1)
            {
                return new List<KeyValuePair<string, string>>();
            }

            var tokens = from option in sections select option.Split("=".ToCharArray());
            int badTokens = tokens.Count(T => T.Length != 2);

            if (badTokens != 0)
            {
                return null;
            }

            List<KeyValuePair<string, string>> options =
                          (from option in tokens
                           select new KeyValuePair<string, string>((string)option[0], (string)option[1])).ToList();
            return options;
        }

        protected static string getOption(List<KeyValuePair<string, string>> options, string key)
        {
            return options.FirstOrDefault(P => P.Key.EqualsIgnoreCaseTrimmed(key)).Value;
        }

        protected int getNameDateHash(string path)
        {
            unchecked
            {
                int timeHash = 1;
                if (File.Exists(path))
                {
                    try
                    {
                        //UTC time, otherwise the hash is timezone dependent, 
                        //and we dont want to reload, just because we traveled.
                        timeHash = (int)((File.GetLastWriteTimeUtc(path).Ticks) % int.MaxValue);
                        timeHash = (timeHash == 0) ? 1 : timeHash;
                    }
                    catch
                    {
                    }
                }
                return Math.Abs(Path.GetFileName(path).GetHashCode() * timeHash);
            }
        }
    }
}
