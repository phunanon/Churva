# Plan for Interpreter

* Binary or text file, or CLI

## Flow

1. Source to ParsedAtom's
2. ParsedAtom's to binary
3. Binary to ChurvaContext
4. ChurvaContext is executed

## Source to ParsedAtom's
TODO

## ParsedAtom's to Binary

Base class is BaseSerialiser, decorated with XSerialiser's.  
ParsedAtom's are looped through, using Rate0 then Rate1 serialiser.

**XSerialiser's**

Check for, and subsequently serialise...

1. Line/block end
2. Variables (declaration)
3. Variables (assignment)
4. Statements
5. Classes (declaration)
6. Subroutines (referenced)
7. Subroutines (declaration)
8. Referenced names
9. Operators
10. Literals

**Rates**

* 0th Rate: 7, 9. Used for initial file evaluation.
* 1st Rate: 1, 2, 3, 4, 5, 6, 7. Used for line evaluation.
* 2nd Rate: 2, 3, 6, 7, 8, 10. Used for expression evaluation (e.g. someCode(), 4 + 3).
* 3rd Rate: 1, 5, 6, 7, 8. Used for sub-expression evaluation (e.g. 4, +, 3).

**TokenChecker operations**

1. Yield
2. DECL, TYPE, 32-bit hashed name, if '=' defer to Parse4, if '(' defer to Parse7
3. ASSIGN, name, defer to Rate2
4. STATEMENT_NAME, (PARAM, Rate2,) ENDPARAM
5. Advance class context, or defer to Parse8
6. TODO
7. CALL, name, ... each ',' do PARAM + Rate2 ... ENDPARAM
8. TODO
9. DREF, name, continue to Rate3
10. TODO
11. TODO
12. TODO

## Binary to Context (TODO)

Check for, and subsequently serialise...

1. Line/block end
2. Variables (declaration)
3. Variables (assignment)
4. Statements
5. Classes (referenced)
6. Classes (declaration)
7. Subroutines (referenced)
8. Subroutines (declaration)
9. Variables (referenced)
10. Operators

**Rates**

* 0th Rate: 7, 9. Used for initial file evaluation.
* 1st Rate: 1, 2, 3, 4, 5, 6, 7, 8. Used for line evaluation.
* 2nd Rate: 2, 3, 5, 7, 9. Used for expression evaluation (e.g. someCode(), 4 + 3).
* 3rd Rate: 1, 5, 7, 8, 9. Used for sub-expression evaluation (e.g. 4, +, 3).