using System.ComponentModel.Composition;
using Plainion.Notebook.Model;
using Plainion;
using Plainion.AppFw.Wpf.Services;

namespace Plainion.Notebook.Services
{
    internal class ProjectService : ProjectService<Project>
    {
        private WikiService myWikiSerivce;

        [ImportingConstructor]
        public ProjectService( WikiService wikiService )
        {
            myWikiSerivce = wikiService;
        }

        public override Project CreateEmptyProject( string location )
        {
            var project = base.CreateEmptyProject( location );

            myWikiSerivce.CreateNewProject( project );

            return project;
        }

        protected override void OnProjectChanging( ProjectChangeEventArgs<Project> args )
        {
            Contract.RequiresNotNullNotEmpty( args.NewProject.Location, "Project must have valid location" );

            if ( myWikiSerivce.IsDaemonRunning )
            {
                myWikiSerivce.StopDaemon();
            }

            base.OnProjectChanging( args );
        }

        protected override void OnProjectChanged( ProjectChangeEventArgs<Project> args )
        {
            myWikiSerivce.StartDaemon( Project );

            base.OnProjectChanged( args );
        }
    }
}
