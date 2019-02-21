#!/usr/bin/churva
var file = File.open("names.txt")
for name; n; names = file.delimit('\n')
	Term.stamp("{n}\{names.Count}\t{name}")
