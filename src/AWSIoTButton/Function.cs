using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializerAttribute(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace AWSIoTButton {
    public class AWSIoTButtonEvent {

        //--- Fields ---
        [JsonProperty("serialNumber")]
        public string SerialNumber;

        [JsonProperty("clickType")]
        public string ClickType;

        [JsonProperty("batteryVoltage")]
        public string BatteryVoltage;
    }

    public class Function {

        //--- Fields ---
        private AmazonDynamoDBClient _client;

        //--- Constructors ---
        public Function() { 
            _client = new AmazonDynamoDBClient();
        }

        //--- Methods ---
        public string Handler(AWSIoTButtonEvent iot, ILambdaContext context) {
            if(iot.ClickType == "LONG") {
                ScanRequest request = new ScanRequest {
                    TableName = "IoTButtonRecords3",
                    AttributesToGet = new[] { "clickType" }.ToList()
                };
                var response = _client.ScanAsync(request).Result;
                foreach(var item in response.Items) {
                    Console.WriteLine($"clickType: {item["clickType"].S}");
                }
                Console.WriteLine("Click received!");
            }
            return "Click received!";
        }
    }
}
