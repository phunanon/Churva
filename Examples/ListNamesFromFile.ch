#!/usr/bin/churva
var file = File.readAll("names.txt")
each name, n: var names = file.delimit('\n')
	Term.stamp("{n}/{names.Length}\t{name}")
Term.stamp("Complete.")
