
namespace Plainion.Wiki.Rendering
{
    /// <summary>
    /// Pre-defined stages of the <see cref="RenderingPipeline"/>.
    /// Custom stages should be defined by using one of theses predefined
    /// stages and adding an offset.
    /// </summary>
    public enum RenderingStage
    {
        /// <summary>
        /// Builds up the <see cref="Plainion.Wiki.AST.Page"/> based on content, sidebar, etc.
        /// Page needs to be built up first to be able to apply
        /// all the preprocessing to all bodies of the page (incl. e.g. sidebar).
        /// </summary>
        BuildUpPage = 1000,

        /// <summary>
        /// Clone the original AST from the parser to allow AST modifications
        /// by the subsequent stages without any impact to the next rendering.
        /// </summary>
        Clone = 2000,

        /// <summary>
        /// Applies configured transformations to known "markup" attributes.
        /// Transformations can be simple text replacements or "pre-defined"
        /// dynamics queries.
        /// </summary>
        AttributeTransformation = 3000,

        /// <summary>
        /// Compilation of the dynamic queries. 
        /// <seealso cref="Plainion.Wiki.AST.CompiledQuery"/>
        /// </summary>
        QueryCompilation = 4000,

        /// <summary>
        /// Execution of the dynamic queries. 
        /// <seealso cref="Plainion.Wiki.AST.CompiledQuery"/>
        /// </summary>
        QueryExecution = 5000,

        /// <summary>
        /// Generic preprocessing of attributes
        /// </summary>
        AttributePreProcessing = 6000,

        /// <summary>
        /// Last possible/supported stage.
        /// </summary>
        Final = int.MaxValue
    }
}
