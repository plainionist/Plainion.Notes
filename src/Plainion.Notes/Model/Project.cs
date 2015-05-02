using System;
using System.IO;
using Plainion.AppFw.Wpf.Model;

namespace Plainion.Notes.Model
{
    public class Project : ProjectBase
    {
        public Project()
        {
            DbFolder = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments ), "Notes.db" );
            Location = DbFolder;
        }

        public string DbFolder { get; private set; }
    }
}
