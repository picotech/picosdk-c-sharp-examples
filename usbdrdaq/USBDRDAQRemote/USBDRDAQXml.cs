/******************************************************************************
 * 
 *  Filename: USBDRDAQXml.cs
 *
 *  Description: This class provides remote server functionality 
 *            
 *  Copyright © 2012-2017 Pico Technology Ltd. See LICENSE file for terms.
 *
 ******************************************************************************/
 
using System;
using System.Net;
using System.Xml;
using System.IO;

namespace DrDAQRemote
{
    public partial class USBDRDAQXml {

        public static XmlDocument PostXMLTransaction(string v_strURL, XmlDocument v_objXMLDoc )
        {
            //Declare XMLResponse document
            XmlDocument XMLResponse = null;

            //Declare an HTTP-specific implementation of the WebRequest class.
            HttpWebRequest objHttpWebRequest;

            //Declare an HTTP-specific implementation of the WebResponse class
            HttpWebResponse objHttpWebResponse = null;

            //Declare a generic view of a sequence of bytes
            Stream objRequestStream = null;

            //Creates an HttpWebRequest for the specified URL.
            objHttpWebRequest = (HttpWebRequest)WebRequest.Create(v_strURL);

            try
            {
                //---------- Start HttpRequest 

                //Set HttpWebRequest properties
                byte[] bytes;
                bytes = System.Text.Encoding.ASCII.GetBytes(v_objXMLDoc.InnerXml);

                objHttpWebRequest.Method = "POST";
                objHttpWebRequest.ContentLength = bytes.Length;
                objHttpWebRequest.ContentType = "text/xml; encoding='utf-8'";

                //Get Stream object 
                objRequestStream = objHttpWebRequest.GetRequestStream();

                //Writes a sequence of bytes to the current stream 
                objRequestStream.Write(bytes, 0, bytes.Length);

                //Close stream
                objRequestStream.Close();

                //---------- End HttpRequest

                //Sends the HttpWebRequest, and waits for a response.
                objHttpWebResponse = (HttpWebResponse)objHttpWebRequest.GetResponse();

                //---------- Start HttpResponse
                if (objHttpWebResponse.StatusCode == HttpStatusCode.OK)
                {
                    // Transmission successful
                }
                else
                {
                    // Transmission failed
                }

                //Close HttpWebResponse
                objHttpWebResponse.Close();
            }
            catch (WebException we)
            {
                //TODO: Add custom exception handling
                throw new Exception(we.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                //Close connections
                objRequestStream.Close();
                objHttpWebResponse.Close();

                //Release objects
                objRequestStream = null;
                objHttpWebResponse = null;
                objHttpWebRequest = null;
            }

            //Return
            return XMLResponse;
        }
    }
}
