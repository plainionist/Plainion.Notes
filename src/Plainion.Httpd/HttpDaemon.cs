using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Globalization;

namespace Plainion.Httpd
{
    /// <summar>
    /// http://www.paraesthesia.com/archive/2008/07/16/simplest-embedded-web-server-ever-with-httplistener.aspx
    /// </summar>>
    public class HttpDaemon : IDisposable
    {
        private Thread myConnectionManagerThread;
        private HttpListener myListener;
        private long myRunState = (long)State.Stopped;

        public HttpDaemon( Uri listenerPrefix )
        {
            if ( !HttpListener.IsSupported )
            {
                throw new NotSupportedException( "The HttpListener class is not supported on this operating system." );
            }
            if ( listenerPrefix == null )
            {
                throw new ArgumentNullException( "listenerPrefix" );
            }

            UniqueId = Guid.NewGuid();
            myListener = new HttpListener();
            myListener.Prefixes.Add( listenerPrefix.AbsoluteUri );
        }

        public void Dispose()
        {
            if ( myListener != null )
            {
                if ( RunState != State.Stopped )
                {
                    Stop();
                }

                myListener = null;
            }

            if ( myConnectionManagerThread != null )
            {
                myConnectionManagerThread.Abort();
                myConnectionManagerThread = null;
            }
        }

        public enum State
        {
            Stopped,
            Stopping,
            Starting,
            Started
        }

        public event EventHandler<HttpRequestEventArgs> IncomingRequest;

        public State RunState
        {
            get { return (State)Interlocked.Read( ref myRunState ); }
        }

        public Guid UniqueId { get; private set; }

        public Uri Url { get; private set; }

        public virtual void Start()
        {
            if ( myConnectionManagerThread == null || myConnectionManagerThread.ThreadState == ThreadState.Stopped )
            {
                myConnectionManagerThread = new Thread( new ThreadStart( ConnectionManagerThreadStart ) );
                myConnectionManagerThread.Name = String.Format( CultureInfo.InvariantCulture, "ConnectionManager_{0}", UniqueId );
            }
            else if ( myConnectionManagerThread.ThreadState == ThreadState.Running )
            {
                throw new ThreadStateException( "The request handling process is already running." );
            }

            if ( myConnectionManagerThread.ThreadState != ThreadState.Unstarted )
            {
                throw new ThreadStateException( "The request handling process was not properly initialized so it could not be started." );
            }
            myConnectionManagerThread.Start();

            long waitTime = DateTime.Now.Ticks + TimeSpan.TicksPerSecond * 10;
            while ( RunState != State.Started )
            {
                Thread.Sleep( 100 );
                if ( DateTime.Now.Ticks > waitTime )
                {
                    throw new TimeoutException( "Unable to start the request handling process." );
                }
            }
        }

        private void ConnectionManagerThreadStart()
        {
            Interlocked.Exchange( ref myRunState, (long)State.Starting );
            try
            {
                if ( !myListener.IsListening )
                {
                    myListener.Start();
                }
                if ( myListener.IsListening )
                {
                    Interlocked.Exchange( ref myRunState, (long)State.Started );
                }

                try
                {
                    while ( RunState == State.Started )
                    {
                        HttpListenerContext context = myListener.GetContext();
                        RaiseIncomingRequest( context );
                    }
                }
                catch ( HttpListenerException )
                {
                    // This will occur when the listener gets shut down.
                    // Just swallow it and move on.
                }
            }
            finally
            {
                Interlocked.Exchange( ref myRunState, (long)State.Stopped );
            }
        }

        private void RaiseIncomingRequest( HttpListenerContext context )
        {
            HttpRequestEventArgs e = new HttpRequestEventArgs( context );
            try
            {
                if ( IncomingRequest != null )
                {
                    IncomingRequest.BeginInvoke( this, e, null, null );
                }
            }
            catch
            {
                // Swallow the exception and/or log it, but you probably don't want to exit
                // just because an incoming request handler failed.
            }
        }

        public virtual void Stop()
        {
            // Setting the runstate to something other than "started" and
            // stopping the listener should abort the AddIncomingRequestToQueue
            // method and allow the ConnectionManagerThreadStart sequence to
            // end, which sets the RunState to Stopped.
            Interlocked.Exchange( ref myRunState, (long)State.Stopping );
            if ( myListener.IsListening )
            {
                myListener.Stop();
            }
            long waitTime = DateTime.Now.Ticks + TimeSpan.TicksPerSecond * 10;
            while ( RunState != State.Stopped )
            {
                Thread.Sleep( 100 );
                if ( DateTime.Now.Ticks > waitTime )
                {
                    throw new TimeoutException( "Unable to stop the web server process." );
                }
            }

            myConnectionManagerThread = null;
        }
    }
}
