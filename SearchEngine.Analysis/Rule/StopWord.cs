using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;
using SearchEngine.Analysis.Configuration;

namespace ThesaurusAnalysis.Rule
{
    /// <summary>
    /// Õ£”√¥ πÊ‘Ú
    /// </summary>
    public class StopWord
    {
        private static Hashtable m_StopWord = new Hashtable();
        private static bool ready = false;
        
        /// <summary>
        /// Õ£”√¥ ºÏ≤‚
        /// </summary>
        /// <param name="word">¥˝ºÏ≤‚¥ ”Ô</param>
        /// <returns></returns>
        public static bool Test(string word)
        {
            if (!ready)
            {
                StreamReader reader = new StreamReader(Path.Combine(Directorys.DictsDirectory, @"stopword.txt"), Encoding.UTF8);
                String stopWord = null;
                while ((stopWord = reader.ReadLine()) != null)
                {
                    if (!m_StopWord.ContainsKey(stopWord))
                        m_StopWord.Add(stopWord, null);
                }
                reader.Close();
                ready = true;
            }

            return m_StopWord.ContainsKey(word);
        }
    }
}
