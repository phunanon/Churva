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
No parameters, no returns: `sub func => do()`  
No parameters, returns: `sub func: i32 => 42`  
Parameters, no returns: `sub func (i32 a) => do(a)`  
Parameters, returns: `sub func (i32 a): i32 => a`  
Single-line: `sub func (i32 a, i32 b): i32 => i32 sum = a + b, sum`  
Multiline:

    sub func (i32 a, i32 b): i32 
        i32 sum = a + b
        => sum

### Flow Control

if, while, for, when  
skip, finish
### Conditionals
!!, in, !0 true

### File Includes

### Exception Handling
error !! alternative