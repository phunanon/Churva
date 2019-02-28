# Plan for Interpreter

* File or CLI

## Tokenisation

### Rates of tokenisation

Base class is Tokeniser, decorated with TokenParsers.  
Source is looped through, per line, using Rate0 then Rate1 parser.

**TokenParsers**

Check for, and subsequently parse...

1. Line/block end
2. Comment
3. Variables (declaration)
4. Variables (assignment)
5. Statements
6. Classes (referenced)
7. Classes (declaration)
8. Subroutines (referenced)
9. Subroutines (declaration)
10. Variables (referenced)
11. Operators

**Rates**

* 0th Rate: 7, 9. Used for file evaluation.
* 1st Rate: 1, 2, 3, 4, 5, 6, 7, 8, 9. Used for line evaluation.
* 2nd Rate: 3, 4, 6, 8, 10. Used for expression evaluation (e.g. someCode(), 4 + 3).
* 3rd Rate: 1, 9, 6, 8, 10. Used for sub-expression evaluation (e.g. 4, +, 3).

**TokenChecker operations**

1. Yield
2. Yield
3. DECL, TYPE, name, if '=' defer to Parse4, if '(' defer to Parse7
4. ASSIGN, name, defer to Rate2
5. STATEMENT_NAME, (PARAM, Rate2,) ENDPARAM
6. Advance class context, or defer to Parse8
7. TODO
8. CALL, name, ... each ',' do PARAM + Rate2 ... ENDPARAM
9. TODO
10. DREF, name, continue to Rate3
11. TODO
12. TODO
13. TODO