using System.Net;
using Plainion.Httpd;
using Plainion.Wiki.Http;
using Plainion.Wiki.Http.Views;

namespace Plainion.Notebook.Services
{
    class ExportView : AbstractRenderView
    {
        private WikiMetadata myWikiMetadata;

        public ExportView(IViewContext context, WikiMetadata wikiMetadata)
            : base(context)
        {
            myWikiMetadata = wikiMetadata;
        }

        public override bool CanHandleRequest(HttpListenerRequest request)
        {
            return GetAction(request) == "AllInOne";
        }

        public override HttpResponse HandleRequest(HttpListenerRequest request)
        {
            var response = new HttpResponse();
            Context.Engine.RenderAllContentIntoOnePage(myWikiMetadata, response.OutputStream);
            response.OutputStream.Close();

            return response;
        }
    }
}
