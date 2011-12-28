// compiles with 
// $  gmcs railsClient.cs

using System;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Text;


public class railsClient
{
    static public void Main()
    {
        Console.WriteLine("We're going to send an XML file to our rails database!");


        Console.WriteLine("Please input the path to your rails app and it's controller (or leave blank if you coded a default):");

        string path = Console.ReadLine();
        string response = "";

        if (path == "")
            response = sendXmlData("http://192.168.0.11:3000/books");
        else
            response = sendXmlData(path);


        Console.WriteLine("\n\n Server Response: \n");
        Console.WriteLine(response);
        Console.WriteLine("\n");


        Console.WriteLine("We're done!");
        Console.Read();
    }

    // return true if data is successfully stored (not yet working!)
    static private string sendXmlData(string pathToAction)
    {
        string response = "";

        HttpWebRequest req = null;
        HttpWebResponse rsp = null;
        try
        {
            string uri = pathToAction;

            string xmlData = getXmlDataString();

            req = setupWebRequest(uri);

            // Wrap the request stream with a text-based writer
            StreamWriter writer = new StreamWriter(req.GetRequestStream());
            // Write the XML text into the stream
            writer.WriteLine(xmlData);
            writer.Close();

            // Send the data to the webserver
            rsp = (HttpWebResponse)req.GetResponse();

            Encoding enc = System.Text.Encoding.GetEncoding(1252);
            StreamReader loResponseStream = new StreamReader(rsp.GetResponseStream(), enc);

            response = loResponseStream.ReadToEnd();

            string statusCode = rsp.StatusCode.ToString();
            response = "Status Code: " +statusCode + "\n\n ResponseStream:\n" + response;
        }
        catch (WebException webEx)
        {
            // 
        }
        catch (Exception ex)
        {

        }
        finally
        {
            if (req != null) req.GetRequestStream().Close();
            if (rsp != null) rsp.GetResponseStream().Close();
        }

        return response;
    }

    static public HttpWebRequest setupWebRequest(string uri)
    {
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
        req.Method = "POST";        // Post method
        req.ContentType = "text/xml";     // content type
        req.Accept = "text/xml";

        return req;
    }


    static public string getXmlDataString()
    {
        string xmlData = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
        xmlData += "<book>";
        xmlData += "<author>Ex Emel</author>";
        xmlData += "<created-at type=\"datetime\">2008-05-26T05:58:38Z</created-at>";
        xmlData += "<id type=\"integer\">2</id>";
        xmlData += "<isbn>1234567890</isbn>";
        xmlData += "<price type=\"decimal\">34.99</price>";
        xmlData += "<title>Posted via XML 2</title>";
        xmlData += " <updated-at type=\"datetime\">2008-05-26T05:58:38Z</updated-at>";
        xmlData += "</book>";

        return xmlData;
    }
}

