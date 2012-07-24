using System;
using System.IO;
using System.Text;
using System.Collections;
using Lucene.Net ;
using Lucene.Net.Analysis;

namespace SearchEngine.Analysis
{
	/// <summary>
    /// ThesaurusAnalyzer 的摘要说明。
	/// </summary>
    public class ThesaurusAnalyzer : Lucene.Net.Analysis.Analyzer 
	{

        public ThesaurusAnalyzer() 
		{
		}

		/// <summary>
		/// Creates a TokenStream which tokenizes all the text in the provided Reader.
		/// </summary>
        /// <returns>A TokenStream build from a ThesaurusTokenizer filtered with ThesaurusSpliter.</returns>
		public override sealed TokenStream TokenStream(String fieldName, TextReader reader) 
		{
            TokenStream result = new ThesaurusTokenizer(reader);
			return result;
		}
	}
}
