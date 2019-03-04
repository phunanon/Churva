using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ChurvaDotnet
{
	static class LexicalParser
	{
		public static List<ParseAtom> ParseSourceLines (IEnumerable<string> source)
		{
			var atoms = new List<ParseAtom>();

			TextOpIndentNewline(source, atoms);
			TextOrNumber(atoms);
			ReconstructFloats(atoms);
			ConstructLiterals(atoms);
			ReconstructOperators(atoms);
			//TODO: Detect code blocks: ENDBLK

			return atoms;
		}

		private static void TextOpIndentNewline (IEnumerable<string> source, List<ParseAtom> atoms)
		{
			ushort linNum = 0;
			var indent = 0;
			foreach (var line in source) {
				//Count indents, detect dedents
				// Spaces
				var initialIndent = line.TakeWhile(c => c == ' ').ToArray();
				//Tabs
				if (!initialIndent.Any()) initialIndent = line.TakeWhile(c => c == '\t').ToArray();
				//Was there any indent?
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
					OpOrTextSplitter(trimmedLine)
						.Select(
							sym => new ParseAtom(
								linNum, (ushort)sym.col, sym.isAlpha ? ParseToken.TEXT : ParseToken.OP, sym.text
							)
						)
				);
				atoms.Add(new ParseAtom(linNum, (ushort)line.Length, ParseToken.ENDLIN, ""));
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
					inspect[1].Token = inspect[0].Text == "'" ? ParseToken.CHAR : ParseToken.STRING;
					inspect[1].Text = Regex.Unescape(inspect[1].Text);
					atoms.RemoveAt(a);
					atoms.RemoveAt(a + 1);
				}
			}
		}

		
		private static char[][] _longOps = Dict.LongOps.Select(o => o.ToCharArray()).ToArray();
		private static void ReconstructOperators (IList<ParseAtom> atoms)
		{
            var prevOp = "";
			for (int a = atoms.Count - 2; a >= 0; --a) {
				if (atoms[a].Token != ParseToken.OP) {
					prevOp = "";
					continue;
				}

				var inspect = atoms[a].Text;
                if (prevOp.Length > 0 && inspect.Length > 0) {
	                foreach (var op in _longOps) {
		                if (!TestPattern(new[] {inspect[0], prevOp[0]}, op)) continue;
		                atoms[a].Text = new string(op);
		                atoms.RemoveAt(a + 1);
		                break;
	                }
				}
				prevOp = atoms[a].Text;
			}
        }

        private static IEnumerable<(int col, string text, bool isAlpha)> OpOrTextSplitter (string text)
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
						if (alphas.Last() != '\\') break; //Check if we were going to escape
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
						yield return (c, $"{ch}", false);
					}

					c += alphas.Length == 0 ? 1 : alphas.Length;
				}
			}
		}

		private static bool TestPattern<T> (T[] array, params T[] pattern) => pattern.Length <= array.Length && array.Take(pattern.Length).SequenceEqual(pattern);
	}
}