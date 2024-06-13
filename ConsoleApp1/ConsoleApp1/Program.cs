using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using pdf_demo;
using System.Collections.Specialized;
using System.Net;
using System.Web.Http.Cors;

namespace pdf_demo
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    class Program
    {
        static void Main(string[] args)
        {
            string[] filePlaceHolder = { "http://localhost:8001/" };
            // listen http req
            string state = "";
            string[] prefixes = filePlaceHolder;
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                //state = "off";
                //return state;
                return;
            }
            // URI prefixes are required,
            // for example "http://contoso.com:8080/index/".
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

            // Create a listener.
            HttpListener listener = new HttpListener();
            // Add the prefixes.
            foreach (string s in prefixes)
            {
                listener.Prefixes.Add(s);
            }
            listener.Start();
            bool found = true;
            while (found == true)
            {
                Console.WriteLine("Listening...");

                //loop
                // Note: The GetContext method blocks while waiting for a request.
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                NameValueCollection query = request.QueryString;
                // Obtain a response object.
                HttpListenerResponse response = context.Response;
                // Construct a response.

                // Enable CORS by setting the appropriate headers
                context.Response.AddHeader("Access-Control-Allow-Origin", "*");
                context.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Authorization"); // Add Authorization header
                context.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, OPTIONS");

                string file = query["file"];
                state = query["type"];
                if (state == "off")
                {
                    found = false;
                    return;
                }

                string fileName = file;
                var document = new Content(fileName);

                //document.ShowInPreviewer();
                //loop
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(file + state);
                // Get a response stream and write the response to it.
                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                // You must close the output stream.
                output.Close();
            };
            listener.Stop();

            // listen http req
        }
    }
}