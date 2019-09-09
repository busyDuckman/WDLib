/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WD_toolbox.Files
{

    /// <summary>
    /// A colection of wildcard based text matching algorithems.
    /// Most of these suck, use 
    ///     bool isMatch(string s, string pattern, bool caseSensative)
    /// to call the best version (least suckiest).
    /// </summary>
    public sealed class Wildcards
    {
        //--------------------------------------------------------------------------------------------
        // Things that go with Pattern Matching
        //--------------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="files"></param>
        /// <param name="pattern">Can used ; to seperate multiple aptterns.</param>
        /// <param name="caseSensative"></param>
        /// <returns></returns>
        public static string[] reduceFileList(string[] files, string pattern, bool caseSensative)
        {
            string[] patterns = pattern.Split(";".ToArray());

            List<string> unmatchedFiles = new List<string>(files);
            List<string> res = new List<string>();
            foreach (string patternToken in patterns)
            {
                List<string> matches = new List<string>();
                foreach (string file in unmatchedFiles)
                {
                    if (isMatch(file, patternToken, caseSensative))
                    {
                        matches.Add(file);
                    }
                }

                //update results
                unmatchedFiles.RemoveAll(S => matches.Contains(S));
                res.AddRange(matches);
            }

            return res.ToArray();
        }



        //--------------------------------------------------------------------------------------------
        // Pattern Matching
        //--------------------------------------------------------------------------------------------
        //                 pattern            string             CaSe  result
        private static Dictionary<string, Dictionary<string, Dictionary<bool, bool>>> resultsDictionary = new Dictionary<string, Dictionary<string, Dictionary<bool, bool>>>();

        public static bool isMatch(string s, string pattern, bool caseSensative)
        {
            //This was such a bottle nect a dictionary of previous results must be maintained   
            bool res;
            if (getPreviousResult(s, pattern, caseSensative, out res))
            {
                return res;
            }

            //Dr Dobbs, fast and fuck knows if its any good.
            res = GeneralTextCompare(s, pattern, caseSensative);

            addResult(s, pattern, caseSensative, res);

            return res;


            //return fastMatch(s, pattern, caseSensative);

            //berkly unix code, fast but not acurate
            /*if (caseSensative)
                return fastMatch(s, pattern);
            else
                return fastMatch(s.ToLower(), pattern.ToLower());*/

            //my code, acurate but too slow for this task
            /*if(caseSensative)
                return isMatch(s, pattern, caseSensative, 0, 0);
            else
                return isMatch(s.ToLower(), pattern.ToLower(), caseSensative, 0, 0);*/
        }

        private static void addResult(string s, string pattern, bool caseSensative, bool res)
        {
            if (!resultsDictionary.ContainsKey(pattern))
            {
                resultsDictionary.Add(pattern, new Dictionary<string, Dictionary<bool, bool>>());
            }

            if (!resultsDictionary[pattern].ContainsKey(s))
            {
                resultsDictionary[pattern].Add(s, new Dictionary<bool, bool>());
            }
            else if (resultsDictionary[pattern].Count > 1000)
            {
                //stop memory growing too large (might want to do some LRU stuff if performance problems emerge)
                resultsDictionary[pattern].Clear();
            }

            if (!resultsDictionary[pattern][s].ContainsKey(caseSensative))
            {
                resultsDictionary[pattern][s].Add(caseSensative, res);
            }
            else
            {
                throw new InvalidOperationException("resultsDictionary already has a value for that search");
            }
        }

        /// <summary>
        /// Return true if a pervious resultExists, result paramater will hold the actual resuult
        /// </summary>
        private static bool getPreviousResult(string s, string pattern, bool caseSensative, out bool result)
        {
            result = false;
            if (resultsDictionary.ContainsKey(pattern))
            {
                var pDict = resultsDictionary[pattern];
                if (pDict.ContainsKey(s))
                {
                    var cDict = pDict[s];
                    if (cDict.ContainsKey(caseSensative))
                    {
                        result = cDict[caseSensative];
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool isMatch(string s, string pattern, bool caseSensative, int patternPos, int stringPos)
        {
            int p = patternPos;
            for (int i = stringPos; i < s.Length; i++)
            {
                if (p >= pattern.Length)
                {
                    return false;
                }
                switch (pattern[p])
                {
                    case '*':
                        for (int n = (i + 1); n < s.Length; n++)
                        {
                            if (isMatch(s, pattern, caseSensative, p, n))
                            {
                                return true;
                            }
                        }
                        p++;
                        break;
                    case '?':
                        p++;
                        break;
                    default:
                        //if(!s[i].ToString().Equals(pattern[p].ToString(), caseSensative ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase))
                        if (s[i] != pattern[p])
                        {
                            return false;
                        }
                        p++;
                        break;
                }

            }
            return (p == pattern.Length);
        }

        /// <summary>
        /// code from berkly unix
        /// http://www.math.utah.edu/pub/misc/mtools-2.0.7.tar.bz2/ 
        /// ported to c#
        /// 
        /// I COULD NOT FIND A LICENSE
        /// Do shell-style pattern matching for '?', '\', '[..]', and '*' wildcards
        /// </summary>
        /// <param name="str">string to match</param>
        /// <param name="pattern">pattern with '?', '\', '[..]', and '*' wildcards</param>
        /// <returns>True if there is a match, otherwise false.</returns>
        private static bool fastMatch(string str, string pattern)
        {
            //add padding for null termination
            str = str ?? "";
            str += "`";
            pattern = pattern ?? "";
            pattern += "`";

            char[] _str = str.ToCharArray();
            char[] _pattern = pattern.ToCharArray();

            //null terminate
            _str[_str.Length - 1] = '\0';
            _pattern[_pattern.Length - 1] = '\0';



            unsafe
            {

                fixed (char* sFix = &_str[0])
                {
                    char* s = sFix;
                    fixed (char* pFix = &_pattern[0])
                    {
                        char* p = pFix;

                        bool matched, reverse;
                        char first, last;

                        for (; *p != '\0'; s++, p++)
                        {
                            switch (*p)
                            {
                                case '?':       /* match any one character */
                                    if (*s == '\0')
                                        return (false);
                                    break;
                                case '*':       /* match everything */
                                    while (*p == '*')
                                        p++;

                                    /* if last char in pattern */
                                    if (*p == '\0')
                                        return (true);

                                    /* search for next char in pattern */
                                    matched = false;
                                    while (*s != '\0')
                                    {
                                        if (*s == *p)
                                        {
                                            matched = true;
                                            break;
                                        }
                                        s++;
                                    }
                                    if (!matched)
                                        return (false);
                                    break;
                                case '[':        /* match range of characters */
                                    first = '\0';
                                    matched = false;
                                    reverse = false;
                                    while (*++p != ']')
                                    {
                                        if (*p == '^')
                                        {
                                            reverse = true;
                                            p++;
                                        }
                                        first = *p;
                                        if (first == ']' || first == '\0')
                                            return (true);

                                        /* if 2nd char is '-' */
                                        if (*(p + 1) == '-')
                                        {
                                            p++;
                                            /* set last to 3rd char ... */
                                            last = *++p;
                                            if (last == ']' || last == '\0')
                                                return (false);
                                            /* test the range of values */
                                            if (*s >= first && *s <= last)
                                            {
                                                matched = true;
                                                p++;
                                                break;
                                            }
                                            return (false);
                                        }
                                        if (*s == *p)
                                            matched = true;
                                    }
                                    if (matched && reverse)
                                        return (false);
                                    if (!matched)
                                        return (false);
                                    break;
                                case '\\':      /* Literal match with next character */
                                    p++;
                                    /* fall thru */
                                    goto default;
                                default:
                                    if (*s != *p)
                                        return (false);
                                    break;
                            }
                        }
                        /* string ended prematurely ? */
                        if (*s != '\0')
                            return (false);
                        else
                            return (true);

                    }
                }
            }

            /*for (int i = 0; i < pattern.Length; i++)
            {
                if (i >= s.Length)
                    return false;
                if (s[i] != pattern[i])
                {
                    return false;
                }
            }
            return true;*/
        }


        /// <summary>
        /// From a Dr Dobbs Article.
        /// The author was as pissed of with the state of these algoritems as I was.
        /// I might assume, for now, that means his is not a peice of shit.
        /// 
        /// Original article.
        /// http://www.ddj.com/database/210200888;jsessionid=OC2SVBB331HTFQE1GHOSKH4ATMY32JVN?pgno=2
        /// </summary>
        /// <param name="TameText">A string without wildcards</param>
        /// <param name="pWildText">A (potentially) corresponding string with wildcards</param>
        /// <param name="bCaseSensitive">match on 'X' vs 'x'</param>
        /// <returns></returns>
        private static bool GeneralTextCompare(string TameText, string WildText, bool CaseSensitive)
        {
            //add padding for null termination
            TameText = TameText ?? "";
            TameText += "`";
            WildText = WildText ?? "";
            WildText += "`";

            char[] _str = TameText.ToCharArray();
            char[] _pattern = WildText.ToCharArray();

            //null terminate
            _str[_str.Length - 1] = '\0';
            _pattern[_pattern.Length - 1] = '\0';


            bool foobar;
            unsafe
            {
                fixed (char* sFix = &_str[0])
                {
                    char* s = sFix;
                    fixed (char* pFix = &_pattern[0])
                    {
                        char* p = pFix;
                        foobar = GeneralTextCompare(s, p, CaseSensitive);
                    }
                }
            }

            return foobar;
        }

        //This function compares text strings, one of which can have wildcards ('*').
        private unsafe static bool GeneralTextCompare(
                                                    char* pTameText,    // A string without wildcards
                                                    char* pWildText,    // A (potentially) corresponding string with wildcards
                                                    bool bCaseSensitive  // By default, match on 'X' vs 'x'
                                                    )
        {
            char cAltTerminator = '\0';   // For function names, for example, you can stop at the first '('
            bool bMatch = true;
            char* pAfterLastWild = null; // The location after the last '*', if we've encountered one
            char* pAfterLastTame = null; // The location in the tame string, from which we started after last wildcard
            char t, w;

            // Walk the text strings one character at a time. 
            while (true)
            {
                t = *pTameText; w = *pWildText;

                // How do you match a unique text string? 
                if ((t == '\0') || t == cAltTerminator)
                { // Easy: unique up on it!
                    if ((w == '\0') || w == cAltTerminator)
                    {
                        break; // "x" matches "x" 
                    }
                    else if (w == '*')
                    {
                        pWildText++; continue; // "x*" matches "x" or "xy" 
                    }
                    else if (pAfterLastTame != null)
                    {
                        if ((*pAfterLastTame == '\0') || *pAfterLastTame == cAltTerminator)
                        {
                            bMatch = false; break;
                        } pTameText = pAfterLastTame++; pWildText = pAfterLastWild; continue;
                    }

                    bMatch = false; break; // "x" doesn't match "xy" 
                }
                else
                {
                    if (!bCaseSensitive)
                    { // Lowercase the characters to be compared. 
                        if (t >= 'A' && t <= 'Z')
                        {
                            t = char.ToLower(t);
                        }

                        if (w >= 'A' && w <= 'Z')
                        {
                            w = char.ToLower(w);
                        }
                    }

                    // How do you match a tame text string? 
                    if (t != w)
                    { // The tame way: unique up on it!
                        if (w == '*')
                        {
                            pAfterLastWild = ++pWildText; pAfterLastTame = pTameText; w = *pWildText;

                            if ((w == '\0') || w == cAltTerminator)
                            {
                                break; // "*" matches "x" 
                            } continue; // "*y" matches "xy" 
                        }
                        else if (pAfterLastWild != null)
                        {
                            if (pAfterLastWild != pWildText)
                            {
                                pWildText = pAfterLastWild; w = *pWildText;
                                if (!bCaseSensitive && w >= 'A' && w <= 'Z')
                                {
                                    w = char.ToLower(w);
                                }

                                if (t == w)
                                {
                                    pWildText++;
                                }
                            } pTameText++; continue; // "*sip*" matches "mississippi" 
                        }
                        else
                        {
                            bMatch = false; break; // "x" doesn't match "y" 
                        }
                    }
                }

                pTameText++; pWildText++;
            }
            return bMatch;
        }
    }


}
