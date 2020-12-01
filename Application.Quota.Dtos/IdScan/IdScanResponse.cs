using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Quota.Dtos
{
    public class AlertDescription
    {
        public string text { get; set; }
    }

    public class FaceImage
    {
        public string contentType { get; set; }
        public string data { get; set; }
    }

    public class BarcodeAlertDescription
    {
        public string text { get; set; }
    }

    public class BarcodeData
    {
        public string text { get; set; }
    }

    public class Response
    {
        public string id { get; set; }
        public int alert { get; set; }
        public AlertDescription alert_description { get; set; }
        public string number { get; set; }
        public string first_name { get; set; }
        public string second_name { get; set; }
        public string first_surname { get; set; }
        public string second_surname { get; set; }
        public string birthdate { get; set; }
        public string birthplace { get; set; }
        public string issue_date { get; set; }
        public string issue_place { get; set; }
        public string genre { get; set; }
        public string height { get; set; }
        public string blood_type { get; set; }
        public double front_confidence { get; set; }
        public double back_confidence { get; set; }
        public FaceImage face_image { get; set; }
        public int barcode_alert { get; set; }
        public BarcodeAlertDescription barcode_alert_description { get; set; }
        public BarcodeData barcode_data { get; set; }
    }

    public class IdScanResponse
    {
        public string success { get; set; }
        public Response response { get; set; }
    }
}
