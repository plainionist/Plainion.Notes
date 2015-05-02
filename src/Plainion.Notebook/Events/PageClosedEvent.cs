using Plainion.Notebook.ViewModels;
using Microsoft.Practices.Prism.PubSubEvents;

namespace Plainion.Notebook.Events
{
    class PageClosedEvent : PubSubEvent<PageViewModel>
    {
    }
}
