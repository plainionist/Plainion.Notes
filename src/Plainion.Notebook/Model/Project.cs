using System.IO;
using Plainion.AppFw.Wpf.Model;

namespace Plainion.Notebook.Model
{
    public class Project : ProjectBase
    {
        protected override void OnLocationChanged()
        {
            DbFolder = Path.Combine( Path.GetDirectoryName( Location ), Path.GetFileNameWithoutExtension( Location ) + ".db" );
            PagesRoot = Path.Combine( DbFolder, "pages" );

            base.OnLocationChanged();
        }

        public string DbFolder { get; private set; }

        public string PagesRoot { get; private set; }
    }
}
