using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Models.StatsModels
{
    public class DashboardStats
    {
        public int UsersCount { get; set; }
        public int ActiveUsers { get; set; }

        public int TotalDownloads { get; set; }


        public int UnpaidSubscriptionCount { get; set; }

        public List<ProductSubscriptionStats> SubscriptionsByProduct { get; set; } = new();
        public decimal Revenue { get; set; }
        public int CashRequests { get; set; }

        public List<MonthlyNewUsers> MonthlyNewUsers { get; set; }=new List<MonthlyNewUsers>();
        public List<ActiveSubscriptions> ActiveSubscriptions { get; set; }=new List<ActiveSubscriptions>();
        public List<TrainingSessions> TrainingSessions { get; set; }=new List<TrainingSessions>();

    }
}
