using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Plainion.Notebook.Model;
using Plainion;
using Plainion.AppFw.Wpf.Services;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout.Serialization;

namespace Plainion.Notebook.Services
{
    [Export( typeof( Serializer ) )]
    [PartCreationPolicy( CreationPolicy.NonShared )]
    internal class Serializer : ISerializer<Project>
    {
        private const int VERSION = 1;
        private const string LayoutStorageName = "Layout.config";

        private string myProjectFile;
        private DockingManager myDockingManager;

        [ImportingConstructor]
        public Serializer( DockingManager dockingManager )
        {
            myDockingManager = dockingManager;
        }

        public string ProjectFile
        {
            get { return myProjectFile; }
            private set
            {
                Contract.RequiresNotNullNotEmpty( value, "projectFile" );

                if ( Path.GetExtension( value ) != ".bnt" )
                {
                    throw new ArgumentException( "Only files with .bnt extension supported" );
                }

                myProjectFile = value;
            }
        }

        public void Serialize( Project project )
        {
            ProjectFile = project.Location;

            if ( !Directory.Exists( project.DbFolder ) )
            {
                Directory.CreateDirectory( project.DbFolder );
            }

            WriteProject( project );
        }

        public void SaveLayout( Project project )
        {
            ProjectFile = project.Location;

            if ( !Directory.Exists( project.DbFolder ) )
            {
                Directory.CreateDirectory( project.DbFolder );
            }

            var serializer = new XmlLayoutSerializer( myDockingManager );
            serializer.Serialize( Path.Combine( project.DbFolder, LayoutStorageName ) );
        }

        private void WriteProject( Project project )
        {
            var doc = new XElement( "Project",
                new XElement( "Version", VERSION ) );

            WriteXDocument( doc, ProjectFile );
        }

        private static void WriteXDocument( XElement doc, string file )
        {
            var settings = new XmlWriterSettings();
            settings.CloseOutput = true;
            settings.Indent = true;
            using ( var writer = XmlWriter.Create( file, settings ) )
            {
                doc.WriteTo( writer );
            }
        }

        [Import( AllowDefault = true )]
        public EventHandler<LayoutSerializationCallbackEventArgs> LayoutSerializationCallback
        {
            get;
            set;
        }

        public Project Deserialize( string file )
        {
            ProjectFile = file;

            var project = new Project();
            project.Location = file;

            return project;
        }

        public bool LoadLayout( Project project )
        {
            var layoutConfig = Path.Combine( project.DbFolder, LayoutStorageName );
            if ( !File.Exists( layoutConfig ) )
            {
                return false;
            }

            var serializer = new XmlLayoutSerializer( myDockingManager );
            serializer.LayoutSerializationCallback += LayoutSerializationCallback;

            serializer.Deserialize( layoutConfig );

            return true;
        }
    }
}
