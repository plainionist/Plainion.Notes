using System;
using System.IO;
using System.Text;

namespace Plainion.Wiki.Rendering
{
    /// <summary>
    /// Context of the renderer and it plugins.
    /// Provides access to the output stream/writer and to the content
    /// which needs to be rendered and its environment (e.g. settings).
    /// </summary>
    public class RenderingContext : IDisposable
    {
        public RenderingContext( Stream output )
        {
            Writer = new StreamWriter( output, new UTF8Encoding( false, true ), 1024, true );
        }

        public TextWriter Writer
        {
            get;
            private set;
        }

        public EngineContext EngineContext
        {
            get;
            set;
        }

        public void Dispose()
        {
            if( Writer != null )
            {
                Writer.Close();
                Writer = null;
            }
        }
    }
}
