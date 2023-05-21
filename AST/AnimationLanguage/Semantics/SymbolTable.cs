using AnimationLanguage.ASTCommon;
using AnimationLanguage.ASTNodes;
namespace AnimationLanguage.Visitor;

//The symbol class represents a variable or function in the symbol table.
public class Symbol
{
    public string Name { get; } // The name of the symbol.
    public string Type { get; } // The type of the symbol.
    public IASTNode? Value { get; set; } // The value of the symbol, if any.

    public Symbol(string name, string type, IASTNode? value = null)
    {
        Name = name;
        Type = type;
        Value = value;
    }

    public Symbol(string name, string type, bool isPrototype, IASTNode parameters)
    {
        Name = name;
        Type = type;
        Value = parameters;
    }
}


//The symbol table class represents a symbol table. It is used to store variables, functions, prototypes and sequences.
//A symbol table is used to check for duplicate declarations and to check if a variable or function exists.
public class SymbolTable
{
    private readonly Dictionary<string, Symbol> symbols = new Dictionary<string, Symbol>(); // The dictionary that stores the symbols.

    public void AddVariable(string name, string type, IASTNode? value = null) // This method adds a variable to the symbol table.
    {
        if (symbols.ContainsKey(name)) // Check if the variable already exists.
        {
            throw new InvalidOperationException($"Variable '{name}' already exists.");
        }

        symbols[name] = new Symbol(name, type, value); // Add the variable to the symbol table by using the name as the key and a new symbol as the value.
    }


    public void AddFunction(string name, string returnType) // This method adds a function to tthe symbol table
    {
        if (symbols.ContainsKey(name)) // Check if the function already exists in the table.
        {
            throw new InvalidOperationException($"Function '{name}' already exists.");
        }

        symbols[name] = new Symbol(name, returnType); // Add the function to the symbol table by using the name as the key and a new symbol as the value.
    }

    public void AddPrototype(string name, string type, IASTNode? value) // This method adds a prototype to the symbol table.
    {
        if (symbols.ContainsKey(name)) // Check if the prototype already exists in the table.
        {
            throw new InvalidOperationException($"Prototype '{name}' already exists.");
        }

        symbols[name] = new Symbol(name, type, value); // Add the prototype to the symbol table by using the name as the key and a new symbol as the value.
    }

    public void AddSequence(string name) // This method adds a sequence to the symbol table.
    {
        if (symbols.ContainsKey(name)) // Check if the sequence already exists in the table. 
        {
            throw new InvalidOperationException($"Sequence '{name}' already exists.");
        }

        symbols[name] = new Symbol(name, "seq"); // Add the sequence to the symbol table by using the name as the key and a new symbol as the value.
    }
    
    
    public void AddParameter(string name, string type, IASTNode? value = null)
    {
        if (symbols.ContainsKey(name))
        {
            throw new InvalidOperationException($"Parameter '{name}' already exists.");
        }

        symbols[name] = new Symbol(name, type, value);
    }

    public Symbol? Lookup(string name) // This method looks up a symbol in the symbol table. It will be used in the type checker to check if a variable or function exists.
    {
        symbols.TryGetValue(name, out Symbol? symbol); // Try to get the symbol from the symbol table if it exists.
        return symbol;
    }
}


// instances of this class will be created when entering different scopes in the language 
public class ScopedSymbolTable
{
    //It is a stack, as the scopes will be entered and exited in a last-in first-out order
    private readonly Stack<SymbolTable> _scopeStack = new Stack<SymbolTable>();

    public ScopedSymbolTable()
    {
        _scopeStack.Push(new SymbolTable()); // Initialize the stack with the global scope.
    }

    public void EnterScope() // This method is called whenever a new scope is enteered.
    {
        _scopeStack.Push(new SymbolTable());
    }

    public void ExitScope() // This method is called whenever a scope is exited.
    {
        _scopeStack.Pop();
    }

    public void AddVariable(string name, string type, IASTNode? value = null) // This method adds a variable to the scope in the top of the stack
    {
        _scopeStack.Peek().AddVariable(name, type, value);
    }

    public void AddFunction(string name, string returnType) // This method adds a function to the scope in the top of the stack.
    {
        _scopeStack.Peek().AddFunction(name, returnType);
    }

    public void AddPrototype(string name, string type, IASTNode? value) // This method adds a prototype to the scope in the top of the stack
    {
        _scopeStack.Peek().AddPrototype(name, type, value);
    }

    public void AddSequence(string name) // This method adds a sequence to the scope in the top of the stack.
    {
        _scopeStack.Peek().AddSequence(name);
    }

    public Symbol? Lookup(string name) // This method looks up a symbol in the scope stack. It will be used in the type checker to check if a variable or function exists.
    {
        foreach (SymbolTable table in _scopeStack) 
        {
            Symbol? symbol = table.Lookup(name); // Try to get the symbol from the symbol table if it exists.
            if (symbol != null)
            {
                return symbol; // return the symbol if it exists.
            }
        }
        return null;
    }
    
    public Symbol? LookupInCurrentScope(string name) 
    {
        // Lookup in the current scope only
        return _scopeStack.Peek().Lookup(name);
    }
    
    
    public bool IsDefinedInCurrentScope(string name) 
    {
        // Check if the variable is defined in the current scope only
        return LookupInCurrentScope(name) != null;
    }
    
    
    public bool IsDefined(string name) // Does the same as the lookup method, but returns a boolean value
    {
        foreach (SymbolTable table in _scopeStack) 
        {
            if (table.Lookup(name) != null)
            {
                return true; // Return true if the symbol is found in any scope.
            }
        }
        return false;
    }

}