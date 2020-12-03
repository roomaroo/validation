using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SourceGenerator
{
    [Generator]
    public class ValidationGenerator : ISourceGenerator
    {
        private const string attributeText = @"
using System;
namespace AutoValidate
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class ValidateAttribute : Attribute
    {
        public ValidateAttribute()
        {
        }
    }

}
";
        public void Initialize(GeneratorInitializationContext context)
        {
            // Register a syntax receiver that will be created for each generation pass
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            // Add the attribute text to the compilation
            context.AddSource("ValidateAttribute", SourceText.From(attributeText, Encoding.UTF8));

            // Get the syntax receiver, which should be populated
            if (context.SyntaxReceiver is not SyntaxReceiver receiver)
            {
                return;
            }

            // Create a new compilation that contains the attribute
            CSharpParseOptions? options = (context.Compilation as CSharpCompilation)?.SyntaxTrees.FirstOrDefault()?.Options as CSharpParseOptions;
            Compilation compilation = context.Compilation.AddSyntaxTrees(CSharpSyntaxTree.ParseText(SourceText.From(attributeText, Encoding.UTF8), options));

            // Get the newly-bound attribute, and INotifyPropertyChanged
            INamedTypeSymbol attributeSymbol = 
                compilation.GetTypeByMetadataName("AutoValidate.ValidateAttribute") 
                ?? throw new InvalidOperationException("Attribute not found even though we just added it");

            // Loop over the candidate fields, keep the ones that are annotated
            List<INamedTypeSymbol> classSymbols = new List<INamedTypeSymbol>();
            foreach (ClassDeclarationSyntax classDeclaration in receiver.CandidateClasses)
            {
                SemanticModel model = compilation.GetSemanticModel(classDeclaration.SyntaxTree);

                // Get the symbol being declared by the field. Keep it if it's annotated
                if (model.GetDeclaredSymbol(classDeclaration) is INamedTypeSymbol classSymbol)
                {
                    var attributes = classSymbol.GetAttributes();
                    if (attributes.Any(ad => ad.AttributeClass?.Equals(attributeSymbol, SymbolEqualityComparer.Default) == true) == true)
                    {
                        classSymbols.Add(classSymbol) ;
                    }
                }
            }

            foreach (var classSymbol in classSymbols)
            {
                string source = ProcessClass(classSymbol, attributeSymbol, context);
                if (source != string.Empty)
                {
                    context.AddSource($"Validated{classSymbol.Name}", source);
                }
            }
        }

        private string ProcessClass(INamedTypeSymbol classSymbol, ISymbol attributeSymbol, GeneratorExecutionContext context)
        {
            if (!classSymbol.ContainingSymbol.Equals(classSymbol.ContainingNamespace, SymbolEqualityComparer.Default))
            {
                return string.Empty; // Not a top-level class
            }

            string namespaceName = classSymbol.ContainingNamespace.ToDisplayString();

            string baseTypesAndInterfaces = string.Join(", ", new[] { classSymbol.BaseType?.Name }.Where(n => n is not null).Concat(classSymbol.Interfaces.Select(i => i.Name)));
            if (baseTypesAndInterfaces == "Object")
            {
                baseTypesAndInterfaces = "";
            }

            // Begin building the generated source
            StringBuilder source = new StringBuilder($@"
namespace {namespaceName}.Validated
{{
    public partial class {classSymbol.Name} { (string.IsNullOrEmpty(baseTypesAndInterfaces) ? "" : $": {baseTypesAndInterfaces}") }
    {{
");
            // Create a property for each source property
            foreach (IPropertySymbol propertySymbol in classSymbol.GetMembers().OfType<IPropertySymbol>())
            {
                ProcessProperty(source, propertySymbol, namespaceName);
            }

            AddConstructor(source, classSymbol, namespaceName);

            source.Append($"    }}{Environment.NewLine}}}");

            return source.ToString();
        }

        private void AddConstructor(StringBuilder source, INamedTypeSymbol classSymbol, string unvalidatedNamespace)
        {
            source.AppendLine();
            source.Append($"        internal {classSymbol.Name}(");

            var paramStrings =
                from property in classSymbol.GetMembers().OfType<IPropertySymbol>()
                let fieldType = ProcessType(property, unvalidatedNamespace)
                select $"{fieldType} {paramName(property.Name)}";

            source.Append(string.Join(", ", paramStrings));
            source.AppendLine(")");
            source.AppendLine("        {");

            foreach (var property in classSymbol.GetMembers().OfType<IPropertySymbol>())
            {
                source.AppendLine($"            this.{property.Name} = {paramName(property.Name)};");
            }

            source.AppendLine("        }");

            string paramName(string propertyName)
            {
                return propertyName.Substring(0, 1).ToLowerInvariant() + propertyName.Substring(1);
            }
        }

        private void ProcessProperty(StringBuilder source, IPropertySymbol propertySymbol, string unvalidatedNamespace)
        {
            string propertyName = propertySymbol.Name;
            string fieldType = ProcessType(propertySymbol, unvalidatedNamespace);

            source.AppendLine($@"        public {fieldType} {propertyName} {{ get; }}");
        }

        private string ProcessType(IPropertySymbol propertySymbol, string unvalidatedNamespace)
        {
            //System.Diagnostics.Debugger.Launch();

            ITypeSymbol propertyType = propertySymbol.Type;
            //if (propertyType.ContainingNamespace?.ToString() == unvalidatedNamespace)
            //{
            //    return propertyType.ToString().Replace(unvalidatedNamespace, $"{unvalidatedNamespace}.Validated");
            //}

            return propertyType.ToString().Replace(unvalidatedNamespace, $"{unvalidatedNamespace}.Validated");
        }

        private class SyntaxReceiver : ISyntaxReceiver
        {
            public List<ClassDeclarationSyntax> CandidateClasses { get; } = new List<ClassDeclarationSyntax>();

            /// <summary>
            /// Called for every syntax node in the compilation. We can inspect the node and save useful info for generation
            /// </summary>
            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax && classDeclarationSyntax.AttributeLists.Count > 0)
                {
                    // If it's got an attribute, it's a candidate
                    CandidateClasses.Add(classDeclarationSyntax);
                }
            }
        }
    }
}
