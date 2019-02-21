#!/usr/bin/churva

sub prompt (var p) => Term.stamp("What is your {p}?"), Term.in()
str name = prompt("name")
u08 age
while !(age = prompt("age") !! 0)
Term.print("Hello, {name}! You were born in {Clock.Year - age}")
