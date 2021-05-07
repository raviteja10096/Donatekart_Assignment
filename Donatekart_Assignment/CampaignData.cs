using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Donatekart_Assignment
{
    public class CampaignData
    {
        public string code { get; set; }
        public string title { get; set; }
        public string shortDesc { get; set; }
        public string imageSrc { get; set; }
        public DateTime created { get; set; }
        public DateTime endDate { get; set; }
        public double totalAmount { get; set; }
        public double procuredAmount { get; set; }
        public double totalProcured { get; set; }
        public double backersCount { get; set; }
        public int categoryId { get; set; }
        public string ngoCode { get; set; }
        public string ngoName { get; set; }
        public int daysLeft { get; set; }
        public double percentage { get; set; }
    }

    public class CampaignDataResponse
    {
        public string title { get; set; }

        public double totalAmount { get; set; }

        public double backersCount { get; set; }
        public DateTime endDate { get; set; }

    }
}
