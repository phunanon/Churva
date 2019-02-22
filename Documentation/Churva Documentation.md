# Churva Documentation

## Introduction
Term, Clock

## Syntax
### Data Types, and Declaration

| Data Type          | Token               | Size                  |
| ------------------ | ------------------- | --------------------- |
| any (determined)   | `var`               | variable              |
| boolean            | `boo`               | 1 bit                 |
| integers un/signed | `u[bits]`/`i[bits]` | 2^n, 8 doubling to 64 |
| string             | `str`               | variable              |
| variable pointer   | `ptr`               | address size          |
| subroutine pointer | `sub`               | address size          |

### Subroutines
#### Definition

| Scheme                    | Syntax                                                 |
| ------------------------- | ------------------------------------------------------ |
| No parameters, no returns | `sub func => do()`                                     |
| No parameters, returns    | `sub func: i32 => 42`                                  |
| Parameters, no returns    | `sub func (i32 a) => do(a)`                            |
| Parameters, returns       | `sub func (i32 a): i32 => a`                           |
| Single-line               | `sub func (i32 a, i32 b): i32 => i32 sum = a + b, sum` |

Multiline:

    sub func (i32 a, i32 b): i32 
        i32 sum = a + b
        => sum

### Flow Control

| Statement                          | Description                                                                |
| ---------------------------------- | -------------------------------------------------------------------------- |
| `if condition`                     | Executes block if condition is true                                        |
| `condition ? if true :  if false`  | Executes blocks depending on condition                                     |
| `when condition`                   | Executes block when condition is true, only once                           |
| `while condition`                  | Loops block if present while condition is true                             |
| `for iterators; condition; repeat` | Accepts iterators, loops block if present while condition is true, repeats |
| `each item, iterator: collection`  | Accepts iterator, loops block, advancing item & iterator each loop         |
| `skip`                             | Skips to next iteration                                                    |
| `finish`                           | Finishes loop                                                              |

Single line `if`: `statement ? code if true : code if false`  
Single line other: `statement => code`

### Conditionals
!!, in, !0 true

### File Includes

### Exception Handling
error !! alternative