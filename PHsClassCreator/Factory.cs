using System.IO;

namespace PHsClassCreator
{
	public class Factory
	{
		private static System.Text.Encoding sGlobalEncoding = new System.Text.UTF8Encoding(false);

		private static bool checkOverride(System.Windows.Forms.IWin32Window sOwner, string sPath, string sFilename)
		{
			return System.Windows.Forms.MessageBox.Show(
				sOwner,
				"The file '" + sFilename + "' is already exists in '" + sPath + "'.\nAre you sure to you want to overwrite it?",
				"Overwrite",
				System.Windows.Forms.MessageBoxButtons.YesNo,
				System.Windows.Forms.MessageBoxIcon.Warning) ==
				System.Windows.Forms.DialogResult.Yes;
		}

		public static int createClass(System.Windows.Forms.IWin32Window sOwner, string sPath, string sFilename, string sNamespace, string sClassname, string sUsername, string sIncludeGd)
		{
			try
			{
				string sHeaderFile = Path.Combine(sPath, sFilename + ".h");
				string sSourceFile = Path.Combine(sPath, sFilename + ".cpp");

				if (File.Exists(sHeaderFile))
				{
					if (File.Exists(sSourceFile))
					{
						if (!Factory.checkOverride(sOwner, sPath, sFilename + ".h' and '" + sFilename + ".cpp"))
						{
							System.Windows.Forms.MessageBox.Show(sOwner, "The file creation is been canceled.", "Cancel", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
							return 1;
						}
					}
					else if (!Factory.checkOverride(sOwner, sPath, sFilename + ".h"))
					{
						System.Windows.Forms.MessageBox.Show(sOwner, "The file creation is been canceled.", "Cancel", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
						return 1;
					}
				}
				else if (File.Exists(sSourceFile) && !Factory.checkOverride(sOwner, sPath, sFilename + ".cpp"))
				{
					System.Windows.Forms.MessageBox.Show(sOwner, "The file creation is been canceled.", "Cancel", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
					return 1;
				}

				System.DateTime sCurrentDate = System.DateTime.Now;

				StreamWriter sWriter = new StreamWriter(sHeaderFile, false, Factory.sGlobalEncoding);
				sWriter.NewLine = "\n";

				sWriter.WriteLine();
				sWriter.WriteLine("/*");
				sWriter.WriteLine(sCurrentDate.ToString("\tyyyy.MM.dd"));
				if (sUsername != null) { sWriter.Write("\tCreated by "); sWriter.Write(sUsername); sWriter.WriteLine("."); }
				sWriter.WriteLine("*/");
				sWriter.WriteLine();

				if (sIncludeGd != null)
				{
					sWriter.Write("#ifndef "); sWriter.Write(sIncludeGd); sWriter.WriteLine("_H");
					sWriter.WriteLine();
					sWriter.Write("#define "); sWriter.Write(sIncludeGd); sWriter.WriteLine("_H");
					sWriter.WriteLine();
				}

				sWriter.WriteLine("/*");
				sWriter.WriteLine("\tTODO : Place your include directives here.");
				sWriter.WriteLine("*/");
				sWriter.WriteLine("#include <utility>");
				sWriter.WriteLine();

				string sIndent = string.Empty;

				if (sNamespace != null)
				{
					sWriter.Write("namespace "); sWriter.WriteLine(sNamespace.Replace(".", "::"));
					sWriter.WriteLine('{');
					sIndent = "\t";
				}

				sWriter.Write(sIndent); sWriter.Write("class "); sWriter.WriteLine(sClassname);
				sWriter.Write(sIndent); sWriter.WriteLine('{');
				sWriter.Write(sIndent); sWriter.WriteLine("private:");
				sWriter.Write(sIndent); sWriter.WriteLine("\t/*");
				sWriter.Write(sIndent); sWriter.WriteLine("\t\tTODO : Place your field declarations here.");
				sWriter.Write(sIndent); sWriter.WriteLine("\t*/");
				sWriter.Write(sIndent); sWriter.WriteLine('\t');
				sWriter.Write(sIndent); sWriter.WriteLine('\t');
				sWriter.Write(sIndent); sWriter.WriteLine("public:");
				sWriter.Write(sIndent); sWriter.Write('\t'); sWriter.Write(sClassname); sWriter.WriteLine("();");
				sWriter.Write(sIndent); sWriter.Write('\t'); sWriter.Write(sClassname); sWriter.Write("(const "); sWriter.Write(sClassname); sWriter.WriteLine(" &rSrc);");
				sWriter.Write(sIndent); sWriter.Write('\t'); sWriter.Write(sClassname); sWriter.Write('('); sWriter.Write(sClassname); sWriter.WriteLine(" &&rSrc);");
				sWriter.Write(sIndent); sWriter.Write("\t~"); sWriter.Write(sClassname); sWriter.WriteLine("();");
				sWriter.Write(sIndent); sWriter.WriteLine("\t/*");
				sWriter.Write(sIndent); sWriter.WriteLine("\t\tTODO : Place your other constructors here.");
				sWriter.Write(sIndent); sWriter.WriteLine("\t*/");
				sWriter.Write(sIndent); sWriter.WriteLine('\t');
				sWriter.Write(sIndent); sWriter.WriteLine('\t');
				sWriter.Write(sIndent); sWriter.WriteLine("public:");
				sWriter.Write(sIndent); sWriter.Write('\t'); sWriter.Write(sClassname); sWriter.Write(" &operator=(const "); sWriter.Write(sClassname); sWriter.WriteLine(" &rSrc);");
				sWriter.Write(sIndent); sWriter.Write('\t'); sWriter.Write(sClassname); sWriter.Write(" &operator=("); sWriter.Write(sClassname); sWriter.WriteLine(" &&rSrc);");
				sWriter.Write(sIndent); sWriter.WriteLine("\t/*");
				sWriter.Write(sIndent); sWriter.WriteLine("\t\tTODO : Place your other operator overloadings here.");
				sWriter.Write(sIndent); sWriter.WriteLine("\t*/");
				sWriter.Write(sIndent); sWriter.WriteLine('\t');
				sWriter.Write(sIndent); sWriter.WriteLine('\t');
				sWriter.Write(sIndent); sWriter.WriteLine("public:");
				sWriter.Write(sIndent); sWriter.WriteLine("\t/*");
				sWriter.Write(sIndent); sWriter.WriteLine("\t\tTODO : Place your member function declarations here.");
				sWriter.Write(sIndent); sWriter.WriteLine("\t*/");
				sWriter.Write(sIndent); sWriter.WriteLine('\t');
				sWriter.Write(sIndent); sWriter.Write("};");

				if (sNamespace != null) { sWriter.WriteLine(); sWriter.Write('}'); }
				if (sIncludeGd != null) { sWriter.WriteLine(); sWriter.WriteLine(); sWriter.Write("#endif"); }

				sWriter.Flush();
				sWriter.Close();

				////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

				sWriter = new StreamWriter(sSourceFile, false, Factory.sGlobalEncoding);
				sWriter.NewLine = "\n";

				sWriter.WriteLine();
				sWriter.WriteLine("/*");
				sWriter.WriteLine(sCurrentDate.ToString("\tyyyy.MM.dd"));
				if (sUsername != null) { sWriter.Write("\tCreated by "); sWriter.Write(sUsername); sWriter.WriteLine("."); }
				sWriter.WriteLine("*/");
				sWriter.WriteLine();

				sWriter.Write("#include \""); sWriter.Write(sFilename); sWriter.WriteLine(".h\"");
				sWriter.WriteLine();

				sIndent = string.Empty;

				if (sNamespace != null)
				{
					sWriter.Write("namespace "); sWriter.WriteLine(sNamespace.Replace(".", "::"));
					sWriter.WriteLine('{');
					sIndent = "\t";
				}

				sWriter.Write(sIndent); sWriter.Write(sClassname); sWriter.Write("::"); sWriter.Write(sClassname); sWriter.WriteLine("()");
				sWriter.Write(sIndent); sWriter.WriteLine('{');
				sWriter.Write(sIndent); sWriter.WriteLine("\t//TODO : Place your implementation of default constructor here.");
				sWriter.Write(sIndent); sWriter.WriteLine('\t');
				sWriter.Write(sIndent); sWriter.WriteLine('}');
				sWriter.Write(sIndent); sWriter.WriteLine();

				sWriter.Write(sIndent); sWriter.Write(sClassname); sWriter.Write("::"); sWriter.Write(sClassname); sWriter.Write("(const "); sWriter.Write(sClassname); sWriter.WriteLine(" &rSrc)");
				sWriter.Write(sIndent); sWriter.WriteLine('{');
				sWriter.Write(sIndent); sWriter.WriteLine("\t//TODO : Place your implementation of copy constructor here.");
				sWriter.Write(sIndent); sWriter.WriteLine('\t');
				sWriter.Write(sIndent); sWriter.WriteLine('}');
				sWriter.Write(sIndent); sWriter.WriteLine();

				sWriter.Write(sIndent); sWriter.Write(sClassname); sWriter.Write("::"); sWriter.Write(sClassname); sWriter.Write('('); sWriter.Write(sClassname); sWriter.WriteLine(" &&rSrc)");
				sWriter.Write(sIndent); sWriter.WriteLine('{');
				sWriter.Write(sIndent); sWriter.WriteLine("\t//TODO : Place your implementation of move constructor here.");
				sWriter.Write(sIndent); sWriter.WriteLine('\t');
				sWriter.Write(sIndent); sWriter.WriteLine('}');
				sWriter.Write(sIndent); sWriter.WriteLine();

				sWriter.Write(sIndent); sWriter.Write(sClassname); sWriter.Write("::"); sWriter.Write('~'); sWriter.Write(sClassname); sWriter.WriteLine("()");
				sWriter.Write(sIndent); sWriter.WriteLine('{');
				sWriter.Write(sIndent); sWriter.WriteLine("\t//TODO : Place your implementation of destructor here.");
				sWriter.Write(sIndent); sWriter.WriteLine('\t');
				sWriter.Write(sIndent); sWriter.WriteLine('}');
				sWriter.Write(sIndent); sWriter.WriteLine();

				sWriter.Write(sIndent); sWriter.WriteLine("/*");
				sWriter.Write(sIndent); sWriter.WriteLine("\tTODO : Place your other constructors here.");
				sWriter.Write(sIndent); sWriter.WriteLine("*/");
				sWriter.Write(sIndent); sWriter.WriteLine();
				sWriter.Write(sIndent); sWriter.WriteLine();

				sWriter.Write(sIndent); sWriter.Write(sClassname); sWriter.Write(" &"); sWriter.Write(sClassname); sWriter.Write("::"); sWriter.Write("operator=(const "); sWriter.Write(sClassname); sWriter.WriteLine(" &rSrc)");
				sWriter.Write(sIndent); sWriter.WriteLine('{');
				sWriter.Write(sIndent); sWriter.WriteLine("\t//TODO : Place your implementation of copy assignment operator here.");
				sWriter.Write(sIndent); sWriter.WriteLine('\t');
				sWriter.Write(sIndent); sWriter.WriteLine('\t');
				sWriter.Write(sIndent); sWriter.WriteLine("\treturn *this;");
				sWriter.Write(sIndent); sWriter.WriteLine('}');
				sWriter.Write(sIndent); sWriter.WriteLine();

				sWriter.Write(sIndent); sWriter.Write(sClassname); sWriter.Write(" &"); sWriter.Write(sClassname); sWriter.Write("::"); sWriter.Write("operator=("); sWriter.Write(sClassname); sWriter.WriteLine(" &&rSrc)");
				sWriter.Write(sIndent); sWriter.WriteLine('{');
				sWriter.Write(sIndent); sWriter.WriteLine("\t//TODO : Place your implementation of move assignment operator here.");
				sWriter.Write(sIndent); sWriter.WriteLine('\t');
				sWriter.Write(sIndent); sWriter.WriteLine('\t');
				sWriter.Write(sIndent); sWriter.WriteLine("\treturn *this;");
				sWriter.Write(sIndent); sWriter.WriteLine('}');
				sWriter.Write(sIndent); sWriter.WriteLine();

				sWriter.Write(sIndent); sWriter.WriteLine("/*");
				sWriter.Write(sIndent); sWriter.WriteLine("\tTODO : Place your other operator overloadings here.");
				sWriter.Write(sIndent); sWriter.WriteLine("*/");
				sWriter.Write(sIndent); sWriter.WriteLine();
				sWriter.Write(sIndent); sWriter.WriteLine();

				sWriter.Write(sIndent); sWriter.WriteLine("/*");
				sWriter.Write(sIndent); sWriter.WriteLine("\tTODO : Place your member function definitions here.");
				sWriter.Write(sIndent); sWriter.WriteLine("*/");

				if (sNamespace != null) { sWriter.WriteLine(); sWriter.Write('}'); }

				sWriter.Flush();
				sWriter.Close();
			}
			catch
			{
				return -1;
			}

			return 0;
		}

		public static int createTemplateClass(System.Windows.Forms.IWin32Window sOwner, string sPath, string sFilename, string sNamespace, string sClassname, string sUsername, string sIncludeGd)
		{
			return -1;
		}

		public static int createInterface(System.Windows.Forms.IWin32Window sOwner, string sPath, string sFilename, string sNamespace, string sClassname, string sUsername, string sIncludeGd)
		{
			return -1;
		}
	}
}