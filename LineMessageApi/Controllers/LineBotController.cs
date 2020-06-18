using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LineMessageApi.Models;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using Microsoft.AspNetCore.Cors;

namespace LineMessageApi.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("MyCORSPolicy")]
    [ApiController]
    public class LineBotController : ControllerBase
    {
        AppSettings _appsettings;

        //public LineBotController(AppSettings appConfig)
        //{
        //    _appsettings = appConfig;
        //}

        // GET: api/LineBot
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/LineBot/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/LineBot
        [HttpPost]
       
        public void Post([FromBody] dynamic req)
        {
            try
            {
                string responseTextJson = "";
                //string lineAccessToken = _appsettings.AppConfig["bAC+29zKlWCJ65Alhc3KBa1+K8H2yWdhrPjVSju95Rj5xe9JmRcdCdh0cnBdB36x6nkmh0uNNSwmTReXcvXAuQnvpg39SKeodMM2/esWm5fAeFjTDGxAVhs22kkoQP8bn7sgEsipXMjCV+8ORRotNQdB04t89/1O/w1cDnyilFU="];
                var input = JsonConvert.SerializeObject(req);

                Events events = JsonConvert.DeserializeObject<Events>(input) as Events;
                string userId = events.EventList[0].Source.UserId;
                string roomId = events.EventList[0].Source.RoomId;
                string groupId = events.EventList[0].Source.GroupId;
                string replyToken = events.EventList[0].ReplyToken;
                string message = events.EventList[0].Message.Text;




                var chatId = string.Empty;
                if (groupId != null)
                {
                    chatId = groupId;
                }
                else if (roomId != null)
                {
                    chatId = roomId;
                }
                else if (userId != null)
                {
                    chatId = userId;
                }

                var replyMessage = string.Empty;
                List<string> res = _messageHandler(message);

                var messagesJson = string.Empty;

                var jsonresult = string.Empty;

                for (int i = 0; i < res.Count; i++)
                {
                    var msg = res[i];
                    messagesJson += msg;
                    jsonresult += "{\"type\":\"text\",\"text\":\"" + msg + "\"}";
                    var str = i == res.Count - 1 ? "" : ",";
                    jsonresult += str;
                }

                jsonresult = string.Empty;
                jsonresult += "{\"type\":\"text\",\"text\":\"" + messagesJson + "\"}";



                responseTextJson = "{\"replyToken\":\"" + replyToken +
                                   "\",\"messages\":[" + jsonresult + "]}";


                var httpWebRequest = (HttpWebRequest)WebRequest.Create(_appsettings.AppConfig["https://liff.line.me/1654237038-GEpLxZqA"]);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("Authorization", "Bearer ZTpomX5cBXXJaZWtrj2Zyh1t5i4iildfNPf0uun4iaO");
                //_logger.Info(string.Format("{0}:{1}", replyToken, responseTextJson));
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(responseTextJson);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                //_logger.Info(ex.Message);
            }
        }

        // PUT: api/LineBot/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        private List<string> _messageHandler(string message)
        {
            string returnMessage;
            var listMessage = new List<string>();
            message = message.ToLower();

            if (message.Contains("Hello"))
            {

                returnMessage = "Hi boss.";
                listMessage.Add(returnMessage);
            }

            return listMessage;
        }
    }
}
