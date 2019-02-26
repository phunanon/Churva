# Plan for Interpreter

* File or CLI

## Tokenisation

### Rates of tokenisation

Base class is Tokeniser, decorated with TokenParsers.  
Source is looped through, per line, using Rate1 parser.

**TokenParsers**

Check for, and subsequently parse...

1. Line/block end
2. Comment
3. Declaration
4. Assignment
5. Statements
6. Classes
7. Subroutines
8. Variables
9. Operators

**Rates**

* 1st Rate: 1, 2, 3, 4, 5, 6, 7. Used for line evaluation.
* 2nd Rate: 3, 4, 6, 7, 8. Used for expression evaluation (e.g. someCode(), 4 + 3).
* 3rd Rate: 1, 9, 6, 7. Used for sub-expression evaluation (e.g. 4, +, 3).

**TokenChecker operations**

1. Yield
2. Yield
3. DECL, TYPE, name, if '=' defer to Parse4
4. ASSIGN, name, defer to Rate2
5. STATEMENT_NAME, (PARAM, Rate2,) ENDPARAM
6. Defer to Parse6+Parse7
7. CALL, name, ... each ',' do PARAM + Rate2 ... ENDPARAM
8. DREF, name, defer to Rate3
9. TODO