using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Plainion.Notebook.ViewModels;

namespace Plainion.Notebook
{
    class PageViewModelFactory
    {
        private CompositionContainer myContainer;

        [ImportingConstructor]
        public PageViewModelFactory( CompositionContainer container )
        {
            myContainer = container;
        }

        public PageViewModel Create( DisplayUrlRequest request )
        {
            var catalog = new TypeCatalog( typeof( PageViewModel ) );

            using ( var scope = new CompositionContainer( catalog, myContainer ) )
            {
                scope.ComposeExportedValue( request );

                return scope.GetExportedValue<PageViewModel>();
            }
        }
    }
}
