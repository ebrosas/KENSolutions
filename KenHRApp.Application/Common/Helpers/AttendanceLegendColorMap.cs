using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.Common.Helpers
{
    public static class AttendanceLegendColorMap
    {
        public static string GetColor(string? legendCode)
        {
            return legendCode switch
            {
                "ALABSENT" => "#e53935",
                "ALPRESENT" => "#43a047",
                "ALLATE" => "#fb8c00",
                "ALLEFTEARLY" => "#8e24aa",
                "ALLEAVE" => "#1e88e5",
                "ALSICK" => "#6d4c41",
                "ALINJURY" => "#5e35b1",
                "ALBUSTRIP" => "#00897b",
                "ALEXCUSE" => "#757575",
                "ALOTHERS" => "#c0ca33",
                _ => "#bdbdbd"
            };
        }
    }
}
