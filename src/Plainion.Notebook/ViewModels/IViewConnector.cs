using System;

namespace Plainion.Notebook.ViewModels
{
    interface IViewConnector
    {
        IDisposable View { get; set; }
    }
}
