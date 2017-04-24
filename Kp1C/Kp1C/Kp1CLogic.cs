/*
 * Copyright 2016 Mikhail Shiryaev
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : KpKBA
 * Summary  : Device communication logic
 * 
 * Author   : Mikhail Zverkov
 * Created  : 2017
 * Modified : 2017
 * 
 * Description
 * 1С communication notifications.
 */


using Scada.Comm.Devices.KpKBA;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml;


namespace Scada.Comm.Devices
{
    /// <summary>
    /// Device communication logic
    /// <para>Логика работы КП</para>
    /// </summary>
    public class Kp1CLogic : KPLogic
    {
       
        private Config config;              // конфигурация соединения с 1С
        private XmlTextReader xmlReader;     
        private bool fatalError;            // фатальная ошибка при инициализации КП
        private string state;               // состояние КП
        private bool writeState;            // вывести состояние КП


        /// <summary>
        /// Конструктор
        /// </summary>
        public Kp1CLogic(int number)
            : base(number)
        {
            CanSendCmd = true;
            ConnRequired = false;
            WorkState = WorkStates.Normal;
            
            config = new Config();
            xmlReader = new XmlTextReader(config.url);
            fatalError = false;
            state = "";
            writeState = false;

            InitKPTags(new List<KPTag>()
            {
                new KPTag(0, Localization.UseRussian ? "---" : "---"),
                new KPTag(1, Localization.UseRussian ? "PrevWastePercent" : ""),
                 new KPTag(2, Localization.UseRussian ? "PrevWasteTarget" : ""),
                  new KPTag(3, Localization.UseRussian ? "TechnologyWaste" : ""),
                  new KPTag(4, Localization.UseRussian ? "SpliceWaste" : ""),
                   new KPTag(5, Localization.UseRussian ? "DesignWaste" : ""),
                   new KPTag(6, Localization.UseRussian ? "TechnicWaste" : ""),
                   new KPTag(7, Localization.UseRussian ? "RebuildWaste" : ""),
                   new KPTag(8, Localization.UseRussian ? "CurOrderNumber1" : ""),
                    new KPTag(9, Localization.UseRussian ? "CurOrderNumber2" : ""),
                    new KPTag(10, Localization.UseRussian ? "CurOrderNumber3" : ""),


                    new KPTag(11, Localization.UseRussian ? "CurWastePercent" : ""),
                     new KPTag(12, Localization.UseRussian ? "CurWasteTarget" : ""),
                      new KPTag(13, Localization.UseRussian ? "ShiftWastePercent" : ""),
                       new KPTag(14, Localization.UseRussian ? "ShiftWasteTarget" : "")


            });
        }


        /// <summary>
        /// Загрузить конфигурацию соединения с 1С
        /// </summary>
        private void LoadConfig()
        {
            string errMsg;
            fatalError = !config.Load(Config.GetFileName(AppDirs.ConfigDir, Number), out errMsg);

            if (fatalError)
            {
                state = Localization.UseRussian ? 
                    "соедининие с KBA невозможна" : 
                    "connecting to KBA is impossible";
                throw new Exception(errMsg);
            }
            else
            {
                state = Localization.UseRussian ? 
                    "Ожидание данных..." :
                    "Waiting for data...";
            }
        }

        /// <summary>
        /// Инициализировать клиент TCP на основе конфигурации соединения
        /// </summary>
        private void InitTcpClient()
        {

         

         

        }
   
        /// <summary>
        /// Выполнить сеанс опроса КП
        /// </summary>
        public override void Session()
        {
            if (writeState)
            {
                WriteToLog("");
                WriteToLog(state);
                writeState = false;
            }

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
            loaded.Load(@"C:\SCADA\ScadaComm\Config" + @"\reply.xml");

            Console.WriteLine("loaded = " + loaded.ToString());

            XmlElement element = loaded.GetElementById("Body");
            element = loaded.GetElementById("GetPrintMachineTableResponse");
            element = loaded.GetElementById("return");
            element.Prefix = "m";


            SetCurData(0, element.GetChildAsDouble("PrevWastePercent"), 1);
            SetCurData(1, element.GetChildAsDouble("PrevWastePercent"), 1);
            SetCurData(2, element.GetChildAsDouble("PrevWasteTarget"), 1);
            SetCurData(3, element.GetChildAsDouble("TechnologyWaste"), 1);
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



        /// <summary>
        /// Выполнить действия при запуске линии связи
        /// </summary>
        public override void OnCommLineStart()
        {
            writeState = true;
          //  LoadConfig();
            InitTcpClient();
            

        }

    }
}