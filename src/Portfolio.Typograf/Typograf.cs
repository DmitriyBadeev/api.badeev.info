using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Portfolio.Typograf
{
	enum Entities
	{
		htmlEntities = 1,
		xmlEntities = 2,
		noEntities = 3,
		mixedEntities = 4
	}

	public class Typograf
	{
		public async static Task<string> Run(string text)
		{
			var xml = await GetXmlResponseFromService(text);

			var re = new Regex(@"<ProcessTextResult>\s*((.|\n)*?)\s*<\/ProcessTextResult>");
			var res = re.Match(xml).Value;
			res = res
				.Replace("<ProcessTextResult>", "")
				.Replace("</ProcessTextResult>", "")
				.Replace("\n", "")
				.Replace("&gt;", ">")
				.Replace("&lt;", "<")
				.Replace("&amp;", "&");

			return res;
		}

		private async static Task<string> GetXmlResponseFromService(string text)
		{
			var client = new HttpClient();

			text = text
				.Replace(">", "&gt;")
				.Replace("<", "&lt;")
				.Replace("&", "&amp;");

			var contentString = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
				"<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"" +
				"xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
				"xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">" +
					"<soap:Body>" +
						"<ProcessText xmlns=\"http://typograf.artlebedev.ru/webservices/\">" +
							"<text>" + text + "</text>" +
							"<entityType>1</entityType>" +
							"<useBr>false</useBr>" +
							"<useP>false</useP>" +
							"<maxNobr>3</maxNobr>" +
						"</ProcessText>" +
					"</soap:Body>" +
				"</soap:Envelope>";

			var content = new StringContent(contentString);

			var response = await client.PostAsync("http://typograf.artlebedev.ru/webservices/typograf.asmx", content);
			var responseContent = await response.Content.ReadAsStringAsync();

			return responseContent;
		}
	}
}
