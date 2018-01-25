# CommandLineCalc

To run
* cd calculator
* dotnet run

To test
* cd calculatorTests
* dotnet test

The interactive shell takes input every time the user hits 'Enter'. It will compute and output a value for the equation's calculation at the point of the
equals sign. For example
1+2=+3=(User hits enter)
3
6

### Character Classes

The Parser encounters different characters and needs to decide what to do with them.

#### Equals (=)
Special case, handled by the shell. Responsible for forcing evaluation of EquationTree and displaying output. Otherwise it can be ignored by the parser. Takes current calculated value and sets it as the root of a new EquationTree.

#### Numbers and Decimal Point (0-9 .)
Concat numbers in a string until something else encountered by the parser.

#### Single Parameter Operators (! and 1/x)
1/x is the only multi character operator and is handled accordingly. Both of these only calculate on a single number. The parser evaluates these greedily. These operators do not end up in the EquationTree.

#### Single Parameter High Precedence Operators (* and /)
When these are encountered we need two numbers, one on either side to calculate the final value. Due to this and order of operations rules, we instead add the operator and previous number to a tree until we need to run a evaluation (encounter =). When adding these operators to the EquationTree we do not want to add these as the root of the EquationTree if the current operator there is lower precedence. We instead, want to add it to the right of all lower precedence operators.

#### Single Parameter Low Precedence Operators (+ and -)
When these are encountered we again need two numbers. We always add these operators as the new root of the EquationTree.

A leading - or a - after another operator indicates that this is a negative number and not an operator. Handled in a custom conditional.

#### Clear Operators (A or C)
These operators modify (remove items) from the existing EquationTree. All Clear (A) sets the entire tree to null. Clear (C) simply removes the last added operator and any numbers (if any) to its right.

#### Quit (Q)
When quit the program.

#### Other
Simply ignored, includes white space

### Equation Tree
The EquationTree is a binary tree that is recursively evaluated to evaluate an equation. The root node must be an oeprator (except an EquationTree with only one Node, then it is a number) and all leaf nodes must be numbers. Evaluation starts at the leaf nodes and the deepest operators and works its way up until the root node's operator is the last evaluated.

### Calculator Limitations
There are some unique challenges around factorials, recipricals, and division.

First off factorials are not defined for negative or non integer numbers. These therefore will throw an exception. Even if starting with an integer as the value typed into the calculator, reciprical and division may create a double. So something like 3/2! or 31/x! will throw exceptions.

Less obvious are the issues with divsion and by exstension recipricals. Since a double (or any primitive for that matter) can only store a finite amount of data, these operations are not exact. Therefore some statements may not evaluate in a perfectly correct mathematical sense. 61/x1/x= should return 6 since the reciprocal of a reciprocal should always equal the original value. (1/6)*6=6. 1/6 represented by a decimal is 0.16666 with the 6 repeating for infinity. By performing division and storing this in a double we truncate and round it so that it actually gets represented by 0.166666666666667. So in the calucaltor 1/6=61/x=0.166666666666667. 0.166666666666667*6=61/x1/x=5.99999999999999. There is no equation preprocessing to simplify unneeded terms. Every operation is user input and evaluated. Any equation with division or recipricals is thus and estimation.

For division and recirpicals there are also divide by zero exceptions.
