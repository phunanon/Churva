# Churva Documentation

## Introduction
Term, Clock

## Syntax
### Data Types, Declaration, and Assignment

| Data Type          | Token               | Size                   |
| ------------------ | ------------------- | ---------------------- |
| explicitly empty   | `nul`               | 0                      |
| any (determined)   | `var`               | variable               |
| boolean            | `boo`               | 1 bit                  |
| integers un/signed | `u[bits]`/`i[bits]` | 2^n, 08 doubling to 64 |
| floating point     | `f32/f64`           | 2^32 or 2^64           |
| decimal            | `dec`               | variable               |
| string             | `str`               | variable               |
| variable pointer   | `ptr`               | address size           |
| subroutine pointer | `sub`               | address size           |

Assignment can be of any evaluated code. Possible methods of declaration and assignment:

    i32 variable1      //Declared
	variable1 = 42     //Assigned
	i32 variable2 = 42 //Declared & Assigned


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

Execute subroutine with and without arguments: `doSum()`, `doSum(1, arg2)`

#### Anonymous

Anonymous subroutines are a scope of code which can be contained on one line, and return the value of the final evaluation. Commas delimit lines of code.  
Here, `myNumber` is assigned as the value of `sum`:

    i32 myNumber = i32 sum = a + b, sum   //
	i32 myNumber = (i32 sum = a + b, sum) // Same behaviour

### Flow Control

| Statement                      | Description                                                                          |
| ------------------------------ | ------------------------------------------------------------------------------------ |
| `if condition`                 | Executes block if condition is true                                                  |
| `else`                         | Executes block if previous `if` condition was false                                  |
| `elif condition`               | Same as `else`, and its condition is true                                            |
| `condition ? true : false`     | Executes blocks depending on condition                                               |
| `when condition`               | Executes block when condition is true, only once                                     |
| `while condition`              | Loops block if present while condition is true                                       |
| `for first; condition; repeat` | Executes `first` if present, loops block & repeat if present while condition is true |
| `each item, iterator: array`   | Accepts iterator, loops block, advancing item & iterator each loop                   |
| `skip`                         | Skips to next iteration                                                              |
| `finish`                       | Finishes loop                                                                        |

Single line `if`: `statement ? code if true : code if false`  
Single line other: `statement => code`

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



### Conditionals
!!, in, !0 true

### File Includes

### Exception Handling
error !! alternative