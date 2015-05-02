using System;

namespace Plainion.Wiki.AST
{
    /// <summary>
    /// Represents a link which can be any valid URL or a page name.
    /// Supports '#' as anchor marker.
    /// </summary>
    [Serializable]
    public class Link : Markup
    {
        /// <summary/>
        public Link( PageName pageName )
            : this( pageName.FullName, pageName.FullName.Substring( 1 ) )
        {
        }

        /// <summary/>
        public Link( string url )
            : this( url, url )
        {
        }

        /// <summary/>
        public Link( string url, string text )
        {
            Contract.RequiresNotNullNotWhitespace( url, "url" );

            SetUrl( url );

            Text = text;
            if ( !IsExternal && Text.StartsWith( "/" ) )
            {
                // TODO: add test
                // it is an absolute page reference
                // -> cut off the leading "/"
                Text = Text.Substring( 1 );
            }
        }

        /// <summary/>
        public Link( string url, string anchor, string text )
            : this( url + "#" + anchor, text )
        {
        }

        private void SetUrl( string url )
        {
            Url = url.Trim();

            if ( Url.Equals( ".", StringComparison.OrdinalIgnoreCase ) )
            {
                // local page
                Url = string.Empty;
            }

            UrlWithoutAnchor = Url;

            var anchorPos = Url.LastIndexOf( "#" );
            if ( anchorPos != -1 )
            {
                UrlWithoutAnchor = Url.Substring( 0, anchorPos );
                Anchor = Url.Substring( anchorPos + 1 );
            }
        }

        /// <summary>
        /// True if the link is URL pointing outside the system, false otherwise.
        /// </summary>
        public static bool IsExternalLink( string url )
        {
            return url.StartsWith( @"http://", StringComparison.OrdinalIgnoreCase ) ||
                url.StartsWith( @"https://", StringComparison.OrdinalIgnoreCase ) ||
                url.StartsWith( @"file://", StringComparison.OrdinalIgnoreCase ) ||
                url.StartsWith( @"\\", StringComparison.OrdinalIgnoreCase );
        }

        /// <summary>
        /// <see cref="IsExternalLink(string)"/>
        /// </summary>
        public bool IsExternal
        {
            get
            {
                return IsExternalLink( Url );
            }
        }

        /// <summary>
        /// Indicates that the renderer should not modify the link.
        /// </summary>
        public bool IsStatic
        {
            get;
            set;
        }

        /// <summary>
        /// URL or page name.
        /// </summary>
        public string Url
        {
            get;
            private set;
        }

        /// <summary/>
        public string UrlWithoutAnchor
        {
            get;
            private set;
        }

        /// <summary>
        /// Anchor added to the end of the url.
        /// </summary>
        public string Anchor
        {
            get;
            private set;
        }

        /// <summary>
        /// Display representation of the link.
        /// </summary>
        public string Text
        {
            get;
            private set;
        }
    }
}
