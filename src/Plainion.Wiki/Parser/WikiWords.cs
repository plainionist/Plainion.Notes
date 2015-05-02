using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.AST;
using Plainion.Wiki.Utils;
using Plainion.Wiki.DataAccess;

namespace Plainion.Wiki.Parser
{
    /// <summary/>
    public class WikiWords
    {
        private List<string> myWords;

        /// <summary/>
        public WikiWords()
        {
            myWords = new List<string>();
        }

        /// <summary/>
        public IEnumerable<string> Words
        {
            get { return myWords; }
        }

        /// <summary/>
        public bool Contains( string word )
        {
            return myWords.Contains( word, StringComparer.OrdinalIgnoreCase );
        }

        /// <summary/>
        public void Add( IEnumerable<PageName> pageNames )
        {
            Add( pageNames.Select( pn => pn.Name ) );
        }

        /// <summary/>
        public void Add( IEnumerable<string> words )
        {
            myWords.AddRange( words );

            myWords = myWords
                .Distinct( StringComparer.OrdinalIgnoreCase )
                .ToList();
        }
    }
}
