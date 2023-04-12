using AnimationLanguage;
namespace AnimationLanguage;

//Vi benytter os af return typen 'object' for base visitoren, da den repræsenterer alle former for returtyper vi kan støde på
//Den kan også returnere null, da ting som assignment ikke returnerer noget.
public class AnimationVisitor: AnimationLanguageRulesBaseVisitor<object?>
{
    private Dictionary<string, object?> Variables = new();

    //Dette er hvad der sker når vi besøger en assignment operation
    public override object? VisitAssignment(AnimationLanguageRulesParser.AssignmentContext context)
    {
        //Vi tildeler de værdier vi har fået fra vores context til tilsvarende variable i vores kode.
        var varType = context.type().GetText();
        var varName = context.IDENTIFIER().First().GetText();
        var value = context.expression().GetText();

        Variables[varName] = value;
        
        return null;
    }
}