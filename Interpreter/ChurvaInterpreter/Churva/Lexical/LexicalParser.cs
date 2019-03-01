using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace ChurvaInterpreter
{
	static class LexicalParser
	{
		public static List<ParseAtom> ParseSourceLines (IEnumerable<string> source)
		{
			var atoms = new List<ParseAtom>();

			TextOrOperatorWithIndent(source, atoms);
			TextOrNumber(atoms);
			ReconstructFloats(atoms);
			ConstructLiterals(atoms);

			return atoms;
		}

		private static void TextOrOperatorWithIndent (IEnumerable<string> source, List<ParseAtom> atoms)
		{
			ushort linNum = 0;
			var indent = 0;
			foreach (var line in source) {
				//Count indents, detect dedents
				var initialIndent = line.TakeWhile(c => c == ' ').ToArray();
				if (!initialIndent.Any()) initialIndent = line.TakeWhile(c => c == '\t').ToArray();
				if (initialIndent.Any()) {
					var newIndent = initialIndent.Length / (initialIndent[0] == '\t' ? 1 : 4);
					if (newIndent != indent) {
						atoms.Add(
							new ParseAtom(
								linNum, 0, newIndent > indent ? ParseToken.INDENT : ParseToken.DEDENT, $"{newIndent}"
							)
						);
						indent = newIndent;
					}
				} else if (indent != 0) {
					indent = 0;
					atoms.Add(new ParseAtom(linNum, 0, ParseToken.DEDENT, $"{0}"));
                }

				var trimmedLine = line.Trim();
				//Ignore comments
				if (trimmedLine.StartsWith("#")) continue;
				//Split line by alphanumeric runs and non-alphanumeric chars
				atoms.AddRange(
					TextCrawler(trimmedLine)
						.Select(
							sym => new ParseAtom(
								linNum, (ushort)sym.col, sym.isAlpha ? ParseToken.TEXT : ParseToken.OP, sym.text
							)
						)
				);
				atoms.Add(new ParseAtom(linNum, line.Length, ParseToken.NL, ""));
				++linNum;
			}
		}

		private static void TextOrNumber (IEnumerable<ParseAtom> atoms)
		{
			foreach (var atom in atoms.Where(a => a.Token == ParseToken.TEXT))
				if (atom.Text.All(ch => char.IsDigit(ch) || ch == '.'))
					atom.Token = ParseToken.NUMBER;
		}

		private static void ReconstructFloats (IList<ParseAtom> atoms)
		{
			//Recontruct NUMBER . NUMBER into one number
			for (int a = atoms.Count - 3; a >= 0; --a) {
				var inspect = atoms.Skip(a).Take(3).ToArray();
				if (inspect[0].Token == ParseToken.NUMBER && inspect[1].Text == "." &&
					inspect[2].Token == ParseToken.NUMBER) {
					inspect[0].Text += $".{inspect[2].Text}";
					atoms.RemoveAt(a + 1);
					atoms.RemoveAt(a + 1);
				}
			}
		}

		private static void ConstructLiterals (IList<ParseAtom> atoms)
		{
			for (int a = atoms.Count - 3; a >= 0; --a) {
				var inspect = atoms.Skip(a).Take(3).ToArray();
				if ((inspect[0].Text + inspect[2].Text == "''") || (inspect[0].Text + inspect[2].Text == "\"\"")) {
					inspect[0].Token = inspect[0].Text == "'" ? ParseToken.CHAR : ParseToken.STRING;
					inspect[0].Text = Regex.Unescape(inspect[1].Text);
					atoms.RemoveAt(a + 1);
					atoms.RemoveAt(a + 1);
				}
			}
		}

		private static IEnumerable<(int col, string text, bool isAlpha)> TextCrawler (string text)
		{
			var escapable = '\0';
			for (var c = 0; c < text.Length;) {
				var next = text.Skip(c).ToArray();
				if (next[0] == ' ') {
					++c;
					continue;
				}

				char[] alphas = { };
				if (escapable != '\0') {
					//Collect whole char/string literal
					while (true) {
						alphas = alphas.Concat(next.TakeWhile(ch => ch != '"' && ch != '\'').ToArray()).ToArray();
						if (alphas.Last() != '\\') break;//Check if we were going to escape
					}

					yield return (c, new string(alphas), true);
					//Handle end " or '
					yield return (c, $"{escapable}", false);
					c += alphas.Length + 1;
					escapable = '\0';
				} else {
					alphas = next.TakeWhile(char.IsLetterOrDigit).ToArray();

					//Yield alphanumeric string, or single character
					if (alphas.Any()) yield return (c, new string(alphas), true);
					else {
						var ch = next[0];
						if (ch == '"' || ch == '\'') escapable = ch;
						if (TestPattern(next, '=', '=')) {
							yield return (c, "==", false); //Be sure to yield == as one operator
							++c;
						}
						else yield return (c, $"{ch}", false);
					}

					c += alphas.Length == 0 ? 1 : alphas.Length;
				}
			}
		}

		private static bool TestPattern<T> (T[] array, params T[] pattern) => pattern.Length <= array.Length && array.Take(pattern.Length).SequenceEqual(pattern);
	}
}