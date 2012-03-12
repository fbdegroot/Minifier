using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualStudio.Shell;
using VSLangProj80;
using Yahoo.Yui.Compressor;

namespace Minifier
{
	/// <summary>
	/// The YUI Compressor is a JavaScript compressor which, in addition to removing comments and white-spaces, obfuscates local variables using the smallest possible variable name. This obfuscation is safe, even when using constructs such as 'eval' or 'with' (although the compression is not optimal is those cases) Compared to jsmin, the average savings is around 20%.
	/// </summary>
	[ComVisible(true)]
	[Guid("596D1CE3-D0E7-4358-913B-6AC040D77470")]
	[CodeGeneratorRegistration(typeof(YUICompressor), "YUI Compressor", vsContextGuids.vsContextGuidVCSProject, GeneratesDesignTimeSource = true)]
	[ProvideObject(typeof(YUICompressor))]
	public class YUICompressor : BaseCodeGeneratorWithSite
	{
		// The name of this generator (use for 'Custom Tool' property of project item)
		internal static string name = "YUICompressor";

		public YUICompressor()
			: base(".min.css")
		{
		}

		/// <summary>
		/// Function that builds the contents of the generated file based on the contents of the input file
		/// </summary>
		/// <param name="inputFileContent">Content of the input file</param>
		/// <returns>Generated file as a byte array</returns>
		protected override byte[] GenerateCode(string inputFileContent)
		{
			return Encoding.UTF8.GetBytes(CssCompressor.Compress(inputFileContent, -1, CssCompressionType.MichaelAshRegexEnhancements));
		}
	}
}