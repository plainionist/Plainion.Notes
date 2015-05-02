using System;
using System.Net;
using System.Text;

namespace Plainion.Httpd
{
    public abstract class GenericHttpHandler
    {
        private HttpListenerContext myContext;

        protected HttpListenerRequest Request
        {
            get { return myContext.Request; }
        }

        public void HandleRequest( HttpListenerContext context )
        {
            myContext = context;
            try
            {
                var response = BuildResponse();
                SendResponse( response );
            }
            finally
            {
                myContext.Response.Close();
                myContext = null;
            }
        }

        private HttpResponse BuildResponse()
        {
            try
            {
                return HandleRequestInternal();
            }
            catch ( Exception ex )
            {
                return HandleGeneralError( ex );
            }
        }

        protected abstract HttpResponse HandleRequestInternal();

        private HttpResponse HandleGeneralError( Exception exception )
        {
            try
            {
                return HandleGeneralErrorInternal( exception );
            }
            catch
            {
                return RenderDefaultErrorPage( exception );
            }
        }

        protected virtual HttpResponse HandleGeneralErrorInternal( Exception exception )
        {
            return RenderDefaultErrorPage( exception );
        }

        private HttpResponse RenderDefaultErrorPage( Exception exception )
        {
            return new HttpResponse( exception.ToString() );
        }

        private void SendResponse( HttpResponse response )
        {
            myContext.Response.StatusCode = (int)HttpStatusCode.OK;
            myContext.Response.StatusDescription = "OK";
            myContext.Response.ContentType = response.ContentType;

            if ( response.RedirectLocation != null )
            {
                myContext.Response.Redirect( response.RedirectLocation );
            }
            else
            {
                var responseContent = response.GetContent();

                myContext.Response.ContentEncoding = Encoding.UTF8;
                myContext.Response.ContentLength64 = responseContent.Length;

                myContext.Response.OutputStream.Write( responseContent, 0, responseContent.Length );
                myContext.Response.OutputStream.Flush();
                myContext.Response.OutputStream.Close();
                myContext.Response.Close();
            }
        }
    }
}
