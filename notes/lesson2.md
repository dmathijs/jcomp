# Unary operator, conditions

Compiler doesn't care about parenthesis, comments, whitespace, ... in the syntax tree because you don't want this in the type checker.

However, if you're refactoring it might be interesting to have a tree with all the information because **all** operations becomme expressable over the tree. (Abstract Syntax Tree)

Function of the binder is that we generate an immutable syntaxtree. And the binder allows us to add additional information for e.g. type checking, what type does an expression evaluate to?



