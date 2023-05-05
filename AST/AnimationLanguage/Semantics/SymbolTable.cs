using AnimationLanguage.ASTNodes;
namespace AnimationLanguage.Visitor;

//The symbol class represents a variable or function in the symbol table.
public class Symbol
{
    public string Name { get; } // The name of the symbol.
    public string Type { get; } // The type of the symbol.

    public Symbol(string name, string type)
    {
        Name = name;
        Type = type;
    }
}

//The symbol table class represents a symbol table. It is used to store variables, functions, prototypes and sequences.
//A symbol table is used to check for duplicate declarations and to check if a variable or function exists.
public class SymbolTable
{
    private readonly Dictionary<string, Symbol> symbols = new Dictionary<string, Symbol>(); // The dictionary that stores the symbols.

    public void AddVariable(string name, string type) // This method adds a variable to the symbol table.
    {
        if (symbols.ContainsKey(name)) // Check if the variable already exists.
        {
            throw new InvalidOperationException($"Variable '{name}' already exists.");
        }

        symbols[name] = new Symbol(name, type); // Add the variable to the symbol table by using the name as the key and a new symbol as the value.
    }

    public void AddFunction(string name, string returnType) // This method adds a function to tthe symbol table
    {
        if (symbols.ContainsKey(name)) // Check if the function already exists in the table.
        {
            throw new InvalidOperationException($"Function '{name}' already exists.");
        }

        symbols[name] = new Symbol(name, returnType); // Add the function to the symbol table by using the name as the key and a new symbol as the value.
    }

    public void AddPrototype(string name, string type) // This method adds a prototype to the symbol table.
    {
        if (symbols.ContainsKey(name)) // Check if the prototype already exists in the table.
        {
            throw new InvalidOperationException($"Prototype '{name}' already exists.");
        }

        symbols[name] = new Symbol(name, type); // Add the prototype to the symbol table by using the name as the key and a new symbol as the value.
    }

    public void AddSequence(string name) // This method adds a sequence to the symbol table.
    {
        if (symbols.ContainsKey(name)) // Check if the sequence already exists in the table. 
        {
            throw new InvalidOperationException($"Sequence '{name}' already exists.");
        }

        symbols[name] = new Symbol(name, "seq"); // Add the sequence to the symbol table by using the name as the key and a new symbol as the value.
    }

    public Symbol? Lookup(string name) // This method looks up a symbol in the symbol table. It will be used in the type checker to check if a variable or function exists.
    {
        symbols.TryGetValue(name, out Symbol? symbol); // Try to get the symbol from the symbol table if it exists.
        return symbol;
    }
}