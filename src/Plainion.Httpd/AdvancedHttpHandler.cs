using System;
using System.Collections.Generic;
using System.Linq;
using Plainion.Httpd.Views;

namespace Plainion.Httpd
{
    public class AdvancedHttpHandler : GenericHttpHandler
    {
        public AdvancedHttpHandler()
        {
            ViewChain = new Stack<AbstractHttpView>();
            ViewChain.Push(new FallbackView());

            ErrorHandler = new GenericErrorHandler();
        }

        public Stack<AbstractHttpView> ViewChain { get; private set; }

        protected override HttpResponse HandleRequestInternal()
        {
            var view = DispatchRequest();
            return RenderView(view);
        }

        private AbstractHttpView DispatchRequest()
        {
            return ViewChain.First(view => view.CanHandleRequest(Request));
        }

        private HttpResponse RenderView(AbstractHttpView view)
        {
            return view.HandleRequest(Request);
        }

        protected override HttpResponse HandleGeneralErrorInternal(Exception exception)
        {
            return ErrorHandler.GenerateResponse(exception);
        }

        protected virtual GenericErrorHandler ErrorHandler { get; set; }
    }
}
