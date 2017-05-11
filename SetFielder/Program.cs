/*
 /input "E:\Users\haxoid\Documents\visual studio 2010\Projects\SetFielder\SetFielder\bin\Debug\input.pdf" 
 * /output "E:\Users\haxoid\Documents\visual studio 2010\Projects\SetFielder\SetFielder\bin\Debug\output.pdf" 
 * /parameters "name.0:Тест Тест Тест | zaemchik:Yes | name.3:Hello"
 * SetFielder.exe /input "C:\Users\k.basargin\Desktop\input.pdf" /output "C:\Users\k.basargin\Desktop\output.pdf" /parameters "name.0:Тест1 Тест1 Тест1 | zaemchik:Yes | name.3:Hello"
 * ilmerge /target:winexe /out:SelfContainedProgram.exe SetFielder.exe itextsharp.dll
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;

namespace SetFielder
{
    class Program
    {
        static void Main(string[] args)
        {          
            char[] delimiterCharsFirst = { '|' };
            char[] delimiterCharsSecond = { ':' };
            Dictionary<string, string> Params = new Dictionary<string, string>();

            string input = "";
            string output = "";
            string parameters = "";

            for (int i = 0; i < args.Length; i++){
                if (args[i] == "/input")
                    if (!String.IsNullOrEmpty(args[i + 1]))
                        input = args[i + 1];
                    else
                        return;
                if (args[i] == "/output")
                    if (!String.IsNullOrEmpty(args[i + 1]))
                        output = args[i + 1];
                    else
                        return;
                if (args[i] == "/parameters")
                    if (!String.IsNullOrEmpty(args[i + 1]))
                        parameters = args[i + 1];
                    else
                        return;               
            }

			string[] elements = parameters.Split(delimiterCharsFirst);	            
            
            foreach (string element in elements){
                string[] split_parameters = element.Split(delimiterCharsSecond);
                if (split_parameters.Length == 2)
                	if(split_parameters[1].Trim() != "")               		
                		try{
                			if(!Params.ContainsKey(split_parameters[0].Trim()))
                				Params.Add(split_parameters[0].Trim(), split_parameters[1].Trim());	                
                		}
	                	catch{
	                    	continue;
	                	}
                	else
                		continue;
                else
                    return;
            }
            
            try{
                var doc = new Document();
                PdfReader reader = new PdfReader(input); //Application.StartupPath + @"\input1.pdf" //input
                PdfStamper stamper = new PdfStamper(reader, new FileStream(output, FileMode.OpenOrCreate));  //Application.StartupPath + @"\output.pdf" //output
                AcroFields fields = stamper.AcroFields;

                if (fields.Fields.Count < Params.Count)
                    return;                
   
                BaseFont baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\calibri.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                fields.AddSubstitutionFont(baseFont); 
                             
                foreach (KeyValuePair<string, string> Par in Params){
	                try{
						fields.SetFieldProperty(Par.Key, "textsize", 11f, null);
						fields.SetField(Par.Key, Par.Value);
	                }
	                catch{
	                    continue;
	                }
                }
                stamper.FormFlattening = false;
                fields.GenerateAppearances = true;
                stamper.Close();
                reader.Close();
            }
            catch{
            	MessageBox.Show("Ошибка печати PDF макета. Обратитесь к администратору.");
                return;
            }
        }
    }
}
