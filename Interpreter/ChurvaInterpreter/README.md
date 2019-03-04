# Churva C# Interpreter

This is a descriptive documentation on the ChurvaInterpreter implementation.

## Implementation

This program takes four steps to interpret Churva source code:

1. Tokenisation of text source
2. Serialisation of tokens
3. Deserialisation into Churva context
4. Evaluation of Churva context

Steps 1 & 2 can be performed independently of 3 & 4, to enable interpreting from a serialised Churva program.

### Tokenisation of Source

All tokens are accompanied by the original source line & column numbers, and the original string.

#### Available tokens

`UNKNOWN, TEXT, ENDLIN, ENDBLK, OP, NUMBER, STRING, CHAR, INDENT, DEDENT`

* `TEXT` is any text-only symbols (e.g. variable, subroutine, class names)
* `ENDLIN` is appended to each source line
* `ENDBLK` is appended to each code block
* `OP` is any non-alphanumeric character (usually operators)
* `NUMBER`, `STRING`, and `CHAR` are literals
* `INDENT` & `DEDENT` follow any change in indentation, and their text is the new indent level

*Note: `UNKNOWN`, and `ENDBLK`, are unimplemented as of this time.*

#### Steps

1. The source is split into either `TEXT`, `OP`, `INDENT`, or `DEDENT`, with `ENDLIN` per line
	1. Alphanumeric text is split by whitespace
	2. Operators are independent tokens
2. `TEXT` are reevaluated into `TEXT` or `NUMBER`
3. `NUMBER . NUMBER` are reconstructed to `NUMBER`
4. `" TEXT "` and `' TEXT '` are reconstructed to escaped `STRING` and `CHAR` (without operators)
5. Multi-character operators are reconstructed

### Serialisation of Tokens



### Deserialisation into Churva Context

### Evaluation of Churva Context
