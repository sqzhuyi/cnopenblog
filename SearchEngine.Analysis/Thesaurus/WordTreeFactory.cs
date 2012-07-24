using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SearchEngine.Analysis.Configuration;

namespace SearchEngine.Analysis.Thesaurus
{
	public class WordTreeFactory
	{
		private static WordTree instance = null;


		public static WordTree GetInstance()
		{
			if (instance == null)
			{
				BuildWordTree();
			}
			return instance;
		}

        public static void BuildWordTree()
        {
            instance = WordTree.CreateWordTree();
            String[] dicts = Directory.GetFiles(Directorys.DictsDirectory, "*.dic");
            if (dicts != null)
            {
                for (int i = 0; i < dicts.Length; i++)
                {
                    StreamReader reader = File.OpenText(dicts[i]);
                    try
                    {
                        String line = null;
                        while ((line = reader.ReadLine()) != null)
                        {
                            line = line.Trim();
                            if (!line.StartsWith("#") && line.Length > 1)
                            {
                                instance.Add(line);
                            }
                        }
                    }
                    finally
                    {
                        if (reader != null)
                            reader.Close();
                    }
                }
            }
        }
	}
}
