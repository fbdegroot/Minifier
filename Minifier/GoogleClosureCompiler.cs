using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Xml;
using Microsoft.VisualStudio.Shell;
using VSLangProj80;

namespace Minifier
{
	/// <summary>
	/// The Closure Compiler is a tool for making JavaScript download and run faster. It is a true compiler for JavaScript. Instead of compiling from a source language to machine code, it compiles from JavaScript to better JavaScript. It parses your JavaScript, analyzes it, removes dead code and rewrites and minimizes what's left. It also checks syntax, variable references, and types, and warns about common JavaScript pitfalls.
	/// </summary>
	[ComVisible(true)]
	[Guid("52B316AA-1997-4c81-9969-83604C09EEB4")]
	[CodeGeneratorRegistration(typeof(GoogleClosureCompiler), "Google Closure Compiler", vsContextGuids.vsContextGuidVCSProject, GeneratesDesignTimeSource = true)]
	[ProvideObject(typeof(GoogleClosureCompiler))]
	public class GoogleClosureCompiler : BaseCodeGeneratorWithSite
	{
		// The name of this generator (use for 'Custom Tool' property of project item)
		internal static string name = "GoogleClosureCompiler";
		internal const string ApiEndpoint = "http://closure-compiler.appspot.com/compile";
		internal const string PostData = "js_code={0}&output_format=xml&output_info=compiled_code&compilation_level=SIMPLE_OPTIMIZATIONS";

		public GoogleClosureCompiler()
			: base(".min.js")
		{
		}

		/// <summary>
		/// Function that builds the contents of the generated file based on the contents of the input file
		/// </summary>
		/// <param name="inputFileContent">Content of the input file</param>
		/// <returns>Generated file as a byte array</returns>
		protected override byte[] GenerateCode(string inputFileContent)
		{
			return Encoding.UTF8.GetBytes(Compress(inputFileContent));
		}

		/// <summary>
		/// Compresses the specified file using Google's Closure Compiler algorithm.
		/// <remarks>
		/// The file to compress must be smaller than 200 kilobytes.
		/// </remarks>
		/// </summary>
		/// <param name="source">The javascript source to compress.</param>
		/// <returns>A compressed version of the specified JavaScript file.</returns>
		private string Compress(string source)
		{
			XmlDocument xml = CallApi(source);
			return xml.SelectSingleNode("//compiledCode").InnerText;
		}

		/// <summary>
		/// Calls the API with the source file as post data.
		/// </summary>
		/// <param name="source">The content of the source file.</param>
		/// <returns>The Xml response from the Google API.</returns>
		private static XmlDocument CallApi(string source)
		{
			using (WebClient client = new WebClient()) {
				client.Headers.Add("content-type", "application/x-www-form-urlencoded");
				string data = string.Format(PostData, HttpUtility.UrlEncode(source));
				string result = client.UploadString(ApiEndpoint, data);

				XmlDocument doc = new XmlDocument();
				doc.LoadXml(result);
				return doc;
			}
		}
	}
}