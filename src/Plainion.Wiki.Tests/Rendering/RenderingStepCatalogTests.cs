using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Plainion.Wiki.Rendering;
using Plainion.Wiki.UnitTests.Testing;
using NUnit.Framework;

namespace Plainion.Wiki.UnitTests.Rendering
{
    [TestFixture]
    public class RenderingStepCatalogTests
    {
        [Test]
        public void Composition_WhenHappened_WikiDefaultStepsFound()
        {
            var catalog = new AssemblyCatalog( typeof( RenderingPipeline ).Assembly );
            var container = new CompositionContainer( catalog );
            container.ComposeParts( catalog );

            var expectedSteps = new Dictionary<int, Type>();
            expectedSteps[ (int)RenderingStage.Clone ] = typeof( CloneStep );
            expectedSteps[ (int)RenderingStage.QueryExecution ] = typeof( QueryExecutionStep );
            expectedSteps[ (int)RenderingStage.AttributePreProcessing ] = typeof( AttributePreProcessingStep );
            expectedSteps[ (int)RenderingStage.BuildUpPage ] = typeof( PageBuildingStep );
            // not supported in this unittest because dependencies are not configured and so not resolvable
            //expectedSteps[ RenderingStage.AttributeTransformation ] = typeof( AttributeTransformationStep );
            // not supported in this unittest because dependencies are not configured and so not resolvable
            //expectedSteps[ RenderingStage.QueryCompilation ] = typeof( QueryCompilationStep );
            XAssert.PluginCatalogContains( expectedSteps, container.GetExportedValue<RenderingStepCatalog>() );
        }
    }
}
