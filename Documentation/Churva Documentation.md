# Churva Documentation

## Introduction
Term, Clock, line continuation

## Syntax
### Data Types, Declaration, and Assignment

| Data Type          | Token               | Size                        | Evaluation |
| ------------------ | ------------------- | --------------------------- | ---------- |
| explicitly empty   | `nul`               | 0                           | value      |
| any (determined)   | `var`               | variable                    | any        |
| boolean            | `boo`               | 1 bit                       | value      |
| integers un/signed | `u[bits]`/`i[bits]` | 2^n, 08 doubling to 64 bits | value      |
| floating point     | `f32/f64`           | 2^32 or 2^64 bits           | value      |
| decimal            | `dec`               | variable                    | value      |
| string             | `str`               | variable                    | value      |
| variable pointer   | `token*`            | address size                | pointer    |
| subroutine pointer | `sub`               | address size                | pointer    |

Assignment can be of any evaluated code. Possible methods of declaration and assignment:

    i32 variable1      //Declared
	variable1 = 42     //Assigned
	i32 variable2 = 42 //Declared & Assigned

* Pointer assignment takes address of variable by default.
* Evaluation of whole declaration and assignment statement returns value for value types, or pointer for pointer types

### Subroutines
#### Named

| Scheme                    | Syntax                                  |
| ------------------------- | --------------------------------------- |
| No parameters, no returns | `sub func => code`                      |
| No parameters, returns    | `sub func: i32 => value`                |
| Parameters, no returns    | `sub func (i32 a, i32 b) => code`       |
| Parameters, returns       | `sub func (i32 a, i32 b): i32 => value` |

Multi-line:

    sub func (i32 a, i32 b): i32 
        i32 sum = a + b
        => sum

Evaluate subroutine with and without arguments: `doSum()`, `doSum(1, arg2)`

#### Anonymous

Anonymous subroutines are a scope of code which can be contained on one line, and return the value of the final evaluation. Semicolons delimit blocks of code.  
Here, `myNumber` is assigned as the value of `sum`:

    i32 myNumber = i32 sum = a + b; sum   //
	i32 myNumber = (i32 sum = a + b; sum) // Same behaviour

### Flow Control

| Statement                      | Action                                                                              |
| ------------------------------ | ----------------------------------------------------------------------------------- |
| `if condition`                 | Evaluate block if condition is true                                                 |
| `else`                         | Evaluate block if previous `if` condition was false                                 |
| `elif condition`               | Same as `else`, and its condition is true                                           |
| `condition ? true : false`     | Evaluate blocks depending on condition                                              |
| `when condition`               | Evaluate block when condition is true, only once                                    |
| `while condition`              | Loop block if present while condition is true                                       |
| `for first; condition; repeat` | Evaluate `first` if present, loop block & repeat if present while condition is true |
| `each item, iterator: array`   | Loop block, advance item & iterator through array each loop                         |
| `each item: array`             | Loop block, advance item through array each loop                                    |
| `skip`                         | Skip to next iteration                                                              |
| `finish`                       | Finish loop                                                                         |

Single line statements: `statement => code`

Evaluation of arrays occurs only once, allowing such scenarios as:

    each item, i : var newArr = csv.delimit(',')
		Term.print("{i}/{newArr.Length}: {item}")

#### Conditionals

Conditionals are evaluated as true when their integer representation is non-zero.

### Operators
#### Unary (1 Operand)

| Operator | Action                    |
| -------- | ------------------------- |
| `!a`     | logical negate            |
| `~a`     | bitwise negate            |
| `+a`     | integer promotion         |
| `-a`     | additive inverse          |
| `++a`    | pre-evaluation increment  |
| `--a`    | pre-evaluation decrement  |
| `a++`    | post-evaluation increment |
| `a--`    | post-evaluation decrement |
| `*a`     | pointer dereference       |
| `&a`     | variable address          |
| `a[n]`   | array access element `n`  |

#### Binary (2 Operands)

| Operator                | Action                      |
| ----------------------- | --------------------------- |
| `a = b`                 | variable assignment         |
| `a + b`                 | arithmetical addition       |
| `a - b`                 | arithmetical subtraction    |
| `a * b`                 | arithmetical multiplication |
| `a / b`                 | arithmetical division       |
| `a % b`                 | arithmetical modulo         |
| `a & b`                 | bitwise AND                 |
| <code>a &#124; b</code> | bitwise OR                  |
| `a ^ b`                 | bitwise XOR                 |
| `a >> b`                | bitwise right shift         |
| `a << b`                | bitwise left shift          |

| Conditional operator | Action                                          |
| -------------------- | ----------------------------------------------- |
| `code1 !! code2`     | Evaluate code2 if code1 throws an exception     |
| `value in array`     | Evaluate as true if a value is within an array |

### Classes

Members: ->, .

### File Includes

### Exceptions
error !! alternative