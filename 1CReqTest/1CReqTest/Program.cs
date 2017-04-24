using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace _1CReqTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://1c.elopak.ru/elopak_1c_v83/ws/ws1.1cws");

            // request.Headers.Add("SOAPAction", "\"http://schemas.xmlsoap.org/soap/envelope/\"");
            // request.ContentType = "text/xml;charset=\"utf-8\"";
            // request.Accept = "text/xml";
            // request.Method = "POST";
            // request.Timeout = 10000;

            // // XML-RPC-команда
            // XmlDocument doc = new XmlDocument();
            // doc.Load(Directory.GetCurrentDirectory() + "/request.xml");

            // byte[] bytes = Encoding.Default.GetBytes(doc.OuterXml);
            // string commandUTF8 = Encoding.UTF8.GetString(bytes);

            // Console.WriteLine("command = " + commandUTF8);



            // StreamWriter streamW = new StreamWriter(request.GetRequestStream());

            //     streamW.Write(commandUTF8);
            // streamW.Close();
            //// Thread.Sleep(1000);
            // try
            // {
            //     WebResponse resp = request.GetResponse();
            //     //  XDocument loaded = XDocument.Load(resp.GetResponseStream()); 

            XmlDocument loaded = new XmlDocument();  
                loaded.Load(Directory.GetCurrentDirectory() + "/reply.xml");

                Console.WriteLine("loaded = " + loaded.ToString());

             //   XmlNode element = loaded.FirstChild.FirstChild.FirstChild.FirstChild.FirstChild;



         //   XmlDocument loaded = new XmlDocument();
         //   loaded.Load(@"C:\SCADA\ScadaComm\Config" + @"\reply.xml");

            Console.WriteLine("loaded = " + loaded.ToString());

            XmlElement element = loaded.GetElementById("Body");
            element = loaded.G("GetPrintMachineTableResponse");
            element = loaded.GetElementById("return");
            element.Prefix = "m";


          Console.WriteLine(" ! " +  element.GetChildAsDouble("PrevWastePercent"));
            element.GetChildAsDouble("PrevWastePercent"), 1);
            element.GetChildAsDouble("PrevWasteTarget"), 1);
            element.GetChildAsDouble("TechnologyWaste"), 1);
            SetCurData(4, element.GetChildAsDouble("SpliceWaste"), 1);
            SetCurData(5, element.GetChildAsDouble("DesignWaste"), 1);
            SetCurData(6, element.GetChildAsDouble("TechnicWaste"), 1);
            SetCurData(7, element.GetChildAsDouble("RebuildWaste"), 1);

            String s = element.GetChildAsString("CurOrderNumber");
            WriteToLog("s = " + s);
            string[] sa = s.Split('-');


            SetCurData(8, Convert.ToDouble(sa[1]), 1);
            SetCurData(9, Convert.ToDouble(sa[2]), 1);
            SetCurData(10, Convert.ToDouble(sa[3]), 1);

            SetCurData(11, element.GetChildAsDouble("CurWastePercent"), 1);
            SetCurData(12, element.GetChildAsDouble("CurWasteTarget"), 1);
            SetCurData(13, element.GetChildAsDouble("ShiftWastePercent"), 1);
            SetCurData(14, element.GetChildAsDouble("ShiftWasteTarget"), 1);


        }

    }

          

            //}
            //catch (System.Net.WebException e)
            //{
            //    Console.WriteLine(e.Message + "/n" + e.StackTrace);
            //}

            Console.ReadKey();
        }
    }
}
